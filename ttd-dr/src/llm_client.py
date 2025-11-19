"""
LLM Client for TTD-DR
Supports OpenAI and Anthropic models
"""

import os
from typing import Optional, List, Dict
from abc import ABC, abstractmethod


class LLMClient(ABC):
    """Abstract base class for LLM clients"""

    @abstractmethod
    def generate(self, prompt: str, temperature: float = 0.7, max_tokens: int = 4096) -> str:
        """Generate a response from the LLM"""
        pass

    @abstractmethod
    def generate_multiple(self, prompt: str, n: int, temperature: float = 0.7, max_tokens: int = 4096) -> List[str]:
        """Generate multiple responses for self-evolution"""
        pass


class OpenAIClient(LLMClient):
    """OpenAI GPT client"""

    def __init__(self, model: str = "gpt-4", api_key: Optional[str] = None):
        try:
            from openai import OpenAI
        except ImportError:
            raise ImportError("Please install openai: pip install openai")

        self.client = OpenAI(api_key=api_key or os.getenv("OPENAI_API_KEY"))
        self.model = model

    def generate(self, prompt: str, temperature: float = 0.7, max_tokens: int = 4096) -> str:
        """Generate a single response"""
        response = self.client.chat.completions.create(
            model=self.model,
            messages=[{"role": "user", "content": prompt}],
            temperature=temperature,
            max_tokens=max_tokens
        )
        return response.choices[0].message.content

    def generate_multiple(self, prompt: str, n: int, temperature: float = 0.7, max_tokens: int = 4096) -> List[str]:
        """Generate multiple responses with varied parameters for exploration"""
        responses = []
        # Vary temperature for diversity (Section 2.2, Step 1: Initial States)
        temperatures = [temperature + (i * 0.1) for i in range(n)]

        for temp in temperatures[:n]:
            try:
                response = self.client.chat.completions.create(
                    model=self.model,
                    messages=[{"role": "user", "content": prompt}],
                    temperature=min(temp, 1.0),
                    max_tokens=max_tokens
                )
                responses.append(response.choices[0].message.content)
            except Exception as e:
                print(f"Error generating response: {e}")
                responses.append("")

        return responses


class AnthropicClient(LLMClient):
    """Anthropic Claude client"""

    def __init__(self, model: str = "claude-3-5-sonnet-20241022", api_key: Optional[str] = None):
        try:
            from anthropic import Anthropic
        except ImportError:
            raise ImportError("Please install anthropic: pip install anthropic")

        self.client = Anthropic(api_key=api_key or os.getenv("ANTHROPIC_API_KEY"))
        self.model = model

    def generate(self, prompt: str, temperature: float = 0.7, max_tokens: int = 4096) -> str:
        """Generate a single response"""
        response = self.client.messages.create(
            model=self.model,
            max_tokens=max_tokens,
            temperature=temperature,
            messages=[{"role": "user", "content": prompt}]
        )
        return response.content[0].text

    def generate_multiple(self, prompt: str, n: int, temperature: float = 0.7, max_tokens: int = 4096) -> List[str]:
        """Generate multiple responses with varied parameters"""
        responses = []
        temperatures = [temperature + (i * 0.1) for i in range(n)]

        for temp in temperatures[:n]:
            try:
                response = self.client.messages.create(
                    model=self.model,
                    max_tokens=max_tokens,
                    temperature=min(temp, 1.0),
                    messages=[{"role": "user", "content": prompt}]
                )
                responses.append(response.content[0].text)
            except Exception as e:
                print(f"Error generating response: {e}")
                responses.append("")

        return responses


def create_llm_client(provider: str, model: str, api_key: Optional[str] = None) -> LLMClient:
    """Factory function to create LLM client"""
    if provider.lower() == "openai":
        return OpenAIClient(model=model, api_key=api_key)
    elif provider.lower() == "anthropic":
        return AnthropicClient(model=model, api_key=api_key)
    else:
        raise ValueError(f"Unsupported provider: {provider}")
