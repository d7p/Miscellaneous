"""
Test-Time Diffusion Deep Researcher (TTD-DR)
Implementation based on arXiv:2507.16075
"""

import logging
from typing import List, Tuple, Dict, Optional
from dataclasses import dataclass
from .llm_client import LLMClient
from .search_tool import SearchTool
from .prompts import Prompts


@dataclass
class AgentConfig:
    """Configuration for TTD-DR agent"""
    max_revision_steps: int = 20
    n_plan: int = 1
    n_query: int = 5
    n_answer: int = 3
    n_report: int = 1
    s_plan: int = 1
    s_query: int = 0
    s_answer: int = 0
    s_report: int = 1
    save_intermediate: bool = True


@dataclass
class AgentState:
    """State maintained throughout the research process"""
    query: str
    plan: str = ""
    draft: str = ""
    qa_pairs: List[Tuple[str, str]] = None
    revision_history: List[str] = None

    def __post_init__(self):
        if self.qa_pairs is None:
            self.qa_pairs = []
        if self.revision_history is None:
            self.revision_history = []


class TTDDRAgent:
    """
    Test-Time Diffusion Deep Researcher
    Implements the complete TTD-DR framework from the paper
    """

    def __init__(
        self,
        llm_client: LLMClient,
        search_tool: SearchTool,
        config: AgentConfig,
        logger: Optional[logging.Logger] = None
    ):
        self.llm = llm_client
        self.search = search_tool
        self.config = config
        self.logger = logger or logging.getLogger(__name__)
        self.prompts = Prompts()

    def research(self, query: str) -> Tuple[str, AgentState]:
        """
        Main research method implementing the full TTD-DR framework
        Returns: (final_report, agent_state)
        """
        self.logger.info(f"Starting research for query: {query}")

        # Initialize agent state
        state = AgentState(query=query)

        # Stage 1: Research Plan Generation
        state.plan = self._stage1_generate_plan(query)
        self.logger.info("Stage 1 completed: Research plan generated")

        # Generate initial draft (R0 in Algorithm 1)
        state.draft = self._generate_initial_draft(query, state.plan)
        state.revision_history.append(state.draft)
        self.logger.info("Initial draft (R0) generated")

        # Denoising with Retrieval Loop (Algorithm 1)
        for step in range(1, self.config.max_revision_steps + 1):
            self.logger.info(f"\n=== Denoising Step {step}/{self.config.max_revision_steps} ===")

            # Stage 2a: Generate search question (Algorithm 1, Line 2)
            question = self._stage2a_generate_question(state)
            self.logger.info(f"Generated question: {question}")

            # Stage 2b: Search and synthesize answer (Algorithm 1, Lines 4-5)
            answer = self._stage2b_search_and_answer(question)
            state.qa_pairs.append((question, answer))
            self.logger.info(f"Answer synthesized ({len(answer)} chars)")

            # Denoising: Revise draft with new information (Algorithm 1, Line 6)
            state.draft = self._denoise_draft(state)
            state.revision_history.append(state.draft)
            self.logger.info(f"Draft revised (revision #{len(state.revision_history)})")

            # Check if we should stop (could implement more sophisticated exit logic)
            if step >= self.config.max_revision_steps:
                self.logger.info("Max revision steps reached")
                break

        # Stage 3: Final Report Generation
        final_report = self._stage3_generate_final_report(state)
        self.logger.info("Stage 3 completed: Final report generated")

        return final_report, state

    def _stage1_generate_plan(self, query: str) -> str:
        """
        Stage 1: Research Plan Generation (Section 2.1, Stage 1)
        With optional self-evolution
        """
        self.logger.info("Stage 1: Generating research plan...")

        prompt = self.prompts.stage1_research_plan(query)

        if self.config.n_plan == 1 and self.config.s_plan == 0:
            # Simple generation without self-evolution
            plan = self.llm.generate(prompt)
        else:
            # With self-evolution
            plan = self._self_evolve(
                prompt=prompt,
                n_variants=self.config.n_plan,
                n_evolution_steps=self.config.s_plan,
                content_type="research plan"
            )

        return plan

    def _stage2a_generate_question(self, state: AgentState) -> str:
        """
        Stage 2a: Search Question Generation (Algorithm 1, Line 2)
        Generate next search query based on current context
        """
        self.logger.info("Stage 2a: Generating search question...")

        prompt = self.prompts.stage2a_search_question(
            query=state.query,
            plan=state.plan,
            draft=state.draft,
            previous_qa=state.qa_pairs
        )

        if self.config.n_query == 1 and self.config.s_query == 0:
            # Simple generation
            question = self.llm.generate(prompt, temperature=0.8)
        else:
            # With self-evolution
            question = self._self_evolve(
                prompt=prompt,
                n_variants=self.config.n_query,
                n_evolution_steps=self.config.s_query,
                content_type="search question"
            )

        return question.strip()

    def _stage2b_search_and_answer(self, question: str) -> str:
        """
        Stage 2b: Answer Searching with RAG (Section 2.1, Stage 2b)
        Search and synthesize answer from retrieved documents
        """
        self.logger.info("Stage 2b: Searching and synthesizing answer...")

        # Perform search
        search_results = self.search.search(question)
        formatted_results = self.search.format_results(search_results)

        # Synthesize answer using RAG
        prompt = self.prompts.stage2b_answer_synthesis(question, formatted_results)

        if self.config.n_answer == 1 and self.config.s_answer == 0:
            # Simple synthesis
            answer = self.llm.generate(prompt)
        else:
            # With self-evolution
            answer = self._self_evolve(
                prompt=prompt,
                n_variants=self.config.n_answer,
                n_evolution_steps=self.config.s_answer,
                content_type="answer"
            )

        return answer

    def _denoise_draft(self, state: AgentState) -> str:
        """
        Denoising with Retrieval (Algorithm 1, Line 6)
        Revise draft by incorporating new information
        """
        self.logger.info("Denoising: Revising draft with new information...")

        prompt = self.prompts.denoising_revision(
            query=state.query,
            current_draft=state.draft,
            qa_pairs=state.qa_pairs
        )

        revised_draft = self.llm.generate(prompt)
        return revised_draft

    def _stage3_generate_final_report(self, state: AgentState) -> str:
        """
        Stage 3: Final Report Generation (Section 2.1, Stage 3)
        Synthesize all information into final comprehensive report
        """
        self.logger.info("Stage 3: Generating final report...")

        prompt = self.prompts.stage3_final_report(
            query=state.query,
            plan=state.plan,
            qa_pairs=state.qa_pairs
        )

        if self.config.n_report == 1 and self.config.s_report == 0:
            # Simple generation
            final_report = self.llm.generate(prompt, max_tokens=8192)
        else:
            # With self-evolution
            final_report = self._self_evolve(
                prompt=prompt,
                n_variants=self.config.n_report,
                n_evolution_steps=self.config.s_report,
                content_type="final report"
            )

        return final_report

    def _generate_initial_draft(self, query: str, plan: str) -> str:
        """
        Generate initial noisy draft (R0 in Algorithm 1)
        Based on LLM's internal knowledge
        """
        self.logger.info("Generating initial draft (R0)...")

        prompt = self.prompts.initial_draft(query, plan)
        draft = self.llm.generate(prompt, max_tokens=4096)

        return draft

    def _self_evolve(
        self,
        prompt: str,
        n_variants: int,
        n_evolution_steps: int,
        content_type: str
    ) -> str:
        """
        Component-wise Self-Evolution (Section 2.2)

        Algorithm:
        1. Generate multiple initial variants
        2. For each variant, apply evolution loop:
           - Get environmental feedback (LLM-as-a-judge)
           - Revise based on feedback
           - Repeat for n_evolution_steps
        3. Merge all evolved variants into final output
        """
        self.logger.info(f"Self-evolution: {n_variants} variants, {n_evolution_steps} steps")

        # Step 1: Generate initial variants with diverse parameters
        variants = self.llm.generate_multiple(prompt, n=n_variants, temperature=0.8)

        if n_evolution_steps == 0:
            # No evolution, just merge initial variants
            if len(variants) == 1:
                return variants[0]
            return self._merge_variants(variants, content_type)

        # Step 2 & 3: Evolution loop for each variant
        evolved_variants = []

        for i, variant in enumerate(variants):
            self.logger.info(f"Evolving variant {i+1}/{len(variants)}...")
            current = variant

            for step in range(n_evolution_steps):
                # Environmental Feedback: Get critique (Section 2.2, Step 2)
                critique_prompt = self.prompts.self_evolution_critique(current, content_type)
                feedback = self.llm.generate(critique_prompt, temperature=0.5)

                # Revision: Improve based on feedback (Section 2.2, Step 3)
                revision_prompt = self.prompts.self_evolution_revision(current, feedback, content_type)
                current = self.llm.generate(revision_prompt, temperature=0.7)

            evolved_variants.append(current)

        # Step 4: Cross-over - Merge evolved variants (Section 2.2, Step 4)
        if len(evolved_variants) == 1:
            return evolved_variants[0]

        final_output = self._merge_variants(evolved_variants, content_type)
        return final_output

    def _merge_variants(self, variants: List[str], content_type: str) -> str:
        """
        Merge multiple variants into single output (Section 2.2, Step 4 - Cross-over)
        """
        self.logger.info(f"Merging {len(variants)} variants...")

        prompt = self.prompts.merge_variants(variants, content_type)
        merged = self.llm.generate(prompt, max_tokens=8192)

        return merged
