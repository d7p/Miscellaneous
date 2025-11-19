"""
Search Tool for TTD-DR
Implements web search functionality (Stage 2b - Answer Searching)
"""

import requests
from typing import List, Dict
from urllib.parse import quote


class SearchResult:
    """Represents a single search result"""

    def __init__(self, title: str, snippet: str, url: str):
        self.title = title
        self.snippet = snippet
        self.url = url

    def __str__(self):
        return f"Title: {self.title}\nURL: {self.url}\nSnippet: {self.snippet}\n"


class SearchTool:
    """Web search tool using DuckDuckGo"""

    def __init__(self, max_results: int = 5):
        self.max_results = max_results

    def search(self, query: str) -> List[SearchResult]:
        """
        Perform web search and return results
        This simulates the search tool mentioned in Stage 2b
        """
        try:
            # Using DuckDuckGo Instant Answer API (free, no API key needed)
            url = f"https://api.duckduckgo.com/?q={quote(query)}&format=json"
            response = requests.get(url, timeout=10)
            data = response.json()

            results = []

            # Get related topics
            if "RelatedTopics" in data:
                for topic in data["RelatedTopics"][:self.max_results]:
                    if isinstance(topic, dict) and "Text" in topic:
                        results.append(SearchResult(
                            title=topic.get("Text", "")[:100],
                            snippet=topic.get("Text", ""),
                            url=topic.get("FirstURL", "")
                        ))

            # If no results from RelatedTopics, use Abstract
            if not results and "AbstractText" in data and data["AbstractText"]:
                results.append(SearchResult(
                    title=data.get("Heading", query),
                    snippet=data.get("AbstractText", ""),
                    url=data.get("AbstractURL", "")
                ))

            # Fallback: create a mock result if no real results
            if not results:
                results.append(SearchResult(
                    title=f"Search results for: {query}",
                    snippet=f"Information about {query}. This is a simulated search result. "
                            f"In production, this would connect to a real search API like Google Search API.",
                    url="https://example.com"
                ))

            return results

        except Exception as e:
            print(f"Search error: {e}")
            # Return a fallback result
            return [SearchResult(
                title=f"Search: {query}",
                snippet=f"[Search functionality would provide real-time information about {query}]",
                url="https://example.com"
            )]

    def format_results(self, results: List[SearchResult]) -> str:
        """Format search results as text"""
        formatted = []
        for i, result in enumerate(results, 1):
            formatted.append(f"\n[Result {i}]")
            formatted.append(str(result))

        return "\n".join(formatted)
