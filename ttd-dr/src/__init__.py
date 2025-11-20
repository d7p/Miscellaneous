"""
TTD-DR: Test-Time Diffusion Deep Researcher
Implementation of arXiv:2507.16075
"""

from .ttd_dr_agent import TTDDRAgent, AgentConfig, AgentState
from .llm_client import create_llm_client, LLMClient
from .search_tool import SearchTool
from .prompts import Prompts
from .utils import load_config, save_output, save_research_session, setup_logging

__version__ = "1.0.0"
__all__ = [
    'TTDDRAgent',
    'AgentConfig',
    'AgentState',
    'create_llm_client',
    'LLMClient',
    'SearchTool',
    'Prompts',
    'load_config',
    'save_output',
    'save_research_session',
    'setup_logging'
]
