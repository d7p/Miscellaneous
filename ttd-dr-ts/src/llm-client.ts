/**
 * LLM Client for TTD-DR
 * Supports OpenAI and Anthropic models
 */

import OpenAI from 'openai';
import Anthropic from '@anthropic-ai/sdk';

/**
 * Abstract base class for LLM clients
 */
export abstract class LLMClient {
  abstract generate(
    prompt: string,
    temperature?: number,
    maxTokens?: number
  ): Promise<string>;

  abstract generateMultiple(
    prompt: string,
    n: number,
    temperature?: number,
    maxTokens?: number
  ): Promise<string[]>;
}

/**
 * OpenAI GPT client
 */
export class OpenAIClient extends LLMClient {
  private client: OpenAI;
  private model: string;

  constructor(model: string = 'gpt-4', apiKey?: string) {
    super();
    this.client = new OpenAI({
      apiKey: apiKey || process.env.OPENAI_API_KEY,
    });
    this.model = model;
  }

  async generate(
    prompt: string,
    temperature: number = 0.7,
    maxTokens: number = 4096
  ): Promise<string> {
    const response = await this.client.chat.completions.create({
      model: this.model,
      messages: [{ role: 'user', content: prompt }],
      temperature,
      max_tokens: maxTokens,
    });

    return response.choices[0]?.message?.content || '';
  }

  async generateMultiple(
    prompt: string,
    n: number,
    temperature: number = 0.7,
    maxTokens: number = 4096
  ): Promise<string[]> {
    const responses: string[] = [];

    // Vary temperature for diversity (Section 2.2, Step 1: Initial States)
    const temperatures = Array.from({ length: n }, (_, i) => temperature + i * 0.1);

    for (let i = 0; i < n; i++) {
      try {
        const temp = Math.min(temperatures[i], 1.0);
        const response = await this.client.chat.completions.create({
          model: this.model,
          messages: [{ role: 'user', content: prompt }],
          temperature: temp,
          max_tokens: maxTokens,
        });

        responses.push(response.choices[0]?.message?.content || '');
      } catch (error) {
        console.error(`Error generating response ${i + 1}:`, error);
        responses.push('');
      }
    }

    return responses;
  }
}

/**
 * Anthropic Claude client
 */
export class AnthropicClient extends LLMClient {
  private client: Anthropic;
  private model: string;

  constructor(model: string = 'claude-3-5-sonnet-20241022', apiKey?: string) {
    super();
    this.client = new Anthropic({
      apiKey: apiKey || process.env.ANTHROPIC_API_KEY,
    });
    this.model = model;
  }

  async generate(
    prompt: string,
    temperature: number = 0.7,
    maxTokens: number = 4096
  ): Promise<string> {
    const response = await this.client.messages.create({
      model: this.model,
      max_tokens: maxTokens,
      temperature,
      messages: [{ role: 'user', content: prompt }],
    });

    const content = response.content[0];
    return content.type === 'text' ? content.text : '';
  }

  async generateMultiple(
    prompt: string,
    n: number,
    temperature: number = 0.7,
    maxTokens: number = 4096
  ): Promise<string[]> {
    const responses: string[] = [];
    const temperatures = Array.from({ length: n }, (_, i) => temperature + i * 0.1);

    for (let i = 0; i < n; i++) {
      try {
        const temp = Math.min(temperatures[i], 1.0);
        const response = await this.client.messages.create({
          model: this.model,
          max_tokens: maxTokens,
          temperature: temp,
          messages: [{ role: 'user', content: prompt }],
        });

        const content = response.content[0];
        responses.push(content.type === 'text' ? content.text : '');
      } catch (error) {
        console.error(`Error generating response ${i + 1}:`, error);
        responses.push('');
      }
    }

    return responses;
  }
}

/**
 * Factory function to create LLM client
 */
export function createLLMClient(
  provider: 'openai' | 'anthropic',
  model: string,
  apiKey?: string
): LLMClient {
  switch (provider.toLowerCase()) {
    case 'openai':
      return new OpenAIClient(model, apiKey);
    case 'anthropic':
      return new AnthropicClient(model, apiKey);
    default:
      throw new Error(`Unsupported provider: ${provider}`);
  }
}
