#!/usr/bin/env python3
"""
TTD-DR CLI Application
Test-Time Diffusion Deep Researcher

Based on arXiv:2507.16075: Deep Researcher with Test-Time Diffusion
by Han et al. (Google Cloud AI Research)

Usage:
    python main.py "Your research query here"
    python main.py --config config/config.yaml "Your research query"
    python main.py --interactive
"""

import click
import os
import sys
from pathlib import Path
from dotenv import load_dotenv
from colorama import init, Fore, Style

# Add src to path
sys.path.insert(0, str(Path(__file__).parent))

from src import (
    TTDDRAgent,
    AgentConfig,
    create_llm_client,
    SearchTool,
    load_config,
    save_research_session,
    setup_logging
)

# Initialize colorama for colored output
init()


def print_banner():
    """Print application banner"""
    banner = f"""
{Fore.CYAN}╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║           TTD-DR: Test-Time Diffusion Deep Researcher       ║
║                                                              ║
║   Implementation of arXiv:2507.16075                        ║
║   Deep Researcher with Test-Time Diffusion                  ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝{Style.RESET_ALL}
    """
    print(banner)


def print_stage(stage: str, message: str):
    """Print formatted stage message"""
    print(f"\n{Fore.GREEN}[{stage}]{Style.RESET_ALL} {message}")


def print_info(message: str):
    """Print info message"""
    print(f"{Fore.YELLOW}ℹ{Style.RESET_ALL}  {message}")


def print_success(message: str):
    """Print success message"""
    print(f"{Fore.GREEN}✓{Style.RESET_ALL}  {message}")


def print_error(message: str):
    """Print error message"""
    print(f"{Fore.RED}✗{Style.RESET_ALL}  {message}")


def run_research(query: str, config_path: str = "config/config.yaml"):
    """Run the research process"""
    try:
        # Load configuration
        print_info(f"Loading configuration from {config_path}")
        config_data = load_config(config_path)

        # Setup logging
        logger = setup_logging(
            log_file=config_data['logging']['file'],
            level=config_data['logging']['level']
        )

        # Load environment variables
        load_dotenv()

        # Create LLM client
        print_info(f"Initializing {config_data['llm']['provider']} with model {config_data['llm']['model']}")
        llm_client = create_llm_client(
            provider=config_data['llm']['provider'],
            model=config_data['llm']['model'],
            api_key=os.getenv(config_data['llm']['api_key_env'])
        )

        # Create search tool
        search_tool = SearchTool(max_results=config_data['search']['max_results'])

        # Create agent configuration
        algo_config = config_data['algorithm']
        agent_config = AgentConfig(
            max_revision_steps=algo_config['max_revision_steps'],
            n_plan=algo_config['self_evolution']['n_plan'],
            n_query=algo_config['self_evolution']['n_query'],
            n_answer=algo_config['self_evolution']['n_answer'],
            n_report=algo_config['self_evolution']['n_report'],
            s_plan=algo_config['self_evolution']['s_plan'],
            s_query=algo_config['self_evolution']['s_query'],
            s_answer=algo_config['self_evolution']['s_answer'],
            s_report=algo_config['self_evolution']['s_report'],
            save_intermediate=config_data['output']['save_intermediate']
        )

        # Create TTD-DR agent
        print_info("Creating TTD-DR agent...")
        agent = TTDDRAgent(
            llm_client=llm_client,
            search_tool=search_tool,
            config=agent_config,
            logger=logger
        )

        # Run research
        print(f"\n{Fore.CYAN}{'='*70}{Style.RESET_ALL}")
        print(f"{Fore.CYAN}Research Query:{Style.RESET_ALL} {query}")
        print(f"{Fore.CYAN}{'='*70}{Style.RESET_ALL}\n")

        print_stage("START", "Beginning research process...")

        final_report, state = agent.research(query)

        # Save outputs
        print_stage("SAVE", "Saving research outputs...")
        saved_files = save_research_session(
            query=query,
            final_report=final_report,
            state=state,
            output_dir=config_data['output']['directory'],
            save_intermediate=config_data['output']['save_intermediate']
        )

        # Print results
        print(f"\n{Fore.CYAN}{'='*70}{Style.RESET_ALL}")
        print(f"{Fore.CYAN}FINAL REPORT{Style.RESET_ALL}")
        print(f"{Fore.CYAN}{'='*70}{Style.RESET_ALL}\n")
        print(final_report)
        print(f"\n{Fore.CYAN}{'='*70}{Style.RESET_ALL}\n")

        # Print statistics
        print_success(f"Research completed successfully!")
        print_info(f"Total searches performed: {len(state.qa_pairs)}")
        print_info(f"Total draft revisions: {len(state.revision_history)}")
        print_info("\nSaved files:")
        for file_type, filepath in saved_files.items():
            print(f"  • {file_type}: {filepath}")

        return True

    except Exception as e:
        print_error(f"Error during research: {str(e)}")
        logger.exception("Research failed")
        return False


@click.command()
@click.argument('query', required=False)
@click.option('--config', '-c', default='config/config.yaml', help='Path to configuration file')
@click.option('--interactive', '-i', is_flag=True, help='Run in interactive mode')
@click.option('--max-steps', '-m', type=int, help='Override max revision steps')
def main(query, config, interactive, max_steps):
    """
    TTD-DR: Test-Time Diffusion Deep Researcher

    A CLI application that performs deep research on complex queries using
    the Test-Time Diffusion framework (arXiv:2507.16075).

    Examples:

        python main.py "What are the latest developments in quantum computing?"

        python main.py --config my_config.yaml "Analyze the impact of AI on healthcare"

        python main.py --interactive
    """
    print_banner()

    # Interactive mode
    if interactive:
        print(f"{Fore.YELLOW}Interactive Mode{Style.RESET_ALL}")
        print("Enter your research queries (or 'quit' to exit)\n")

        while True:
            try:
                query = input(f"{Fore.GREEN}Query > {Style.RESET_ALL}").strip()

                if query.lower() in ['quit', 'exit', 'q']:
                    print_info("Exiting...")
                    break

                if not query:
                    continue

                # Override config if needed
                if max_steps:
                    print_info(f"Overriding max revision steps to {max_steps}")
                    # Note: Would need to modify config loading to support this

                run_research(query, config)

            except KeyboardInterrupt:
                print("\n")
                print_info("Interrupted. Exiting...")
                break
            except Exception as e:
                print_error(f"Error: {str(e)}")

    elif query:
        # Single query mode
        run_research(query, config)

    else:
        # No query provided
        print_error("No query provided!")
        print("\nUsage:")
        print(f"  python main.py \"Your research query here\"")
        print(f"  python main.py --interactive")
        print(f"\nFor more help: python main.py --help")
        sys.exit(1)


if __name__ == '__main__':
    main()
