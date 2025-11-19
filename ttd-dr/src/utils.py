"""
Utility functions for TTD-DR
"""

import os
import json
import yaml
from datetime import datetime
from pathlib import Path
from typing import Dict, Any


def load_config(config_path: str) -> Dict[str, Any]:
    """Load configuration from YAML file"""
    with open(config_path, 'r') as f:
        return yaml.safe_load(f)


def save_output(
    content: str,
    filename: str,
    output_dir: str = "output",
    format: str = "markdown"
) -> str:
    """Save content to file"""
    # Create output directory if it doesn't exist
    Path(output_dir).mkdir(parents=True, exist_ok=True)

    # Add timestamp to filename
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    base_name = filename.replace(" ", "_").replace("?", "").replace("/", "_")

    # Determine file extension
    if format == "markdown":
        ext = ".md"
    elif format == "json":
        ext = ".json"
    else:
        ext = ".txt"

    filepath = os.path.join(output_dir, f"{base_name}_{timestamp}{ext}")

    # Save content
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)

    return filepath


def save_research_session(
    query: str,
    final_report: str,
    state: Any,
    output_dir: str = "output",
    save_intermediate: bool = True
) -> Dict[str, str]:
    """
    Save complete research session including:
    - Final report
    - Research plan
    - Search history (Q&A pairs)
    - Intermediate drafts (if enabled)
    """
    saved_files = {}

    # Save final report
    report_path = save_output(
        content=final_report,
        filename=f"report_{query[:30]}",
        output_dir=output_dir,
        format="markdown"
    )
    saved_files['report'] = report_path

    # Save research plan
    plan_path = save_output(
        content=f"# Research Plan\n\nQuery: {query}\n\n{state.plan}",
        filename=f"plan_{query[:30]}",
        output_dir=output_dir,
        format="markdown"
    )
    saved_files['plan'] = plan_path

    # Save search history
    search_history = "# Search History\n\n"
    for i, (q, a) in enumerate(state.qa_pairs, 1):
        search_history += f"## Search {i}\n\n"
        search_history += f"**Question:** {q}\n\n"
        search_history += f"**Answer:**\n{a}\n\n"
        search_history += "---\n\n"

    history_path = save_output(
        content=search_history,
        filename=f"search_history_{query[:30]}",
        output_dir=output_dir,
        format="markdown"
    )
    saved_files['search_history'] = history_path

    # Save intermediate drafts if enabled
    if save_intermediate and state.revision_history:
        drafts_content = "# Draft Revision History\n\n"
        for i, draft in enumerate(state.revision_history):
            drafts_content += f"## Draft {i} (R{i})\n\n"
            drafts_content += draft + "\n\n"
            drafts_content += "=" * 80 + "\n\n"

        drafts_path = save_output(
            content=drafts_content,
            filename=f"drafts_{query[:30]}",
            output_dir=output_dir,
            format="markdown"
        )
        saved_files['drafts'] = drafts_path

    # Save metadata as JSON
    metadata = {
        "query": query,
        "timestamp": datetime.now().isoformat(),
        "num_searches": len(state.qa_pairs),
        "num_revisions": len(state.revision_history),
        "files": saved_files
    }

    metadata_path = save_output(
        content=json.dumps(metadata, indent=2),
        filename=f"metadata_{query[:30]}",
        output_dir=output_dir,
        format="json"
    )
    saved_files['metadata'] = metadata_path

    return saved_files


def setup_logging(log_file: str = "ttd-dr.log", level: str = "INFO"):
    """Setup logging configuration"""
    import logging

    # Create logs directory if it doesn't exist
    log_dir = os.path.dirname(log_file)
    if log_dir:
        Path(log_dir).mkdir(parents=True, exist_ok=True)

    # Configure logging
    log_level = getattr(logging, level.upper(), logging.INFO)

    logging.basicConfig(
        level=log_level,
        format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
        handlers=[
            logging.FileHandler(log_file),
            logging.StreamHandler()
        ]
    )

    return logging.getLogger(__name__)
