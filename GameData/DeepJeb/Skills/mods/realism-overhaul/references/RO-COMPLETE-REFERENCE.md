# Realism Overhaul — Complete Reference

Comprehensive quick-reference for the KSP-RO mod suite (Kerbal Space Program 1.12.x). Covers all 68 repositories across 8 categories. Self-contained — each section provides everything needed without cross-referencing.

---

## Table of Contents

- [1. Complete Repository Index (All 68)](#1-complete-repository-index-all-68)
- [2. System Requirements](#2-system-requirements)
- [3. Core & Foundational Mods](#3-core--foundational-mods)
- [4. Engine & Propulsion](#4-engine--propulsion)
- [5. Parts, Tanks & Capsules](#5-parts-tanks--capsules)
- [6. Life Support & Reliability](#6-life-support--reliability)
- [7. Visuals & Environment](#7-visuals--environment)
- [8. Historical Spacecraft & Launch Vehicles](#8-historical-spacecraft--launch-vehicles)
- [9. Utilities & Tools](#9-utilities--tools)
- [10. Infrastructure & Build Tools](#10-infrastructure--build-tools)
- [11. Dependency Tree & Conflict Map](#11-dependency-tree--conflict-map)
- [12. False KSP Lessons](#12-false-ksp-lessons)
- [13. Cross-Mod Integration Notes](#13-cross-mod-integration-notes)
- [14. Key Technical Terms Glossary](#14-key-technical-terms-glossary)
- [15. Consolidated FAQ](#15-consolidated-faq)

---

## 1. Complete Repository Index (All 68)

Complete index of all 68 public repositories in the [KSP-RO GitHub organization](https://github.com/KSP-RO). Data from GitHub API, updated 2026-06-22.

### 1.1 Core / Foundational Mods

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 1 | RealismOverhaul | 414 | 285 | 83 | C# | Multipatch to KSP to give things realistic stats and sizes |
| 2 | RP-1 | 462 | 244 | 185 | C# | Realistic Progression One — Career mode for Realism Overhaul |
| 3 | RealSolarSystem `(fork)` | 147 | 95 | 24 | Wolfram | Changes KSP's solar system to make it like the real one |
| 4 | RealFuels | 58 | 70 | 29 | C# | Modular fuel tanks and engines, with real fuels |
| 5 | ProceduralParts | 75 | 82 | 30 | C# | Continuation of StretchySRBs/StretchyTanks |

### 1.2 Engine & Propulsion

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 6 | SolverEngines | 8 | 20 | 6 | C# | ModuleEnginesFX subclass supporting AJE-style solvers |
| 7 | AJE | 18 | 23 | 9 | C# | Advanced Jet Engine for KSP |
| 8 | ROEngines | 23 | 47 | 20 | C# | Engine models/part configs replacing generic Squad engines |
| 9 | RealPlume | 35 | 37 | 6 | Perl | Collection of effects and polished plume configs |
| 10 | RealPlume-StockConfigs | 23 | 42 | 5 | — | RealPlume configs for stock-sized engines |

### 1.3 Parts / Tanks / Capsules

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 11 | ROTanks | 14 | 18 | 12 | Python | Modular tanks (and more) derived from SSTU |
| 12 | ROCapsules | 13 | 35 | 10 | — | Best capsule models for RO parts |
| 13 | ROHeatshields | 2 | 10 | 5 | C# | Dynamically resizable heatshields |
| 14 | ProceduralFairings `(fork)` | 12 | 17 | 13 | C# | Procedural Fairings mod |
| 15 | ProceduralFairings-ForEverything | 13 | 15 | 6 | — | Add PF to every mod with fairings |
| 16 | ProceduralSolidsLibrary | 3 | 3 | 0 | C# | (procedural solids library) |
| 17 | StagedAnimation | 2 | 3 | 1 | C# | Animation on stage |

### 1.4 Life Support, Crew & Reliability

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 18 | TacLifeSupport `(fork)` | 45 | 25 | 26 | C# | TAC Life Support (food, water, oxygen) |
| 19 | TestFlight | 44 | 33 | 35 | C# | Part research and reliability system |
| 20 | ROKerbalism | 27 | 39 | 20 | — | Kerbalism configuration for RO/RP-1 |
| 21 | Kerbalism `(fork)` | 2 | 2 | 1 | C# | Kerbalism life support & science (parent) |
| 22 | DeadlyReentry | 28 | 9 | 3 | C# | Heat and force damage — **⚠️ INCOMPATIBLE with RO v18.x+** |
| 23 | FlightSchool `(fork)` | 2 | 3 | 0 | C# | Kerbal flight school requirement |

### 1.5 Visual & Environmental

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 24 | RSS-Textures | 54 | 47 | 3 | C++ | Real Solar System Textures |
| 25 | RSSVE | 35 | 15 | 2 | — | Visual enhancement pack for RSS |
| 26 | RSS-CanaveralHD `(fork)` | 4 | 3 | 2 | — | HD terrain for Cape Canaveral/KSC |
| 27 | ScaledRSS-Textures | 6 | 6 | 0 | — | Scaled textures for RSS |
| 28 | RealHeat | 13 | 19 | 1 | C# | Real heating (not temperature) model |
| 29 | RealISRU | 13 | 12 | 6 | C# | Realistic ISRU |
| 30 | ROSolar | 7 | 12 | 3 | Python | (solar-related RO mod) |
| 31 | RSSTimeFormatter | 13 | 19 | 1 | C# | Time formatting for RSS |

### 1.6 Infrastructure, Build Tools & Libraries

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 32 | BuildTools | 1 | 2 | 0 | Python | Workflow snippets for KSP-RO CI/CD |
| 33 | BuildLibs | 0 | 1 | 0 | — | Encrypted library files for building |
| 34 | CFGProjectGenerator | 2 | 2 | 0 | C# | Generates .csproj from .cfg files |
| 35 | ROLibrary | 6 | 12 | 2 | C# | Shared code library for ROx family |
| 36 | ROUtils | 1 | 3 | 1 | C# | Base library for KSP-RO mods |
| 37 | ROLoadingImages | 1 | 4 | 0 | C# | (loading screen images) |
| 38 | ContractConfigurator `(fork)` | 11 | 15 | 11 | C# | Config-based contract creation |
| 39 | RP-1-ExpressInstall | 2 | 5 | 8 | — | Express install support for RO/RP-1 on CKAN |
| 40 | RP-1-ExpressInstall-Graphics | 1 | 7 | 4 | — | Express install stub for RSS graphics |
| 41 | RONoCareer | 0 | 2 | 0 | — | Express install stub (no career) |
| 42 | ECM-Viewer | 0 | 0 | 0 | HTML | (ECM viewer) |
| 43 | ProgramsIssueTracker | 2 | 0 | 0 | — | (issue tracker for programs) |

### 1.7 Launch Vehicles & Spacecraft (Historical)

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 44 | SovietRockets | 13 | 6 | 0 | — | Soyuz and Proton Rockets |
| 45 | SovietSpacecraft | 9 | 9 | 0 | — | Soyuz, Vostok, Voskhod, TKS/VA |
| 46 | SovietProbes | 10 | 4 | 0 | — | Long-term Soviet probe project |
| 47 | USProbesPack | 12 | 8 | 0 | — | Long-term US probe project |
| 48 | USRockets | 4 | 4 | 0 | — | Older/obscure US launchers |
| 49 | Antares-Cygnus | 5 | 5 | 0 | — | Antares LV, Cygnus cargo vehicle |
| 50 | SalyutStations | 11 | 8 | 0 | — | DOS/OPS style stations |
| 51 | Skylab | 5 | 4 | 0 | — | Broken and unbroken Skylab |
| 52 | RNMisc | 4 | 4 | 0 | — | Raidernick's Misc Mods |
| 53 | USSovietSolarPanelsPack | 2 | 6 | 0 | — | US and Soviet solar panel parts |
| 54 | ROStations | 2 | 3 | 1 | C# | (station parts) |
| 55 | FASA-RO | 19 | 20 | 9 | ShaderLab | FASA Realism Overhaul Edition |
| 56 | FASA-Retexture | 2 | 3 | 0 | — | FASA retexture (US flag) |
| 57 | GoForLaunch | 3 | 3 | 1 | — | Collaborative RP-0 career |
| 58 | CanaveralPads | 2 | 2 | 0 | — | Launch pad configurations |

### 1.8 Other Utilities

| # | Repo | Stars | Forks | Issues | Language | Description |
|---|------|-------|-------|--------|----------|-------------|
| 59 | Kerbal-Joint-Reinforcement-Continued `(fork)` | 39 | 13 | 2 | C# | Physics stabilizer plugin |
| 60 | Kerbal-Konstructs `(fork)` | 16 | 14 | 14 | C# | Static object placement mod |
| 61 | KSCSwitcher `(fork)` | 20 | 22 | 3 | C# | Moves KSC to a new location |
| 62 | KerbalRenamer `(fork)` | 8 | 12 | 3 | C# | Kerbal renaming tool |
| 63 | RealAntennas `(fork)` | 4 | 16 | 22 | C# | Better antenna/link calculations |
| 64 | LunarTransferPlanner | 4 | 6 | 10 | C# | Lunar transfer planning tool |
| 65 | CustomPreLaunchChecks `(fork)` | 4 | 2 | 0 | C# | Pre-launch check system |
| 66 | Blastwave `(fork)` | 1 | 2 | 0 | C# | (explosion effects) |
| 67 | RP1AnalyticsWebApp | 3 | 8 | 2 | C# | Spyware v2.0 (RP-1 analytics) |
| 68 | RP-1-ExpressInstall-Graphics | 1 | 7 | 4 | — | (graphics express install stub) |

### Key URLs

- **KSP-RO Organization:** https://github.com/KSP-RO
- **RO Forum Thread:** https://forum.kerbalspaceprogram.com/topic/155700-112-ksp-ro-realism-overhaul-16-may-2022/
- **RO Wiki (False KSP Lessons):** https://github.com/KSP-RO/RealismOverhaul/wiki/False-KSP-Lessons
- **RP-1 Express Install Guide:** https://github.com/KSP-RO/RP-1/wiki/RO-&-RP-1-Express-Installation-for-1.12.5
- **Support Discord:** https://discord.gg/ZGbR6nv (alternate: https://discord.gg/V73jjNd)

---

## 2. System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| KSP Version | 1.12.x | 1.12.x |
| CPU | Core i5 (quad-core) | Core i7 / Ryzen 7 |
| RAM | 8 GB | 16–32 GB |
| GPU | Dedicated GPU, 1 GB VRAM, DX10 | Dedicated GPU, 4+ GB VRAM |
| Storage | SSD strongly recommended | SSD |
| OS | Windows 7+ / Linux / macOS (64-bit) | 64-bit required |

> **Integrated graphics are NOT recommended.** KSP 32-bit is not supported (3.5 GB RAM hard cap is unusable with RO). RSS texture resolution: 2048 (standard), 4096 (recommended), 8192 (ultra-high, may cause memory issues).

---

## 3. Core & Foundational Mods

### 3.1 RealismOverhaul (The Flagship)

- **Repository:** [KSP-RO/RealismOverhaul](https://github.com/KSP-RO/RealismOverhaul)
- **License:** CC-BY-SA — KSP Compatibility: 1.12.x only
- **CKAN:** Meta-package `RealismOverhaul`; Express install: RP-1 Express Install

RO is a comprehensive "multipatch to KSP" that transforms Kerbal Space Program into a realistic spaceflight simulator by modifying parts and gameplay to match real-world rocket science, engineering, and orbital mechanics. RO does NOT add new parts — it applies **Module Manager patches** to existing parts.

#### Key Changes from Stock KSP

- **Engines:** Real sizes, real thrust/Isp, limited ignitions, ullage simulation, real fuels
- **Pods/Capsules:** Full-size, real-weight equivalents
- **Propellant Tanks:** Correct dry mass ratios, multiple tank types with real-world vehicle examples
- **Solar Panels:** Lower power output, lighter mass, decay over time
- **Reaction Wheels:** Much weaker (realistic torque, no magic attitude control)
- **ElectricCharge:** Displayed in watts in tooltips and Solar Panel menu
- **Crew mass:** 100 kg each (person + suit)

#### Hard DEPENDS (14 mods — CKAN installs automatically)

| # | Mod | Purpose |
|---|-----|---------|
| 1 | ModuleManager | Config patching engine |
| 2 | TweakScaleRescaled-Redist | Part scaling support |
| 3 | AdvancedJetEngine (AJE) | Realistic jet engine thermodynamics |
| 4 | FAR (Ferram Aerospace Research) | Realistic aerodynamics |
| 5 | KerbalJointReinforcementContinued | Physics joint stabilization (prevents "Kraken") |
| 6 | RealChute + RealChuteForStock | Realistic parachute behavior |
| 7 | RealFuels | Real propellants, tank types, engine performance |
| 8 | ROUtils | Base library for all RO mods |
| 9 | RealHeat | Real heating model (conductive/convective/radiative) |
| 10 | RealPlume + SmokeScreen | Realistic exhaust plume effects |
| 11 | KSPCommunityFixes | Community bug-fix compilation |
| 12 | KerbalChangelog | In-game changelog display |

RealSolarSystem, SolverEngines, and ProceduralParts are **recommends** (not hard-required). `EngineGroupController` is unique to RO — bundled with the standard install.

#### Installation Locations

| Source | Target |
|--------|--------|
| `GameData/RealismOverhaul` | `KSP/GameData/` |
| `GameData/EngineGroupController` | `KSP/GameData/` |

### 3.2 RP-1 (Realistic Progression One)

- **Repository:** [KSP-RO/RP-1](https://github.com/KSP-RO/RP-1) — License: CC-BY-NC-SA-4.0
- Comprehensive career mode: historically-inspired progression from early sounding rockets through the Apollo era and beyond
- Fully icon-rebuilt tech tree designed for RO/RP-1
- Difficulty levels: Easy/Normal (recommended for beginners), Moderate/Hard
- **Required:** RealismOverhaul, RealSolarSystem, RealFuels, ProceduralParts, SolverEngines, Community Resource Pack, Module Manager

#### Community Gameplay Strategies

- **Early career:** Invest 100–150 points in VAB with level 4–5 pad **before** heavy science investment. ~7k funds/15 days on sounding rockets with 20 VAB points.
- **Orbital launches:** 20t pad orbital launches possible with RD-101 + 2×AJ10-27 + XASR stages but difficult. Recommended: 60t pad with proper orbital engines.
- **LR105 class engines** cannot be air-lit until tech tier 5 or 7.
- Mixed Soviet/US rocket combinations can yield useful early sustainer + booster combos.

### 3.3 RealSolarSystem (RSS)

- **Repository:** [KSP-RO/RealSolarSystem](https://github.com/KSP-RO/RealSolarSystem) — License: CC-BY-NC-SA
- Replaces the Kerbol system with a 1:1 scale Solar System — Earth at 6,371 km radius, Sun at 1 AU, all major bodies with correct sizes, masses, orbits, rotation periods, and axial tilts
- Custom launch sites via KSCSwitcher, biome maps for all planetary bodies, configurable texture resolutions (2048/4096/8192)
- **Dependencies:** Kopernicus, CustomBarnKit, KSCSwitcher, Module Manager
- Powered by Kopernicus. Not compatible with KSP 1.4.x.

### 3.4 RealFuels

- **Repository:** [KSP-RO/RealFuels](https://github.com/KSP-RO/RealFuels) — License: CC-BY-SA
- Overhauls KSP's fuel and engine system: real-world propellants, realistic tank masses, physically accurate engine performance. 1 unit = 1 liter at STP.
- **Dependencies:** Module Manager, Community Resource Pack, SolverEngines

#### Tank Types

| Tank Type | Description | Use Case |
|-----------|-------------|----------|
| Default | Regular construction, minimal insulation | Titan/Saturn stages |
| Structural | Heavier, reinforced | Aircraft, spaceplanes |
| ServiceModule | Pressure-fed capable (200 atm) | Service modules, RCS |
| Fuselage | Heavier ServiceModule variant | Aircraft, spaceplanes |
| Cryogenic | Highly insulated for cryo fuels | Cryogenic upper stages |
| Balloon | Thin-walled, very light, fragile | Atlas sustainer |
| BalloonCryo | Balloon + cryo insulation | Centaur upper stage |

#### Fuel Mixtures

**Chemical (Liquid):**

| Mixture | Characteristics | Best Use |
|---------|----------------|----------|
| RP-1 + LOx | Standard/benchmark. Non-toxic, dense, mildly cryogenic. | Lower stages |
| LH2 + LOx | High Isp, less dense (~75% TWR of RP-1). Cryogenic boiloff. | Upper stages (launch + few days) |
| LCH4 + LOx | Midway between RP-1 and LH2. Less cryogenic. | Mid-performance upper stages |
| Hypergolics (N2O4/MMH/AZ50/UDMH) | Storable, toxic, ~95% Isp of RP-1. No ignition needed. | Long-duration missions, RCS |
| RP-1 + HTP | Hypergolic when heated. Storable, non-toxic. | Alternative storable |
| Alcohol (75% Ethanol) + LOx | Worse than kerolox in all respects. | Historical (V-2) |
| Monopropellants (Hydrazine, N2O, HTP) | Hydrazine highest performance but very toxic. | RCS |
| Solid Fuel | High thrust, low Isp. Doesn't scale well. | Boosters |

**Nuclear (Liquid):**

| Fuel | Isp | Thrust | Notes |
|------|-----|--------|-------|
| Liquid Hydrogen | Highest | Lowest | Benchmark; worse density than hydrolox |
| Liquid Methane | Lower | Higher | Often better total delta-V than LH2 despite lower Isp |
| Liquid Ammonia | Lower | Moderate | Moderate density |
| LOx-Augmented NTR | 65–70% of LH2 mode | ~8× increase | Consumes LOx as reaction mass |

#### Tech Levels (Engine Progression)

| TL | Era | Key Events |
|----|-----|------------|
| 0 | 1945–1955 | WW2 / early rocketry |
| 1 | 1957+ | Early Space Age (Sputnik, Explorer) |
| 2 | 1962+ | Gemini, Saturn I era |
| 3 | 1967+ | Apollo era |
| 4 | 1968+ | Apollo Applications Program |
| 5 | 1978+ | Shuttle era |
| 6 | 1985+ | 80s/90s launch vehicles |
| 7 | 2005+ | Present-day launchers |

#### Engine Type Classifications

| Code | Type | Characteristics |
|------|------|-----------------|
| O | Orbital maneuvering | Pressure-fed, vacuum-optimized |
| U | Upper stage | Few restarts, vacuum-optimized |
| U+ | Advanced upper stage | Higher performance |
| L | Lower stage | Booster, no restarts |
| L+ | SSTO-optimized | Single stage to orbit |
| A | Aerospike | Altitude-compensating nozzle |
| S | Solid (standard) | Standard solid booster |
| S+ | Solid (vacuum) | Vacuum-optimized solid |
| N | Nuclear thermal | Nuclear thermal rocket |

#### Ullage & Limited Ignitions

- **Ignitions:** Limited number of starts (or unlimited with `ignitions = -1`)
- **Ullage Check:** Propellant must be "Very Stable" or engine risks flameout
- **Pressure-Fed:** Requires ServiceModule tank type (200 atm)
- **Ignition Resources:** Consumes resources (e.g., ElectricCharge) per start

#### LH2 Boil-Off Management

Liquid Hydrogen boil-off requires active management: use exterior radiator fin parts mounted on cryogenic tanks. Refrigeration draws significant ElectricCharge. For interplanetary trips, use room-temperature hypergolics instead — LH2 impractical beyond days/weeks. LH2 tanks must use **Cryogenic** or **BalloonCryo** tank type.

### 3.5 ProceduralParts

- **Repository:** [KSP-RO/ProceduralParts](https://github.com/KSP-RO/ProceduralParts) — License: CC-BY-SA 3.0
- Procedurally generate rocket parts in any size and shape. Spiritual successor to StretchyTanks/StretchySRBs.
- Tank types for RealFuels, Kethane, EPL, TAC Life Support; procedural heat shields; FAR drag models auto-update; SRB support with low/high-altitude switching; tech-limited sizing

#### Known Issues

| Issue | Resolution |
|-------|-----------|
| Stock decouplers missing with ProceduralParts installed | SmartTanks conflict — remove or disable SmartTanks |
| Limited part dimensions | Enable all part upgrades in sandbox via Difficulty Settings |

---

## 4. Engine & Propulsion

### 4.1 SolverEngines

- **Repository:** [KSP-RO/SolverEngines](https://github.com/KSP-RO/SolverEngines) — License: LGPL, 42 releases
- Developer-oriented plugin that splits KSP's engine system into two components: an engine module (KSP interaction) and an engine solver (physics calculations). **Not an end-user mod** — do not install unless another mod requires it.

**Core Architecture:**

| Component | Description |
|-----------|-------------|
| `ModuleEnginesSolver` | Engine module (derives from `ModuleEnginesFX`) |
| `EngineSolver` | Physics solver (calculates thrust, Isp, fuel flow) |
| Replacement intake module | Derives from `ModuleResourceIntake` |
| Flight/engine stats GUI | Real-time engine telemetry display |
| `ModuleAnimateEmissive` | Replaces `ModuleAnimateHeat` for emissive heat animation |

**Reference Implementations:** AJE (air-breathing jet engines), RealFuels (`ModuleEnginesRF` for liquid rocket engines).

### 4.2 AJE (Advanced Jet Engine)

- **Repository:** [KSP-RO/AJE](https://github.com/KSP-RO/AJE) — License: LGPL v2, 22 releases
- Replaces KSP's simplified jet engine model with a physics-based simulation incorporating real thermodynamic cycles (Brayton cycle). Incorporates code from **NASA EngineSim** and **JSBSim** (LGPLv2).
- Simulates: turbojets, turbofans, turboprops, ramjets, and scramjets
- **Dependencies:** Module Manager, SolverEngines (bundled)

**Thermodynamic Parameters Modeled:** Compressor pressure ratio and efficiency, turbine inlet temperature (TIT), bypass ratio (turbofans), inlet recovery factor, nozzle expansion ratio, mechanical losses.

#### SABRE / LAPCAT Engine Details

| Parameter | Value |
|-----------|-------|
| Sea level thrust | 600–800 kN per engine (per REL docs, NOT ~1960 kN Wikipedia figure) |
| TWR | ~0.6 per REL documentation |
| Air-breathing speed limit | ~Mach 5.5 (~1.7 km/s) before serious overheating |
| Precoolers | **Cosmetic/non-functional** — community workaround: add `thermalMassModifier = 5` and `skinMassPerArea = 2` |

**Known issue:** Some RP-1 users report problems with newer AJE versions; downgrading to v2.10.0 fixes it. SABRE's Isp in jet mode may be too low.

### 4.3 ROEngines

- **Repository:** [KSP-RO/ROEngines](https://github.com/KSP-RO/ROEngines) — 45 releases
- High-quality, accurate 3D engine models replacing generic Squad engines for RO.
- **50+ liquid engines** covering American, Soviet/Russian, European programs
- **19 solid engines** including boosters, upper stage solids, and separation motors
- **13 small maneuvering/landing engines** (RCS thrusters, apogee motors, landing engines)
- **Dependencies:** Module Manager, Realism Overhaul, B9PartSwitch, ROLibrary (bundled), Patch Manager (bundled)

#### Liquid Engines (Partial List)

| Engine | Source |
|--------|--------|
| A-4, Aerobee, Agena | RealEngines, Taerobee, BDB |
| AJ10 variants (137, 190, Early/Mid/Advanced) | BDB, SSTU |
| BE-3, BE-4 | NicheParts, Provenance Aerospace |
| C-1, E-1, F-1, F-1B, G-1 | BDB, Astral Manufactures |
| Gamma-2/8/301, HM7 | CRE, Astral Manufactures |
| H-1C/D, J-2, J-2S, J-2T, J-2X | BDB, SSTU |
| LE-5, LE-7 | Forgotten Real Engines |
| LMAE, LMDE | RealEngines |
| LR101, LR105, LR79, LR87/LR91/LR89 variants | BDB |
| M-1, MB-35, MB-60, Merlin Family | BDB, SSTU |
| NERVA, NERVA II, XE Prime | BDB |
| NK-33, NK-43, RD series (0105-301) | RealEngines/Soviet Rockets |
| RL10 variants, RS-68, RS-25 SSME | BDB/RMM |
| Rutherford, RZ.20, RZ Series | NicheParts, CRE/BDB |
| Viking-2/4/5, XLR10/11/25/41/43/99 | Forgotten Real Engines, Various |

#### Solid Engines (19)

Aerobee Aerojet, Agena Retro/Spin, AJ60, AJ-260 SL/FL, Alcyone-1, Algol 1/2, Altair I/II/III, Antares 1/2, Baby Sergeant, Castor 1/2/4/4AXL/30/30XL/120, GEM40/46/60/63, M55, Nike-M5E1, SR19, SR73, SRMU, R-103, Star-5D/8/13/17/37/48B, UA-1200 Series.

### 4.4 RealPlume & RealPlume-StockConfigs

- **RealPlume:** [KSP-RO/RealPlume](https://github.com/KSP-RO/RealPlume) — License: CC-BY-NC-SA, 30 releases
- Realistic expanding plumes built on SmokeScreen. Three-layer architecture: SmokeScreen (GPU-driven particles) → RealPlume (templates/textures/sounds) → RealPlume-StockConfigs (engine-specific assignments).

**Plume profiles by fuel type:**

| Fuel Type | Plume Appearance |
|-----------|-----------------|
| Kerolox | Yellow-orange, sooty |
| Hydrolox | Pale blue-violet, semi-transparent |
| Hypergolic | Yellow-white |
| Solid | Bright white with smoke |
| Nuclear | Violet-blue, faint |

Waterfall compatibility: RealPlume-Stock v4.0.8+ auto-detects and removes RealPlume configs when Waterfall plume present. **Conflicts:** HotRockets (incompatible).

### 4.5 Engine Technical Details

#### Ullage Simulation

Propellants require **ullage** — acceleration to settle liquids before engine ignition in microgravity.

- **Required for:** All pump-fed liquid engines in microgravity
- **Not required for:** Pressure-fed hypergolic engines (RCS thrusters)
- **Methods:** Hot-staging, ullage motors, RCS burns, spin-stabilization

#### Limited Ignitions

| Engine Type | Typical Ignitions | Examples |
|-------------|-------------------|----------|
| First-stage pump-fed | 1 | RD-107, F-1, LR-79 |
| Upper stage pump-fed | 1–3 | RL10 (multiple), J-2 (2), Kestrel |
| Pressure-fed hypergolic | Unlimited | R-4D, LEROS, SuperDraco |
| Solid motors | 1 (cannot restart) | Castors, GEMs, Alcyone |

#### Throttle Limits

Very few engines throttle significantly:

| Engine | Throttle Range | Notes |
|--------|---------------|-------|
| LMDE | ~10%–100% | Deepest throttling liquid engine |
| RS-25 (SSME) | ~70%–109% | Throttled to reduce G-loads |
| SuperDraco | ~20%–100% | Deep-throttling hypergolic |
| BE-3 | ~18%–100% | Deep-throttling hydrolox |
| Most pump-fed | Fixed (100%) | Cannot throttle at all |

Real rockets manage G-loads by shutting engines down early, not throttling.

#### Pressure-Fed vs Pump-Fed

| Type | Fuel Delivery | Tank Requirement | Examples |
|------|--------------|------------------|----------|
| Pressure-fed | High-pressure gas forces fuel in | ServiceModule or Fuselage tank | TRW LM Descent Engine |
| Pump-fed | Turbopump delivers fuel | Standard tank types | RL-10, RD-180, RS-68 |

**RL-10 note:** Despite being used on Centaur, RL-10 is **NOT** pressure-fed — it uses a turbopump.

#### Engine Ignition Requirements

**H-1 engine example:** Requires TEATEB (pyrotechnic igniter fluid), ElectricCharge, Kerosene, and LqdOxygen. Limited ignitions.

**General rule:** Hover over an engine in the VAB — the tooltip (provided by RealFuels) shows ignition requirements (blue text), remaining ignitions, and whether pressure-fed.

#### Verniers & Ullage Motors

- **Verniers:** Small engines with significant gimbal for attitude control during/after main engine burn. Example: Atlas sustainer (350 kN main + 2× 5 kN verniers).
- **Ullage motors:** Liquid or solid motors firing briefly before main ignition to settle propellants.
- **Engine clustering:** For 2–6 MN thrust range, use Procedural Fairings thrust plate with multiple nodes. Available engines: RD-191 (2 MN), RD-180 (4 MN), RS-68 (3.4 MN).

---

## 5. Parts, Tanks & Capsules

### 5.1 ROTanks

- **Repository:** [KSP-RO/ROTanks](https://github.com/KSP-RO/ROTanks) — 19 releases, CC BY-NC-SA
- Modular, procedural fuel tanks and structural components based on modified code from SSTU. Fully resizable in diameter and length, with texture switching and multiple tank type options.
- **Dependencies:** Textures Unlimited, ROLibrary

#### Tank Type Mapping

| Tank Type | Description | Typical Uses |
|-----------|-------------|--------------|
| Cryogenic | Active boil-off simulation for supercooled propellants | LH2, LOX, methalox |
| Service Module | Pressure-fed tank configuration with helium pressurization | Hypergolic propellants (MMH/NTO) |
| Balloon | Thin-wall tension-bearing tank (requires pressurization) | Atlas stage, Centaur |
| Structural | Non-fuel-bearing structural components | Interstages, adapters |
| Spherical | CryoTanks-style inline spherical tanks | Compact designs |

#### Tech Levels by Era

| Era | Available Tanks | Materials |
|-----|-----------------|-----------|
| 1950s | Steel balloon, basic SM tanks | Steel, aluminum |
| 1960s | Aluminum cryo, common SM, balloon | Aluminum alloys, stainless steel |
| 1970s | Aluminum-lithium cryo | Al-Li alloys, titanium |
| 1980s+ | Composite cryo, advanced SM | Carbon composites, Al-Li |

#### Historical Tank Geometries

- **Atlas:** Balloon tank (stainless steel, pressure-stabilized)
- **Centaur:** Balloon tank (stainless steel, cryogenic)
- **Titan II:** Stiffened skin tanks (aluminum)
- **Saturn S-IC, S-II, S-IVB:** Common bulkhead tanks (aluminum)
- **Saturn S-IVB:** Single common bulkhead separating LH2 and LOX

#### Balloon Tanks (Appendix)

Balloon (pressure-stabilized) tanks are thin-walled and rely on internal pressure for structural integrity. In RO, balloon tanks are lighter but more fragile than conventional stiffened-skin tanks. RealFuels' "pressurized" tank type is for pressure-fed engines only, not for balloon tanks.

#### ServiceModule Tank (Appendix)

Required for pressure-fed engines (TRW LM Descent Engine, R-4D, SuperDraco). Uses helium pressurization to force propellants into the engine at ~200 atm. Cannot be used with pump-fed engines — the high tank pressure would overwhelm the turbopump inlet.

#### Source Assets (Credits)

| Source Modder | Assets Contributed |
|---|---|
| Shadowmage45 (SSTU) | Core modular tank system, inline spherical tanks |
| Frizzank (FASA) | Atlas tank, Atlas skirt, Atlas mount, Titan 2 interstage |
| CobaltWolf (BDB) | LDC Titan interstage decoupler, Titan 2 decoupler, mounts, Centaur-D/V, Saturn SII |
| Beale (Tantares) | Interstage trusses |
| Katniss | MS-II, MS-V, STS, S-IVB interstages |
| Nertea (CryoTanks) | Inline spherical tanks |

### 5.2 ROCapsules

- **Repository:** [KSP-RO/ROCapsules](https://github.com/KSP-RO/ROCapsules) — 38 releases
- Curated collection of historically accurate crew capsule models covering 12 spacecraft families.
- **Dependencies:** Module Manager, Realism Overhaul, B9PartSwitch, Textures Unlimited, ROLibrary

#### Apollo (DECQ)

Command Module with detailed interior; Drogue Docking Port; Drogue Parachute Pack; CM Forward Heat Shield; High-Gain Antenna (S-band); Launch Escape System tower; LES Boattail Plug; Main Parachute Pack (three cluster); Service Module RCS Block (quad); Service Module with SPS engine.

#### Apollo Block III+ (DECQ) / Block III/V (BDB by CobaltWolf)

Block III+ CM with heat shield. Block III Service Engine (LMAE), Block V Service Engine (TR-201), Block III+/IV Mission Modules, Block V Solar Array, Apollo Docking Mechanism Probe.

#### GE Apollo D2 (AlternateApollo by Mcdouble)

Heatshield Adapter, AJ10-133-LH engine, HGA, Abort Motors, Descent Module, Docking Probe, 3 Interstages, 2 Mission Modules, Service Module, 2 Skirt Sections, Solar Array.

#### Dynasoar / X-20 Moroz

Cockpit, Window Cover, Wing, Elevon, Rudder, Front/Rear Skids, Cargo Bay, Cabin, Crew Tube, Equipment Compartment, Aft Bay, Telemetry Antenna, Docking Arm, Docking Rod. **Requires KSPWheel** for landing gear.

#### Gemini (BDB by CobaltWolf; FASA by Frizzank)

Agena Target Vehicle Docking Port, UHF Antenna, Cabin, Adapter Equipment Section, Nose Fairings, Main/Drogue Parachutes, OAMS Thruster Pack (Orbit Attitude and Maneuvering System), Re-entry Control System, Adapter Retrograde Section, Gemini B variants. **Requires ModuleDepthMask.**

#### Big Gemini (BDB by CobaltWolf)

Cabin, Decoupler, Docking Adapter, Heatshield, LES, MOL Docking Port, Service Module.

#### LEM / Apollo Lunar Module (DECQ)

LM Ascent Stage, Descent Stage, Decoupler, Descent Engine (throttleable), Lunar Roving Vehicle (foldable).

#### Mercury (FASA by Frizzank)

Mercury-Atlas Adapter, Mercury-Redstone Adapter, Command Pod, LES, Nose Fairing & Antenna, Parachute Pack, Posigrade SRM, RCS Roll Thrusters (H2O2), Retro Solid Rocket Pack (3), Retro Strap & Decoupler.

#### Orion / SLS (DECQ)

Crew Module, European Service Module (ESM), ESM Fairing, Decoupler, Forward Heatshield & NASA Docking System, Heatshield (ablative), Launch Abort System, Main Parachute, RCS, ESM Solar Panels (4 array wings).

#### CST-100 Starliner

Crew Module, Heat Shield, Parachute Pack, Nose Cone, Launch Abort Engines, Service Module.

#### Vostok / Voskhod (Soviet Spacecraft by RaiderNick)

Voskhod Airlock, Voskhod Descent Module, Voskhod Retro Package + Decoupler, Vostok Descent Module, Vostok/Voskhod Decoupler/Parachute/Service Module (common).

#### Docking Ports

| Part | Description | Source |
|------|-------------|--------|
| APAS 89/95 Active | Androgynous Peripheral Attach System (active) | BDB |
| APAS 89/95 Passive | APAS docking ring (passive) | BDB |
| NASA Docking System Active | International docking standard (active) | CST-100 |
| NASA Docking System Passive | International docking standard (passive) | CST-100 |

### 5.3 ROHeatshields

- **Repository:** [KSP-RO/ROHeatshields](https://github.com/KSP-RO/ROHeatshields) — 11 releases, MIT (code) / CC-BY-NC-SA 4.0 (assets)
- Single, versatile heatshield part — dynamically resizable in diameter, configurable geometries (conical, flat, ogive), configurable thermal protection levels.
- **Dependencies:** Textures Unlimited, ROLibrary

**Heat Shield Type Selection:**

| Protection Level | Suitable For |
|-----------------|--------------|
| Heat sink | LEO only |
| Low ablative | LEO to lunar |
| High ablative (BLEO) | Beyond low Earth orbit, interplanetary return |

### 5.4 ProceduralFairings

- **Repository:** [KSP-RO/ProceduralFairings](https://github.com/KSP-RO/ProceduralFairings) — 19 releases, CC-BY 4.0
- Auto-reshaping payload fairings that automatically reshape to enclose any attached payload. Parts under the **Payload** tab in VAB/SPH.
- **Dependencies:** Module Manager, KSPCommunityFixes, ROUtils

#### Auto-Reshaping Mechanics

1. **Mesh detection:** Scans all parts attached above the fairing base
2. **Collision volume calculation:** Computes minimum fairing volume needed
3. **Dynamic reshaping:** Fairing walls automatically conform to payload shape
4. **Real-time updates:** Shape updates as you add/move parts

#### Fairing Shape Customization Parameters

| Parameter | Range | Description |
|-----------|-------|-------------|
| `baseConeShape1-4` | 0–1 | Controls base section profile curvature |
| `baseConeSegments` | 2–50 | Vertical segments in base |
| `noseConeShape1-4` | 0–1 | Controls nose section profile curvature |
| `noseConeSegments` | 2–50 | Vertical segments in nose |
| `noseHeightRatio` | 0.5–5 | Nose cone height as fraction of base diameter |

#### Historic Fairing Shape Reference

| Shape | baseConeShape1-4 | baseConeSegments | noseConeShape1-4 | noseConeSegments | noseHeightRatio |
|-------|------------------|------------------|------------------|------------------|-----------------|
| Conic | 0.3, 0.3, 0.7, 0.7 | 7 | 0.1, 0, 0.7, 0.7 | 11 | 2 |
| Ogive (Egg) | 0.3, 0.2, 1, 0.5 | 7 | 0.5, 0, 1, 0.7 | 11 | 2 |
| Cone-Egg | 0.3, 0.3, 0.7, 0.7 | 3 | 0.5, 0, 1, 0.7 | 11 | 2 |
| Atlas V | 0.1, 1, 0, 0.7 | 3 | 0.1, 0, 0.7, 0.7 | 11 | 3.5 |
| Delta | 0, 0, 0, 0 | 3 | 0.3, 0, 1, 0.8 | 11 | 2 |
| Jupiter/Titan | 0.3, 0.3, 0.7, 0.7 | 3 | 0, 0, 0.7, 0.2 | 3 | 2 |
| Long March | 0.7, 0.7, 0.3, 0.3 | 5 | 0.2, 0, 0.7, 0.2 | 50 | 2.8 |
| Proton | 0, 0, 0, 0 | 4 | 1.25, 0.2, 0.1, 0.8 | 3 | 3 |
| Soyuz | 0.3, 0.3, 0.9, 1 | 2 | 0.54, 0, 0.52, 0.035 | 10 | 2.2 |

#### Career Tech Level Limitations

Configurable in `PF_Settings.cfg`:

| Tech Level | Min Diameter | Max Diameter | Max Height |
|------------|-------------|-------------|------------|
| Early | 0.625m | 2.5m | 5m |
| Mid | 0.625m | 5m | 10m |
| Advanced | 0.625m | 10m+ | 20m+ |

#### Inline Fairings & Interstage Bases

Place the lower fairing base under the section, then an inverted fairing base above it. Low-profile base rings available for tight interstage configurations. Interstage bases act as both decoupler and fairing mount point.

### 5.5 ProceduralFairings-ForEverything (PFFE)

- **Repository:** [KSP-RO/ProceduralFairings-ForEverything](https://github.com/KSP-RO/ProceduralFairings-ForEverything) — 8 releases, CC-BY-NC-SA 4.0
- Patches mods with their own fairing systems to instead use Procedural Fairings, removing VAB clutter.

### 5.6 ProceduralSolidsLibrary

- Configuration data library for procedural solid rocket motor components — casing materials, grain geometries (cylindrical/star/finocyl burn profiles), nozzles, propellant formulations (PBAN, HTPB, NEPE-75, Double-base). Typically bundled with RP-1.

### 5.7 StagedAnimation

- Lightweight KSP plugin that plays an animation on a part when staged, without decoupling. Drop-in replacement for ModuleAnimatedDecoupler. Also provides `ModuleAnimateGenericExtra` with deploy limit labels and toggle visibility.

---

## 6. Life Support & Reliability

### ⚠️ CRITICAL: DeadlyReentry Compatibility

**DeadlyReentry is INCOMPATIBLE with Realism Overhaul v18.x and newer.** The RO `.netkan` explicitly lists `DeadlyReentry` in its `conflicts:` section — CKAN will refuse to install both. RO v18.x provides its own integrated heating and thermal damage system via **RealHeat** (a hard dependency), making DeadlyReentry unnecessary and conflicting.

- For reentry heating: **RealHeat + RO's built-in thermal configs**
- For heatshields: **ROHeatshields**
- The DRE documentation below is provided for **historical reference only**

### ⚠️ Life Support: Kerbalism vs TACLS (Mutually Exclusive)

The RO `.netkan` recommends **one of** `Kerbalism-Config-RO` or `TACLS` for life support. They are mutually exclusive — install only one.

- **ROKerbalism:** Comprehensive life support + radiation + science + stress simulation. Recommended for deep simulation.
- **TACLS (TAC Life Support):** Simpler resource-based life support (food, water, oxygen). Recommended for basic LS.

### 6.1 TacLifeSupport

- **Repository:** [KSP-RO/TacLifeSupport](https://github.com/KSP-RO/TacLifeSupport) — 32 releases, CC BY-NC-SA 3.0
- **Dependency:** REPOSoftTechKSPUtils (background resource processing)

**Resource Requirements:**

| Resource | Purpose | Survival Time Without |
|---|---|---|
| Food | Basic nutrition | 30 days |
| Water | Hydration | 3 days |
| Oxygen | Breathing | 2 hours |
| Electricity | Air quality & climate control | 2 hours |

**Key Mechanics:** Crewed pods come pre-stocked with 3 days of resources. EVA suits carry a half-day supply. Background simulation — Kerbals consume resources even when the vessel is not active. Recyclers extend mission duration by recovering waste products. HexCan containers for Food, Water, Oxygen.

### 6.2 TestFlight

- **Repository:** [KSP-RO/TestFlight](https://github.com/KSP-RO/TestFlight) — 123 releases, CC BY-NC-SA 4.0
- Configurable, extensible part research and reliability system. Engineers generate **flight data** from actual launches — this data persists across builds and improves reliability for the same part types.

**Core Concept:** Launch "Super Rocket 1" with a Mainsail → generates 10,000 units of flight data → Build "Super Rocket 2" with same Mainsail → it starts with 10,000 flight data units already banked. More flights = more data = more reliable parts.

**FlightData System:** Persistent across builds, part-specific tracking (different engine models do NOT share data), Engineer Kerbals generate more flight data per flight.

**Reliability Curve:** Logarithmic/asymptotic model — rapid improvement from early data, diminishing returns as a part matures.

**Failure Types (8 total):**

| Failure Type | Description |
|---|---|
| Engine Failure | Complete engine loss |
| Engine Performance Loss | Reduced thrust output |
| Explosion | Catastrophic part failure |
| Ignition Failure | Engine fails to ignite |
| Gimbal Lock | Loss of thrust vectoring |
| Reduced Max Thrust | Throttle-limited operation |
| Resource Leak | Propellant or resource loss |
| Engine Shutdown | Premature engine cutoff |

**Ignition Reliability (RealFuels Integration):** TestFlight integrates with `ModuleEngineRF`. Engines with limited ignitions can fail to start if reliability is low.

### 6.3 ROKerbalism

- **Repository:** [KSP-RO/ROKerbalism](https://github.com/KSP-RO/ROKerbalism) — 38 releases
- Kerbalism configuration pack for Realism Overhaul and RP-1 with historically realistic spacecraft capabilities and life support systems.

**Key Differences from Stock Kerbalism:**

| Feature | ROKerbalism Behavior |
|---|---|
| Reliability | Disabled — conflicts with TestFlight |
| Spacecraft Customization | Not available unless using advanced near-future parts |
| Life Support Systems | Each capsule has its real historical LS configuration |
| Recycling | Not available until ISS era (~2000s) |
| Science Over Time | All experiments take time (Kerbalism 3.0+) |
| RP-1 Integration | LS subsystems unlockable via tech tree; old capsules upgradeable |

**Communications:** Stock CommNet is insufficient. **Recommended:** RealAntennas. **RemoteTech is incompatible** with ROKerbalism.

**Supported Mods:** ROCapsules, Ven's Stock Revamp, FASA (ROCapsules preferred), SXT, CxAerospace, RN Soyuz, BDB (partial), Tokamak Industries.

### 6.4 Kerbalism (Upstream Reference — NOT Compatible)

> ⚠️ **NOT compatible with current RO.** For RO installations, use **ROKerbalism** (above).

**Original Repo:** [Kerbalism/Kerbalism](https://github.com/Kerbalism/Kerbalism) — License: Unlicense (Public Domain). KSP 1.5–1.10 only.

**Core Features (Upstream):** Life Support (food, water, oxygen with death on depletion). Stress System (inadequate living space/pressure/comfort → mistakes). Radiation (cosmic background + solar flares + planetary belts; EVA dangerous during flares). Reliability & Failures (limited operational lifetimes). ISRU (semi-realistic extraction/conversion). Science Over Time (experiments produce data over years). Background processing (all vessels simulated continuously).

**Three Radiation Sources:**

| Source | Countermeasure |
|--------|---------------|
| Cosmic Background Radiation | Minimal shielding; long missions need dedicated shielding |
| Solar Radiation / Flares | Active monitoring; shielded storm shelters; flare warnings |
| Planetary Radiation Belts | Mission planning to avoid or quickly transit through belts |

### 6.5 DeadlyReentry — ⚠️ INCOMPATIBLE (Historical Reference)

**Repository:** [KSP-RO/DeadlyReentry](https://github.com/KSP-RO/DeadlyReentry) — License: CC BY-SA 4.0

**Atmospheric Reentry Heating (Historical):** Calculates atmospheric density from temperature and pressure; convective, conductive, and radiative heating; part-by-part thermal simulation; parts visually burn up when exceeding tolerance. As of v7.9.0, DRE no longer implements its own reentry heating — since KSP 1.0, DRE tweaks stock parameters to make reentry deadlier.

**G-Force Damage Formula:** `gToleranceMult * sqrt(6 * crashTolerance)` — default `gToleranceMult` = 2.5. Parts typically survive up to 12–15 Gs. Kerbals can survive ~9G from LEO reentry (steep).

**Heat Shield Types (Historical Reference):**

| Type | Example | Behavior |
|------|---------|----------|
| Ablative | Apollo, Dragon, Orion | Burns away material; good for high peak heating |
| TPS Tiles | Space Shuttle | Absorbs and re-radiates heat; good for sustained low heat flux |
| PICA | Stardust, Dragon | Advanced ablative; very high heat tolerance |

**Multiplier Tuning (Historical):** Default heat multiplier in RO: ~25x (accounts for 8 km/s orbital velocities at Earth scale). Adjustable via **Alt+D+R**. LEO-rated shields will NOT survive translunar or interplanetary reentry velocities.

---

## 7. Visuals & Environment

### 7.1 RSS-Textures

- **Repository:** [KSP-RO/RSS-Textures](https://github.com/KSP-RO/RSS-Textures) — 16 releases
- High-resolution planetary texture maps for RSS. Contains image data — elevation maps, color maps, surface detail textures.

**Resolution Tiers:**

| Resolution | VRAM Usage | Recommended Use |
|------------|-----------|-----------------|
| 2048 | ~500 MB | Low-memory systems, integrated GPUs |
| 4096 | ~1–2 GB | Most systems — recommended default |
| 8192 | ~4–6 GB | High-end systems with 6+ GB VRAM |
| 16384 | ~8–16 GB | Extreme detail; requires powerful GPU |

**Known Issues:** Memory crashes on 16384 with <8 GB VRAM. Missing texture artifacts if not merged correctly. Texture stitching at lower resolutions (2048, 4096) — visible seams between adjacent texture tiles on Earth. Delete old texture folders completely before upgrading.

### 7.2 RSSVE (Real Solar System Visual Enhancements)

- **Repository:** [KSP-RO/RSSVE](https://github.com/KSP-RO/RSSVE) — 13 releases, CC-BY-NC-SA 4.0
- Visual enhancement pack for RSS — cloud layers, atmospheric effects. Works with EVE and Scatterer.
- **Dependencies:** RSS, EVE, Scatterer

**EVE Configuration:** Cloud layers (stratus, cumulus, cirrus), city lights layer for nighttime, aurora effects at high latitudes. **Scatterer Integration:** Atmospheric scattering, ocean scattering, shadow effects.

> **Critical:** Install the specific versions of EVE, Scatterer, and RSSVE listed on the RSSVE release page. Using newer versions can cause visual glitches or crashes.

**Known Issues:** Cloud flickering at high time warp (>10x). Atmospheric scattering mismatch if Scatterer version doesn't match. Performance impact: RSSVE + Scatterer can reduce frame rates 20–50% on mid-range systems.

### 7.3 RSS-CanaveralHD

- **Repository:** [KSP-RO/RSS-CanaveralHD](https://github.com/KSP-RO/RSS-CanaveralHD) — 2 releases, CC-BY-NC-SA 4.0
- High-definition terrain elevation and color maps for Cape Canaveral region (KSC, CCAFS, surroundings). Heightmap derived from U.S. Geological Survey data (Public Domain). Requires RSS.

### 7.4 RealHeat

- **Repository:** [KSP-RO/RealHeat](https://github.com/KSP-RO/RealHeat) — 16 releases
- Real heating (thermal energy content) model for KSP. Tracks **heat energy in Joules** rather than just temperature. A hard dependency of RO v18.x.
- **Dependencies:** ModuleManager. Realism Overhaul recommended.

**Heat vs Temperature:**

| Concept | Stock KSP | RealHeat |
|---------|-----------|----------|
| Primary quantity | Temperature (K) | Heat energy (J) |
| Heat transfer | Simplified linear | Realistic conduction, convection, radiation |
| Part interaction | Parts treated independently | Heat flux between connected parts |
| Thermal mass | Abstract | Real — larger parts hold more heat |
| Transient behavior | Instant equilibrium | Real thermal time constants |

**Three Modes of Heat Transfer:**

1. **Conduction** — Heat transfer between connected parts; depends on material thermal conductivity
2. **Convection** — Heat transfer to/from atmosphere; dominant during atmospheric flight and reentry
3. **Radiation** — Infrared emission and solar absorption; Stefan-Boltzmann law; planetary infrared (albedo effect)

**Operational Impact:** Engine heating requires thermal management for sustained burns. Solar panels heat up and degrade if overheated. Requires properly sized radiators. More realistic cryogenic boiloff.

### 7.5 RealISRU

- **Repository:** [KSP-RO/RealISRU](https://github.com/KSP-RO/RealISRU)
- Realistic In-Situ Resource Utilization. Overhauls stock ISRU with real-world chemical processes.
- **Dependencies:** Realism Overhaul, ModuleManager, Community Resource Pack

**Five Resource Chains:**

| Input Resource | Process | Output Resource | Real-World Analogue |
|----------------|---------|-----------------|---------------------|
| Water (H₂O) | Electrolysis | Oxygen + Hydrogen (LH₂) | Splitting water into breathable O₂ and propellant |
| Mars Atmosphere (CO₂) | Sabatier / Zirconia | Oxygen + Methane | MOXIE / ISRU for Mars return |
| Regolith (generic) | Carbothermal reduction | Oxygen + Metals | Lunar/Martian oxygen extraction |
| Water Ice | Purification & electrolysis | Propellant + Life Support | Lunar polar ice processing |
| Hydrated Minerals | Thermal extraction | Water | In-situ water from minerals |

**Conversion Equipment:** Electrolyzer, Sabatier Reactor, Carbothermal Reactor, Water Purifier.

**Extraction Sources:** Planetary surfaces (scoop/mining from regolith), atmospheres (filtering atmospheric gases — Martian CO₂, Venusian atmosphere), ice deposits (identified via biome maps in RSS).

### 7.6 ROSolar

- **Repository:** [KSP-RO/ROSolar](https://github.com/KSP-RO/ROSolar) — 11 releases
- Centralized collection of solar panel models from various mods, properly rescaled and configured for RO. Part of the "ROx" family.

### 7.7 RSSTimeFormatter

- **Repository:** [KSP-RO/RSSTimeFormatter](https://github.com/KSP-RO/RSSTimeFormatter) — 16 releases
- Replaces KSP's stock time system with a real-world Gregorian/proleptic calendar. Displays actual dates (e.g., "1957 October 4"). Installed as `RSSDateTime` in GameData. Essential for historical mission replication in RP-1.
- **Dependencies:** Real Solar System, ModuleManager

---

## 8. Historical Spacecraft & Launch Vehicles

### 8.1 SovietRockets

- **Repository:** [KSP-RO/SovietRockets](https://github.com/KSP-RO/SovietRockets) — 39 releases, CC BY-NC-ND 3.0
- Soyuz, Proton, Zenit-2, Fregat-1 upper stage. Old School Fairings support, pre-built craft files. Recommended companion mods: Salyut Stations, Soviet Probes.

#### Soyuz Variants

| Variant | Stage Config | Payload to LEO | Notable Features |
|---------|-------------|----------------|------------------|
| Soyuz (R-7 Semyorka) | 4-stage (R-7 core + 4 boosters) | ~6.5 tonnes | Original ICBM-derived launcher |
| Soyuz-U | 3-stage (R-7 + Blok I upper) | ~7.0 tonnes | Most-flown variant (780+ launches) |
| Soyuz-U2 | 3-stage (synthetic kerosene) | ~7.2 tonnes | Used Syntin fuel; higher performance |
| Soyuz-FG | 3-stage (upgraded engines) | ~7.5 tonnes | Introduced digital flight control |
| Soyuz-2.1a | 3-stage (digital) | ~7.0 tonnes | Modernized; no longer human-rated |
| Soyuz-2.1b | 3-stage (RD-0124 upper) | ~8.2 tonnes | Upgraded upper stage |
| Soyuz-ST | 3-stage (tropicalized) | ~9.0 tonnes | Launched from Kourou, French Guiana |

#### Proton Variants

| Variant | Stage Config | Payload to LEO/GTO | Notable Features |
|---------|-------------|--------------------|------------------|
| Proton (UR-500) | 2-stage | ~12 tonnes LEO | Original heavy ICBM |
| Proton-K | 3-stage | ~20 t LEO / ~5 t GTO | Workhorse for space station modules |
| Proton-K/D | 3-stage + D upper | ~5.7 t GTO | Block D upper stage for lunar/GTO |
| Proton-K/DM | 3-stage + DM upper | ~4.5 t GTO | DM upper stage (restartable) |
| Proton-M | 3-stage (+ Breeze-M) | ~22 t LEO / ~6.5 t GTO | Modernized with Breeze-M upper stage |

#### Zenit Details

- **Zenit-2** — 2-stage, ~13.7 tonnes to LEO, RD-171 engine (4-chamber, 4-nozzle)
- **Zenit-3SL** (Sea Launch) — 3-stage with Block DM-SL, ~6 tonnes to GTO
- **Zenit-3F** (Zenit-2SB/Fregat) — Fregat-SB upper stage for higher energy orbits

#### Fregat Upper Stage

- Hypergolic upper stage for Soyuz and Zenit
- Multi-restart capability (up to 20+ ignitions)
- Dry mass: ~930 kg, Propellant: N₂O₄/UDMH, Isp: ~333s

Credits: Thorton & Igel (Soyuz, Fregat1, some Proton parts), DECQ (R7 textures), buran.ru (Zenit meshes).

### 8.2 SovietSpacecraft

- **Repository:** [KSP-RO/SovietSpacecraft](https://github.com/KSP-RO/SovietSpacecraft) — 26 releases, CC BY-NC-ND 3.0
- Soyuz 7K-T, 7K-OKA, 7K-OKP, 7K-T-AF, 7K-LOK (lunar orbiter), 7K-LK (lunar lander), Progress 7K-TG, Vostok, Voskhod, TKS/VA.
- **Requires:** SovietRockets. Strongly recommended: Salyut Stations.

**Vostok:** 1 cosmonaut, ~4.7 t, ~4.4 m / ~2.4 m, up to 5 days. Reentry: ejection seat (cosmonaut ejected at ~7 km). Missions: Vostok 1–6 (1961–1963).

**Voskhod:** 2–3 cosmonauts, ~5.7 t, ~5.0 m / ~2.4 m, up to 22 days. First 3-crew mission (Voskhod 1); First spacewalk — Leonov (Voskhod 2).

**Soyuz Variants:**

| Variant | Mass | Length | Crew | Purpose |
|---------|------|--------|------|---------|
| 7K-OK | ~6.6 t | ~7.5 m | 2–3 | Orbital ops; first-generation |
| 7K-T | ~6.8 t | ~7.5 m | 2 | Second-gen; batteries instead of solar panels |
| 7K-T-AF | ~6.8 t | ~7.5 m | 2 | Astrophysical observatory (Orion telescope) |
| 7K-LOK | ~9.9 t | ~10 m | 2 | Lunar orbiter (Soviet lunar program) |
| 7K-LK | ~5.5 t | ~5 m | 1 | Lunar lander (never flew crewed) |
| Progress 7K-TG | ~7.2 t | ~7.9 m | 0 | Uncrewed cargo |

**TKS/VA:** Crew 2–3, ~21.6 t fully loaded, VA capsule ~4.8 t, ~17 m / ~4.1 m (FGB module). Purpose: Almaz military stations; FGB modules later used for Mir core and ISS Zarya.

### 8.3 SovietProbes

- **Repository:** [KSP-RO/SovietProbes](https://github.com/KSP-RO/SovietProbes) — 24 releases, CC BY-NC-ND 3.0
- Sputnik 1/2/3 (complete), Luna 2 (impactor), Luna 3 (far-side photographer), Luna 9 (soft lander), Luna 10 (orbiter). Molniya, Polyot — planned. Requires SovietRockets.

### 8.4 USProbesPack

- **Repository:** [KSP-RO/USProbesPack](https://github.com/KSP-RO/USProbesPack) — 40 releases, CC BY-NC-ND 3.0
- Pioneer 6/7/8/9, Pioneer 10/11, Voyager, Galileo, Magellan, New Horizons, TDRS, EOS, NEO satellites, Vanguard-1.

**Pioneer Probes:**

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| Pioneer 6/7/8/9 | Solar orbit | ~63–147 kg | Interplanetary space weather network |
| Pioneer 10 | Jupiter flyby | ~258 kg | First across asteroid belt; Pioneer plaque |
| Pioneer 11 | Jupiter/Saturn | ~259 kg | First Saturn flyby; Jupiter gravity assist |

**Mariner Probes:**

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| Mariner 2 | Venus flyby | ~203 kg | First successful Venus flyby (1962) |
| Mariner 4 | Mars flyby | ~261 kg | First Mars flyby; first close-up images (1965) |
| Mariner 5 | Venus flyby | ~245 kg | Venus atmosphere measurements |
| Mariner 6/7 | Mars flyby | ~413 kg each | Mars flyby photography |
| Mariner 9 | Mars orbiter | ~558 kg | First spacecraft to orbit another planet (1971) |
| Mariner 10 | Venus/Mercury | ~474 kg | First Mercury flyby; first gravity assist |

**Ranger Probes:** Ranger 7/8/9 — Lunar impactors (~367 kg each). Block III design: 6 TV cameras, transmitted until impact. No landing capability.

**Surveyor Probes:** Surveyor 1/3/5/7 — Lunar landers (~292–306 kg). Soft landing with retro-rockets and vernier engines. Three landing legs, stable on slopes up to ~20°.

### 8.5 Antares-Cygnus

- **Repository:** [KSP-RO/Antares-Cygnus](https://github.com/KSP-RO/Antares-Cygnus) — 13 releases, CC BY-NC-ND 3.0
- Authors: Kartoffelkuchen & Raidernick. Legacy RemoteTech support, KIS support, pre-built craft files.

**Antares Variants:**

| Variant | Stage 1 | Stage 2 | Payload to LEO | Notes |
|---------|---------|---------|----------------|-------|
| Antares 110 | Castor 30A (solid) | ATK solid | ~5.0 t | Original |
| Antares 120 | 2x RD-181 (kerolox) | Castor 30XL | ~6.1 t | Upgraded first stage |
| Antares 130 | 2x RD-181 | Briz-M (hypergolic) | ~6.8 t | Restartable upper |
| Antares 230 | 2x RD-181 | Castor 30XL | ~6.1 t | Current operational |
| Antares 230+ | 2x RD-181 | Castor 30XL (enhanced) | ~6.5 t | Latest (2025+) |

**First stage (Antares 230):** 2x RD-181, RP-1/LOX, ~3,840 kN total thrust. **Second stage (Castor 30XL):** Solid motor, ~140 s burn, ~497 kN thrust.

**Cygnus Variants:**

| Variant | Pressurized Volume | Total Mass | Propellant | Features |
|---------|-------------------|------------|------------|----------|
| Cygnus Standard | ~18 m³ | ~5.6 t | NTO/MMH | Original ISS resupply |
| Cygnus Enhanced | ~27 m³ | ~8.0 t | NTO/MMH | Extended cargo capacity |
| Cygnus PCM | ~27 m³ | ~8.0 t | NTO/MMH | Enhanced power generation |

**Service module:** Ultraflex solar arrays, hypergolic bipropellant RCS, CBM berthing (not direct docking), incendiary reentry at end of mission. **CRS-2 (PCM):** Up to 3,750 kg pressurized, 1,250 kg unpressurized cargo. 3.5 kW average power.

### 8.6 SalyutStations

- **Repository:** [KSP-RO/SalyutStations](https://github.com/KSP-RO/SalyutStations) — 30 releases, CC BY-NC-ND 3.0
- Salyut 1, 4, 6, 7 (DOS series) and Almaz 2, 3, 5 (OPS military series). Fully functional replicas with historical color schemes. Requires SovietRockets. Recommended: SovietSpacecraft.

### 8.7 Skylab

- **Repository:** [KSP-RO/Skylab](https://github.com/KSP-RO/Skylab) — 15 releases, CC BY-NC-ND 3.0
- Broken and unbroken versions. Components: Main OWS module (converted S-IVB stage), left solar array (damaged config), micrometeorite shield (EVA repair), nosecone with ATM and docking adapter. Saturn V auto-patching priority: DECQ (highest) > OLDD (middle) > FASA (lowest). Launch guidance: Steep ascent profile, level off only at end (~200 m/s DV remaining). Must use Skylab ASAS for control.

### 8.8 FASA-RO

- **Repository:** [KSP-RO/FASA-RO](https://github.com/KSP-RO/FASA-RO) — 11 releases, CC-BY-SA
- RO-compatible edition of FASA (Frizzank's American Space Agency). Provides only RO patches — **requires original FASA mod parts**.

**Saturn V Stages:**

| Stage | Engine(s) | Burn Time | Thrust |
|-------|-----------|-----------|--------|
| S-IC | 5x F-1 | ~150–168 s | ~35,100 kN SL each |
| S-II | 5x J-2 | ~360–390 s | ~5,150 kN vac each |
| S-IVB | 1x J-2 | ~470 s + ~335 s (TLI) | ~1,033 kN vac |

**Apollo CSM/LM:**

| Vehicle | Mass | Crew | Key Features |
|---------|------|------|--------------|
| Command Module | ~5.6 t | 3 | Ablative heatshield |
| Service Module | ~24.5 t (prop) | 0 | SPS engine (310s Isp) |
| Lunar Module | ~15.2 t (fueled) | 2 | Descent/ascent stages |

**Gemini:** Capsule ~3.8 t, 2 crew with ejection seats. Titan II GLV ~154 t liftoff (hypergolic launcher). Agena Target ~3.2 t.

### 8.9 RNMisc, ROStations, USRockets

- **RNMisc:** Miscellaneous spacecraft parts — MIR Docking Port, Spacelab pressurized laboratory. 8 releases, CC BY-NC-ND 3.0.
- **ROStations:** Station parts and configs for RO, with BDB integration docs. Requires Realism Overhaul and ModuleManager.
- **USRockets:** Older, obscure US launchers. 30 releases, CC BY-NC-ND 3.0. Old School Fairings support, pre-built craft files.

### 8.10 Explorer 1 Ascent Procedure

Explorer 1 launched on a Juno I (modified Jupiter-C). To replicate in RO:

1. Change engine config from Ethanol/LOx to **Hydyne/LOx**
2. Fly ascent until booster burnout, decouple booster cluster
3. RCS orient to **90° heading, 0° pitch** (horizontal, eastward)
4. Spin up upper stage and decouple payload at ~10 seconds to apogee
5. Fire solid upper stages (Baby Sergeant rockets) in succession
6. Spin-stabilization required — Juno I upper cluster was spin-stabilized

**Hydyne:** 60% UDMH + 40% DET. Storable hypergolic fuel, higher performance than ethanol. Available as RealFuels propellant option for Jupiter-C / Juno I engine config.

---

## 9. Utilities & Tools

### 9.1 Kerbal Joint Reinforcement Continued (KJR-C) — ⚠️ Hard DEPENDS

- **Repository:** [KSP-RO/Kerbal-Joint-Reinforcement-Continued](https://github.com/KSP-RO/Kerbal-Joint-Reinforcement-Continued)
- Physics stabilizer — community continuation of ferram4's original. Prevents Kraken (physics instability) by reinforcing vessel joints during loading, decoupling, launch.

**Features:** Physics easing (slowly dials up external forces), launch clamp easing, interstage connection reinforcement, connection stiffness (larger parts get stiffer), sequential stack parts get extra anti-wobble connection, world-space joints (heaviest parts connect directly to ground on pad).

**Configuration (config.xml):**

| Parameter | Default | Description |
|---|---|---|
| `reinforceAttachNodes` | 1 | Toggle all joint stiffening |
| `multiPartAttachNodeReinforcement` | 1 | Extra stiffening for stack parts |
| `reinforceDecouplersFurther` | 1 | Interstage connection stiffening |
| `reinforceLaunchClampsFurther` | 1 | Launch clamp stiffening |
| `worldSpaceJoints` | 1 | Ground-connected heaviest parts on pad |
| `useVolumeNotArea` | 1 | Use volume not area for connection strength |
| `massForAdjustment` | 0.01 | Parts below this mass are not stiffened |
| `breakForceMultiplier` | 1 | Scale failure strength for forces |
| `breakTorqueMultiplier` | 1 | Scale failure strength for torque |

**KJR-Next (KJRn):** Complete rewrite by Rudolf Meier. Alternative to KJR-Continued. Fully automatic, compatible with DLC robotic parts and Infernal Robotics Next. Compiled for KSP 1.12.3 (works 1.4+). Better performance — no stuttering/freezes. Available on CKAN.

**Known Issues:** Overconstrained rockets (too many launch clamps) may spontaneously disintegrate. Certain PartModule types exempt from stiffening (pWings, KerbalEVA, procedural wings, etc.).

### 9.2 Kerbal Konstructs

- **Repository:** [KSP-RO/Kerbal-Konstructs](https://github.com/KSP-RO/Kerbal-Konstructs) — 17 releases, MIT (plugin) / Custom (assets)
- Static object placement (launch sites, air bases, fuel stations) with in-game editor. Spiritual successor to KerbTown. Dependency for many RO launch site mods.
- **Requires:** CustomPreLaunchChecks — without it, KK will not function.

**⚠️ Known Issue:** In-game editor broken on KSP 1.12.x — editor for creating/modifying statics non-functional. Existing statics display and facilities work.

**Compatible mods (KSP 1.12.x):** Kerbin Side Remastered, KSC Harbor, KSC Extended, Water Launch Sites.

### 9.3 KSCSwitcher

- **Repository:** [KSP-RO/KSCSwitcher](https://github.com/KSP-RO/KSCSwitcher) — 13 releases
- Relocates the Kerbal Space Center to any Earth coordinate. Essential for RO/RP-1 historical launches. Integrates with Kerbal Konstructs for physical launch pad placement.

**Launch Sites:**

| Site | Latitude | Notes |
|------|----------|-------|
| Cape Canaveral, FL | 28.5°N | Kennedy Space Center / CCSFS |
| Baikonur Cosmodrome, Kazakhstan | 45.9°N | Russian/Soviet primary site |
| Kourou, French Guiana | 5.2°N | European Spaceport (optimal equatorial/GTO) |
| Vandenberg SFB, CA | 34.7°N | Polar orbit launches |
| Plesetsk Cosmodrome, Russia | 62.8°N | High-inclination launches |
| Wallops Flight Facility, VA | 37.9°N | NASA suborbital/small orbital |
| Tanegashima Space Center, Japan | 30.4°N | JAXA launches |
| Satish Dhawan Space Centre, India | 13.7°N | ISRO launches |
| Jiuquan Satellite Launch Center, China | 40.9°N | CNSA launches |

Settings via ModuleManager patches or in-game UI. Does not require RP-1. Switching mid-career may cause issues — restart new career for clean switching.

### 9.4 RealAntennas

- **Repository:** [KSP-RO/RealAntennas](https://github.com/KSP-RO/RealAntennas) — 14 releases
- Physics-based RF communications model replacing stock CommNet range system. Implements proper antenna gain, transmit power, RF frequency/bandwidth, receiver noise modeling, variable data rates, multiple modulation schemes.

**Core Model:**

```
RxPower = TxPower + TxGain - FreeSpacePathLoss + RxGain
SNR = RxPower - Rx_NoiseFloor
```

**Friis Transmission Equation:** `FSPL(dB) = 20 × log10(distance) + 20 × log10(frequency) + 32.44`

**Shannon-Hartley Theorem:** `C = B × log2(1 + SNR)` where B = bandwidth.

**Noise floor:** `NoiseFloor(dBm) = 10 × log10(k × T × B) + 30`

**Key Features:** Multiple antennas per node, asymmetric links, no antenna combining (at most one link between node pairs based on best antenna pair), multiple modulation schemes (BPSK/QPSK/8PSK/QAM varieties), band-specific behavior.

**Dish Mechanics:** Diameter directly affects gain. Pointing accuracy matters — off-axis alignment reduces effective gain. Beam width narrows with frequency and dish size.

### 9.5 ContractConfigurator

- **Repository:** [KSP-RO/ContractConfigurator](https://github.com/KSP-RO/ContractConfigurator) — 38 releases, MIT (core) / GPL v2.0 (CC_RemoteTech.dll)
- Config-file-based contract creation framework. Foundation for RP-1 contract packs. **⚠️ Last release: v1.30.5, Oct 2020 — may be outdated for KSP 1.12+; newer versions may exist under KSP-RO maintenance.**

**Contract Sections:** Parameters (objectives), Requirements (prerequisites), Data Nodes (state storage), Behavior (logic/flow), Rewards (funds, science, reputation).

**12 Key Parameter Types:** VesselParameterGroup, ReachState, OrbitParameter, SpecificVessel, PartParameter, ScienceParameter, DurationParameter, CrewParameter, ResourceParameter, CommsParameter, Contracts.ActiveContracts, Contracts.CompletedContracts.

### 9.6 LunarTransferPlanner

- Simple GUI for optimal lunar transfer launch windows in RSS. Configurable flight time (default 4 days), shows next two minimum-inclination windows. Optional: Toolbar, Kerbal Alarm Clock. License: MIT.

### 9.7 CustomPreLaunchChecks

- Configurable pre-launch validation checks — vessel cannot launch until criteria are met. Required dependency for Kerbal Konstructs.

### 9.8 FlightSchool

- RP-1 career addon: newly hired kerbals must complete training before serving as pilots. Training consumes time and funds. Requires RP-1.

### 9.9 KerbalRenamer

- Culturally diverse kerbal names from 55+ cultures. Named profiles (e.g., "1951" for early space nations), CUSTOM profile, surname-first culture support, binary gender system.

### 9.10 CanaveralPads

- Historically accurate Cape Canaveral/KSC launch complexes. LC-39 (Apollo/Space Shuttle, by Katniss), various pads by Aviation365. **Dependencies:** RSS-CanaveralHD, Kerbal Konstructs (fixed version), OSSNTR, Tundra Space Center. License: CC-BY-NC-SA 4.0.

---

## 10. Infrastructure & Build Tools

### 10.1 BuildTools

- **Repository:** [KSP-RO/BuildTools](https://github.com/KSP-RO/BuildTools) — 53 commits
- Reusable GitHub Actions workflows and composite actions for KSP-RO CI/CD. All KSP-RO mod repositories reference these.

**Composite Actions:** `checkout-BuildTools`, `download-assemblies`, `download-assemblies-v2` (CKAN-based, preferred), `process-changelog`, `remove-excess-dlls`, `update-assembly-info`, `update-version-file`, `update-version-in-readme`.

**Reusable Workflows:** `check-secret.yml` (verifies decryption password), `validate-cfg-files.yml` (validates `.cfg` files using KSPMMCfgParser).

**CI/CD Pipeline (10 steps):** Trigger → Checkout → Secret Check → Assembly Download → Build → Validation (KSPMMCfgParser) → Changelog Processing → Version Updates → Cleanup → Artifact.

### 10.2 BuildLibs

- Encrypted library files for building KSP-RO mods. Contents: `ExtraLibs/` and `KSP_x64_Data.zip`. Decryption key available to KSP-RO maintainers. Not intended for end users.

### 10.3 CFGProjectGenerator

- Generates `.csproj` files from `.cfg` files for IDE-based development. Automates creation of Visual Studio project files that include all `.cfg` files from a mod's `GameData` directory.

### 10.4 ROLibrary

- **Repository:** [KSP-RO/ROLibrary](https://github.com/KSP-RO/ROLibrary) — 30 releases
- Shared C# code library for ROx family mods (ROCapsules, ROEngines, ROSolar, ROHeatshields, ROTanks). Centralized shared code ensuring consistency.
- **Data flow:** `ModuleManager patches → ROx mod .cfg files → ROLibrary API → ROx PartModules → In-game behavior`
- **Dependencies:** ModuleManager, Realism Overhaul. Automatically installed when installing any ROx mod.

### 10.5 ROUtils

- **Repository:** [KSP-RO/ROUtils](https://github.com/KSP-RO/ROUtils) — 4 releases, CC-BY-NC-SA 4.0
- Lowest-level utility library for the entire KSP-RO ecosystem. Used across ALL KSP-RO mods.
- **Capabilities:** Logging infrastructure, configuration helpers, extension methods on KSP stock classes, type converters (KSP/SI/real-world units), assembly attribute helpers.
- ROLibrary builds on ROUtils for the ROx family. RO itself depends on ROUtils.

### 10.6 ROLoadingImages

- Replaces KSP loading screens with RSS/RO/RP-1 themed images. Lightweight cosmetic mod — simple texture replacement. License: CC-BY-NC-SA 4.0.

### 10.7 RP-1-ExpressInstall

- CKAN metapackage for one-click RO/RP-1 installation. Handles version locking and dependency resolution for 80+ interdependent mods. Identifier `RP-1-ExpressInstall`, latest version 2.0-r5. License: CC-BY-NC-SA.

### 10.8 RP1AnalyticsWebApp

- "Spyware v2.0" — web app for RP-1 career analytics. Collects and visualizes opt-in career progression data. Stack: ASP.NET Core, MongoDB, Vite frontend, MS Azure. Data collected (opt-in only): Career progression, tech tree unlocks, facility upgrades, financial history, vessel statistics. No personal identifying information.

### 10.9 ECM-Viewer

- Web-based Engine Configurator Module viewer. Static single-page web app (single `index.html`). Browsable interface for ECM configuration data — engine parameters in human-readable tables.

### 10.10 GoForLaunch

- Collaborative RP-0 career save. Multiple players share one career, taking turns launching missions.

---

## 11. Dependency Tree & Conflict Map

### 11.1 Dependency Tree

```
RealSolarSystem (RSS)
    ↑
RealismOverhaul (RO)
    ↑
RP-1 (Realistic Progression One)
    │
    ├── RealFuels ─── SolverEngines
    │                       ↑
    └── ProceduralParts ────┘
```

**Full dependency chain (KSP 1.12.x):**

```
ROUtils ───┬── RealismOverhaul ───┬── ROx mods (ROEngines, ROCapsules, etc.)
           │                      └── RP-1 ───┬── RP-1-ExpressInstall
           │                                  └── RP1AnalyticsWebApp
           ├── BuildTools ───┬── BuildLibs (encrypted assemblies)
           │                 └── CFGProjectGenerator
           ├── ROLibrary (ROx-specific shared code)
           └── ROLoadingImages (cosmetic)
```

**Utility dependency chain:**

| Mod | Depends On | Used By |
|---|---|---|
| KJR-Continued | — | All large craft, RO/RP-1 |
| Kerbal Konstructs | CustomPreLaunchChecks | CanaveralPads, Tundra Space Center, KSCSwitcher |
| KSCSwitcher | Kerbal Konstructs (optional) | RP-1 career mode |
| RealAntennas | — | RO/RSS (replaces stock CommNet) |
| ContractConfigurator | ModuleManager | RP-1 contracts |
| CustomPreLaunchChecks | — | Kerbal Konstructs |
| CanaveralPads | RSS-CanaveralHD, Kerbal Konstructs, OSSNTR, Tundra Space Center | RO/RP-1 launch sites |

### 11.2 Engine/Propulsion Dependency Overview

```
                    ┌─────────────────┐
                    │  SolverEngines  │  (Developer plugin, engine simulation core)
                    └────────┬────────┘
                             │
              ┌──────────────┼──────────────┐
              ▼              ▼              ▼
        ┌──────────┐  ┌──────────┐  ┌──────────┐
        │   AJE    │  │ RealFuels│  │ (Other)  │
        │ (Jets)   │  │(Rockets) │  │  Mods    │
        └──────────┘  └──────────┘  └──────────┘
                                      │
                                      ▼
                              ┌────────────┐
                              │ ROEngines  │
                              │(3D models) │
                              └────────────┘

                    ┌─────────────────────────┐
                    │   SmokeScreen (Plugin)   │
                    └────────────┬────────────┘
                                 │
                    ┌────────────▼────────────┐
                    │       RealPlume          │ (Core effect templates)
                    └────────────┬────────────┘
                                 │
                    ┌────────────▼────────────┐
                    │ RealPlume-StockConfigs   │ (Engine-specific assignments)
                    └─────────────────────────┘
```

### 11.3 CONFLICTS (Must NOT Install)

| Mod | Reason |
|-----|--------|
| **DeadlyReentry** | ⚠️ RO v18.x uses RealHeat for thermal modeling; CKAN refuses both |
| EngineLightRelit | Conflicts with RO engine lighting configs |
| RocketSoundEnhancement | Conflicts with RO sound configs |
| StockWaterfallEffects | Conflicts with RO plume system |
| WaterfallRestock | Conflicts with RO plume system |
| TweakableEverythingCont | Incompatible tweak system |
| RealPlumeConfigs | RO provides its own (`provides:` in .netkan) |
| TestFlightConfig | RO provides its own (`provides:` in .netkan) |
| HotRockets | Incompatible with RealPlume |
| Community Tech Tree | Incompatible with RP-1 historical tech tree |
| SSTU | Does not work with current KSP/RO — use ROTanks + ROEngines |
| Sol/RSS-Reborn | Not officially supported |
| Volumetric clouds (Blackrack) | Not available in RSS |
| VisibleMapsActive | Should be avoided |

> **General rule:** Assume any mod not explicitly marked as RSS/RO/RP-1 compatible is incompatible.

### 11.4 Mods to Avoid

| Mod | Reason |
|-----|--------|
| DeadlyReentry | **INCOMPATIBLE with RO v18.x** — use RealHeat + ROHeatshields |
| EngineLightRelit | Conflicts with RO engine lighting |
| RocketSoundEnhancement | Conflicts with RO sound configs |
| StockWaterfallEffects | RO provides own Waterfall configs |
| WaterfallRestock | Conflicts with RO Waterfall setup |
| TweakableEverythingCont | Incompatible with RO TweakScale handling |
| Community Tech Tree | Incompatible with RP-1 historical tech tree |
| SSTU | Does not work with current KSP/RO — use ROTanks + ROEngines |

### 11.5 Recommended Additional Mods

| Mod | Description |
|-----|-------------|
| Hangar Extender | Extended VAB/SPH camera |
| KSC Switcher | Multiple launch sites |
| Procedural Fairings | Custom fairing construction |
| Textures Unlimited | Visual texture customization |
| Waterfall | Advanced engine plume effects |
| Principia *(manual)* | N-body physics |
| Custom Asteroids | Realistic asteroid distribution |
| PlanetShine | Planetary albedo lighting |
| Connected Living Space | IVA crew transfer |
| RCS Build Aid | RCS placement tool |

---

## 12. False KSP Lessons

> *"A lot you will need to unlearn."* — Moving from Kerbin to Earth.

### Complete Table of 10 Myths Debunked

| # | Myth | Reality |
|---|------|---------|
| **1** | **MOAR BOOSTERS is best** | High TWR causes aerodynamic destruction, mass penalty, inability to throttle down. Delta-V is a function of mass ratio. Optimal LEO: small 30s solids, 3-minute core, 6-minute upper stage. |
| **2** | **Rocket engines all throttle to 0.1%** | Very few engines throttle. Deep throttling (landing): LMDE got to ~10% max. Shallow throttling (reduce G-loads): RS-25 throttles to ~70%. Most engines don't throttle at all. |
| **3** | **Rockets must throttle down due to air resistance** | Real rockets throttle at Max Q for structural loading, not efficiency. Optimal flight profile is "bang-bang control" (100% throttle or 0% coasting). |
| **4** | **Rocket engines are infinitely restartable** | Restarting is limited. The ullage problem: propellants float away from feed lines in freefall. Solutions: ullage motors, RCS burns. Most first-stage engines: 1 ignition. Pressure-fed hypergolic: effectively infinite. |
| **5** | **Burn up to get apoapsis, then burn right to circularize** | Most real LVs perform a single continuous burn interrupted only by staging. Many burn well after apogee (Saturn V, hydrolox upper stages). Gravity turn takes ~3 minutes of a 7–12+ minute ascent. |
| **6** | **All propellants are created equal** | Kerolox (dense, ~350s Isp), Hydrolox (1/2.84 density, ~460s Isp), Hypergolic storables (room-temp storable, no boil-off). NTR: uses LH2 (0.07085 kg/L), up to 1000s Isp. |
| **7** | **Rocket engines and fuel tanks are heavy** | Real engines have very high TWR (maxing over 150:1). Atlas D sustainer: loaded 113 tonnes, dry 2.347 tonnes. In KSP, only liquid engines/tanks underperform (too heavy by factor of 3–8). |
| **8** | **Reaction wheels are magical all-powerful devices** | Attitude control uses gimbaled thrust and RCS. Reaction wheels have limited torque, especially under thrust, and can only apply torque temporarily (spin up → need RCS to spin down). |
| **9** | **Orbital rendezvous is easy from any starting orbit** | Real rendezvous requires waiting for launch windows, launching into the same plane (with small "dogleg" corrections), placing chaser in slightly lower/behind orbit, and maintaining low-velocity approaches. |
| **10** | **A heatshield is enough for any reentry; shallower is better** | LEO-rated shields won't survive translunar/interplanetary reentry. Shallower reentries: lower peak heat flux, higher total heat load. Steep reentries (Mercury negative perigee): high peak heating, low total load, 9G. Lifting reentries (STS): much higher total load, lower peak flux. Heat shield types: Ablative (Apollo, Dragon, Orion), TPS tiles (Space Shuttle), PICA (Stardust, Dragon). |

### Engine-Related False Lessons (Supplement)

| Stock KSP Myth | RO Reality |
|----------------|------------|
| All engines throttle to 0.1% | Very few engines throttle at all |
| Infinite restarts | Most engines have 1–3 ignitions max |
| Moar Boosters always helps | Higher TWR = heavier engines = less delta-v |
| All propellants are equal | Kerolox, hydrolox, hypergolics differ dramatically |

---

## 13. Cross-Mod Integration Notes

### 13.1 Either/Or Pairs (from .netkan)

| Choice A | Choice B | Purpose |
|----------|----------|---------|
| RemoteTech | RealAntennas | Communications |
| Kerbalism-Config-RO | TACLS | Life support |
| TestFlight | TestLite | Part reliability |
| VenStockRevamp | Restock | Stock visual overhaul |

### 13.2 Deprecated / Superseded Mods

| Old Mod | Replace With | Status |
|---------|-------------|--------|
| DeadlyReentry | RealHeat + ROHeatshields | ⚠️ CONFLICTS with RO v18.x |
| EngineIgnitor (standalone) | RealFuels (integrated) | Merged into RealFuels |
| RemoteTech | RealAntennas | Legacy support only |
| AIES Aerospace | Not maintained | May not work with current RO |
| Kerbalism (v3.11, 2019) | ROKerbalism | OUTDATED — KSP 1.5–1.10 only |
| ContractConfigurator (v1.30.5, 2020) | KSP-RO maintained fork | Potentially outdated |
| RP-0 | RP-1 (Realistic Progression One) | Superseded career mode |
| StretchySRBs / StretchyTanks | ProceduralParts | Legacy — ProceduralParts hides old parts |

### 13.3 Communications: RemoteTech vs RealAntennas

The RO `.netkan` recommends one of RemoteTech or RealAntennas. They serve the same purpose (realistic antenna/link simulation) and should not both be installed. **RealAntennas is the modern, RO-recommended option.** RemoteTech standalone not recommended. Soviet and US mods include legacy RemoteTech support — for modern RO, use RealAntennas.

**RemoteTech is incompatible with ROKerbalism.**

---

## 14. Key Technical Terms Glossary

| Term | Definition |
|------|-----------|
| **Ullage** | Settling propellants to tank bottoms before engine ignition in microgravity; required for all pump-fed liquid engines |
| **Vernier** | Small gimballed engine for fine trajectory control during/after main engine burn |
| **Pressure-fed** | Engine where high-pressure gas (helium, ~200 atm) forces propellant in; requires ServiceModule tank type |
| **Pump-fed** | Engine using a turbopump to deliver fuel; uses standard tank types |
| **Balloon tank** | Thin-walled tank relying on internal pressure for structural integrity (Centaur, Atlas); lighter but more fragile |
| **ServiceModule tank** | Pressurized tank type required for pressure-fed engines (~200 atm helium pressurization) |
| **TEATEB** | Triethylaluminium-triethylborane — pyrotechnic hypergolic igniter fluid (H-1 engine example) |
| **Hydyne** | 60% UDMH + 40% diethylenetriamine — high-energy fuel for Juno I (Explorer 1) |
| **Boil-off** | Cryogenic propellant evaporation over time (especially LH2); managed with exterior radiator fins and cryogenic tank types |
| **Kraken** | Community term for KSP physics instability causing spontaneous vessel destruction; mitigated by KJR-Continued |
| **ModuleEngineRF** | RealFuels engine module handling ignitions, throttle, fuel types, and ullage simulation |
| **TestFlight** | Reliability simulation system — engines become more reliable with accumulated flight data |
| **ModuleManager** | Patch-loading framework that applies RO configs at game load time |
| **CKAN** | Comprehensive Kerbal Archive Network — mod manager and repository |
| **Metapackage** | Package defining dependencies but containing no code (e.g., RP-1-ExpressInstall) |
| **.netkan** | CKAN package metadata format (JSON) — defines dependencies, conflicts, and provides |
| **ECM** | Engine Configurator Module — defines real engine parameters in RO (thrust, Isp, throttle, ignitions, ullage, gimbal) |
| **ROx** | RO-specific part mods family: ROEngines, ROCapsules, ROTanks, ROSolar, ROHeatshields |
| **LES** | Launch Escape System — tower with abort motors for crew capsule emergency escape |
| **HGA** | High-Gain Antenna — directional communications antenna for deep-space missions |
| **CBM** | Common Berthing Mechanism — used by Cygnus for ISS attachment (not direct docking) |
| **KSPMMCfgParser** | Tool by HebaruSan/CKAN validating `.cfg` file syntax in CI/CD pipelines |
| **Composite Action** | GitHub Actions reusable step-level action (BuildTools) |
| **Reusable Workflow** | GitHub Actions reusable job-level workflow (BuildTools) |

---

## 15. Consolidated FAQ

### Installation Issues

**CKAN won't let me install RO?** Start with clean KSP 1.12.x install (build.txt version 1024+). Check for conflicting mods.

**CKAN Express Install conflicts (RP-1)?** Known: Tweakscale Rescaled Redistributable v3.2.2 vs v2.4.8.6; Stockalike RF Configs conflicts. Check version compatibilities.

**Manual installation tips?** Delete old RealismOverhaul folder first. Use ONLY the ModularFlightIntegrator from Kopernicus.

**ModuleManager stuck at "Applying patches."** First load after installing RO can take 5–15 minutes. If hung >30 minutes, check for mod conflicts or insufficient RAM.

**Game crashes on load with RO installed.** Common causes: insufficient RAM (16+ GB recommended), outdated mods, missing dependencies. Try CKAN Express Install.

### Engine & Propulsion

**Engines won't light or flameout quickly?** Check in order: 1) Ullage simulation — propellants not settled, 2) Electric charge — spark required for ignition, 3) Tank type — verify correct type (ServiceModule for pressure-fed), 4) Ignition count — check remaining ignitions in RealFuels engine config, 5) Engine config — verify fuel type matching.

**Engines turning off too early?** Two common causes: 1) TestFlight may simulate reliability failures (more flights = more reliable), 2) MechJeb "Limit Acceleration" minimum set to 0 may throttle real engines to 0.

**Zero ignitions remaining?** Edit RealFuels config files, find `ignitions = X`, modify or remove.

**How to deal with ullage in upper stages?** Hot-staging for ascent; ullage motors or small RCS burns for orbital maneuvers.

**My engine keeps failing (TestFlight).** New engines have low flight data and higher failure rates. Fly more test flights with the same engine type.

**How do I check reliability (TestFlight)?** Right-click menu in flight shows accumulated flight data and current estimated reliability percentage.

**Can I disable TestFlight?** Remove from GameData or set reliability to 100% via config. However, RO balance assumes TestFlight is present.

**SABRE engine issues?** Known: some RP-1 users report problems with newer AJE versions; downgrading to v2.10.0 fixes it. SABRE's Isp in jet mode may be too low. Precoolers are cosmetic/non-functional.

### Parts, Tanks & Capsules

**Tank keeps losing propellant (boil-off).** Cryogenic tanks simulate boil-off. Use Service Module tanks for hypergolic propellants to avoid evaporation.

**Tank mass seems wrong.** Each tank type has different mass ratios. In RP-1 career, mass ratios improve with tech level progression.

**Service module tanks aren't working with pressure-fed engines.** Service Module tank type is required for pressure-fed engines. If the tank is set to Cryogenic, hypergolic pressurization may not work.

**Gemini capsule parts are missing.** Gemini requires ModuleDepthMask. Verify this plugin is installed.

**Dynasoar parts don't work properly.** Dynasoar requires KSPWheel for landing gear functionality.

**Apollo LES won't separate.** Check that staging is correctly configured. The LES decoupler and abort motor must be staged in the correct sequence.

**Crewed spacecraft weighs much more than expected.** Each crew member adds 100 kg (person + suit) to part mass. This is a RO feature.

**Parts don't appear in the VAB.** Check mod installation, Module Manager, and RO patch application in KSP log.

**Fairings don't automatically reshape.** Try removing and re-attaching the fairing base. Ensure payload is fully connected and KSPCommunityFixes is installed.

**Fairings clip through the payload.** Increase fairing diameter or adjust shape parameters. Some payload shapes are too complex for auto-reshaping.

**Can't create inline fairings.** Place an inverted fairing base above the enclosed section. You may need low-profile base rings for tight interstage spaces.

**Fairings won't separate.** Check staging sequence. Ensure fairing is set to jettison in the right-click menu.

**Can I use Community Tech Tree with RP-1?** No — RP-1 uses its own historical tech tree.

**Can I use RealPlume-StockConfigs with RO?** No — RO uses its own plume configs tailored to real-scale engines.

**SSTU doesn't work with RO.** SSTU is incompatible with RO/RP-1 for KSP 1.6.1+. Use ROTanks and ROEngines instead.

### Life Support & Reliability

**Life support resources depleting too fast?** Verify storage for mission duration. Check recycler functionality in TACLS. In Kerbalism, verify consumption appropriate for crew count.

**Can I combine TAC Life Support with Kerbalism?** **No.** The RO `.netkan` recommends one or the other — they are mutually exclusive.

**Parachutes aren't working or burn up.** Use RealChute (RO standard). Adjust max chute temperature via tweakables.

**RCS thrusters burn up during reentry.** RCS thrusters have low maxTemp threshold. Mitigation: place RCS thrusters above the fuselage or behind wings for aerodynamic shielding.

**Craft burns up on reentry even with heatshield (historical DRE).** Verify shield facing retrograde. Check heat multiplier (Alt+D+R). LEO reentry requires ~25x multiplier in RO.

### Visuals & Environment

**Earth shows up as grey sphere or checkerboard.** Textures not installed correctly. Ensure contents of the chosen resolution folder are merged directly into `GameData/`, not nested.

**Game crashes when switching to map view.** Texture resolution may be too high for available VRAM. Step down: 16384 → 8192 → 4096.

**Seams/tears visible on Earth's surface.** Known artifact at lower resolutions (2048, 4096). Use 8192 to minimize seams. Ensure no conflicting texture mods.

**How do I upgrade to higher-resolution textures?** Delete the old texture folder completely from GameData before installing higher-resolution ones.

**Clouds aren't showing (RSSVE).** Verify EVE at correct version per RSSVE release. Check `EnvironmentalVisualEnhancements.dll` in GameData. Ensure Scatterer installed.

**Game crashes on load (RSSVE).** Usually version mismatch. Use exactly the EVE and Scatterer versions on the RSSVE release page.

**Clouds flicker badly.** Reduce time warp to 10x or lower. Known limitation of EVE rendering.

**Parts are overheating when they shouldn't (RealHeat).** RealHeat models physical heat transfer — parts in direct sunlight or near hot engines will heat realistically. Ensure adequate radiator surface area.

**Engines overheat during long burns.** Sustained burns produce significant heat. Use engine plate radiators or limit burn times.

**Oceans appear black/glitched.** Scatterer version mismatch. Reinstall Scatterer configs from the RSSVE release package.

**ISRU equipment isn't processing.** Check: 1) Adequate power, 2) Correct input resources present, 3) Output storage not full, 4) Equipment on correct celestial body.

### Historical Spacecraft

**Soyuz/Proton engine won't stage separate.** Check decoupler staging sequence. SovietRockets uses realistic staging events. Use included craft files as reference.

**Proton rocket keeps exploding on the pad.** Proton uses hypergolic propellants (N₂O₄/UDMH). Check that RealFuels tank types are set correctly for hypergolic fuels.

**Vostok/Voskhod won't separate from service module.** Verify separation sequence in craft files. Reentry mode differs from US capsules (ejection seat for Vostok).

**Why is TKS/VA so heavy?** ~21 tonnes fully loaded. FGB module is very massive. Use Proton-K or Proton-M.

**No solar panels on Soyuz 7K-T?** Correct — 7K-T used batteries for reduced mass. Mission limited to ~2 days. Upgrade to 7K-OK for solar panels.

**Can I use Soyuz craft files without SovietRockets?** No. SovietSpacecraft requires SovietRockets.

**Pioneer/Voyager has no power.** Pioneer/Voyager use RTGs — ensure RTG part is attached and has sufficient fuel.

**Mariner probe not communicating.** Check antenna deployment and power. Early probes had low-power transmitters. RealAntennas recommended for interplanetary distances.

**Launcher for Ranger/Surveyor?** Ranger used Atlas-Agena; Surveyor used Atlas-Centaur. Install USRockets or appropriate US launcher mods.

**FASA parts missing or scaled incorrectly.** FASA-RO provides only RO patches — original FASA mod must be installed first.

**Saturn V keeps exploding or underpowered.** Verify staging matches craft files. Use steep ascent profile — "only level off at the end." Apollo LM descent engine throttles ~10–60%.

**Gemini won't separate from Titan II.** Gemini uses "fire-in-the-hole" staging. Use included craft file staging. Verify retro-rocket activation.

**Which Saturn V for Skylab?** Skylab auto-detects FASA, OLDD, or DECQ. DECQ highest priority.

**Cygnus won't dock with ISS.** Cygnus uses CBM (Common Berthing Mechanism) — berthed by robotic arm, not direct docking.

**Which Antares variant is current?** Antares 230+ (latest). For RP-1 careers, Antares 230 typically available from the 2010s era.

**Cygnus has no RCS/propellant.** Cygnus uses hypergolic bipropellant RCS. Check service module tanks have NTO/MMH.

**RemoteTech vs RealAntennas?** Soviet and US mods include legacy RemoteTech support. For modern RO, use RealAntennas. RemoteTech standalone not recommended.

**Will adding part packs break my save?** Adding part packs works via ModuleManager patches. Removing packs may cause missing part errors. Always back up saves.

### Utilities & Tools

**No communication link (RealAntennas).** Verify antenna band matches — UHF cannot talk to X-band dish at interplanetary distances. Dish pointing: steerable dishes must be oriented toward target.

**Asymmetric comms (RealAntennas).** Vessel may receive commands but not transmit science — designed behavior for asymmetric links.

**Launch site not appearing (KSCSwitcher).** Verify Kerbal Konstructs installed, static objects loaded.

**Wrong terrain elevation (KSCSwitcher).** Adjust KSCSwitcher altitude offset.

**Mid-career switching (KSCSwitcher).** May cause issues — restart for clean switching.

**Contracts not appearing (ContractConfigurator).** Check `.cfg` placement and ModuleManager processing (`ModuleManager.ConfigCache`).

**Requirements not triggering (ContractConfigurator).** Verify prerequisites completed, tech level unlocked. Use only RP-1 contracts or explicitly compatible packs.

**Overconstrained rockets (KJR).** Too many launch clamps → spontaneous disintegration from phantom forces.

**Performance issues (KJR).** Adjust `massForAdjustment` upward if experiencing lag with large craft.

### General Support

- **Discord:** https://discord.gg/ZGbR6nv or https://discord.gg/V73jjNd
- **GitHub Issues:** https://github.com/KSP-RO/RealismOverhaul/issues
- **Release Thread:** https://forum.kerbalspaceprogram.com/topic/155700-112-ksp-ro-realism-overhaul-16-may-2022/
- **Support Policy:** No support without logs (KSP.log / output_log.txt) and reproduction steps

**Venus entry tips?** Lunar-rated heat shields survive Venus entry from hyperbolic trajectories. Set periapsis >100 km for safer entry. Multiple aerobraking passes possible. Very thick atmosphere — may not need parachutes.

**Crew mass too high?** Each crew member = 100 kg (person + suit). This is realistic.

**CKAN is recommended** — handles dependency resolution and version compatibility.

**Start with clean KSP install** for RO/RP-1 to avoid conflicts.

**Random crashes in VAB/SPH.** Usually memory pressure. Reduce texture resolution, remove unused part packs.

---

*End of Realism Overhaul — Complete Reference. All information consolidated from the KSP-RO skill reference files. Last updated: 2026-06-22.*
