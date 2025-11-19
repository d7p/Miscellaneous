/**
 * Search Tool for TTD-DR
 * Implements web search functionality (Stage 2b - Answer Searching)
 */

import fetch from 'node-fetch';
import { SearchResult } from './types';

/**
 * Web search tool using DuckDuckGo
 */
export class SearchTool {
  private maxResults: number;

  constructor(maxResults: number = 5) {
    this.maxResults = maxResults;
  }

  /**
   * Perform web search and return results
   * This simulates the search tool mentioned in Stage 2b
   */
  async search(query: string): Promise<SearchResult[]> {
    try {
      // Using DuckDuckGo Instant Answer API (free, no API key needed)
      const url = `https://api.duckduckgo.com/?q=${encodeURIComponent(query)}&format=json`;
      const response = await fetch(url);
      const data = await response.json() as any;

      const results: SearchResult[] = [];

      // Get related topics
      if (data.RelatedTopics && Array.isArray(data.RelatedTopics)) {
        for (const topic of data.RelatedTopics.slice(0, this.maxResults)) {
          if (topic.Text && topic.FirstURL) {
            results.push({
              title: topic.Text.substring(0, 100),
              snippet: topic.Text,
              url: topic.FirstURL,
            });
          }
        }
      }

      // If no results from RelatedTopics, use Abstract
      if (results.length === 0 && data.AbstractText) {
        results.push({
          title: data.Heading || query,
          snippet: data.AbstractText,
          url: data.AbstractURL || '',
        });
      }

      // Fallback: create a mock result if no real results
      if (results.length === 0) {
        results.push({
          title: `Search results for: ${query}`,
          snippet: `Information about ${query}. This is a simulated search result. ` +
                   `In production, this would connect to a real search API like Google Search API.`,
          url: 'https://example.com',
        });
      }

      return results;
    } catch (error) {
      console.error(`Search error: ${error}`);
      // Return a fallback result
      return [{
        title: `Search: ${query}`,
        snippet: `[Search functionality would provide real-time information about ${query}]`,
        url: 'https://example.com',
      }];
    }
  }

  /**
   * Format search results as text
   */
  formatResults(results: SearchResult[]): string {
    const formatted: string[] = [];

    results.forEach((result, index) => {
      formatted.push(`\n[Result ${index + 1}]`);
      formatted.push(`Title: ${result.title}`);
      formatted.push(`URL: ${result.url}`);
      formatted.push(`Snippet: ${result.snippet}\n`);
    });

    return formatted.join('\n');
  }
}
