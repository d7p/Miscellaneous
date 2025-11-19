#!/usr/bin/env python3
"""
Example usage of TTD-DR programmatically
"""

import sys
from pathlib import Path
import os
from dotenv import load_dotenv

# Add src to path
sys.path.insert(0, str(Path(__file__).parent))

from src import (
    TTDDRAgent,
    AgentConfig,
    create_llm_client,
    SearchTool,
    save_research_session,
    setup_logging
)


def main():
    """Example research session"""

    # Load environment variables
    load_dotenv()

    # Setup logging
    logger = setup_logging(level="INFO")

    print("="*70)
    print("TTD-DR Example: Programmatic Usage")
    print("="*70)

    # Configure the agent
    config = AgentConfig(
        max_revision_steps=5,  # Fewer steps for faster demo
        n_plan=1,
        n_query=3,  # 3 query variants
        n_answer=2,  # 2 answer variants
        n_report=1,
        s_plan=1,
        s_query=0,
        s_answer=0,
        s_report=1,
        save_intermediate=True
    )

    # Create LLM client (using OpenAI GPT-4 by default)
    print("\n[1] Creating LLM client...")
    llm_client = create_llm_client(
        provider="openai",
        model="gpt-4",
        api_key=os.getenv("OPENAI_API_KEY")
    )

    # Create search tool
    print("[2] Creating search tool...")
    search_tool = SearchTool(max_results=5)

    # Create TTD-DR agent
    print("[3] Creating TTD-DR agent...")
    agent = TTDDRAgent(
        llm_client=llm_client,
        search_tool=search_tool,
        config=config,
        logger=logger
    )

    # Define research query
    query = "What are the main applications of large language models in 2025?"

    print(f"\n[4] Starting research on query:\n    '{query}'")
    print("\nThis may take several minutes...\n")

    # Run research
    final_report, state = agent.research(query)

    # Save results
    print("\n[5] Saving research outputs...")
    saved_files = save_research_session(
        query=query,
        final_report=final_report,
        state=state,
        output_dir="output",
        save_intermediate=True
    )

    # Display results
    print("\n" + "="*70)
    print("FINAL REPORT")
    print("="*70 + "\n")
    print(final_report)
    print("\n" + "="*70 + "\n")

    # Display statistics
    print(f"✓ Research completed!")
    print(f"  • Total searches: {len(state.qa_pairs)}")
    print(f"  • Draft revisions: {len(state.revision_history)}")
    print(f"\n✓ Files saved:")
    for file_type, filepath in saved_files.items():
        print(f"  • {file_type}: {filepath}")

    print("\n" + "="*70)
    print("Example completed successfully!")
    print("="*70)


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        print("\n\nInterrupted by user.")
        sys.exit(0)
    except Exception as e:
        print(f"\n\nError: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)
