# DeepJeb Architecture

## Overview

DeepJeb is structured as four assemblies in three conceptual layers. The guiding principle: **push as much logic as possible into pure C# that can be tested without Unity**.

## Layer Diagram

```
┌──────────────────────────────────────────────────────────┐
│                   DeepJeb.Mod                             │
│  ┌────────────────────────────────────────────────────┐  │
│  │ KSPAddon entry → DI wiring → Bootstrap sequence    │  │
│  └────────────────────────────────────────────────────┘  │
├──────────────────────────────────────────────────────────┤
│                   DeepJeb.Unity                           │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌────────────┐  │
│  │ Chat UI  │ │ Settings │ │ API Wizard│ │  Toolbar   │  │
│  │ (IMGUI)  │ │ (IMGUI)  │ │ (IMGUI)  │ │ (AppLaunch)│  │
│  └──────────┘ └──────────┘ └──────────┘ └────────────┘  │
│  ┌────────────────────────────────────────────────────┐  │
│  │ FileTools, GameStateTool (ITool implementations)    │  │
│  └────────────────────────────────────────────────────┘  │
│  ┌────────────────────────────────────────────────────┐  │
│  │ ModLifecycle (scene change, auto-save, singleton)   │  │
│  └────────────────────────────────────────────────────┘  │
├──────────────────────────────────────────────────────────┤
│                   DeepJeb.Protocol                        │
│  ┌──────────────────┐ ┌──────────────┐ ┌──────────────┐ │
│  │ OpenAI-compatible │ │  Anthropic   │ │    Google    │ │
│  │ (12+ providers)  │ │   (native)   │ │   (native)   │ │
│  └──────────────────┘ └──────────────┘ └──────────────┘ │
├──────────────────────────────────────────────────────────┤
│                   DeepJeb.Core                            │
│  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌─────────┐ │
│  │ Agent  │ │Security│ │Session │ │ Skills │ │ Context │ │
│  │ Loop   │ │Pipeline│ │ Store  │ │ Matcher│ │ Manager │ │
│  └────────┘ └────────┘ └────────┘ └────────┘ └─────────┘ │
│  Skills: standard Claude SKILL.md docs with YAML frontmatter │
│  ┌────────────────────────────────────────────────────┐  │
│  │              Models (ChatMessage, ToolCall, etc.)   │  │
│  └────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────┘
```

## Data Flow: Message Send

```
User types message
       │
       ▼
┌─────────────────┐
│  ChatWindow      │  IMGUI input field, 10K char limit
│  (Unity)         │
└────────┬────────┘
         │ string
         ▼
┌─────────────────┐
│  FilterPipeline  │  Hard keywords → Soft keywords → ...
│  (Core)          │  Block? → show red inline error in UI
└────────┬────────┘
         │ allowed
         ▼
┌─────────────────┐
│  ISkillMatcher   │  Score skills vs message, pick top 2
│  (Core)          │  Inject [KNOWLEDGE: ...] system messages
└────────┬────────┘
         │ augmented messages
         ▼
┌─────────────────┐
│  IContextManager │  Count tokens, truncate if ≥90% limit
│  (Core)          │  Preserve system prompt, drop oldest pairs
└────────┬────────┘
         │ truncated messages
         ▼
┌─────────────────┐
│  Protocol Client │  POST to API with tools, reasoning, stream
│  (Protocol)      │  Parse SSE stream → onToken callbacks
└────────┬────────┘
         │ response text OR tool_calls
         ▼
    ┌────┴────┐
    │         │
    ▼         ▼
  text     tool_calls
    │         │
    │         ▼
    │    ┌─────────────┐
    │    │ Agent Loop   │  Max 10 rounds, repeat detection
    │    │ (Core)       │  Force summary at round 5
    │    └──────┬──────┘
    │           │ tool result
    │           ▼
    │    ┌─────────────┐
    │    │ ToolRegistry │  Dispatch to ITool.ExecuteAsync
    │    │ (Core)       │
    │    └──────┬──────┘
    │           │ result string
    │           ▼
    │    back to Protocol Client (next round)
    │
    ▼
┌─────────────────┐
│  ChatWindow      │  Render markdown, code blocks, tables
│  (Unity)         │  Auto-scroll to bottom
└─────────────────┘
```

## Agent Loop State Machine

```
                    ┌──────────┐
                    │   IDLE   │
                    └────┬─────┘
                         │ user message received
                         ▼
                    ┌──────────┐
              ┌────>│ SENDING  │──(error)──> display error, stop
              │     └────┬─────┘
              │          │ response received
              │          ▼
              │     ┌──────────────┐
              │     │ CHECK RESULT │
              │     └──┬───────┬───┘
              │        │       │
              │   tool_calls   text (finish_reason=stop)
              │        │       │
              │        ▼       ▼
              │   ┌────────┐  ┌──────────┐
              │   │EXECUTE │  │  DISPLAY │──> auto-save session
              │   │ TOOL   │  └──────────┘
              │   └───┬────┘
              │       │
              │       ├── round >= max_rounds? ──> force summary, stop
              │       ├── repeated result ×6?  ──> stop
              │       ├── write_file success?  ──> extra summary round
              │       │
              │       ▼
              └── continue loop
```

## API Provider Mapping

12+ providers map to 3 protocol interfaces, not 14 classes:

| Protocol Interface | Providers | Endpoint |
|-------------------|-----------|----------|
| `IOpenAiCompatibleApi` | OpenAI, DeepSeek, Grok, MiniMax, Mistral, Moonshot, OpenRouter, Qwen, Xiaomi MiMo, Z.ai, Custom | `POST /v1/chat/completions` |
| `IAnthropicCompatibleApi` | Anthropic, Custom (Anthropic-compatible) | `POST /v1/messages` |
| `IGoogleGenerativeApi` | Google Gemini | `POST /v1beta/models/{model}:generateContent` |

Provider identity = configuration data (Base URL + API Key + Protocol Type), not a code class.

## Security Pipeline

```
Input (user message or AI response)
    │
    ▼
[Hard Keyword Filter] ──block──> "Message blocked: [reason]"
    │ (60+ keywords: identity manipulation, explicit content, extreme violence)
    ▼
[Soft Keyword Filter] ──block──> "Message blocked: [reason]"
    │ (30+ keywords, cumulative threshold of 2 hits in same session)
    ▼
[Path Sandbox]        ──block──> "Access denied: [reason]"
    │ (only for tool calls — absolute paths, .., ~, Squad/ forbidden)
    ▼
Allowed — proceed to execution
```

## Skill System

Skills are **standard Claude Skill documents** — `SKILL.md` files with YAML frontmatter + Markdown body, organized under `GameData/DeepJeb/Skills/`. The system loads, scores, and injects skills into the AI's system prompt.

### Directory Layout

```
GameData/DeepJeb/Skills/
├── stock-game/                    # Always-active base knowledge
│   └── ksp-world-knowledge/
│       ├── SKILL.md
│       └── references/
│           ├── kerbol-system.md
│           ├── orbital-and-design.md
│           └── ...
└── mods/                          # Conditionally activated per installed mod
    ├── kos-programming/
    │   ├── SKILL.md               # applies_when: "directory_exists:GameData/kOS"
    │   └── references/
    ├── module-manager/
    │   ├── SKILL.md               # applies_when: "file_exists:GameData/ModuleManager*.dll"
    │   └── references/
    ├── mechjeb/
    │   ├── SKILL.md               # applies_when: "directory_exists:GameData/MechJeb2"
    │   └── references/
    └── krpc-programming/
        ├── SKILL.md               # applies_when: "directory_exists:GameData/kRPC"
        └── references/
```

### SKILL.md Format

```markdown
---
name: kos-programming              # Unique kebab-case identifier
description: >                     # Used for keyword matching
  Comprehensive knowledge base for kOS...
author: Acea
sources:                           # Attribution URLs
  - https://ksp-kos.github.io/KOS/
category: mods
version: 1.0
last_updated: 2026-06-17
applies_when: "directory_exists:GameData/kOS"
---

# Skill Title

Markdown knowledge body...
```

### Activation Conditions

| Condition | Syntax | Example |
|-----------|--------|---------|
| Always active | `"always"` | `ksp-world-knowledge` |
| Directory check | `"directory_exists:GameData/ModName"` | `"directory_exists:GameData/kOS"` |
| File glob check | `"file_exists:GameData/Pattern*.dll"` | `"file_exists:GameData/ModuleManager*.dll"` |

Conditions are evaluated at match time, not load time. A skill that exists on disk but whose condition fails is simply skipped for that message.

### Matching & Injection Pipeline

```
User message received
        │
        ▼
┌──────────────────────────┐
│ 1. Evaluate conditions   │  Check applies_when for every loaded skill
│    directory_exists?     │  → Skip skills whose condition fails
│    file_exists?          │
└──────────┬───────────────┘
           │ eligible skills
           ▼
┌──────────────────────────┐
│ 2. Keyword scoring       │  User message × skill description
│    Tokenize & compare    │  Cosine similarity or BM25
└──────────┬───────────────┘
           │ scored skills
           ▼
┌──────────────────────────┐
│ 3. Select top 2          │  Highest scoring skills that passed
└──────────┬───────────────┘
           │
           ▼
┌──────────────────────────┐
│ 4. Inject system messages│  One [KNOWLEDGE: name] message per match
│    - Skill body          │  + up to 3 most-relevant reference files
│    - Selected references │  Inserted before the conversation history
└──────────────────────────┘
```

### Reference File Injection

Each skill's `references/` directory contains supplementary `.md` files. When a skill matches:

1. Each reference file is scored independently against the user message
2. The top 3 scoring references are injected alongside the skill body
3. References are formatted as `[KNOWLEDGE: skill-name/reference-title]` system messages

This keeps the skill body compact (overview + key concepts) while making detailed reference material available on demand.

## Configuration Storage

All persistent data lives under `GameData/DeepJeb/`:

```
GameData/
├── DeepJeb/
│   ├── DeepJeb.cfg              # JSON: API keys (XOR+Base64), model limits, settings
│   ├── Sessions/
│   │   ├── 20260620-143021.session   # JSON: full message array + metadata
│   │   └── ...
│   └── Plugins/
│       ├── DeepJeb.dll          # Main mod assembly
│       ├── DeepJeb.Core.dll
│       ├── DeepJeb.Protocol.dll
│       ├── DeepJeb.Unity.dll
│       └── Newtonsoft.Json.dll
└── Skills/                      # Standard Claude Skill documents
    ├── stock-game/
    │   └── ksp-world-knowledge/
    │       ├── SKILL.md         # YAML frontmatter + Markdown body
    │       └── references/      # Supplementary .md files
    └── mods/
        ├── kos-programming/
        │   ├── SKILL.md
        │   └── references/
        ├── krpc-programming/
        │   ├── SKILL.md
        │   └── references/
        ├── mechjeb/
        │   ├── SKILL.md
        │   └── references/
        └── module-manager/
            ├── SKILL.md
            └── references/
```

## Testing Strategy

| Test Type | Target | Framework | Runs Without KSP |
|-----------|--------|-----------|------------------|
| Security filters | Input → allowed/blocked | NUnit | Yes |
| Path sandbox | Path resolution, traversal rejection | NUnit | Yes |
| Skill matcher | Keyword scoring, top-N selection | NUnit | Yes |
| Context manager | Token estimation, truncation logic | NUnit | Yes |
| Session store | Serialize/deserialize JSON sessions | NUnit | Yes |
| Protocol clients | Request building, response parsing (mock HTTP) | NUnit | Yes |
| IMGUI rendering | Layout correctness, scroll behavior | Manual | No |
| KSP integration | Toolbar, scene switching, DontDestroyOnLoad | Manual | No |
| End-to-end | Real API call → tool execution → response | Manual | No |

## Design Decisions & Rationale

1. **Old-style .csproj (not SDK-style)** — .NET Framework 4.6 does not support SDK-style projects. We use the traditional XML format with `ToolsVersion="15.0"`.

2. **Embedded MiniJSON (zero external JSON dependency)** — DeepJeb bundles its own JSON parser/serializer (~550 lines: `MiniJson.cs` + `JsonMapper.cs`). No NuGet packages, no DLLs to bundle, no netstandard façade issues. The typed mapper (`JsonMapper`) handles object↔JSON via reflection; the dynamic parser (`MiniJson`) handles API request/response trees. Both compile directly into `DeepJeb.Core.dll`.

3. **IMGUI (not UGUI/UIToolkit)** — KSP's Unity 2019 uses the old IMGUI system for mod windows. UIToolkit is not available. uGUI (Canvas-based) is possible but IMGUI is simpler for draggable windows and consistent with KSP's modding conventions.

4. **Coroutines + callbacks (not async/await throughout)** — While C# 7 supports async/await, Unity 2019's implementation is incomplete (no `await` in `OnGUI`, limited SynchronizationContext). HTTP calls use `UnityWebRequest` with coroutine callbacks for the Unity layer; Core layer uses `Task`-based async for testability.

5. **Separate test assemblies per source project** — Each source project has a corresponding test project. This enforces that tests only depend on the layer under test. Core tests don't reference Unity; Protocol tests mock HTTP.

6. **Skills are standard Claude SKILL.md documents (not custom .skill files)** — Users can add skills by dropping `SKILL.md` files into `GameData/DeepJeb/Skills/{category}/{skill-name}/`. No recompilation needed. The format is the standard Claude Skill format: YAML frontmatter (`name`, `description`, `author`, `sources`, `category`, `version`, `last_updated`, `applies_when`) + Markdown body, with optional `references/` subdirectory for supplementary knowledge files. This avoids inventing a proprietary format and makes skills portable across Claude-based tools.

7. **No C# Value Tuples — use classes instead** — C# 7.0 value tuples `(string, List, string)` require `System.ValueTuple.dll` at runtime. KSP's Mono does not include this assembly. All multi-return-value patterns use named classes (e.g. `AiResponse`, `ChatResponse`) which compile to plain IL with zero extra dependencies.

8. **API keys XOR+Base64 (not DPAPI)** — KSP runs on Linux/Mac/Windows. DPAPI is Windows-only. XOR+Base64 is trivially reversible but prevents casual snooping. The threat model is "don't show keys on stream," not "defeat a determined attacker with disk access."
