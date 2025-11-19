# TTD-DR: Test-Time Diffusion Deep Researcher (TypeScript)

A complete TypeScript implementation of the **Test-Time Diffusion Deep Researcher (TTD-DR)** framework from the paper:

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
- 💪 **Full TypeScript**: Strong typing, modern async/await, and ES2020+ features

## Installation

### Requirements

- Node.js 18.0.0 or higher
- npm or yarn
- OpenAI API key or Anthropic API key

### Setup

1. **Navigate to the project directory:**
   ```bash
   cd ttd-dr-ts
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Configure API keys:**
   Create a `.env` file:
   ```bash
   cp .env.example .env
   ```

   Edit `.env` and add your API key:
   ```bash
   # For OpenAI
   OPENAI_API_KEY=sk-your-key-here

   # Or for Anthropic
   ANTHROPIC_API_KEY=sk-ant-your-key-here
   ```

4. **Build the project:**
   ```bash
   npm run build
   ```

## Usage

### Development Mode (with ts-node)

```bash
# Basic query
npm run dev -- "What are the latest developments in quantum computing?"

# Interactive mode
npm run dev -- --interactive

# Custom config
npm run dev -- --config my-config.yaml "Your query"
```

### Production Mode (compiled)

```bash
# Build first
npm run build

# Run
node dist/main.js "Your research query"
```

### Programmatic Usage

```typescript
import { TTDDRAgent, createLLMClient, SearchTool, setupLogging } from './src';
import { AgentConfig, LogLevel } from './src/types';

// Configure the agent
const config: AgentConfig = {
  maxRevisionSteps: 10,
  nPlan: 1,
  nQuery: 5,
  nAnswer: 3,
  nReport: 1,
  sPlan: 1,
  sQuery: 0,
  sAnswer: 0,
  sReport: 1,
  saveIntermediate: true,
};

// Create components
const llmClient = createLLMClient('openai', 'gpt-4', process.env.OPENAI_API_KEY);
const searchTool = new SearchTool(5);
const logger = setupLogging('ttd-dr.log', LogLevel.INFO);

// Create and run agent
const agent = new TTDDRAgent(llmClient, searchTool, config, logger);
const [report, state] = await agent.research('Your query here');

console.log(report);
```

See `src/example.ts` for a complete example.

## Configuration

Edit `config/config.yaml` to customize:

```yaml
llm:
  provider: "openai"  # or "anthropic"
  model: "gpt-4"
  apiKeyEnv: "OPENAI_API_KEY"
  temperature: 0.7
  maxTokens: 4096

algorithm:
  maxRevisionSteps: 20  # N in Algorithm 1
  selfEvolution:
    nQuery: 5           # Number of query variants
    nAnswer: 3          # Number of answer variants
    sPlan: 1            # Plan evolution steps
    sReport: 1          # Report evolution steps
```

## Scripts

```bash
npm run build      # Compile TypeScript to JavaScript
npm run dev        # Run in development mode with ts-node
npm run start      # Run compiled version
npm test           # Run setup tests
npm run clean      # Remove dist directory
npm run rebuild    # Clean and build
```

## Project Structure

```
ttd-dr-ts/
├── src/
│   ├── types.ts           # TypeScript type definitions
│   ├── llm-client.ts      # LLM client abstraction
│   ├── search-tool.ts     # Web search functionality
│   ├── prompts.ts         # All prompts for each stage
│   ├── ttd-dr-agent.ts    # Core TTD-DR agent
│   ├── utils.ts           # Utilities and output management
│   ├── index.ts           # Main exports
│   ├── main.ts            # CLI application
│   ├── example.ts         # Programmatic usage example
│   └── test-setup.ts      # Installation verification
├── config/
│   └── config.yaml        # Configuration file
├── dist/                  # Compiled JavaScript (generated)
├── output/                # Research outputs (generated)
├── package.json
├── tsconfig.json
├── .env.example
└── README.md
```

## TypeScript Features

This implementation leverages modern TypeScript features:

- **Strong Typing**: All interfaces and types defined in `src/types.ts`
- **Async/Await**: Promise-based async operations throughout
- **Abstract Classes**: `LLMClient` base class for provider abstraction
- **Strict Mode**: Full TypeScript strict mode enabled
- **ES Modules**: Modern import/export syntax
- **Type Inference**: Extensive use of type inference for cleaner code

## Type Definitions

Key types are defined in `src/types.ts`:

```typescript
interface AgentConfig {
  maxRevisionSteps: number;
  nPlan: number;
  nQuery: number;
  nAnswer: number;
  nReport: number;
  sPlan: number;
  sQuery: number;
  sAnswer: number;
  sReport: number;
  saveIntermediate: boolean;
}

interface AgentState {
  query: string;
  plan: string;
  draft: string;
  qaPairs: Array<[string, string]>;
  revisionHistory: string[];
}
```

## Examples

### Technology Research
```bash
npm run dev -- "What are the key challenges in developing AGI?"
```

### Business Analysis
```bash
npm run dev -- "Analyze the impact of remote work on startup culture"
```

### Scientific Investigation
```bash
npm run dev -- "What are the current approaches to extending human lifespan?"
```

## Output Files

After each research session, TTD-DR generates:

```
output/
├── report_your_query_2025-11-19T21-50-00.md
├── plan_your_query_2025-11-19T21-50-00.md
├── search_history_your_query_2025-11-19T21-50-00.md
├── drafts_your_query_2025-11-19T21-50-00.md
└── metadata_your_query_2025-11-19T21-50-00.json
```

## Testing

Verify your setup:

```bash
npm test
```

This checks:
- All dependencies are installed
- Project structure is correct
- Environment is configured
- Modules can be imported

## Customization

### Adding a New LLM Provider

Extend the `LLMClient` abstract class:

```typescript
export class CustomLLMClient extends LLMClient {
  async generate(prompt: string, temperature?: number, maxTokens?: number): Promise<string> {
    // Your implementation
  }

  async generateMultiple(prompt: string, n: number, temperature?: number, maxTokens?: number): Promise<string[]> {
    // Your implementation
  }
}
```

### Customizing Prompts

Edit methods in `src/prompts.ts`:

```typescript
static stage1ResearchPlan(query: string): string {
  return `Your custom prompt here...`;
}
```

## Performance Notes

- **Token Usage**: Deep research with self-evolution can be expensive (many LLM calls)
- **Time**: Each research session takes 10-30 minutes depending on configuration
- **Optimization**: Reduce `maxRevisionSteps` or disable self-evolution for faster results

## Comparison with Python Version

| Feature | TypeScript | Python |
|---------|-----------|--------|
| **Type Safety** | ✅ Full static typing | ⚠️ Optional (type hints) |
| **Performance** | ✅ Faster startup | ⚠️ Slower startup |
| **Ecosystem** | ✅ npm packages | ✅ pip packages |
| **Async** | ✅ Native async/await | ✅ asyncio |
| **Deployment** | ✅ Easy with Node.js | ✅ Easy with venv |

## Troubleshooting

### "Cannot find module 'openai'"
```bash
npm install
```

### "OPENAI_API_KEY is not defined"
```bash
cp .env.example .env
# Edit .env and add your API key
```

### TypeScript compilation errors
```bash
npm run clean
npm install
npm run build
```

## Paper Reference

```bibtex
@article{han2025deepresearcher,
  title={Deep Researcher with Test-Time Diffusion},
  author={Han, Rujun and Chen, Yanfei and CuiZhu, Zoey and others},
  journal={arXiv preprint arXiv:2507.16075},
  year={2025}
}
```

## License

MIT

## Acknowledgments

Based on the paper "Deep Researcher with Test-Time Diffusion" by Han et al., Google Cloud AI Research (arXiv:2507.16075).

---

**Note**: This is an independent TypeScript implementation based on the published paper. It is not affiliated with Google or the original authors.
