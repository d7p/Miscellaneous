/**
 * Prompts for TTD-DR
 * Based on the paper's methodology
 */

/**
 * Collection of prompts for each stage of TTD-DR
 */
export class Prompts {
  /**
   * Stage 1: Research Plan Generation
   * Generate a structured research plan outlining key areas for the final report
   */
  static stage1ResearchPlan(query: string): string {
    return `You are a research planning expert. Given a user query, create a detailed research plan.

User Query: ${query}

Your task is to generate a structured research plan that outlines the key areas and topics that need to be investigated to fully address this query.

The research plan should:
1. Break down the query into main themes or aspects
2. Identify key areas that need investigation
3. Outline the structure of the final report
4. List specific topics or questions that need to be addressed

Output a well-structured research plan in markdown format with clear sections and bullet points.`;
  }

  /**
   * Stage 2a: Search Question Generation (Algorithm 1, Line 2)
   * Generate the next search query based on current draft and context
   */
  static stage2aSearchQuestion(
    query: string,
    plan: string,
    draft: string,
    previousQa: Array<[string, string]>
  ): string {
    const qaHistory = previousQa
      .slice(-5)
      .map(([q, a]) => `Q: ${q}\nA: ${a.substring(0, 200)}...`)
      .join('\n');

    return `You are a research assistant generating search questions to fill gaps in a research report.

User Query: ${query}

Research Plan:
${plan}

Current Draft Report:
${draft}

Previous Search History (last 5):
${qaHistory}

Your task: Based on the current draft and research plan, identify what information is still missing or needs verification. Generate ONE specific, focused search question that would help improve the draft.

The search question should:
- Target specific gaps or weak areas in the current draft
- Be concrete and searchable
- Help verify or expand existing information
- Advance the research toward completing the plan

Output only the search question, without any additional explanation.`;
  }

  /**
   * Stage 2b: Answer Searching - RAG-based synthesis (Section 2.1, Stage 2b)
   * Synthesize a precise answer from retrieved documents
   */
  static stage2bAnswerSynthesis(question: string, searchResults: string): string {
    return `You are a research analyst. You need to synthesize information from search results to answer a specific question.

Question: ${question}

Search Results:
${searchResults}

Your task: Analyze the search results and synthesize a comprehensive, accurate answer to the question.

Requirements:
- Extract key facts and information relevant to the question
- Combine information from multiple sources if applicable
- Be accurate and cite specific details from the results
- Keep the answer focused and concise (2-3 paragraphs)
- If the search results don't contain relevant information, state that clearly

Provide your synthesized answer:`;
  }

  /**
   * Stage 3: Final Report Generation (Section 2.1, Stage 3)
   * Synthesize all gathered information into a comprehensive final report
   */
  static stage3FinalReport(query: string, plan: string, qaPairs: Array<[string, string]>): string {
    const researchFindings = qaPairs
      .map(([q, a]) => `Research Question: ${q}\nFindings: ${a}`)
      .join('\n\n');

    return `You are an expert research report writer. Generate a comprehensive, well-structured research report.

Original Query: ${query}

Research Plan:
${plan}

Research Findings:
${researchFindings}

Your task: Synthesize all the research findings into a comprehensive, coherent final report that fully addresses the user's query.

The report should:
1. Have a clear structure with an introduction, body sections, and conclusion
2. Integrate all relevant findings from the research
3. Be well-organized and easy to read
4. Provide comprehensive coverage of the topic
5. Be factual and accurate based on the research conducted
6. Use markdown formatting with headers, bullet points, etc.

Generate the final research report:`;
  }

  /**
   * Denoising with Retrieval - Report Revision (Algorithm 1, Line 6)
   * Refine the draft by incorporating new information
   */
  static denoisingRevision(
    query: string,
    currentDraft: string,
    qaPairs: Array<[string, string]>
  ): string {
    const latestResearch = qaPairs
      .slice(-3)
      .map(([q, a]) => `Q: ${q}\nA: ${a}`)
      .join('\n');

    return `You are refining a research report draft by incorporating new information.

Original Query: ${query}

Current Draft:
${currentDraft}

New Research Findings:
${latestResearch}

Your task: Revise and improve the current draft by:
1. Incorporating the new research findings
2. Removing imprecisions or errors
3. Filling in gaps with the new information
4. Improving clarity and coherence
5. Maintaining the overall structure and flow

Output the revised draft (complete report, not just changes):`;
  }

  /**
   * Self-Evolution: Environmental Feedback (Section 2.2, Step 2)
   * LLM-as-a-judge to provide critique
   */
  static selfEvolutionCritique(content: string, contentType: string): string {
    return `You are an expert evaluator assessing a ${contentType}.

${contentType.toUpperCase()}:
${content}

Evaluate this ${contentType} on the following criteria:
1. Helpfulness: Does it effectively serve its purpose?
2. Comprehensiveness: Is all necessary information included?
3. Clarity: Is it well-written and easy to understand?
4. Accuracy: Is the information correct and well-reasoned?

Provide:
1. A score from 1-10 for each criterion
2. Specific, actionable feedback for improvement
3. Identify any gaps, errors, or areas that need enhancement

Format your response as:
SCORES:
- Helpfulness: X/10
- Comprehensiveness: X/10
- Clarity: X/10
- Accuracy: X/10

FEEDBACK:
[Your detailed feedback here]`;
  }

  /**
   * Self-Evolution: Revision Step (Section 2.2, Step 3)
   * Revise based on feedback
   */
  static selfEvolutionRevision(content: string, feedback: string, contentType: string): string {
    return `You are refining a ${contentType} based on expert feedback.

ORIGINAL ${contentType.toUpperCase()}:
${content}

EXPERT FEEDBACK:
${feedback}

Your task: Revise the ${contentType} to address all the feedback and improve the scores.

Output the improved version:`;
  }

  /**
   * Self-Evolution: Cross-over (Section 2.2, Step 4)
   * Merge multiple evolved variants into one high-quality output
   */
  static mergeVariants(variants: string[], contentType: string): string {
    const variantsText = variants
      .map((v, i) => `VARIANT ${i + 1}:\n${v}`)
      .join('\n\n---\n\n');

    return `You are combining multiple ${contentType} variants into a single, superior version.

${variantsText}

Your task: Merge these variants by:
1. Taking the best information from each variant
2. Reconciling any conflicting information logically
3. Creating a comprehensive, coherent final version
4. Ensuring no valuable information is lost

Output the merged ${contentType}:`;
  }

  /**
   * Generate initial noisy draft (Algorithm 1, R0)
   * This draft is based primarily on LLM's internal knowledge
   */
  static initialDraft(query: string, plan: string): string {
    return `You are a research writer creating an initial draft report.

User Query: ${query}

Research Plan:
${plan}

Your task: Write an initial draft report addressing the query. This is a preliminary draft that will be refined later.

Base your draft on:
1. The research plan structure
2. Your general knowledge (you may not have all specific details yet)
3. Logical reasoning about what information would be relevant

The draft should:
- Follow the structure outlined in the research plan
- Be well-organized with clear sections
- Include placeholders or general statements where specific information is not yet available
- Provide a foundation that can be refined with external research

Use markdown formatting. Write the initial draft:`;
  }
}
