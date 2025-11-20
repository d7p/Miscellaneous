"""
Prompts for TTD-DR
Based on the paper's methodology
"""

from typing import List


class Prompts:
    """Collection of prompts for each stage of TTD-DR"""

    @staticmethod
    def stage1_research_plan(query: str) -> str:
        """
        Stage 1: Research Plan Generation
        Generate a structured research plan outlining key areas for the final report
        """
        return f"""You are a research planning expert. Given a user query, create a detailed research plan.

User Query: {query}

Your task is to generate a structured research plan that outlines the key areas and topics that need to be investigated to fully address this query.

The research plan should:
1. Break down the query into main themes or aspects
2. Identify key areas that need investigation
3. Outline the structure of the final report
4. List specific topics or questions that need to be addressed

Output a well-structured research plan in markdown format with clear sections and bullet points.
"""

    @staticmethod
    def stage2a_search_question(query: str, plan: str, draft: str, previous_qa: List[tuple]) -> str:
        """
        Stage 2a: Search Question Generation (Algorithm 1, Line 2)
        Generate the next search query based on current draft and context
        """
        qa_history = "\n".join([f"Q: {q}\nA: {a[:200]}..." for q, a in previous_qa[-5:]])  # Last 5 QA pairs

        return f"""You are a research assistant generating search questions to fill gaps in a research report.

User Query: {query}

Research Plan:
{plan}

Current Draft Report:
{draft}

Previous Search History (last 5):
{qa_history}

Your task: Based on the current draft and research plan, identify what information is still missing or needs verification. Generate ONE specific, focused search question that would help improve the draft.

The search question should:
- Target specific gaps or weak areas in the current draft
- Be concrete and searchable
- Help verify or expand existing information
- Advance the research toward completing the plan

Output only the search question, without any additional explanation.
"""

    @staticmethod
    def stage2b_answer_synthesis(question: str, search_results: str) -> str:
        """
        Stage 2b: Answer Searching - RAG-based synthesis (Section 2.1, Stage 2b)
        Synthesize a precise answer from retrieved documents
        """
        return f"""You are a research analyst. You need to synthesize information from search results to answer a specific question.

Question: {question}

Search Results:
{search_results}

Your task: Analyze the search results and synthesize a comprehensive, accurate answer to the question.

Requirements:
- Extract key facts and information relevant to the question
- Combine information from multiple sources if applicable
- Be accurate and cite specific details from the results
- Keep the answer focused and concise (2-3 paragraphs)
- If the search results don't contain relevant information, state that clearly

Provide your synthesized answer:
"""

    @staticmethod
    def stage3_final_report(query: str, plan: str, qa_pairs: List[tuple]) -> str:
        """
        Stage 3: Final Report Generation (Section 2.1, Stage 3)
        Synthesize all gathered information into a comprehensive final report
        """
        research_findings = "\n\n".join([
            f"Research Question: {q}\nFindings: {a}"
            for q, a in qa_pairs
        ])

        return f"""You are an expert research report writer. Generate a comprehensive, well-structured research report.

Original Query: {query}

Research Plan:
{plan}

Research Findings:
{research_findings}

Your task: Synthesize all the research findings into a comprehensive, coherent final report that fully addresses the user's query.

The report should:
1. Have a clear structure with an introduction, body sections, and conclusion
2. Integrate all relevant findings from the research
3. Be well-organized and easy to read
4. Provide comprehensive coverage of the topic
5. Be factual and accurate based on the research conducted
6. Use markdown formatting with headers, bullet points, etc.

Generate the final research report:
"""

    @staticmethod
    def denoising_revision(query: str, current_draft: str, qa_pairs: List[tuple]) -> str:
        """
        Denoising with Retrieval - Report Revision (Algorithm 1, Line 6)
        Refine the draft by incorporating new information
        """
        latest_research = "\n".join([
            f"Q: {q}\nA: {a}"
            for q, a in qa_pairs[-3:]  # Last 3 QA pairs
        ])

        return f"""You are refining a research report draft by incorporating new information.

Original Query: {query}

Current Draft:
{current_draft}

New Research Findings:
{latest_research}

Your task: Revise and improve the current draft by:
1. Incorporating the new research findings
2. Removing imprecisions or errors
3. Filling in gaps with the new information
4. Improving clarity and coherence
5. Maintaining the overall structure and flow

Output the revised draft (complete report, not just changes):
"""

    @staticmethod
    def self_evolution_critique(content: str, content_type: str) -> str:
        """
        Self-Evolution: Environmental Feedback (Section 2.2, Step 2)
        LLM-as-a-judge to provide critique
        """
        return f"""You are an expert evaluator assessing a {content_type}.

{content_type.upper()}:
{content}

Evaluate this {content_type} on the following criteria:
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
[Your detailed feedback here]
"""

    @staticmethod
    def self_evolution_revision(content: str, feedback: str, content_type: str) -> str:
        """
        Self-Evolution: Revision Step (Section 2.2, Step 3)
        Revise based on feedback
        """
        return f"""You are refining a {content_type} based on expert feedback.

ORIGINAL {content_type.upper()}:
{content}

EXPERT FEEDBACK:
{feedback}

Your task: Revise the {content_type} to address all the feedback and improve the scores.

Output the improved version:
"""

    @staticmethod
    def merge_variants(variants: List[str], content_type: str) -> str:
        """
        Self-Evolution: Cross-over (Section 2.2, Step 4)
        Merge multiple evolved variants into one high-quality output
        """
        variants_text = "\n\n---\n\n".join([
            f"VARIANT {i+1}:\n{v}"
            for i, v in enumerate(variants)
        ])

        return f"""You are combining multiple {content_type} variants into a single, superior version.

{variants_text}

Your task: Merge these variants by:
1. Taking the best information from each variant
2. Reconciling any conflicting information logically
3. Creating a comprehensive, coherent final version
4. Ensuring no valuable information is lost

Output the merged {content_type}:
"""

    @staticmethod
    def initial_draft(query: str, plan: str) -> str:
        """
        Generate initial noisy draft (Algorithm 1, R0)
        This draft is based primarily on LLM's internal knowledge
        """
        return f"""You are a research writer creating an initial draft report.

User Query: {query}

Research Plan:
{plan}

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

Use markdown formatting. Write the initial draft:
"""
