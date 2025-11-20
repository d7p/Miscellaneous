/**
 * Search interface for TTD-DR agents
 */

import { SearchResult } from './types';

/**
 * Common interface for all search tools
 */
export interface ISearchTool {
  search(query: string): Promise<SearchResult[]>;
  formatResults(results: SearchResult[]): string;
}
