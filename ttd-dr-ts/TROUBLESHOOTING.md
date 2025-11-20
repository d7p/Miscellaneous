# Troubleshooting Guide

## Search Provider Issues

### DuckDuckGo Returns HTML Instead of JSON

**Error:**
```
FetchError: invalid json response body at https://duckduckgo.com/
reason: Unexpected token '<', "<!DOCTYPE "... is not valid JSON
```

**Cause:**
DuckDuckGo's Instant Answer API is unreliable and often returns HTML instead of JSON. This is a known issue with their free API.

**Solution 1: Switch to Playwright (Recommended, Free)**

Edit `config/config.yaml`:
```yaml
search:
  provider: "playwright"  # Change from "duckduckgo"
```

Then install Playwright browsers (first time only):
```bash
npx playwright install chromium
```

**Solution 2: Use SerpAPI (Best Quality, Paid)**

Get API key from https://serpapi.com, then:

1. Add to `.env`:
   ```bash
   SERPAPI_API_KEY=your_key_here
   ```

2. Edit `config/config.yaml`:
   ```yaml
   search:
     provider: "serpapi"
   ```

**What's Been Fixed:**

The DuckDuckGo provider now:
- ✅ Checks content-type before parsing JSON
- ✅ Validates response is JSON before parsing
- ✅ Provides helpful fallback results with instructions
- ✅ Shows clear warning messages
- ✅ Continues working instead of crashing

You'll see a warning like:
```
⚠️ DuckDuckGo API is currently unavailable. This is a simulated result.

For production research, please:
1. Use SerpAPI provider (high-quality Google/Bing results)
2. Use Playwright provider (full page content extraction)
```

### SerpAPI Issues

#### "API key not found"

**Solution:**
```bash
# Add to .env file
echo "SERPAPI_API_KEY=your_key_here" >> .env
```

#### "Rate limit exceeded"

**Solution:**
- Upgrade your SerpAPI plan
- Reduce `maxResults` in config
- Add delays between requests

### Playwright Issues

#### "Executable doesn't exist"

**Error:**
```
browserType.launch: Executable doesn't exist at /path/to/chromium
```

**Solution:**
```bash
# Install Playwright browsers
npx playwright install chromium

# Or install all browsers
npx playwright install
```

#### "Browser crashes or timeouts"

**Solution 1: Increase timeout**
```yaml
playwright:
  timeout: 60000  # 60 seconds instead of 30
```

**Solution 2: Use headless mode**
```yaml
playwright:
  headless: true  # Better performance
```

**Solution 3: Reduce content extraction**
```yaml
playwright:
  extractContent: false  # Just get snippets
```

#### "Out of memory errors"

**Solution:**
```yaml
search:
  maxResults: 3  # Reduce from 5
  playwright:
    maxContentLength: 5000  # Reduce from 10000
```

## General Troubleshooting

### Check your configuration

```bash
cat config/config.yaml
```

Make sure:
- Provider name is spelled correctly
- Required config sections exist
- YAML formatting is valid

### Test each provider individually

```typescript
import { EnhancedSearchTool } from './src/enhanced-search-tool';

// Test Playwright
const pw = new EnhancedSearchTool({
  provider: 'playwright',
  maxResults: 2,
  playwright: { browser: 'chromium', headless: true, timeout: 30000, extractContent: true, maxContentLength: 5000 }
});
const results = await pw.search('test query');
console.log(results);
```

### Enable debug logging

```yaml
logging:
  level: "DEBUG"  # See all details
```

### Check dependencies

```bash
npm install  # Reinstall all dependencies
```

## Provider Recommendations

| Scenario | Recommended Provider | Why |
|----------|---------------------|-----|
| **Production** | SerpAPI | Most reliable, best quality |
| **Development** | Playwright | Free, reliable, good quality |
| **Quick testing** | DuckDuckGo | No setup, but unreliable |
| **Deep research** | Playwright | Full page content |
| **News/current** | SerpAPI | Real-time results |
| **Budget-friendly** | Playwright | No API costs |

## Quick Fixes

### I just want it to work now!

```yaml
# Use Playwright - it just works
search:
  provider: "playwright"
```

Then:
```bash
npx playwright install chromium
npm run dev -- "your query"
```

### I need the best results

```yaml
# Use SerpAPI
search:
  provider: "serpapi"
```

Get key from https://serpapi.com (free tier available), add to `.env`:
```bash
SERPAPI_API_KEY=your_key_here
```

## Still Having Issues?

1. Check you're using Node.js 18+: `node --version`
2. Clear node_modules: `rm -rf node_modules && npm install`
3. Rebuild: `npm run rebuild`
4. Check logs: `cat ttd-dr.log`
5. Try with minimal config:
   ```yaml
   search:
     provider: "playwright"
     maxResults: 1
     playwright:
       browser: "chromium"
       headless: true
       timeout: 60000
       extractContent: false
       maxContentLength: 1000
   ```

## Error Reference

| Error | Provider | Solution |
|-------|----------|----------|
| JSON parse error | DuckDuckGo | Switch to Playwright/SerpAPI |
| Executable not found | Playwright | `npx playwright install` |
| API key error | SerpAPI | Add key to `.env` |
| Timeout | Playwright | Increase timeout |
| Rate limit | SerpAPI | Upgrade plan |
| Out of memory | Playwright | Reduce maxResults |

## Performance Issues

### Search is too slow

**For Playwright:**
```yaml
playwright:
  extractContent: false  # Just get snippets, much faster
  timeout: 15000  # Reduce timeout
```

**For SerpAPI:**
```yaml
search:
  maxResults: 3  # Reduce results
```

### Too many API calls

```yaml
algorithm:
  maxRevisionSteps: 10  # Reduce from 20
  selfEvolution:
    nQuery: 2  # Reduce from 5
```

## Contact & Support

- Check `SEARCH_PROVIDERS.md` for detailed provider documentation
- Check `README.md` for general usage
- Review config examples in `config/config.yaml`
