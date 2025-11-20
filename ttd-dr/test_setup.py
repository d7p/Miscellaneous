#!/usr/bin/env python3
"""
Test script to verify TTD-DR installation and setup
"""

import sys
from pathlib import Path
import os

def test_imports():
    """Test that all required modules can be imported"""
    print("Testing imports...")

    try:
        import click
        print("✓ click")
    except ImportError:
        print("✗ click - Run: pip install click")
        return False

    try:
        import yaml
        print("✓ PyYAML")
    except ImportError:
        print("✗ PyYAML - Run: pip install pyyaml")
        return False

    try:
        import requests
        print("✓ requests")
    except ImportError:
        print("✗ requests - Run: pip install requests")
        return False

    try:
        from dotenv import load_dotenv
        print("✓ python-dotenv")
    except ImportError:
        print("✗ python-dotenv - Run: pip install python-dotenv")
        return False

    try:
        import colorama
        print("✓ colorama")
    except ImportError:
        print("✗ colorama - Run: pip install colorama")
        return False

    try:
        from pydantic import BaseModel
        print("✓ pydantic")
    except ImportError:
        print("✗ pydantic - Run: pip install pydantic")
        return False

    return True


def test_project_structure():
    """Test that project structure is correct"""
    print("\nTesting project structure...")

    required_files = [
        "config/config.yaml",
        "src/__init__.py",
        "src/ttd_dr_agent.py",
        "src/llm_client.py",
        "src/search_tool.py",
        "src/prompts.py",
        "src/utils.py",
        "main.py",
        "requirements.txt",
        "README.md"
    ]

    all_exist = True
    for filepath in required_files:
        if Path(filepath).exists():
            print(f"✓ {filepath}")
        else:
            print(f"✗ {filepath} - Missing!")
            all_exist = False

    return all_exist


def test_env_file():
    """Test .env file configuration"""
    print("\nTesting environment configuration...")

    if not Path(".env").exists():
        print("✗ .env file not found")
        print("  Create it: cp .env.example .env")
        print("  Then add your API key")
        return False

    print("✓ .env file exists")

    from dotenv import load_dotenv
    load_dotenv()

    openai_key = os.getenv("OPENAI_API_KEY")
    anthropic_key = os.getenv("ANTHROPIC_API_KEY")

    if openai_key and openai_key != "your_openai_api_key_here":
        print("✓ OPENAI_API_KEY is set")
        return True
    elif anthropic_key and anthropic_key != "your_anthropic_api_key_here":
        print("✓ ANTHROPIC_API_KEY is set")
        return True
    else:
        print("✗ No valid API key found in .env")
        print("  Add either OPENAI_API_KEY or ANTHROPIC_API_KEY")
        return False


def test_src_module():
    """Test that src module can be imported"""
    print("\nTesting src module...")

    sys.path.insert(0, str(Path(__file__).parent))

    try:
        from src import TTDDRAgent, AgentConfig, create_llm_client, SearchTool
        print("✓ Core modules imported successfully")
        return True
    except Exception as e:
        print(f"✗ Error importing src modules: {e}")
        return False


def main():
    """Run all tests"""
    print("="*70)
    print("TTD-DR Setup Test")
    print("="*70)

    tests = [
        ("Required Python packages", test_imports),
        ("Project structure", test_project_structure),
        ("Environment configuration", test_env_file),
        ("Source modules", test_src_module)
    ]

    results = []
    for test_name, test_func in tests:
        print(f"\n{'='*70}")
        print(f"Test: {test_name}")
        print(f"{'='*70}")
        result = test_func()
        results.append((test_name, result))

    # Summary
    print(f"\n{'='*70}")
    print("SUMMARY")
    print(f"{'='*70}")

    all_passed = True
    for test_name, result in results:
        status = "✓ PASS" if result else "✗ FAIL"
        print(f"{status} - {test_name}")
        if not result:
            all_passed = False

    print(f"{'='*70}")

    if all_passed:
        print("\n✓ All tests passed! You're ready to use TTD-DR.")
        print("\nNext steps:")
        print("  1. Try: python main.py \"What is quantum computing?\"")
        print("  2. Or: python main.py --interactive")
        print("  3. Or: python example.py")
        print("\nSee QUICKSTART.md for more information.")
        return 0
    else:
        print("\n✗ Some tests failed. Please fix the issues above.")
        print("\nCommon solutions:")
        print("  • Install dependencies: pip install -r requirements.txt")
        print("  • Create .env file: cp .env.example .env")
        print("  • Add API key to .env file")
        return 1


if __name__ == '__main__':
    sys.exit(main())
