#!/usr/bin/env node
/**
 * TTD-DR CLI Application
 * Test-Time Diffusion Deep Researcher
 *
 * Based on arXiv:2507.16075: Deep Researcher with Test-Time Diffusion
 * by Han et al. (Google Cloud AI Research)
 */

import { Command } from 'commander';
import chalk from 'chalk';
import * as dotenv from 'dotenv';
import * as readline from 'readline';
import { TTDDRAgent } from './ttd-dr-agent';
import { createLLMClient } from './llm-client';
import { EnhancedSearchTool } from './enhanced-search-tool';
import { loadConfig, saveResearchSession, setupLogging, configToAgentConfig } from './utils';
import { LogLevel } from './types';

// Load environment variables
dotenv.config();

function printBanner(): void {
  const banner = `
${chalk.cyan('╔══════════════════════════════════════════════════════════════╗')}
${chalk.cyan('║                                                              ║')}
${chalk.cyan('║           TTD-DR: Test-Time Diffusion Deep Researcher       ║')}
${chalk.cyan('║                                                              ║')}
${chalk.cyan('║   Implementation of arXiv:2507.16075                        ║')}
${chalk.cyan('║   Deep Researcher with Test-Time Diffusion                  ║')}
${chalk.cyan('║                                                              ║')}
${chalk.cyan('╚══════════════════════════════════════════════════════════════╝')}
  `;
  console.log(banner);
}

function printStage(stage: string, message: string): void {
  console.log(`\n${chalk.green(`[${stage}]`)} ${message}`);
}

function printInfo(message: string): void {
  console.log(`${chalk.yellow('ℹ')}  ${message}`);
}

function printSuccess(message: string): void {
  console.log(`${chalk.green('✓')}  ${message}`);
}

function printError(message: string): void {
  console.log(`${chalk.red('✗')}  ${message}`);
}

async function runResearch(query: string, configPath: string = 'config/config.yaml'): Promise<boolean> {
  try {
    // Load configuration
    printInfo(`Loading configuration from ${configPath}`);
    const config = loadConfig(configPath);

    // Setup logging
    const logger = setupLogging(
      config.logging.file,
      LogLevel[config.logging.level]
    );

    // Create LLM client
    printInfo(`Initializing ${config.llm.provider} with model ${config.llm.model}`);
    const apiKey = process.env[config.llm.apiKeyEnv];
    if (!apiKey) {
      throw new Error(`API key not found in environment variable: ${config.llm.apiKeyEnv}`);
    }

    const llmClient = createLLMClient(
      config.llm.provider,
      config.llm.model,
      apiKey
    );

    // Create enhanced search tool
    printInfo(`Initializing ${config.search.provider} search provider`);
    const searchTool = new EnhancedSearchTool(config.search);

    // Create agent configuration
    const agentConfig = configToAgentConfig(config);

    // Create TTD-DR agent
    printInfo('Creating TTD-DR agent...');
    const agent = new TTDDRAgent(llmClient, searchTool, agentConfig, logger);

    // Run research
    console.log(`\n${chalk.cyan('='.repeat(70))}`);
    console.log(`${chalk.cyan('Research Query:')} ${query}`);
    console.log(`${chalk.cyan('='.repeat(70))}\n`);

    printStage('START', 'Beginning research process...');

    const [finalReport, state] = await agent.research(query);

    // Save outputs
    printStage('SAVE', 'Saving research outputs...');
    const savedFiles = saveResearchSession(
      query,
      finalReport,
      state,
      config.output.directory,
      config.output.saveIntermediate
    );

    // Print results
    console.log(`\n${chalk.cyan('='.repeat(70))}`);
    console.log(`${chalk.cyan('FINAL REPORT')}`);
    console.log(`${chalk.cyan('='.repeat(70))}\n`);
    console.log(finalReport);
    console.log(`\n${chalk.cyan('='.repeat(70))}\n`);

    // Print statistics
    printSuccess('Research completed successfully!');
    printInfo(`Total searches performed: ${state.qaPairs.length}`);
    printInfo(`Total draft revisions: ${state.revisionHistory.length}`);
    printInfo('\nSaved files:');
    Object.entries(savedFiles).forEach(([fileType, filepath]) => {
      console.log(`  • ${fileType}: ${filepath}`);
    });

    return true;
  } catch (error) {
    printError(`Error during research: ${error}`);
    console.error(error);
    return false;
  }
}

async function interactiveMode(configPath: string): Promise<void> {
  console.log(`${chalk.yellow('Interactive Mode')}`);
  console.log("Enter your research queries (or 'quit' to exit)\n");

  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
    prompt: `${chalk.green('Query > ')}`,
  });

  rl.prompt();

  rl.on('line', async (line: string) => {
    const query = line.trim();

    if (['quit', 'exit', 'q'].includes(query.toLowerCase())) {
      printInfo('Exiting...');
      rl.close();
      return;
    }

    if (!query) {
      rl.prompt();
      return;
    }

    await runResearch(query, configPath);
    rl.prompt();
  });

  rl.on('close', () => {
    process.exit(0);
  });
}

// CLI Program
const program = new Command();

program
  .name('ttd-dr')
  .description('TTD-DR: Test-Time Diffusion Deep Researcher')
  .version('1.0.0')
  .argument('[query]', 'Research query')
  .option('-c, --config <path>', 'Path to configuration file', 'config/config.yaml')
  .option('-i, --interactive', 'Run in interactive mode')
  .action(async (query: string | undefined, options: any) => {
    printBanner();

    if (options.interactive) {
      await interactiveMode(options.config);
    } else if (query) {
      const success = await runResearch(query, options.config);
      process.exit(success ? 0 : 1);
    } else {
      printError('No query provided!');
      console.log('\nUsage:');
      console.log('  npx ts-node src/main.ts "Your research query here"');
      console.log('  npm run dev -- "Your research query here"');
      console.log('  npm run dev -- --interactive');
      console.log('\nFor more help: npm run dev -- --help');
      process.exit(1);
    }
  });

program.parse();
