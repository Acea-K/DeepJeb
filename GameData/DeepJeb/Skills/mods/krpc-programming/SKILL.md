---
name: krpc-programming
description: >
  Comprehensive knowledge base for kRPC (Kerbal Remote Procedure Call), an
  external scripting framework for Kerbal Space Program. Covers architecture,
  client libraries (Python/C#/Lua/Java), connection setup, SpaceCenter API,
  AutoPilot, Vessel control, reference frames, streaming data, event system,
  extending with custom services, and community resources. Compiled from
  official kRPC documentation (v0.5.4), GitHub, and community sources.
author: Acea
sources:
  - https://krpc.github.io/krpc/
  - https://krpc.github.io/krpc/getting-started.html
  - https://krpc.github.io/krpc/tutorials/autopilot.html
  - https://krpc.github.io/krpc/extending.html
  - https://krpc.github.io/krpc/third-party.html
  - https://github.com/krpc/krpc
  - https://genhis.github.io/KRPC.MechJeb/
category: gaming
version: 1.0
last_updated: 2026-06-17
applies_when: "directory_exists:GameData/kRPC"
---

# kRPC Programming — Remote Procedure Call for KSP

> **⚠ CONDITIONAL SKILL:** This knowledge base applies **only** when kRPC is installed in the current KSP instance. To verify: check that the directory `GameData/kRPC` exists within the KSP installation folder. If this directory is not present, the information below may not be relevant to the user's current setup.

**Detection method:**
```bash
# Linux/macOS
ls -d <KSP_ROOT>/GameData/kRPC 2>/dev/null && echo "kRPC installed" || echo "kRPC NOT installed"

# Windows (PowerShell)
Test-Path "<KSP_ROOT>\GameData\kRPC" -PathType Container
```

> This .skill file is a structured world knowledge base for LLM retrieval. It is NOT installed as a Hermes Agent skill. For general KSP game mechanics, see ksp-world-knowledge.skill. For kOS scripting, see kos-programming.skill. For MechJeb integration, see mechjeb.skill.

---

## 1. WHAT IS KRPC?

**kRPC** (Kerbal Remote Procedure Call) is a mod framework that allows external programs to control Kerbal Space Program in real-time via remote procedure calls. It provides a server plugin running inside KSP and client libraries in multiple programming languages.

- **Repository:** https://github.com/krpc/krpc
- **Documentation:** https://krpc.github.io/krpc/
- **SpaceDock:** https://spacedock.info/mod/69/kRPC
- **Current Version:** 0.5.4
- **License:** GNU Lesser General Public License v3.0

**Key Features:**
- Control vessels from external scripts (Python, C#, Lua, Java, C++, Go, Rust, Ruby, Haskell, Node.js, Julia)
- Real-time bidirectional communication
- Low-latency streaming of telemetry data
- Extensible via custom services (add new API endpoints from mods)
- Supports local and network connections
- Integrates with MechJeb, Ferram Aerospace Research, Infernal Robotics, Kerbal Alarm Clock

---

## 2. ARCHITECTURE

```
┌─────────────────────────────────────────────────────┐
│                   External Program                  │
│  (Python / C# / Lua / Java / etc.)                  │
│                                                     │
│  ┌─────────────┐    ┌─────────────┐                 │
│  │  Client API │    │  Stream API │                 │
│  └──────┬──────┘    └──────┬──────┘                 │
│         │                  │                        │
└─────────┼──────────────────┼────────────────────────┘
          │  gRPC / Protobuf │
          ▼                  ▼
┌─────────────────────────────────────────────────────┐
│                 kRPC Server Plugin                  │
│              (running inside KSP)                   │
│                                                     │
│  ┌─────────────────────────────────────────────┐   │
│  │           SpaceCenter Service               │   │
│  │  (Vessel, Orbit, Flight, Control, Parts)    │   │
│  └─────────────────────────────────────────────┘   │
│                                                     │
│  ┌─────────────────────────────────────────────┐   │
│  │         Custom Services (from mods)         │   │
│  │  (kRPC.MechJeb, InfernalRobotics, etc.)    │   │
│  └─────────────────────────────────────────────┘   │
│                                                     │
│  ┌─────────────────────────────────────────────┐   │
│  │              KSP Game API                    │   │
│  └─────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

**Communication Protocol:**
- **RPC Port:** 50000 (default) — bidirectional remote procedure calls
- **Stream Port:** 50001 (default) — continuous data streaming
- **Protocol:** Protobuf over TCP (default), WebSocket, or SerialIO

---

## 3. INSTALLATION & SETUP

### 3.1 Server Plugin
1. Download from GitHub Releases, SpaceDock, Curse, or CKAN
2. Install into KSP's `GameData/` folder
3. Start KSP, load a save
4. Server window appears — click "Start Server" (green light)

**Server Configuration (Edit button):**
| Setting | Default | Description |
|---------|---------|-------------|
| Protocol | Protobuf over TCP | Wire format for communication |
| Address | localhost | Network interface to bind |
| RPC Port | 50000 | Remote procedure call port |
| Stream Port | 50001 | Data streaming port |
| Auto-start | false | Start server when KSP loads |
| Auto-accept | false | Accept clients without popup |

### 3.2 Python Client
```bash
pip install krpc
```

### 3.3 Other Client Libraries
| Language | Package / NuGet | Status |
|----------|----------------|--------|
| Python | `pip install krpc` | Official |
| C# | `KRPC.Client` (NuGet) | Official |
| Lua | `luarocks install krpc` | Official |
| Java | `krpc-java` (Maven) | Official |
| C++ | Client stub generation required | Official |
| Go | `github.com/nathan-boulestin/krpc` | Community |
| Rust | `krpc-mars` or `krpc-client` | Community |
| Ruby | `krpc-rb` | Community |
| Haskell | `krpc-hs` | Community |
| Node.js | `krpc-node` | Community |
| Julia | `KRPC.jl` | Community |

---

## 4. CONNECTION & BASIC USAGE

> Connection examples (Python/C#/Lua) and SpaceCenter service API reference extracted to [references/core-api.md](references/core-api.md), covering connection setup, vessel properties, control inputs, reference frames, flight telemetry, and autopilot.

---

## 5. CORE API — SPACECENTER SERVICE

> SpaceCenter service API reference extracted to [references/core-api.md](references/core-api.md), covering vessel properties table, control inputs, and reference frames.

---

## 6. FLIGHT TELEMETRY

> Flight telemetry and orbit information code examples extracted to [references/core-api.md](references/core-api.md).

---

## 7. AUTOPILOT

> AutoPilot usage, configuration, and error reporting extracted to [references/core-api.md](references/core-api.md).

---

## 8. STREAMING DATA

> Detailed streaming examples extracted to [references/streaming-data.md](references/streaming-data.md), covering stream creation, polling vs streaming, waiting for conditions, callbacks, and custom events.

---

## 9. VESSEL PARTS & RESOURCES

> Vessel parts and resources code examples extracted to [references/extras.md](references/extras.md), covering accessing/controlling parts and resource management.

---

## 10. ORBITAL MECHANICS

> Detailed code examples extracted to [references/orbital-mechanics.md](references/orbital-mechanics.md), covering maneuver node creation/execution and Hohmann transfer calculations with Python.

---

## 11. WARP & TIME CONTROL

> Time warp code examples extracted to [references/extras.md](references/extras.md).

---

## 12. EXTENDING KRPC — CUSTOM SERVICES

> C# custom service API and available mod services extracted to [references/extras.md](references/extras.md).

---

## 13. COMPLETE EXAMPLE — LAUNCH TO ORBIT

> Full Python launch-to-orbit script extracted to [references/launch-to-orbit-example.md](references/launch-to-orbit-example.md), covering gravity turn, coast to apoapsis, burn time calculation, and circularization.

---

## 14. COMMUNITY RESOURCES

> Community resources, third-party extensions, and quick reference extracted to [references/extras.md](references/extras.md).

**Related Knowledge Bases:**
- KSP World Knowledge: ksp-world-knowledge.skill
- kOS Programming: kos-programming.skill
- MechJeb: mechjeb.skill

---

## APPENDIX: QUICK REFERENCE

> Connection and stream quick reference extracted to [references/extras.md](references/extras.md).
