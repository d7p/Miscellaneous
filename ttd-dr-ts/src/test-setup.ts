#!/usr/bin/env ts-node
/**
 * Test script to verify TTD-DR installation and setup
 */

import * as fs from 'fs';
import * as path from 'path';
import * as dotenv from 'dotenv';

function testImports(): boolean {
  console.log('Testing imports...');
  let allPassed = true;

  try {
    require('commander');
    console.log('✓ commander');
  } catch {
    console.log('✗ commander - Run: npm install');
    allPassed = false;
  }

  try {
    require('chalk');
    console.log('✓ chalk');
  } catch {
    console.log('✗ chalk - Run: npm install');
    allPassed = false;
  }

  try {
    require('js-yaml');
    console.log('✓ js-yaml');
  } catch {
    console.log('✗ js-yaml - Run: npm install');
    allPassed = false;
  }

  try {
    require('dotenv');
    console.log('✓ dotenv');
  } catch {
    console.log('✗ dotenv - Run: npm install');
    allPassed = false;
  }

  try {
    require('node-fetch');
    console.log('✓ node-fetch');
  } catch {
    console.log('✗ node-fetch - Run: npm install');
    allPassed = false;
  }

  try {
    require('openai');
    console.log('✓ openai');
  } catch {
    console.log('✗ openai - Run: npm install');
    allPassed = false;
  }

  try {
    require('@anthropic-ai/sdk');
    console.log('✓ @anthropic-ai/sdk');
  } catch {
    console.log('✗ @anthropic-ai/sdk - Run: npm install');
    allPassed = false;
  }

  return allPassed;
}

function testProjectStructure(): boolean {
  console.log('\nTesting project structure...');

  const requiredFiles = [
    'config/config.yaml',
    'src/types.ts',
    'src/llm-client.ts',
    'src/search-tool.ts',
    'src/prompts.ts',
    'src/ttd-dr-agent.ts',
    'src/utils.ts',
    'src/main.ts',
    'src/index.ts',
    'package.json',
    'tsconfig.json',
  ];

  let allExist = true;
  for (const filepath of requiredFiles) {
    if (fs.existsSync(filepath)) {
      console.log(`✓ ${filepath}`);
    } else {
      console.log(`✗ ${filepath} - Missing!`);
      allExist = false;
    }
  }

  return allExist;
}

function testEnvFile(): boolean {
  console.log('\nTesting environment configuration...');

  if (!fs.existsSync('.env')) {
    console.log('✗ .env file not found');
    console.log('  Create it: cp .env.example .env');
    console.log('  Then add your API key');
    return false;
  }

  console.log('✓ .env file exists');

  dotenv.config();

  const openaiKey = process.env.OPENAI_API_KEY;
  const anthropicKey = process.env.ANTHROPIC_API_KEY;

  if (openaiKey && openaiKey !== 'your_openai_api_key_here') {
    console.log('✓ OPENAI_API_KEY is set');
    return true;
  } else if (anthropicKey && anthropicKey !== 'your_anthropic_api_key_here') {
    console.log('✓ ANTHROPIC_API_KEY is set');
    return true;
  } else {
    console.log('✗ No valid API key found in .env');
    console.log('  Add either OPENAI_API_KEY or ANTHROPIC_API_KEY');
    return false;
  }
}

function testSrcModule(): boolean {
  console.log('\nTesting src module...');

  try {
    require('./index');
    console.log('✓ Core modules imported successfully');
    return true;
  } catch (error) {
    console.log(`✗ Error importing src modules: ${error}`);
    return false;
  }
}

function main(): number {
  console.log('='.repeat(70));
  console.log('TTD-DR Setup Test');
  console.log('='.repeat(70));

  const tests: Array<[string, () => boolean]> = [
    ['Required Node packages', testImports],
    ['Project structure', testProjectStructure],
    ['Environment configuration', testEnvFile],
    ['Source modules', testSrcModule],
  ];

  const results: Array<[string, boolean]> = [];

  for (const [testName, testFunc] of tests) {
    console.log(`\n${'='.repeat(70)}`);
    console.log(`Test: ${testName}`);
    console.log('='.repeat(70));
    const result = testFunc();
    results.push([testName, result]);
  }

  // Summary
  console.log(`\n${'='.repeat(70)}`);
  console.log('SUMMARY');
  console.log('='.repeat(70));

  let allPassed = true;
  for (const [testName, result] of results) {
    const status = result ? '✓ PASS' : '✗ FAIL';
    console.log(`${status} - ${testName}`);
    if (!result) {
      allPassed = false;
    }
  }

  console.log('='.repeat(70));

  if (allPassed) {
    console.log("\n✓ All tests passed! You're ready to use TTD-DR.");
    console.log('\nNext steps:');
    console.log('  1. Try: npm run dev -- "What is quantum computing?"');
    console.log('  2. Or: npm run dev -- --interactive');
    console.log('  3. Or: npx ts-node src/example.ts');
    console.log('\nSee README.md for more information.');
    return 0;
  } else {
    console.log('\n✗ Some tests failed. Please fix the issues above.');
    console.log('\nCommon solutions:');
    console.log('  • Install dependencies: npm install');
    console.log('  • Create .env file: cp .env.example .env');
    console.log('  • Add API key to .env file');
    return 1;
  }
}

if (require.main === module) {
  process.exit(main());
}
