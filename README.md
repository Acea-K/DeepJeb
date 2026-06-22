<p align="center">
  <img src="assets/Textures/DeepJebLogo.png" alt="DeepJeb Logo" width="256">
</p>

<h1 align="center">DeepJeb</h1>

<p align="center">
  <strong>AI Assistant for Kerbal Space Program</strong><br>
  An in-game AI chat window that reads, writes, and helps you build.<br>
  <em>KSP 1.12.5 · Unity 2019.2 · C# 7.3 · Zero Dependencies</em>
</p>

<p align="center">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="MIT License"></a>
  <a href="#"><img src="https://img.shields.io/badge/KSP-1.12.5-blue" alt="KSP 1.12.5"></a>
  <a href="#"><img src="https://img.shields.io/badge/version-0.5.0-green" alt="v0.5.0"></a>
</p>

<p align="center">
  <strong>English</strong> | <a href="README_cn.md">中文</a>
</p>

---

## What is DeepJeb?

DeepJeb embeds an AI chat window directly inside KSP. Ask it anything — write Module Manager patches, configure mods, explain orbital mechanics, debug a kOS script, or design a rocket that won't flip on ascent.

DeepJeb ships with a comprehensive **KSP world knowledge base**: game mechanics, orbital physics (Kepler's laws, delta-V, gravity assists), all stock celestial bodies, spacecraft design principles, DLC content, modding conventions, and community resources. But its real power is the **skill system** — you can drop any `SKILL.md` document into the `Skills/` folder and the AI will load it as domain knowledge. Teach it about your favorite mod, your custom planet pack, or your personal build conventions. The skill format is open and documented — your expertise, your rules.

> **You need your own API key.** DeepJeb does not include or provide any AI service — you connect it to your own OpenAI, Anthropic, Google Gemini, DeepSeek, or other compatible API account. All API traffic goes directly from your machine to the provider you configure. You can also point it at a locally deployed LLM (via Ollama, vLLM, or any OpenAI-compatible endpoint) to keep everything fully under your own control.

---

## What Can It Do?

DeepJeb comes with **6 built-in knowledge bases** (Claude Skills) and **7 file-system tools** the AI can use.

### Knowledge Bases (Skills)

| Skill | Description |
|-------|-------------|
| **KSP World Knowledge** | Stock game mechanics, orbital physics, celestial bodies, spacecraft design, DLC content, modding tips, community resources |
| **Module Manager** | MM patch syntax, operators, ordering directives, NEEDS/DEPENDS checking, variables, common patterns |
| **kOS Programming** | KerboScript language reference, flight control, maneuver nodes, boot files, triggers |
| **kRPC Programming** | Architecture, Python/C#/Lua client APIs, SpaceCenter, AutoPilot, vessel control, streaming data |
| **MechJeb** | All guidance modules, career integration, real-time value modification via kRPC/kOS |
| **Realism Overhaul** | Comprehensive RO/RP-1/RSS mod suite — 68 repos, installation, parts, engines, life support, historical spacecraft, troubleshooting |

Skills are matched automatically by keyword overlap with your question — the top 2 hits are injected as context.

### AI Agent Tools

| Tool | What the AI can do |
|------|-------------------|
| `read_file` | Read any file inside GameData |
| `write_file` | Create or overwrite a file (auto-creates directories, backs up previous version) |
| `delete_file` | Delete a file (creates timestamped `.bak` backup first) |
| `list_directory` | List directory contents with file sizes and modification times |
| `file_exists` | Check if a file or directory exists |
| `backup_file` | Create a timestamped `.bak` snapshot without modifying the original |
| `get_game_state` | Report current KSP game state (vessel, orbit, biome, resources) |

The AI can read Squad/SquadExpansion files but **cannot modify or delete** them.

---

## Supported Providers

**12 built-in presets + custom provider support** across 3 protocol families:

| Protocol | Built-in Presets |
|----------|-----------------|
| **OpenAI-compatible** | OpenAI, DeepSeek, OpenRouter, Grok (xAI), Mistral, Together AI, Perplexity, Groq, Ollama, vLLM, Custom |
| **Anthropic** | Anthropic (Claude) |
| **Google Gemini** | Google (Gemini) |

Custom endpoints, API keys, model lists, and provider names are all configurable in-game through the Settings window. Model lists are fetched live from each API.

---

## What is a Claude Skill?

DeepJeb's knowledge bases are **Claude Skills** — a standard format for packaging domain expertise with an AI assistant. Each skill is a `SKILL.md` file with YAML frontmatter (name, description, triggers) and a Markdown body containing the knowledge. Skills are placed in the `Skills/` directory and loaded at startup.

### How Skills Work

- **[Claude Code Skills documentation](https://docs.anthropic.com/en/docs/claude-code/skills)** — official guide
- **[Creating custom skills](https://docs.anthropic.com/en/docs/claude-code/skills#creating-custom-skills)** — authoring guide

To add your own skill to DeepJeb, create a `SKILL.md` file in `GameData/DeepJeb/Skills/{category}/{name}/` with:

```yaml
---
name: your-skill-name
description: >
  What this skill covers.
---
# Your knowledge content here
```

Reference files (scripts, tables, examples) can be placed in a `references/` subdirectory — they'll be injected alongside the skill when matched.

### Conditional Skill Activation

You can use the `when_to_use` frontmatter field to make a skill only activate when a specific mod is present. The AI agent can check `GameData/` for installed mods before loading the skill:

```yaml
---
name: my-mod-guide
description: >
  Knowledge base for MyMod. Only activate when the mod is installed.
when_to_use: |
  Trigger when the user's GameData folder contains "MyMod".
condition: file_exists("MyMod/") -> true
---
# MyMod configuration guide
```

Use `file_exists` or `list_directory` tool calls as conditions to gate skill loading — this way DeepJeb won't load irrelevant knowledge for mods you don't have installed.

---

## Installation

1. Copy the `DeepJeb/` folder into your KSP `GameData/` directory
2. Launch KSP — the DeepJeb toolbar icon appears in all scenes
3. Click the icon to open the chat window
4. Open Settings to configure an API provider and model
5. Start chatting

**Requirements:** KSP 1.12.0+ (tested on 1.12.5). No additional mods or dependencies required.

---

## Known Issues

- **Model availability**: Model lists are fetched live from each API provider. If the API is unreachable, the model dropdown shows the last cached list or "Loading..." indefinitely. Check your API key and network connection.
- **Context truncation**: Very long conversations may lose earlier messages when approaching the model's context window limit. Use `/clear` periodically for long sessions.
- **UI scaling**: The chat window uses fixed pixel dimensions (600×500 default). On very small or very large screens, the layout may not scale ideally.
- **KSP scene transitions**: The chat window persists across scene changes, but in-progress AI generation is stopped on scene load.
- **Google Gemini API key**: Due to the Gemini API's query-parameter authentication, the API key may appear in KSP's debug console logs when using Alt+F12 debugging.
- **Streaming performance**: Very long AI responses may cause minor UI frame-rate fluctuations during token-by-token rendering.

---

## License

[MIT License](LICENSE)

Copyright © 2026 Acea - vibe coded using Claude Code / DeepSeek V4 Pro

MiniJSON based on the public-domain implementation by Calvin Rien.

---

<p align="center">
  <sub>Built for the Kerbal Space Program modding community. Fly safe.</sub>
</p>
