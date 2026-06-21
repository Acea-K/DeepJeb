---
name: mechjeb
description: >
  Standalone knowledge base for MechJeb2, the most popular autopilot and flight
  assistant mod for Kerbal Space Program. Covers all guidance modules, usage,
  career mode integration, user-facing interfaces, and real-time value modification
  via kRPC and kOS. Extracted from ksp-world-knowledge.skill. Compiled from MechJeb
  Wiki, GitHub, CurseForge, Reddit, KSP Forum, and kRPC.MechJeb documentation.
author: Acea
sources:
  - https://github.com/MuMech/MechJeb2
  - https://github.com/MuMech/MechJeb2/wiki
  - https://genhis.github.io/KRPC.MechJeb/
  - https://www.curseforge.com/kerbal/ksp-mods/mechjeb
  - https://www.reddit.com/r/KerbalAcademy/
  - https://forum.kerbalspaceprogram.com/
category: gaming
version: 1.0
last_updated: 2026-06-17
applies_when: "directory_exists:GameData/MechJeb2"
---

# MechJeb — KSP Flight Assistant & Autopilot Knowledge Base

> **⚠ CONDITIONAL SKILL:** This knowledge base applies **only** when MechJeb2 is installed in the current KSP instance. To verify: check that the directory `GameData/MechJeb2` exists within the KSP installation folder. If this directory is not present, the information below may not be relevant to the user's current setup.

**Detection method:**
```bash
# Linux/macOS
ls -d <KSP_ROOT>/GameData/MechJeb2 2>/dev/null && echo "MechJeb installed" || echo "MechJeb NOT installed"

# Windows (PowerShell)
Test-Path "<KSP_ROOT>\GameData\MechJeb2" -PathType Container
```

---

## 1. WHAT IS MECHJEB?

**MechJeb 2** (developed by Sarbian / Anatid Robotics and Multiversal Mechatronics) is the most popular and comprehensive autopilot and flight assistant mod for Kerbal Space Program. With over 3.8 million downloads on CurseForge, it provides both detailed information displays and fully automated guidance systems.

- **Repository:** https://github.com/MuMech/MechJeb2
- **CurseForge:** https://www.curseforge.com/kerbal/ksp-mods/mechjeb
- **Wiki:** https://github.com/MuMech/MechJeb2/wiki
- **CKAN:** Available for one-click install
- **kRPC API:** https://genhis.github.io/KRPC.MechJeb/
- **kOS Addon:** kOS.MechJeb2.Addon on CKAN

**Note:** If MechJeb is installed but the GUI windows do not appear in the flight scene, ensure that the latest version of **Module Manager** is installed (see [module-manager.skill] for syntax reference). MechJeb requires Module Manager to function correctly.

**Philosophical note:** Some players consider MechJeb "cheating" because it automates flights. Others view it as an essential tool that reduces repetitive manual piloting and provides data that should be available to any mission control team. Both perspectives are valid — it's a single-player game.

---

## 2. CORE MODULES

### 2.1 Information Panels
MechJeb provides extensive flight data readouts beyond what the stock game offers, including:
- Delta-V readouts (per-stage, real-time, with atmospheric/vacuum breakdown)
- Orbital parameters (inclination, eccentricity, period, phase angles)
- Surface information (biome, coordinates, slope)
- Vessel stats (TWR, mass, part count, resource levels)
- Target relative information (distance, closing speed, alignment)

**Exposed Data Fields (via kRPC):**
- `Orbital`: Apoapsis, Periapsis, Inclination, Eccentricity, Semi-major axis, Orbital Period
- `Attitude`: Angle of Attack, Angle of Sideslip, Displacement Angle
- `Atmospheric`: Dynamic pressure, Atmospheric density, Mach, Speed of sound
- `Target`: Phase angle to target, Relative inclination, Time to closest approach
- `Vessel`: Vessel mass, Max thrust, G force, Part count

### 2.2 Ascent Guidance
- **Classic Ascent Profile:** Traditional gravity turn with configurable turn start altitude, shape, and final orbit target
- **Stock-style GravityTurn Profile:** 3-burn ascent to orbit (~2800 dV on stock Kerbin)
- **Primer Vector Guidance (PVG):** Advanced ascent profile optimized for RSS/RO
- **Timed launch:** Launch to intercept a target in orbit or to match a specific orbital plane

**Key Parameters:**
- Turn start altitude (default ~100 m for stock, higher for heavy rockets)
- Turn shape exponent (0-1, controls turn steepness)
- Target orbit altitude and inclination
- Max angle of attack limit
- Autostage settings
- Skip circularization toggle
- Auto-deploy solar panels and antennas

### 2.3 Landing Guidance
- Select a target location on the surface
- MechJeb calculates and executes a precision landing burn
- Shows predicted landing point before committing
- Works on vacuum bodies (Mun, Minmus, etc.) and atmospheric bodies (with parachute support)
- Sequence: Coast → Braking → Final descent → Touchdown
- Auto-warp, deploy landing gear, deploy parachutes options

### 2.4 Rendezvous & Docking Autopilot
- Automates the entire rendezvous sequence: Hohmann transfer, phasing orbits, final approach
- Docking autopilot handles terminal guidance and alignment
- Requires controlling from a docking port
- Shows error distances (X, Y, Z) for precise alignment
- Can reduce dozens of manual steps to a single button press

### 2.5 Maneuver Planner
- Pre-programmed maneuvers for common flight operations:
  - Circularize orbit (at apoapsis, periapsis, or AN/DN)
  - Change inclination
  - Transfer to another planet/moon (Hohmann, intercept, flyby/impact)
  - Match target velocity
  - Fine-tune closest approach
  - Advanced transfer with porkchop plot selection

**Burn Scheduling Options:**
- At the optimum time
- At the next apoapsis/periapsis
- At closest approach to target
- At the equatorial AN/DN

### 2.6 Other Modules
- **Rover Autopilot:** Waypoint-based driving with target speed control
- **Aircraft Autopilot:** Altitude and heading hold
- **Smart A.S.S. / A.C.S. (Attitude Selection System):** Quick attitude buttons (prograde, retrograde, normal, radial, target, surface velocity, node)
  - Modes: OBT (Orbit), SURF (Surface), TGT (Target), ADV (Advanced), AUTO
- **Node Execution:** Auto-execute maneuver nodes at precise timing with merge capability
- **Translatron:** Hold vertical velocity or surface speed (useful for hovering)
- **RCS Balancer:** Adjusts RCS output with Overdrive for power vs. fuel efficiency
- **Warp Helper:** Warps to periapsis, apoapsis, maneuver node, SoI transition, phase angle
- **Landing Predictions:** Hoverslam settings for vertical phase altitude, touchdown speed, simulation interval (see §2.3)
- **Utilities:** Auto-stage, auto-warp, auto-deploy solar panels (see §2.2, §4.2)

---

## 3. USER-FACING INTERFACES

### 3.1 GUI Windows
MechJeb opens as a floating GUI window in the flight scene. Each module has its own tab/window that can be toggled independently. The main interface elements are:

| Window | Purpose | Key Controls |
|--------|---------|--------------|
| **Ascent Guidance** | Launch to orbit | Enable/disable, target altitude/inclination, turn shape |
| **Landing Guidance** | Precision landing | Target selector, predicted landing point, execute |
| **Rendezvous Guidance** | Close approach | Sequence buttons, approach speed |
| **Maneuver Node Editor** | Create/edit nodes | Node type, timing, parameters |
| **Docking Autopilot** | Terminal docking | Error display, alignment controls |
| **Rover Autopilot** | Surface navigation | Waypoint list, target speed |
| **Smart A.S.S.** | Quick attitude | Mode selector, attitude buttons |
| **Information readouts** | Telemetry data | Delta-V, orbital, surface, target data |

### 3.2 Parts Required
- **Feature Unlock Parts:** In Career mode, these unlock modules:
  - MechJeb Features - Maneuver & Translatron
  - MechJeb Features - Rover Autopilot
  - MechJeb Features - Rendezvous & Docking
  - MechJeb Features - Ascent, Landing, Spaceplane

### 3.3 Career Mode Progression
> See **§5. CAREER MODE INTEGRATION** for the complete tech tree unlock table and important notes.

---

## 4. PROGRAMMING API REFERENCE

> Detailed programming examples and API references have been extracted to separate files:
>
> - **kRPC Python API, kOS Integration, MM Patches, C# API:** [references/programming-api.md](references/programming-api.md)
>
> These cover kRPC.MechJeb setup, AscentAutopilot properties, Classic Ascent Profile parameters, kOS.MechJeb2.Addon usage, Module Manager .cfg patch examples, and C# plugin API.

---

## 5. CAREER MODE INTEGRATION

In Career/Science mode, MechJeb features are unlocked progressively through the tech tree:

| Tech Level | Unlocked Features |
|------------|------------------|
| Flight Control | Basic info panels, Smart A.S.S. |
| Advanced Flight Control | Ascent guidance, maneuver planner |
| Unmanned Tech | Landing guidance, rendezvous autopilot |
| Advanced Unmanned Tech | Full automation suite |

The exact tech tree placement depends on which tree mods are installed (CTT, etc.). The progression is designed to prevent new players from having full autopilot immediately.

**Important Note:** The Tracking Station must be upgraded to Level 2 for some features to work correctly.

---

## 6. COMMON ISSUES & SOLUTIONS

> Troubleshooting guide extracted to [references/troubleshooting.md](references/troubleshooting.md), covering MechJeb not appearing, Windows loading issues, ascent/landing guidance failures, kRPC connection issues, and kOS addon not found.

---

## 7. CHINESE LOCALIZATION

MechJeb has a Chinese language translation patch. Search for "MechJeb2 Chinese patch" or "MechJeb2 汉化" to find the community-maintained Chinese translation.
- The Chinese patch is typically distributed as a replacement language file
- May need to be updated after MechJeb version upgrades
- Installation: Replace the localization file in the MechJeb folder

---

## 8. COMMUNITY RESOURCES

> Full community resources list extracted to [references/community-resources.md](references/community-resources.md), including official links, programming integration references, tutorials, and related tools.

**Related Knowledge Bases:**
- KSP World Knowledge: ksp-world-knowledge.skill
- Module Manager: module-manager.skill
- kOS Programming: kos-programming.skill
- kRPC Programming: krpc-programming.skill

---
