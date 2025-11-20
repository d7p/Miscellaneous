# TTD-DR: Test-Time Diffusion Deep Researcher

A complete implementation of the **Test-Time Diffusion Deep Researcher (TTD-DR)** framework from the paper:

> **"Deep Researcher with Test-Time Diffusion"**
> Han et al., Google Cloud AI Research
> arXiv:2507.16075 (July 2025)

## Overview

TTD-DR is a novel AI research agent that conceptualizes research report generation as a diffusion process. It mimics the iterative nature of human research through cycles of planning, drafting, searching, and revision.

### Key Features

- 🔬 **Three-Stage Research Pipeline**: Plan generation, iterative search & synthesis, and final report generation
- 🔄 **Denoising with Retrieval**: Iteratively refines draft reports using external information
- 🧬 **Component-wise Self-Evolution**: Optimizes each component through critique and revision
- 🔍 **RAG-based Answer Synthesis**: Synthesizes precise answers from retrieved documents
- 📊 **Comprehensive Output**: Saves final reports, intermediate drafts, search history, and metadata

## Architecture

The TTD-DR framework consists of two synergistic mechanisms:

### 1. Report-Level Denoising with Retrieval (Algorithm 1)

```
For each revision step:
  1. Generate search question based on current draft
  2. Retrieve external information
  3. Synthesize answer from retrieved documents
  4. Revise draft by incorporating new information
```

### 2. Component-wise Self-Evolution

```
For each component (plan, question, answer, report):
  1. Generate multiple initial variants
  2. Apply evolution loop:
     - Get environmental feedback (LLM-as-judge)
     - Revise based on feedback
  3. Merge evolved variants
```

## Research Process

```
┌─────────────────────────────────────────────────────────┐
│ Stage 1: Research Plan Generation                       │
│ - Generate structured research plan                     │
│ - Outline key areas for investigation                   │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ Initial Draft Generation (R0)                           │
│ - Create preliminary draft from LLM's internal knowledge│
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ Denoising Loop (Up to N iterations)                     │
│                                                          │
│  ┌────────────────────────────────────────────┐        │
│  │ Stage 2a: Generate Search Question         │        │
│  │ - Based on current draft and gaps          │        │
│  └────────────────────────────────────────────┘        │
│                     ↓                                    │
│  ┌────────────────────────────────────────────┐        │
│  │ Stage 2b: Search & Answer Synthesis        │        │
│  │ - Perform web search                       │        │
│  │ - Synthesize answer with RAG               │        │
│  └────────────────────────────────────────────┘        │
│                     ↓                                    │
│  ┌────────────────────────────────────────────┐        │
│  │ Denoise Draft                              │        │
│  │ - Incorporate new information              │        │
│  │ - Remove imprecisions                      │        │
│  │ - Save revised draft (R_t)                 │        │
│  └────────────────────────────────────────────┘        │
│                     ↓                                    │
│              (Repeat N times)                            │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ Stage 3: Final Report Generation                        │
│ - Synthesize all research findings                      │
│ - Generate comprehensive final report                   │
└─────────────────────────────────────────────────────────┘
```

## Installation

### Requirements

- Python 3.8+
- OpenAI API key or Anthropic API key

### Setup

1. **Clone or navigate to the project directory:**
   ```bash
   cd ttd-dr
   ```

2. **Install dependencies:**
   ```bash
   pip install -r requirements.txt
   ```

3. **Configure API keys:**
   Create a `.env` file in the project root:
   ```bash
   # For OpenAI
   OPENAI_API_KEY=your_openai_api_key_here

   # Or for Anthropic
   ANTHROPIC_API_KEY=your_anthropic_api_key_here
   ```

4. **Configure settings (optional):**
   Edit `config/config.yaml` to customize:
   - LLM provider and model
   - Algorithm parameters (max revision steps, self-evolution settings)
   - Output preferences

## Usage

### Basic Usage

```bash
python main.py "What are the latest developments in quantum computing?"
```

### Interactive Mode

```bash
python main.py --interactive
```

In interactive mode, you can enter multiple queries:
```
Query > What are the applications of deep learning in healthcare?
[Research process runs...]

Query > Analyze the impact of climate change on global food security
[Research process runs...]

Query > quit
```

### Custom Configuration

```bash
python main.py --config my_config.yaml "Your research query"
```

## Configuration

The `config/config.yaml` file contains all configurable parameters:

### LLM Configuration
```yaml
llm:
  provider: "openai"  # or "anthropic"
  model: "gpt-4"  # or "claude-3-5-sonnet-20241022"
  temperature: 0.7
  max_tokens: 4096
```

### Algorithm Configuration

Based on Table 4 from the paper:

```yaml
algorithm:
  max_revision_steps: 20  # N in Algorithm 1

  self_evolution:
    n_plan: 1      # Number of initial plan variants
    n_query: 5     # Number of initial query variants
    n_answer: 3    # Number of initial answer variants
    n_report: 1    # Number of initial report variants

    s_plan: 1      # Plan evolution steps
    s_query: 0     # Query evolution steps
    s_answer: 0    # Answer evolution steps
    s_report: 1    # Report evolution steps
```

### Output Configuration
```yaml
output:
  directory: "output"
  save_intermediate: true  # Save intermediate drafts
  save_search_history: true
  format: "markdown"
```

## Output Files

After each research session, TTD-DR generates:

1. **Final Report** (`report_*.md`) - The comprehensive research report
2. **Research Plan** (`plan_*.md`) - The structured research plan
3. **Search History** (`search_history_*.md`) - All Q&A pairs from searches
4. **Draft History** (`drafts_*.md`) - All intermediate draft revisions (if enabled)
5. **Metadata** (`metadata_*.json`) - Session statistics and file references

Example output structure:
```
output/
├── report_quantum_computing_20250119_143052.md
├── plan_quantum_computing_20250119_143052.md
├── search_history_quantum_computing_20250119_143052.md
├── drafts_quantum_computing_20250119_143052.md
└── metadata_quantum_computing_20250119_143052.json
```

## Examples

### Example 1: Technology Research
```bash
python main.py "What are the key challenges in developing AGI?"
```

This will:
1. Generate a research plan covering key AGI challenges
2. Create an initial draft
3. Iteratively search for information about technical, ethical, and safety challenges
4. Refine the draft with each new finding
5. Generate a comprehensive final report

### Example 2: Business Analysis
```bash
python main.py "Analyze the impact of remote work on startup culture"
```

### Example 3: Scientific Investigation
```bash
python main.py "What are the current approaches to extending human lifespan?"
```

## How It Differs from Other Research Agents

| Feature | TTD-DR | Traditional Agents |
|---------|--------|-------------------|
| **Draft-centric** | ✅ Maintains evolving draft throughout | ❌ Collects info then generates |
| **Denoising Process** | ✅ Iterative refinement like diffusion | ❌ One-shot generation |
| **Self-Evolution** | ✅ Each component optimizes itself | ❌ Fixed pipeline |
| **Information Loss** | ✅ Minimized via continuous integration | ❌ Higher due to delayed synthesis |
| **Research Direction** | ✅ Dynamically guided by draft | ❌ Pre-planned or reactive |

## Paper Reference

If you use this implementation in your research, please cite:

```bibtex
@article{han2025deepresearcher,
  title={Deep Researcher with Test-Time Diffusion},
  author={Han, Rujun and Chen, Yanfei and CuiZhu, Zoey and others},
  journal={arXiv preprint arXiv:2507.16075},
  year={2025}
}
```

## Algorithm Details

### Algorithm 1: Denoising with Retrieval

From the paper (Section 2.3):

```
Input: q (query), M (agents), P (plan), R0 (initial draft), Q, A (histories)

for t = 1 to N do:
    Qt = M_Q(q, P, R_{t-1}, Q, A)     // Generate search question
    Q.append(Qt)

    At = M_A(Qt)                       // Retrieve and synthesize answer
    A.append(At)

    Rt = M_R(q, R_{t-1}, Q, A)        // Denoise draft with new info

    if exit_loop then break
end for
```

### Self-Evolution Process

From Section 2.2:

1. **Initial States**: Generate n variants with diverse parameters
2. **Environmental Feedback**: LLM-as-judge evaluates on:
   - Helpfulness
   - Comprehensiveness
   - Clarity
   - Accuracy
3. **Revision**: Improve based on feedback (repeat s times)
4. **Cross-over**: Merge evolved variants

## Performance Notes

Based on paper results (Table 1):

- **LongForm Research**: 69.1% win rate vs OpenAI Deep Research
- **DeepConsult**: 74.5% win rate vs OpenAI Deep Research
- **HLE-Search**: 33.9% correctness
- **GAIA**: 69.1% correctness

## Customization

### Using Different LLM Providers

**OpenAI (GPT-4):**
```yaml
llm:
  provider: "openai"
  model: "gpt-4"
  api_key_env: "OPENAI_API_KEY"
```

**Anthropic (Claude):**
```yaml
llm:
  provider: "anthropic"
  model: "claude-3-5-sonnet-20241022"
  api_key_env: "ANTHROPIC_API_KEY"
```

### Adjusting Research Depth

For **faster, lighter research** (fewer iterations):
```yaml
algorithm:
  max_revision_steps: 5
  self_evolution:
    n_query: 2
    n_answer: 1
```

For **deeper, more thorough research**:
```yaml
algorithm:
  max_revision_steps: 30
  self_evolution:
    n_query: 10
    n_answer: 5
    s_answer: 2  # Enable answer evolution
```

## Limitations

1. **Search Tool**: Currently uses DuckDuckGo API (limited). For production, integrate with Google Search API or similar.
2. **LLM Costs**: Deep research with self-evolution can be expensive (many LLM calls).
3. **Time**: Each research session can take 10-30 minutes depending on configuration.
4. **Multimodal**: This implementation focuses on text. Browsing and code execution not included.

## Future Enhancements

- [ ] Integration with advanced search APIs (Google, Bing)
- [ ] Web browsing capability
- [ ] Code execution for experiments
- [ ] Multi-modal support (images, PDFs)
- [ ] Agent tuning with RL
- [ ] Streaming output for real-time feedback
- [ ] Parallel search execution

## Project Structure

```
ttd-dr/
├── config/
│   └── config.yaml          # Configuration file
├── src/
│   ├── __init__.py
│   ├── ttd_dr_agent.py      # Main agent implementation
│   ├── llm_client.py        # LLM client abstraction
│   ├── search_tool.py       # Web search functionality
│   ├── prompts.py           # All prompts for each stage
│   └── utils.py             # Utility functions
├── output/                   # Generated reports (created at runtime)
├── main.py                   # CLI application
├── requirements.txt          # Python dependencies
└── README.md                 # This file
```

## Troubleshooting

### API Key Issues
```
Error: No API key found
```
Solution: Ensure `.env` file exists with correct API key:
```bash
echo "OPENAI_API_KEY=your_key_here" > .env
```

### Import Errors
```
ModuleNotFoundError: No module named 'openai'
```
Solution: Install dependencies:
```bash
pip install -r requirements.txt
```

### Search Failures
If searches consistently fail, the DuckDuckGo API may be rate-limited. The implementation includes fallback mock results for demonstration.

## Contributing

This is a research implementation of the TTD-DR paper. Contributions welcome:

- Enhanced search providers
- Additional LLM providers
- Improved evaluation metrics
- Performance optimizations

## License

This implementation is provided for research and educational purposes.

## Acknowledgments

Based on the paper "Deep Researcher with Test-Time Diffusion" by Han et al., Google Cloud AI Research (arXiv:2507.16075).

## Contact

For questions about the implementation or research, please refer to the original paper.

---

**Note**: This is an independent implementation based on the published paper. It is not affiliated with Google or the original authors.
