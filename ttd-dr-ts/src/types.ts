/**
 * Type definitions for TTD-DR
 * Based on arXiv:2507.16075
 */

export interface LLMConfig {
  provider: 'openai' | 'anthropic';
  model: string;
  apiKeyEnv: string;
  temperature: number;
  maxTokens: number;
}

export interface SearchConfig {
  provider: string;
  maxResults: number;
}

export interface SelfEvolutionConfig {
  nPlan: number;
  nQuery: number;
  nAnswer: number;
  nReport: number;
  sPlan: number;
  sQuery: number;
  sAnswer: number;
  sReport: number;
}

export interface AlgorithmConfig {
  maxRevisionSteps: number;
  selfEvolution: SelfEvolutionConfig;
}

export interface OutputConfig {
  directory: string;
  saveIntermediate: boolean;
  saveSearchHistory: boolean;
  format: 'markdown' | 'txt' | 'json';
}

export interface LoggingConfig {
  level: 'DEBUG' | 'INFO' | 'WARNING' | 'ERROR';
  file: string;
}

export interface Config {
  llm: LLMConfig;
  search: SearchConfig;
  algorithm: AlgorithmConfig;
  output: OutputConfig;
  logging: LoggingConfig;
}

export interface AgentConfig {
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

export interface AgentState {
  query: string;
  plan: string;
  draft: string;
  qaPairs: Array<[string, string]>;
  revisionHistory: string[];
}

export interface SearchResult {
  title: string;
  snippet: string;
  url: string;
}

export interface SavedFiles {
  report: string;
  plan: string;
  searchHistory: string;
  drafts?: string;
  metadata: string;
}

export interface ResearchMetadata {
  query: string;
  timestamp: string;
  numSearches: number;
  numRevisions: number;
  files: SavedFiles;
}

export enum LogLevel {
  DEBUG = 'DEBUG',
  INFO = 'INFO',
  WARNING = 'WARNING',
  ERROR = 'ERROR'
}
