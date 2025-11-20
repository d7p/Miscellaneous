# Search Providers Guide

TTD-DR supports multiple search providers for enhanced web research capabilities. This guide explains how to configure and use each provider.

## Available Providers

### 1. SerpAPI (Recommended for Production)

**Best for**: High-quality, reliable search results from Google, Bing, etc.

#### Features:
- ✅ Real-time search results from major search engines
- ✅ Structured data extraction
- ✅ High reliability and uptime
- ✅ Rich snippets and metadata
- ⚠️ Requires paid API key

#### Setup:

1. Get API key from [https://serpapi.com](https://serpapi.com)
2. Add to `.env`:
   ```bash
   SERPAPI_API_KEY=your_api_key_here
   ```

3. Configure in `config/config.yaml`:
   ```yaml
   search:
     provider: "serpapi"
     maxResults: 5
     serpapi:
       apiKeyEnv: "SERPAPI_API_KEY"
       engine: "google"  # or "bing", "duckduckgo", etc.
   ```

#### Supported Engines:
- `google` - Google Search (recommended)
- `bing` - Bing Search
- `yahoo` - Yahoo Search
- `duckduckgo` - DuckDuckGo Search
- And many more...

### 2. Playwright (Best for Deep Content Extraction)

**Best for**: Extracting full content from web pages, JavaScript-heavy sites

#### Features:
- ✅ Real browser automation
- ✅ Full page content extraction
- ✅ HTML to Markdown conversion
- ✅ JavaScript rendering
- ✅ No API key required
- ⚠️ Slower than API-based search
- ⚠️ Higher resource usage

#### Setup:

1. Install Playwright browsers:
   ```bash
   npx playwright install
   ```

2. Configure in `config/config.yaml`:
   ```yaml
   search:
     provider: "playwright"
     maxResults: 5
     playwright:
       browser: "chromium"  # "chromium", "firefox", or "webkit"
       headless: true
       timeout: 30000  # milliseconds
       extractContent: true  # Convert full page to markdown
       maxContentLength: 10000  # Max chars per page
   ```

#### How it works:
1. Searches Google for the query
2. Extracts search result URLs
3. Visits each URL with a real browser
4. Extracts main content
5. Converts HTML to clean Markdown
6. Returns enriched search results

#### Browser Options:
- `chromium` - Chromium (Chrome) - Fast, best compatibility
- `firefox` - Firefox - Good for privacy-focused research
- `webkit` - Safari/WebKit - Good for testing Apple ecosystem

### 3. DuckDuckGo (Default, Free)

**Best for**: Quick testing, development, no API key needed

#### Features:
- ✅ No API key required
- ✅ Free to use
- ✅ Privacy-focused
- ⚠️ Limited results
- ⚠️ Basic snippets only
- ⚠️ Rate limiting

#### Setup:

Configure in `config/config.yaml`:
```yaml
search:
  provider: "duckduckgo"
  maxResults: 5
```

No additional configuration needed!

## Comparison Matrix

| Feature | SerpAPI | Playwright | DuckDuckGo |
|---------|---------|------------|------------|
| **API Key Required** | ✅ Yes | ❌ No | ❌ No |
| **Cost** | 💰 Paid | 🆓 Free | 🆓 Free |
| **Speed** | ⚡ Fast | 🐌 Slow | ⚡ Fast |
| **Content Quality** | 🌟🌟🌟🌟🌟 | 🌟🌟🌟🌟🌟 | 🌟🌟 |
| **Full Page Content** | ❌ No | ✅ Yes | ❌ No |
| **Reliability** | 🌟🌟🌟🌟🌟 | 🌟🌟🌟🌟 | 🌟🌟🌟 |
| **Setup Complexity** | Easy | Medium | None |
| **Best For** | Production | Deep Research | Development |

## Usage Examples

### Programmatic Usage

```typescript
import { EnhancedSearchTool } from './src/enhanced-search-tool';
import { SearchConfig } from './src/types';

// Example 1: Using SerpAPI
const serpConfig: SearchConfig = {
  provider: 'serpapi',
  maxResults: 5,
  serpapi: {
    apiKeyEnv: 'SERPAPI_API_KEY',
    engine: 'google',
  },
};
const serpTool = new EnhancedSearchTool(serpConfig);
const results = await serpTool.search('quantum computing applications');

// Example 2: Using Playwright
const playwrightConfig: SearchConfig = {
  provider: 'playwright',
  maxResults: 3,
  playwright: {
    browser: 'chromium',
    headless: true,
    timeout: 30000,
    extractContent: true,
    maxContentLength: 10000,
  },
};
const pwTool = new EnhancedSearchTool(playwrightConfig);
const fullContent = await pwTool.search('AI research papers');

// Example 3: Using DuckDuckGo
const ddgConfig: SearchConfig = {
  provider: 'duckduckgo',
  maxResults: 5,
};
const ddgTool = new EnhancedSearchTool(ddgConfig);
const quickResults = await ddgTool.search('TypeScript tutorials');
```

### Custom Search Provider

You can also create custom search providers:

```typescript
import { SearchProvider } from './src/enhanced-search-tool';
import { SearchResult } from './src/types';

class CustomSearchProvider extends SearchProvider {
  async search(query: string): Promise<SearchResult[]> {
    // Your custom search logic
    return [
      {
        title: 'Custom Result',
        snippet: 'Your content here',
        url: 'https://example.com',
      },
    ];
  }
}
```

## Performance Tips

### SerpAPI
- Use caching to avoid redundant API calls
- Monitor your API quota
- Consider using `num` parameter wisely

### Playwright
- Use `headless: true` for better performance
- Adjust `timeout` based on site responsiveness
- Limit `maxContentLength` to reduce processing time
- Consider running fewer parallel searches

### DuckDuckGo
- Add delays between requests to avoid rate limiting
- Use for development only, not production

## Troubleshooting

### SerpAPI Issues

**"API key not found"**
```bash
# Make sure .env file exists and has the key
echo "SERPAPI_API_KEY=your_key_here" >> .env
```

**"Rate limit exceeded"**
- Upgrade your SerpAPI plan
- Add request throttling

### Playwright Issues

**"Browser not found"**
```bash
# Install browsers
npx playwright install chromium
```

**"Timeout errors"**
```yaml
# Increase timeout in config.yaml
playwright:
  timeout: 60000  # 60 seconds
```

**"Memory issues"**
- Reduce `maxResults`
- Disable `extractContent`
- Use `headless: true`

### DuckDuckGo Issues

**"No results returned"**
- DuckDuckGo API has limitations
- Try SerpAPI or Playwright instead

## Best Practices

1. **Development**: Use DuckDuckGo for quick testing
2. **Production**: Use SerpAPI for reliability
3. **Deep Research**: Use Playwright when you need full page content
4. **Cost Optimization**: Start with DuckDuckGo, upgrade to SerpAPI when needed
5. **Content Extraction**: Use Playwright for academic papers, documentation sites
6. **Speed**: Use SerpAPI for fastest results

## Cost Comparison

### SerpAPI Pricing
- Free tier: 100 searches/month
- Starter: $50/month (5,000 searches)
- Professional: $250/month (30,000 searches)
- [See full pricing](https://serpapi.com/pricing)

### Playwright
- **Free**: No API costs
- Server costs depend on your infrastructure
- Higher CPU/memory usage

### DuckDuckGo
- **Free**: No costs
- Rate limited
- Limited functionality

## Recommendations by Use Case

| Use Case | Recommended Provider | Reason |
|----------|---------------------|--------|
| Development/Testing | DuckDuckGo | Free, quick setup |
| Production Research | SerpAPI | Reliable, high quality |
| Academic Research | Playwright | Full content extraction |
| News/Current Events | SerpAPI (Google) | Real-time, relevant |
| Technical Documentation | Playwright | Code snippets, formatting |
| Cost-Sensitive | DuckDuckGo → SerpAPI | Start free, scale up |
| Privacy-Focused | Playwright or DuckDuckGo | No tracking |
| High Volume | SerpAPI | Better rate limits |

## Migration Guide

### From DuckDuckGo to SerpAPI

1. Get SerpAPI key
2. Update `config.yaml`:
   ```yaml
   search:
     provider: "serpapi"  # Changed from "duckduckgo"
     serpapi:
       apiKeyEnv: "SERPAPI_API_KEY"
       engine: "google"
   ```
3. Add to `.env`: `SERPAPI_API_KEY=your_key`
4. No code changes needed!

### From SerpAPI to Playwright

1. Install Playwright: `npx playwright install`
2. Update `config.yaml`:
   ```yaml
   search:
     provider: "playwright"  # Changed from "serpapi"
     playwright:
       browser: "chromium"
       headless: true
       timeout: 30000
       extractContent: true
       maxContentLength: 10000
   ```
3. No code changes needed!

## Summary

Choose your search provider based on your needs:

- **Just starting?** → DuckDuckGo
- **Need reliability?** → SerpAPI
- **Need deep content?** → Playwright
- **In production?** → SerpAPI
- **Budget conscious?** → DuckDuckGo → SerpAPI (as needed)
- **Privacy focused?** → Playwright

All providers work seamlessly with TTD-DR - just change the configuration!
