---
name: module-manager
description: >
  Knowledge base for Module Manager (MM), the essential KSP modding tool that
  allows config-file patching at load time. Covers patch syntax, operators,
  ordering directives, NEEDS dependency checking, variables, common patterns,
  and troubleshooting. Compiled from the official GitHub wiki, the KSP Forum
  thread, and community guides.
author: Acea
sources:
  - https://github.com/sarbian/ModuleManager/wiki/Module-Manager-Syntax
  - https://github.com/sarbian/ModuleManager/wiki/Patch-Ordering
  - https://github.com/sarbian/ModuleManager
  - https://forum.kerbalspaceprogram.com/topic/50533-18x-112x-module-manager-423-july-03th-2023-fireworks-season/
  - https://forum.kerbalspaceprogram.com/topic/198716-basic-module-manager-tutorial/
category: gaming
version: 1.0
last_updated: 2026-06-18
applies_when: "file_exists:GameData/ModuleManager*.dll"
---

# Module Manager — KSP Config Patching Tool

> **⚠ CONDITIONAL SKILL:** This knowledge base applies **only** when Module Manager is installed in the current KSP instance. To verify: check that `GameData/ModuleManager*.dll` (e.g. `ModuleManager.4.2.3.dll`) exists directly in the GameData folder. If not present, the information below is not relevant.

**Detection method:**
```bash
# Linux/macOS
ls GameData/ModuleManager*.dll 2>/dev/null && echo "MM installed" || echo "MM NOT installed"

# Windows (PowerShell)
Test-Path "$env:KSP_DIR\GameData\ModuleManager*.dll" -PathType Leaf
```

---

## 1. WHAT IS MODULE MANAGER?

**Module Manager** (MM), originally by Ialdabaoth and maintained by Sarbian & Blowfish, is the single most important dependency mod in KSP. It lets mod authors write **patch files** (`.cfg`) that modify game parts, modules, and properties **at load time**, without overwriting any original files.

- **Latest Version:** 4.2.3 (July 2023)
- **KSP Compatibility:** 1.8.x – 1.12.x
- **Repository:** https://github.com/sarbian/ModuleManager
- **Download:** https://ksp.sarbian.com/jenkins/job/ModuleManager/lastStableBuild
- **CKAN:** Available as `ModuleManager` (auto-installed by most mod packs)

**Philosophical note:** MM is the foundation of the KSP modding ecosystem. Almost every mod either ships with MM or requires it. Without MM, mod authors would have to manually overwrite Squad's config files — breaking compatibility with every other mod.

---

## 2. CORE CONCEPTS

### 2.1 ConfigNode Structure

KSP stores all part, module, and game configuration in a nested tree of **ConfigNodes**. Module Manager patches operate directly on this tree.

```
// Comment (everything after // is ignored)
PART
{
    name = myPart
    module = Part
    MODULE
    {
        name = ModuleEngines
    }
}
```

### 2.2 Patch Prefixes

Every patch line starts with an operator character that tells MM what to do:

| Operator | Name | Description |
|----------|------|-------------|
| `@` | Edit | Modify existing node or value |
| `+` or `$` | Copy | Duplicate a node/value |
| `-` or `!` | Delete | Remove node or value |
| `%` | Edit-or-Create | Edit if exists; create if not |
| `&` | Create | Create only if not already present |
| `\|` | Rename | Rename a node |
| `#` | Copy Node | Copy a node from elsewhere in the tree |

### 2.3 Patch Ordering Directives

At the top-level node, you can specify **one** ordering directive to control when the patch applies:

| Directive | Pass | Use Case |
|-----------|------|----------|
| `:FIRST` | First pass | Critical foundational changes |
| `:BEFORE[modname]` | Before mod's pass | Prepare data for another mod |
| `:FOR[modname]` | During mod's pass | **Register your mod's name** for others to reference |
| `:AFTER[modname]` | After mod's pass | Modify after another mod has done its work |
| `:LAST` | Late pass | Late-stage modifications |
| `:FINAL` | Final pass | **Reserved for end-user personal patches only** |

**Application order overall:** `INSERT(first)` → `:FIRST` → `(no directive)` → `:BEFORE/:FOR/:AFTER` (sorted alphabetically by modname) → `:LAST` → `:FINAL`

### 2.4 Valid Modname Values

Module Manager builds a list of valid modnames (case-insensitive) from:
- DLL files in `GameData/` (uses assembly name, not filename)
- Folders directly in `GameData/`
- Mods that declare `:FOR[modname]` in any of their patches

**Example — registering so other mods can reference you:**
```
// Even if MyCoolMod lives in a subfolder, this :FOR declaration
// makes it visible to :BEFORE[MyCoolMod] and :AFTER[MyCoolMod]
@PART[somePart]:FOR[MyCoolMod]
{
    @someValue = 1
}
```

---

## 3. PATCH OPERATORS

> Full patch operator reference extracted to [references/patch-operators.md](references/patch-operators.md), covering value/node syntax, Insert, Edit (`@`), Copy (`+`/`$`), Delete (`-`/`!`), Edit-or-Create (`%`), Create (`&`), Rename (`|`), and Copy Node (`#`) with code examples for each.

---

## 4. DEPENDENCY CHECKING (`NEEDS`)

The `:NEEDS` directive can be used on patches, nodes, or individual values to conditionally apply changes based on which mods are installed.

**Syntax:**
```
:NEEDS[expression]
```

**Operators:**
| Symbol | Meaning |
|--------|---------|
| `&` or `,` | AND |
| `\|` | OR |
| `!` | NOT |

**Evaluation precedence:** `!` (NOT) binds tighter than `&` (AND), which binds tighter than `|` (OR). Use parentheses implicitly — `NEEDS[A|B&C]` = A OR (B AND C).

**Examples:**

```
// Patch only if both RealFuels and ModularFuelSystem are installed
PART:NEEDS[RealFuels&ModularFuelSystem] { ... }

// Patch if either mod is present
PART:NEEDS[RealFuels|ModularFuelSystem] { ... }

// Different descriptions depending on which mod is active
PART
{
    name = myPart
    description:NEEDS[RealFuels&!ModularFuelSystem] = Used in Real Fuels
    description:NEEDS[ModularFuelSystem&!RealFuels] = Used in Modular Fuel System
}
```

**DLL Detection:** Module Manager checks the **assembly name** (File Description in DLL properties), not the filename. A DLL named `MyMod.dll` with assembly name `MyCoolMod` registers as `MyCoolMod`.

---

## 5. VARIABLES

Module Manager supports variable substitution using `#` and `$` syntax.

| Symbol | Meaning |
|--------|---------|
| `#` | Variable indicator |
| `$` | Start/end of variable identifier |
| `@` | Reference a different top-level node |

**Example — copy a value from another part:**
```
@title = #$@PART[liquidEngine1-2]/title$
@MODULE[ModuleEngines]
{
    @PROPELLANT[LiquidFuel]
    {
        @mass = #$../../mass$    // Reference parent node's mass
    }
}
```

---

## 6. COMMON USE CASES

> Common use case code examples, troubleshooting guide, and community resources extracted to [references/examples-and-refs.md](references/examples-and-refs.md), covering adding modules, modifying engines, cloning parts, removing modules, conditional patching, setting defaults, and common issues with solutions.

---

## 7. BEST PRACTICES

- **Use `:FOR[YourModName]`** at least once so other modders can reference you with `:BEFORE`/`:AFTER`.
- **Reserve `:FINAL`** for end-user personal patches only — never ship a distributed mod with `:FINAL`.
- **Prefer `:BEFORE`/`:AFTER`** over `:FINAL` for inter-mod compatibility.
- **Use `&` (create)** for setting safe defaults that users can override.
- **Use `!` (delete) with caution** — test that the deleted module truly exists first.
- **Test your patches** on a clean install of your target dependencies to catch unexpected interactions.
- **Wildcards `*` and `?`** are powerful but expensive — narrow your target as much as possible.

---

## 8. COMMON ISSUES & SOLUTIONS

> Troubleshooting guide and community resources extracted to [references/examples-and-refs.md](references/examples-and-refs.md), covering MM hang at load, patch not applying, slow loading, wrong values, and installation errors.

---

## 9. COMMUNITY RESOURCES

> Community resources (official links, tutorials) extracted to [references/examples-and-refs.md](references/examples-and-refs.md).

**Related Knowledge Bases:**
- KSP World Knowledge: ksp-world-knowledge.skill
- MechJeb: mechjeb.skill
- kRPC Programming: krpc-programming.skill
