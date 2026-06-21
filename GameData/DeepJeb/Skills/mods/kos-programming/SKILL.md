---
name: kos-programming
description: >
  Comprehensive knowledge base for kOS (Kerbal Operating System), the programmable
  scripting mod for Kerbal Space Program. Covers the KerboScript language, core
  features, bound variables, flight control, maneuver nodes, math library, file
  system, boot files, triggers, and community resources. Compiled from official
  kOS documentation (v1.4.0.0), KSP-KOS GitHub, Reddit r/Kos, and
  SpaceDock.ru community guides.
author: Acea
sources:
  - https://ksp-kos.github.io/KOS/
  - https://github.com/KSP-KOS/KOS
  - https://www.reddit.com/r/Kos/
  - https://spacedock.ru/kerbal-space-program/guides-ksp/
  - /opt/data/workspace/20260617/KSPWorldKnowledge/kspWorldKnowledge.skill
category: gaming
version: 1.0
last_updated: 2026-06-17
applies_when: "directory_exists:GameData/kOS"
---

# kOS Programming — Kerbal Operating System Knowledge Base

> **⚠ CONDITIONAL SKILL:** This knowledge base applies **only** when kOS is installed in the current KSP instance. To verify: check that the directory `GameData/kOS` exists within the KSP installation folder. If this directory is not present, the information below may not be relevant to the user's current setup.

**Detection method:**
```bash
# Linux/macOS
ls -d <KSP_ROOT>/GameData/kOS 2>/dev/null && echo "kOS installed" || echo "kOS NOT installed"

# Windows (PowerShell)
Test-Path "<KSP_ROOT>\GameData\kOS" -PathType Container
```

> This .skill file is a structured world knowledge base for LLM retrieval. It is NOT installed as a Hermes Agent skill. For general KSP game mechanics, see ksp-world-knowledge.skill. For MechJeb (autopilot mod), see mechjeb.skill.

---

## 1. WHAT IS kOS?

**kOS** (Kerbal Operating System) is a community-supported mod for Kerbal Space Program that adds a fully programmable onboard computer to every spacecraft. Players write scripts in **KerboScript**, a domain-specific scripting language, to automate launch, orbital maneuvers, rendezvous, docking, landing, and more.

- **Version:** 1.4.0.0 (latest)
- **Repository:** https://github.com/KSP-KOS/KOS
- **Documentation:** https://ksp-kos.github.io/KOS/
- **Reddit community:** https://www.reddit.com/r/Kos/
- **Original author:** Kevin Laity (Nivekk); now maintained by KSP-KOS org

**Philosophy:** kOS is to programming what KSP is to rocket science — you don't have to know what you're doing to get started, but you'll learn by accident. It scales with your skill level.

---

## 2. INSTALLATION & SETUP

### 2.1 Install the Mod
Copy the contents of the kOS zip into your KSP folder — it installs into `GameData/`.

### 2.2 Add kOS Processor to a Vessel
In the VAB/SPH, find the **Comptronix CX-4181** kOS processor part and attach it to your vessel. This is the hardware that runs scripts.

### 2.3 Open the Terminal
- Press **[ (open bracket)** to open the kOS terminal window
- The terminal is a text-based interface for typing commands directly
- Type `print "Hello Kerbin!"` and press Enter to test

### 2.4 Boot Files
- Create a file named `boot.ks` in the kOS volume (e.g., on the processor's disk)
- This script runs automatically when the vessel loads or launches
- Used for auto-configuration, staging setup, and pre-flight checks

---

## 3. KERBOSCRIPT LANGUAGE

> Full language reference extracted to [references/language-reference.md](references/language-reference.md), covering syntax, types, variables, scoping, operators, control structures, user functions, and built-in name protection.

---

## 4. FLIGHT CONTROL

> Flight control, triggers, maneuver nodes, and encounter/transfer code examples extracted to [references/flight-and-maneuvers.md](references/flight-and-maneuvers.md), covering steering, throttle, staging, RCS/SAS, action groups, WHEN/ON/WAIT triggers, maneuver node creation/execution, and transfer calculations.

---

## 5. BUILT-IN VARIABLES (BOUND VARIABLES)

> Full reference tables for bound variables, math library, list operations, and file system extracted to [references/reference-tables.md](references/reference-tables.md), covering ship/orbit/flight/resource/control variables, vector/trig/math functions, list methods, and file I/O.

---

## 6. MATH LIBRARY

> Math library reference extracted to [references/reference-tables.md](references/reference-tables.md), covering vector operations, trigonometric functions, math functions, and constants.

---

## 7. LIST OPERATIONS

> List operations reference extracted to [references/reference-tables.md](references/reference-tables.md), covering list creation and all list methods.

---

## 8. FILE SYSTEM

> File system reference extracted to [references/reference-tables.md](references/reference-tables.md), covering volumes, reading/writing, file methods, and JSON support.

---

## 9. TRIGGERS AND EVENTS

> Trigger and event code examples extracted to [references/flight-and-maneuvers.md](references/flight-and-maneuvers.md), covering WHEN/ON triggers and WAIT commands.

---

## 10. MANEUVER NODES

> Maneuver node code examples extracted to [references/flight-and-maneuvers.md](references/flight-and-maneuvers.md), covering node creation, execution, and common maneuvers.

---

## 11. ENCOUNTERS AND TRANSFERS

> Encounter check and transfer window calculator code examples extracted to [references/flight-and-maneuvers.md](references/flight-and-maneuvers.md).

---

## 12. COMMON SCRIPTS

> Ready-to-use script examples extracted to [references/example-scripts.md](references/example-scripts.md):
>
> - **Launch Script** — Standard launch-to-orbit sequence
> - **Gravity Turn** — Dynamic pitch steering
> - **Landing Script** — Retrograde suicide burn

---

## 13. ADDONS AND INTEGRATION

### 13.1 kOS.MechJeb2.Addon
Direct integration between kOS and MechJeb. Install via CKAN. For detailed API and usage, see **mechjeb.skill §4.3**.

### 13.2 kRPC Integration
- **kIPC:** Inter-Process Communication bridge between kOS and kRPC. For details, see **krpc-programming.skill**.

### 13.3 Other Addons
- **kOS-EVA:** Control Kerbals on EVA
- **kOS-ScanSat:** Integration with ScanSat mod

---

## 14. TIPS AND BEST PRACTICES

### 14.1 Script Organization
- Use `@LAZYGLOBAL OFF.` at the top of scripts
- Define functions with `FUNCTION name { PARAMETER p1. ... }`
- Use `RUNPATH("script.ks", arg1, arg2).` for modular code

### 14.2 Performance
- Minimize `WAIT UNTIL` in tight loops
- Cache frequently accessed values
- Use `WHEN` triggers instead of polling where possible

### 14.3 Debugging
- Check the kOS terminal for error messages and stack traces
- Use `LIST` commands to inspect variables, bodies, vessels, and parts in scope
- Use `LOG` to write debug output to persistent files for post-flight analysis

### 14.4 Common Pitfalls
- **Case insensitivity:** Everything is case-insensitive
- **Decimal points:** Always use `.` for decimal (not `,`)
- **Vector math:** Remember vectors use parentheses: `V(x,y,z)`
- **Trigger scope:** `WHEN` triggers capture variables by reference, not by value
- **@LAZYGLOBAL OFF:** Requires explicit variable declarations
- **Built-in protection:** Cannot overwrite built-in names without `@CLOBBERBUILTINS`

---

## APPENDIX: QUICK REFERENCE

> Essential commands and resource links extracted to [references/quick-reference.md](references/quick-reference.md), covering PRINT/SET/LOCK/UNLOCK/RUN commands, LIST operations, and key documentation links.
