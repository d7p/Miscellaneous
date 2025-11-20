#!/usr/bin/env ts-node
/**
 * Example usage of TTD-DR programmatically
 */

import * as dotenv from 'dotenv';
import { TTDDRAgent } from './ttd-dr-agent';
import { createLLMClient } from './llm-client';
import { SearchTool } from './search-tool';
import { saveResearchSession, setupLogging } from './utils';
import { AgentConfig, LogLevel } from './types';

// Load environment variables
dotenv.config();

async function main(): Promise<void> {
  console.log('='.repeat(70));
  console.log('TTD-DR Example: Programmatic Usage');
  console.log('='.repeat(70));

  // Configure the agent
  const config: AgentConfig = {
    maxRevisionSteps: 5, // Fewer steps for faster demo
    nPlan: 1,
    nQuery: 3, // 3 query variants
    nAnswer: 2, // 2 answer variants
    nReport: 1,
    sPlan: 1,
    sQuery: 0,
    sAnswer: 0,
    sReport: 1,
    saveIntermediate: true,
  };

  // Create LLM client (using OpenAI GPT-4 by default)
  console.log('\n[1] Creating LLM client...');
  const apiKey = process.env.OPENAI_API_KEY;
  if (!apiKey) {
    throw new Error('OPENAI_API_KEY not found in environment');
  }

  const llmClient = createLLMClient('openai', 'gpt-4', apiKey);

  // Create search tool
  console.log('[2] Creating search tool...');
  const searchTool = new SearchTool(5);

  // Setup logger
  console.log('[3] Setting up logger...');
  const logger = setupLogging('ttd-dr.log', LogLevel.INFO);

  // Create TTD-DR agent
  console.log('[4] Creating TTD-DR agent...');
  const agent = new TTDDRAgent(llmClient, searchTool, config, logger);

  // Define research query
  const query = 'What are the main applications of large language models in 2025?';

  console.log(`\n[5] Starting research on query:\n    '${query}'`);
  console.log('\nThis may take several minutes...\n');

  // Run research
  const [finalReport, state] = await agent.research(query);

  // Save results
  console.log('\n[6] Saving research outputs...');
  const savedFiles = saveResearchSession(
    query,
    finalReport,
    state,
    'output',
    true
  );

  // Display results
  console.log('\n' + '='.repeat(70));
  console.log('FINAL REPORT');
  console.log('='.repeat(70) + '\n');
  console.log(finalReport);
  console.log('\n' + '='.repeat(70) + '\n');

  // Display statistics
  console.log('✓ Research completed!');
  console.log(`  • Total searches: ${state.qaPairs.length}`);
  console.log(`  • Draft revisions: ${state.revisionHistory.length}`);
  console.log('\n✓ Files saved:');
  Object.entries(savedFiles).forEach(([fileType, filepath]) => {
    console.log(`  • ${fileType}: ${filepath}`);
  });

  console.log('\n' + '='.repeat(70));
  console.log('Example completed successfully!');
  console.log('='.repeat(70));
}

if (require.main === module) {
  main()
    .then(() => process.exit(0))
    .catch((error) => {
      console.error('\n\nError:', error);
      process.exit(1);
    });
}
