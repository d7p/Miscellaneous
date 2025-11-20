# TTD-DR Quick Start Guide

Get started with Test-Time Diffusion Deep Researcher in 5 minutes!

## Prerequisites

- Python 3.8 or higher
- An OpenAI API key (or Anthropic API key)
- Internet connection for web searches

## Step-by-Step Setup

### 1. Navigate to the Project Directory

```bash
cd ttd-dr
```

### 2. Install Dependencies

```bash
pip install -r requirements.txt
```

This will install:
- `openai` - For GPT-4 access
- `anthropic` - For Claude access
- `requests` - For web searches
- `click` - For CLI
- `pydantic` - For data validation
- `python-dotenv` - For environment variables
- `colorama` - For colored output

### 3. Configure Your API Key

Create a `.env` file:

```bash
cp .env.example .env
```

Edit `.env` and add your API key:

```bash
# For OpenAI (recommended)
OPENAI_API_KEY=sk-your-key-here

# OR for Anthropic
ANTHROPIC_API_KEY=sk-ant-your-key-here
```

### 4. Run Your First Research Query

```bash
python main.py "What are the latest breakthroughs in renewable energy?"
```

That's it! The system will:
1. ✅ Generate a research plan
2. ✅ Create an initial draft
3. ✅ Perform up to 20 search iterations
4. ✅ Refine the draft with each search
5. ✅ Generate a comprehensive final report
6. ✅ Save all outputs to the `output/` directory

## Example Commands

### Basic Research Query
```bash
python main.py "Explain quantum computing applications in cryptography"
```

### Interactive Mode
```bash
python main.py --interactive
```

Then enter queries interactively:
```
Query > What is the future of electric vehicles?
[Research runs...]

Query > How does CRISPR gene editing work?
[Research runs...]

Query > quit
```

### Using Custom Configuration
```bash
python main.py --config config/config.yaml "Your query here"
```

## Understanding the Output

After running a query, check the `output/` directory:

```
output/
├── report_your_query_20250119_143052.md        # Final report
├── plan_your_query_20250119_143052.md          # Research plan
├── search_history_your_query_20250119_143052.md # All searches
├── drafts_your_query_20250119_143052.md        # Draft evolution
└── metadata_your_query_20250119_143052.json    # Statistics
```

## Configuration Tips

### For Faster Research (Development/Testing)

Edit `config/config.yaml`:

```yaml
algorithm:
  max_revision_steps: 5  # Instead of 20
  self_evolution:
    n_query: 2  # Instead of 5
    n_answer: 1  # Instead of 3
```

### For Deeper Research (Production)

```yaml
algorithm:
  max_revision_steps: 30
  self_evolution:
    n_query: 10
    n_answer: 5
    s_answer: 1  # Enable answer evolution
```

### Using Claude Instead of GPT-4

Edit `config/config.yaml`:

```yaml
llm:
  provider: "anthropic"
  model: "claude-3-5-sonnet-20241022"
  api_key_env: "ANTHROPIC_API_KEY"
```

## Programmatic Usage

Want to use TTD-DR in your own Python code?

```python
from src import TTDDRAgent, AgentConfig, create_llm_client, SearchTool
import os

# Setup
llm = create_llm_client("openai", "gpt-4", os.getenv("OPENAI_API_KEY"))
search = SearchTool(max_results=5)
config = AgentConfig(max_revision_steps=10)

# Create agent
agent = TTDDRAgent(llm, search, config)

# Run research
report, state = agent.research("Your research query here")

print(report)
```

See `example.py` for a complete programmatic example:

```bash
python example.py
```

## Common Issues

### "No API key found"
**Solution**: Make sure `.env` file exists and contains your API key.

```bash
cat .env  # Check if file exists
```

### "Module not found"
**Solution**: Install dependencies:

```bash
pip install -r requirements.txt
```

### "Rate limit exceeded"
**Solution**: Your API key has hit rate limits. Wait a few minutes or upgrade your API plan.

### Searches returning mock data
**Solution**: This is normal. The DuckDuckGo API has limitations. For production, integrate a premium search API.

## Next Steps

1. **Read the full README**: See `README.md` for detailed documentation
2. **Explore the paper**: Read arXiv:2507.16075 to understand the theory
3. **Customize prompts**: Edit `src/prompts.py` to adjust behavior
4. **Tune configuration**: Experiment with `config/config.yaml` settings
5. **Try different queries**: Test various research topics

## Sample Research Queries

Here are some interesting queries to try:

**Technology:**
- "What are the key challenges in developing safe AGI?"
- "Compare the architectures of GPT-4 and Claude 3"
- "Explain the latest developments in quantum computing"

**Science:**
- "What are the most promising approaches to cancer treatment in 2025?"
- "How does mRNA vaccine technology work?"
- "Analyze the potential of nuclear fusion for clean energy"

**Business:**
- "What are the trends in remote work and their impact on productivity?"
- "Analyze the business model of successful SaaS companies"
- "How is AI transforming the financial services industry?"

**Society:**
- "What are the ethical implications of facial recognition technology?"
- "Analyze the impact of social media on mental health"
- "How can education systems adapt to AI and automation?"

## Performance Expectations

- **Quick research** (5 steps): ~3-5 minutes
- **Standard research** (20 steps): ~10-15 minutes
- **Deep research** (30 steps with evolution): ~20-30 minutes

Time varies based on:
- LLM response speed
- Number of revision steps
- Self-evolution settings
- Search tool performance

## Tips for Best Results

1. **Be specific**: "Applications of transformers in NLP" vs "Tell me about AI"
2. **Complex queries**: TTD-DR shines on multi-faceted research questions
3. **Adjust depth**: Use fewer steps for simple queries, more for complex ones
4. **Review intermediates**: Check `drafts_*.md` to see how the report evolved
5. **Experiment**: Try different configurations to find what works for you

## Getting Help

- **Documentation**: See `README.md`
- **Configuration**: See comments in `config/config.yaml`
- **Code**: All code is documented in `src/`
- **Paper**: Read arXiv:2507.16075 for theoretical background

## What's Next?

You're ready to use TTD-DR! Try running some research queries and explore the outputs.

Happy researching! 🔬✨
