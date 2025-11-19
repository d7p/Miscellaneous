/**
 * Test-Time Diffusion Deep Researcher (TTD-DR)
 * Implementation based on arXiv:2507.16075
 */

import { LLMClient } from './llm-client';
import { SearchTool } from './search-tool';
import { Prompts } from './prompts';
import { AgentConfig, AgentState } from './types';
import { Logger } from './utils';

/**
 * Test-Time Diffusion Deep Researcher
 * Implements the complete TTD-DR framework from the paper
 */
export class TTDDRAgent {
  private llm: LLMClient;
  private search: SearchTool;
  private config: AgentConfig;
  private logger: Logger;

  constructor(
    llmClient: LLMClient,
    searchTool: SearchTool,
    config: AgentConfig,
    logger: Logger
  ) {
    this.llm = llmClient;
    this.search = searchTool;
    this.config = config;
    this.logger = logger;
  }

  /**
   * Main research method implementing the full TTD-DR framework
   * Returns: [final_report, agent_state]
   */
  async research(query: string): Promise<[string, AgentState]> {
    this.logger.info(`Starting research for query: ${query}`);

    // Initialize agent state
    const state: AgentState = {
      query,
      plan: '',
      draft: '',
      qaPairs: [],
      revisionHistory: [],
    };

    // Stage 1: Research Plan Generation
    state.plan = await this.stage1GeneratePlan(query);
    this.logger.info('Stage 1 completed: Research plan generated');

    // Generate initial draft (R0 in Algorithm 1)
    state.draft = await this.generateInitialDraft(query, state.plan);
    state.revisionHistory.push(state.draft);
    this.logger.info('Initial draft (R0) generated');

    // Denoising with Retrieval Loop (Algorithm 1)
    for (let step = 1; step <= this.config.maxRevisionSteps; step++) {
      this.logger.info(`\n=== Denoising Step ${step}/${this.config.maxRevisionSteps} ===`);

      // Stage 2a: Generate search question (Algorithm 1, Line 2)
      const question = await this.stage2aGenerateQuestion(state);
      this.logger.info(`Generated question: ${question}`);

      // Stage 2b: Search and synthesize answer (Algorithm 1, Lines 4-5)
      const answer = await this.stage2bSearchAndAnswer(question);
      state.qaPairs.push([question, answer]);
      this.logger.info(`Answer synthesized (${answer.length} chars)`);

      // Denoising: Revise draft with new information (Algorithm 1, Line 6)
      state.draft = await this.denoiseDraft(state);
      state.revisionHistory.push(state.draft);
      this.logger.info(`Draft revised (revision #${state.revisionHistory.length})`);

      // Check if we should stop
      if (step >= this.config.maxRevisionSteps) {
        this.logger.info('Max revision steps reached');
        break;
      }
    }

    // Stage 3: Final Report Generation
    const finalReport = await this.stage3GenerateFinalReport(state);
    this.logger.info('Stage 3 completed: Final report generated');

    return [finalReport, state];
  }

  /**
   * Stage 1: Research Plan Generation (Section 2.1, Stage 1)
   * With optional self-evolution
   */
  private async stage1GeneratePlan(query: string): Promise<string> {
    this.logger.info('Stage 1: Generating research plan...');

    const prompt = Prompts.stage1ResearchPlan(query);

    if (this.config.nPlan === 1 && this.config.sPlan === 0) {
      // Simple generation without self-evolution
      return await this.llm.generate(prompt);
    } else {
      // With self-evolution
      return await this.selfEvolve(
        prompt,
        this.config.nPlan,
        this.config.sPlan,
        'research plan'
      );
    }
  }

  /**
   * Stage 2a: Search Question Generation (Algorithm 1, Line 2)
   * Generate next search query based on current context
   */
  private async stage2aGenerateQuestion(state: AgentState): Promise<string> {
    this.logger.info('Stage 2a: Generating search question...');

    const prompt = Prompts.stage2aSearchQuestion(
      state.query,
      state.plan,
      state.draft,
      state.qaPairs
    );

    let question: string;
    if (this.config.nQuery === 1 && this.config.sQuery === 0) {
      // Simple generation
      question = await this.llm.generate(prompt, 0.8);
    } else {
      // With self-evolution
      question = await this.selfEvolve(
        prompt,
        this.config.nQuery,
        this.config.sQuery,
        'search question'
      );
    }

    return question.trim();
  }

  /**
   * Stage 2b: Answer Searching with RAG (Section 2.1, Stage 2b)
   * Search and synthesize answer from retrieved documents
   */
  private async stage2bSearchAndAnswer(question: string): Promise<string> {
    this.logger.info('Stage 2b: Searching and synthesizing answer...');

    // Perform search
    const searchResults = await this.search.search(question);
    const formattedResults = this.search.formatResults(searchResults);

    // Synthesize answer using RAG
    const prompt = Prompts.stage2bAnswerSynthesis(question, formattedResults);

    if (this.config.nAnswer === 1 && this.config.sAnswer === 0) {
      // Simple synthesis
      return await this.llm.generate(prompt);
    } else {
      // With self-evolution
      return await this.selfEvolve(
        prompt,
        this.config.nAnswer,
        this.config.sAnswer,
        'answer'
      );
    }
  }

  /**
   * Denoising with Retrieval (Algorithm 1, Line 6)
   * Revise draft by incorporating new information
   */
  private async denoiseDraft(state: AgentState): Promise<string> {
    this.logger.info('Denoising: Revising draft with new information...');

    const prompt = Prompts.denoisingRevision(
      state.query,
      state.draft,
      state.qaPairs
    );

    return await this.llm.generate(prompt);
  }

  /**
   * Stage 3: Final Report Generation (Section 2.1, Stage 3)
   * Synthesize all information into final comprehensive report
   */
  private async stage3GenerateFinalReport(state: AgentState): Promise<string> {
    this.logger.info('Stage 3: Generating final report...');

    const prompt = Prompts.stage3FinalReport(
      state.query,
      state.plan,
      state.qaPairs
    );

    if (this.config.nReport === 1 && this.config.sReport === 0) {
      // Simple generation
      return await this.llm.generate(prompt, 0.7, 8192);
    } else {
      // With self-evolution
      return await this.selfEvolve(
        prompt,
        this.config.nReport,
        this.config.sReport,
        'final report'
      );
    }
  }

  /**
   * Generate initial noisy draft (R0 in Algorithm 1)
   * Based on LLM's internal knowledge
   */
  private async generateInitialDraft(query: string, plan: string): Promise<string> {
    this.logger.info('Generating initial draft (R0)...');

    const prompt = Prompts.initialDraft(query, plan);
    return await this.llm.generate(prompt, 0.7, 4096);
  }

  /**
   * Component-wise Self-Evolution (Section 2.2)
   *
   * Algorithm:
   * 1. Generate multiple initial variants
   * 2. For each variant, apply evolution loop:
   *    - Get environmental feedback (LLM-as-a-judge)
   *    - Revise based on feedback
   *    - Repeat for nEvolutionSteps
   * 3. Merge all evolved variants into final output
   */
  private async selfEvolve(
    prompt: string,
    nVariants: number,
    nEvolutionSteps: number,
    contentType: string
  ): Promise<string> {
    this.logger.info(`Self-evolution: ${nVariants} variants, ${nEvolutionSteps} steps`);

    // Step 1: Generate initial variants with diverse parameters
    const variants = await this.llm.generateMultiple(prompt, nVariants, 0.8);

    if (nEvolutionSteps === 0) {
      // No evolution, just merge initial variants
      if (variants.length === 1) {
        return variants[0];
      }
      return await this.mergeVariants(variants, contentType);
    }

    // Steps 2 & 3: Evolution loop for each variant
    const evolvedVariants: string[] = [];

    for (let i = 0; i < variants.length; i++) {
      this.logger.info(`Evolving variant ${i + 1}/${variants.length}...`);
      let current = variants[i];

      for (let step = 0; step < nEvolutionSteps; step++) {
        // Environmental Feedback: Get critique (Section 2.2, Step 2)
        const critiquePrompt = Prompts.selfEvolutionCritique(current, contentType);
        const feedback = await this.llm.generate(critiquePrompt, 0.5);

        // Revision: Improve based on feedback (Section 2.2, Step 3)
        const revisionPrompt = Prompts.selfEvolutionRevision(current, feedback, contentType);
        current = await this.llm.generate(revisionPrompt, 0.7);
      }

      evolvedVariants.push(current);
    }

    // Step 4: Cross-over - Merge evolved variants (Section 2.2, Step 4)
    if (evolvedVariants.length === 1) {
      return evolvedVariants[0];
    }

    return await this.mergeVariants(evolvedVariants, contentType);
  }

  /**
   * Merge multiple variants into single output (Section 2.2, Step 4 - Cross-over)
   */
  private async mergeVariants(variants: string[], contentType: string): Promise<string> {
    this.logger.info(`Merging ${variants.length} variants...`);

    const prompt = Prompts.mergeVariants(variants, contentType);
    return await this.llm.generate(prompt, 0.7, 8192);
  }
}
