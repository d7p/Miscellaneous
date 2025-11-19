/**
 * Utility functions for TTD-DR
 */

import * as fs from 'fs';
import * as path from 'path';
import * as yaml from 'js-yaml';
import { Config, AgentState, SavedFiles, ResearchMetadata, AgentConfig, LogLevel } from './types';

/**
 * Load configuration from YAML file
 */
export function loadConfig(configPath: string): Config {
  const fileContents = fs.readFileSync(configPath, 'utf8');
  return yaml.load(fileContents) as Config;
}

/**
 * Convert Config to AgentConfig
 */
export function configToAgentConfig(config: Config): AgentConfig {
  return {
    maxRevisionSteps: config.algorithm.maxRevisionSteps,
    nPlan: config.algorithm.selfEvolution.nPlan,
    nQuery: config.algorithm.selfEvolution.nQuery,
    nAnswer: config.algorithm.selfEvolution.nAnswer,
    nReport: config.algorithm.selfEvolution.nReport,
    sPlan: config.algorithm.selfEvolution.sPlan,
    sQuery: config.algorithm.selfEvolution.sQuery,
    sAnswer: config.algorithm.selfEvolution.sAnswer,
    sReport: config.algorithm.selfEvolution.sReport,
    saveIntermediate: config.output.saveIntermediate,
  };
}

/**
 * Save content to file
 */
export function saveOutput(
  content: string,
  filename: string,
  outputDir: string = 'output',
  format: string = 'markdown'
): string {
  // Create output directory if it doesn't exist
  if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
  }

  // Add timestamp to filename
  const timestamp = new Date().toISOString().replace(/:/g, '-').replace(/\..+/, '');
  const baseName = filename.replace(/\s+/g, '_').replace(/[?/]/g, '');

  // Determine file extension
  let ext = '.txt';
  if (format === 'markdown') ext = '.md';
  else if (format === 'json') ext = '.json';

  const filepath = path.join(outputDir, `${baseName}_${timestamp}${ext}`);

  // Save content
  fs.writeFileSync(filepath, content, 'utf8');

  return filepath;
}

/**
 * Save complete research session
 */
export function saveResearchSession(
  query: string,
  finalReport: string,
  state: AgentState,
  outputDir: string = 'output',
  saveIntermediate: boolean = true
): SavedFiles {
  const savedFiles: Partial<SavedFiles> = {};

  // Save final report
  savedFiles.report = saveOutput(
    finalReport,
    `report_${query.substring(0, 30)}`,
    outputDir,
    'markdown'
  );

  // Save research plan
  const planContent = `# Research Plan\n\nQuery: ${query}\n\n${state.plan}`;
  savedFiles.plan = saveOutput(
    planContent,
    `plan_${query.substring(0, 30)}`,
    outputDir,
    'markdown'
  );

  // Save search history
  let searchHistory = '# Search History\n\n';
  state.qaPairs.forEach(([q, a], i) => {
    searchHistory += `## Search ${i + 1}\n\n`;
    searchHistory += `**Question:** ${q}\n\n`;
    searchHistory += `**Answer:**\n${a}\n\n`;
    searchHistory += '---\n\n';
  });

  savedFiles.searchHistory = saveOutput(
    searchHistory,
    `search_history_${query.substring(0, 30)}`,
    outputDir,
    'markdown'
  );

  // Save intermediate drafts if enabled
  if (saveIntermediate && state.revisionHistory.length > 0) {
    let draftsContent = '# Draft Revision History\n\n';
    state.revisionHistory.forEach((draft, i) => {
      draftsContent += `## Draft ${i} (R${i})\n\n`;
      draftsContent += draft + '\n\n';
      draftsContent += '='.repeat(80) + '\n\n';
    });

    savedFiles.drafts = saveOutput(
      draftsContent,
      `drafts_${query.substring(0, 30)}`,
      outputDir,
      'markdown'
    );
  }

  // Save metadata as JSON
  const metadata: ResearchMetadata = {
    query,
    timestamp: new Date().toISOString(),
    numSearches: state.qaPairs.length,
    numRevisions: state.revisionHistory.length,
    files: savedFiles as SavedFiles,
  };

  savedFiles.metadata = saveOutput(
    JSON.stringify(metadata, null, 2),
    `metadata_${query.substring(0, 30)}`,
    outputDir,
    'json'
  );

  return savedFiles as SavedFiles;
}

/**
 * Logger class
 */
export class Logger {
  private level: LogLevel;
  private logFile?: string;

  constructor(level: LogLevel = LogLevel.INFO, logFile?: string) {
    this.level = level;
    this.logFile = logFile;
  }

  private log(level: LogLevel, message: string): void {
    const levels = [LogLevel.DEBUG, LogLevel.INFO, LogLevel.WARNING, LogLevel.ERROR];
    const currentLevelIndex = levels.indexOf(this.level);
    const messageLevelIndex = levels.indexOf(level);

    if (messageLevelIndex >= currentLevelIndex) {
      const timestamp = new Date().toISOString();
      const logMessage = `${timestamp} - ${level} - ${message}`;

      // Console output
      console.log(logMessage);

      // File output
      if (this.logFile) {
        const logDir = path.dirname(this.logFile);
        if (logDir && !fs.existsSync(logDir)) {
          fs.mkdirSync(logDir, { recursive: true });
        }
        fs.appendFileSync(this.logFile, logMessage + '\n', 'utf8');
      }
    }
  }

  debug(message: string): void {
    this.log(LogLevel.DEBUG, message);
  }

  info(message: string): void {
    this.log(LogLevel.INFO, message);
  }

  warning(message: string): void {
    this.log(LogLevel.WARNING, message);
  }

  error(message: string): void {
    this.log(LogLevel.ERROR, message);
  }
}

/**
 * Setup logging
 */
export function setupLogging(logFile: string = 'ttd-dr.log', level: LogLevel = LogLevel.INFO): Logger {
  return new Logger(level, logFile);
}
