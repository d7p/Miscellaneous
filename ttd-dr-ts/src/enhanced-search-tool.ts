/**
 * Enhanced Search Tool for TTD-DR
 * Supports multiple search providers: SerpAPI, Playwright browsing, and DuckDuckGo
 */

import fetch from 'node-fetch';
import { chromium, firefox, webkit, Browser, Page } from 'playwright';
import * as cheerio from 'cheerio';
import TurndownService from 'turndown';
import { SearchResult, SearchConfig } from './types';
import { ISearchTool } from './search-interface';

/**
 * Abstract base class for search providers
 */
export abstract class SearchProvider {
  protected maxResults: number;

  constructor(maxResults: number = 5) {
    this.maxResults = maxResults;
  }

  abstract search(query: string): Promise<SearchResult[]>;
}

/**
 * SerpAPI search provider (Google, Bing, etc.)
 * Requires API key from https://serpapi.com
 */
export class SerpAPIProvider extends SearchProvider {
  private apiKey: string;
  private engine: string;

  constructor(apiKey: string, engine: string = 'google', maxResults: number = 5) {
    super(maxResults);
    this.apiKey = apiKey;
    this.engine = engine;
  }

  async search(query: string): Promise<SearchResult[]> {
    try {
      const params = new URLSearchParams({
        q: query,
        api_key: this.apiKey,
        engine: this.engine,
        num: this.maxResults.toString(),
      });

      const url = `https://serpapi.com/search?${params.toString()}`;
      const response = await fetch(url);
      const data = await response.json() as any;

      const results: SearchResult[] = [];

      // Parse organic results
      if (data.organic_results && Array.isArray(data.organic_results)) {
        for (const result of data.organic_results.slice(0, this.maxResults)) {
          results.push({
            title: result.title || '',
            snippet: result.snippet || result.description || '',
            url: result.link || '',
          });
        }
      }

      // If no results, check answer box
      if (results.length === 0 && data.answer_box) {
        results.push({
          title: data.answer_box.title || query,
          snippet: data.answer_box.snippet || data.answer_box.answer || '',
          url: data.answer_box.link || '',
        });
      }

      return results;
    } catch (error) {
      console.error(`SerpAPI search error: ${error}`);
      return this.getFallbackResults(query);
    }
  }

  private getFallbackResults(query: string): SearchResult[] {
    return [{
      title: `Search: ${query}`,
      snippet: `[SerpAPI search would provide results for ${query}. Please check your API key and connection.]`,
      url: 'https://serpapi.com',
    }];
  }
}

/**
 * Playwright-based web browsing and content extraction
 */
export class PlaywrightProvider extends SearchProvider {
  private browserType: 'chromium' | 'firefox' | 'webkit';
  private headless: boolean;
  private timeout: number;
  private extractContent: boolean;
  private maxContentLength: number;
  private turndownService: TurndownService;

  constructor(
    browserType: 'chromium' | 'firefox' | 'webkit' = 'chromium',
    headless: boolean = true,
    timeout: number = 30000,
    extractContent: boolean = true,
    maxContentLength: number = 10000,
    maxResults: number = 5
  ) {
    super(maxResults);
    this.browserType = browserType;
    this.headless = headless;
    this.timeout = timeout;
    this.extractContent = extractContent;
    this.maxContentLength = maxContentLength;
    this.turndownService = new TurndownService({
      headingStyle: 'atx',
      codeBlockStyle: 'fenced',
    });
  }

  async search(query: string): Promise<SearchResult[]> {
    let browser: Browser | null = null;
    try {
      // Launch browser
      const browserEngine = this.getBrowserEngine();
      browser = await browserEngine.launch({ headless: this.headless });
      const context = await browser.newContext();
      const page = await context.newPage();

      // Search on Google
      const searchUrl = `https://www.google.com/search?q=${encodeURIComponent(query)}`;
      await page.goto(searchUrl, { timeout: this.timeout });

      // Extract search results
      const results = await this.extractSearchResults(page);

      // Optionally extract content from pages
      if (this.extractContent && results.length > 0) {
        const enhancedResults: SearchResult[] = [];
        for (const result of results.slice(0, Math.min(3, this.maxResults))) {
          const enhanced = await this.extractPageContent(result, browser);
          enhancedResults.push(enhanced);
        }
        await browser.close();
        return enhancedResults;
      }

      await browser.close();
      return results;
    } catch (error) {
      console.error(`Playwright search error: ${error}`);
      if (browser) {
        await browser.close();
      }
      return this.getFallbackResults(query);
    }
  }

  private getBrowserEngine() {
    switch (this.browserType) {
      case 'firefox':
        return firefox;
      case 'webkit':
        return webkit;
      default:
        return chromium;
    }
  }

  private async extractSearchResults(page: Page): Promise<SearchResult[]> {
    const results: SearchResult[] = [];

    try {
      // Wait for search results to load
      await page.waitForSelector('div#search', { timeout: this.timeout });

      // Extract results using Cheerio
      const html = await page.content();
      const $ = cheerio.load(html);

      // Google search result selectors
      $('div.g').each((i, elem) => {
        if (results.length >= this.maxResults) return;

        const $elem = $(elem);
        const titleElem = $elem.find('h3').first();
        const linkElem = $elem.find('a').first();
        const snippetElem = $elem.find('div[data-sncf], div.VwiC3b').first();

        const title = titleElem.text().trim();
        const url = linkElem.attr('href') || '';
        const snippet = snippetElem.text().trim();

        if (title && url) {
          results.push({ title, snippet, url });
        }
      });
    } catch (error) {
      console.error(`Error extracting search results: ${error}`);
    }

    return results;
  }

  private async extractPageContent(result: SearchResult, browser: Browser): Promise<SearchResult> {
    try {
      const page = await browser.newPage();
      await page.goto(result.url, { timeout: this.timeout, waitUntil: 'domcontentloaded' });

      // Wait a bit for content to load
      await page.waitForTimeout(1000);

      // Get page content
      const html = await page.content();
      const $ = cheerio.load(html);

      // Remove unwanted elements
      $('script, style, nav, header, footer, iframe, noscript').remove();

      // Extract main content
      const mainContent = $('article, main, .content, #content, .main').first();
      const contentHtml = mainContent.length > 0 ? mainContent.html() || '' : $('body').html() || '';

      // Convert HTML to markdown
      let markdown = this.turndownService.turndown(contentHtml);

      // Limit length
      if (markdown.length > this.maxContentLength) {
        markdown = markdown.substring(0, this.maxContentLength) + '\n\n[Content truncated...]';
      }

      await page.close();

      return {
        ...result,
        snippet: markdown || result.snippet,
      };
    } catch (error) {
      console.error(`Error extracting page content from ${result.url}: ${error}`);
      return result;
    }
  }

  private getFallbackResults(query: string): SearchResult[] {
    return [{
      title: `Search: ${query}`,
      snippet: `[Playwright browser search would provide results for ${query}]`,
      url: 'https://www.google.com/search?q=' + encodeURIComponent(query),
    }];
  }
}

/**
 * DuckDuckGo search provider (original implementation)
 */
export class DuckDuckGoProvider extends SearchProvider {
  async search(query: string): Promise<SearchResult[]> {
    try {
      const url = `https://api.duckduckgo.com/?q=${encodeURIComponent(query)}&format=json`;
      const response = await fetch(url);
      const data = await response.json() as any;

      const results: SearchResult[] = [];

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

      if (results.length === 0 && data.AbstractText) {
        results.push({
          title: data.Heading || query,
          snippet: data.AbstractText,
          url: data.AbstractURL || '',
        });
      }

      if (results.length === 0) {
        results.push({
          title: `Search results for: ${query}`,
          snippet: `Information about ${query}. This is a simulated search result.`,
          url: 'https://example.com',
        });
      }

      return results;
    } catch (error) {
      console.error(`DuckDuckGo search error: ${error}`);
      return [{
        title: `Search: ${query}`,
        snippet: `[Search functionality would provide real-time information about ${query}]`,
        url: 'https://example.com',
      }];
    }
  }
}

/**
 * Enhanced search tool with multiple provider support
 */
export class EnhancedSearchTool implements ISearchTool {
  private provider: SearchProvider;
  private maxResults: number;

  constructor(config: SearchConfig) {
    this.maxResults = config.maxResults;
    this.provider = this.createProvider(config);
  }

  private createProvider(config: SearchConfig): SearchProvider {
    switch (config.provider) {
      case 'serpapi':
        if (!config.serpapi) {
          throw new Error('SerpAPI configuration is required when using serpapi provider');
        }
        const apiKey = process.env[config.serpapi.apiKeyEnv];
        if (!apiKey) {
          throw new Error(`SerpAPI key not found in environment: ${config.serpapi.apiKeyEnv}`);
        }
        return new SerpAPIProvider(apiKey, config.serpapi.engine, config.maxResults);

      case 'playwright':
        if (!config.playwright) {
          throw new Error('Playwright configuration is required when using playwright provider');
        }
        return new PlaywrightProvider(
          config.playwright.browser,
          config.playwright.headless,
          config.playwright.timeout,
          config.playwright.extractContent,
          config.playwright.maxContentLength,
          config.maxResults
        );

      case 'duckduckgo':
      default:
        return new DuckDuckGoProvider(config.maxResults);
    }
  }

  async search(query: string): Promise<SearchResult[]> {
    return await this.provider.search(query);
  }

  formatResults(results: SearchResult[]): string {
    const formatted: string[] = [];

    results.forEach((result, index) => {
      formatted.push(`\n[Result ${index + 1}]`);
      formatted.push(`Title: ${result.title}`);
      formatted.push(`URL: ${result.url}`);
      formatted.push(`Content:\n${result.snippet}\n`);
    });

    return formatted.join('\n');
  }
}

// Export legacy SearchTool for backward compatibility
export class SearchTool extends DuckDuckGoProvider implements ISearchTool {
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
