---
name: realism-overhaul
description: >
  Comprehensive knowledge base for the Realism Overhaul (RO) mod suite in Kerbal Space Program
  (KSP 1.12.x). Covers all 68 KSP-RO GitHub organization repositories across 8 categories:
  core foundational mods (RealismOverhaul, RP-1, RSS, RealFuels, ProceduralParts),
  engine & propulsion (SolverEngines, AJE, ROEngines, RealPlume), parts/tanks/capsules
  (ROTanks, ROCapsules, ProceduralFairings), life support & reliability (TACLS, TestFlight,
  ROKerbalism, DeadlyReentry), visual enhancements (RSS-Textures, RSSVE, RealHeat, RealISRU),
  historical spacecraft (SovietRockets, USProbesPack, FASA-RO, Skylab, SalyutStations),
  utility mods (KJR, KerbalKonstructs, KSCSwitcher, RealAntennas, ContractConfigurator),
  and infrastructure/build tools. Includes installation dependencies, .netkan conflict maps,
  changelog highlights through v18.0.0, forum thread references, technical deep-dives,
  troubleshooting FAQs, and the official "False KSP Lessons" guide.
author: Acea
sources:
  - https://github.com/KSP-RO (all 68 repositories)
  - https://github.com/KSP-RO/RealismOverhaul (RO master repo)
  - https://raw.githubusercontent.com/KSP-RO/RealismOverhaul/master/RealismOverhaul.netkan
  - https://github.com/KSP-RO/RealismOverhaul/wiki
  - https://github.com/KSP-RO/RP-1/wiki
  - https://github.com/KSP-RO/RealSolarSystem/wiki
  - https://forum.kerbalspaceprogram.com/topic/155700-112-ksp-ro-realism-overhaul-16-may-2022/ (RO release thread)
  - https://forum.kerbalspaceprogram.com/topic/88409-realism-overhaul-discussion-thread/ (RO discussion thread)
  - https://forum.kerbalspaceprogram.com/topic/190040-rp-1-realistic-progression-one-for-ksp-1123 (RP-1 thread)
  - https://forum.kerbalspaceprogram.com/topic/177216-112-real-solar-system (RSS thread)
  - https://forum.kerbalspaceprogram.com/topic/58236-18-real-fuels (RealFuels thread)
  - https://forum.kerbalspaceprogram.com/topic/50296-112-deadly-reentry-v790-the-barbie-edition-aug-5th-2021 (DRE thread)
  - https://forum.kerbalspaceprogram.com/topic/204080-18-procedural-parts (ProceduralParts thread)
  - https://forum.kerbalspaceprogram.com/topic/139868-18-111-advanced-jet-engine-v2170-june-26 (AJE thread)
  - https://forum.kerbalspaceprogram.com/topic/188033-ksp112x-realplume-stock-v408-realplume-v1332-25jun2021 (RealPlume thread)
  - https://forum.kerbalspaceprogram.com/topic/146465-112x-tac-life-support-v0180-release-19th-sep-2021 (TACLS thread)
  - https://forum.kerbalspaceprogram.com/topic/184206-kerbal-joint-reinforcement-next (KJR thread)
  - https://forum.kerbalspaceprogram.com/topic/204210-ksp-18-kerbal-konstructs-continued (KK thread)
  - https://forum.kerbalspaceprogram.com/topic/91625-1101-contract-configurator-v1305-2020-10-05 (CC thread)
  - https://forum.kerbalspaceprogram.com/topic/190382-15-110-kerbalism-311 (Kerbalism thread)
  - https://web.archive.org (Wayback Machine archives of forum threads)
  - https://spacedock.info (SpaceDock mod metadata)
  - https://github.com/KSP-CKAN (CKAN metadata)
  - https://www.reddit.com/r/RealSolarSystem/ (RSS/RO subreddit)
category: gaming
version: 1.0
last_updated: 2026-06-22
applies_when: "list_directory:GameData/RealismOverhaul"
---
# Realism Overhaul (RO) — KSP Mod Suite Knowledge Base

> **⚠ CONDITIONAL SKILL:** This knowledge base applies when Realism Overhaul is installed in the current KSP instance. To verify: check that `<KSP_ROOT>/GameData/RealismOverhaul/` exists as a directory. If not present, the information below is not relevant.

**Detection method:**
```bash
# Linux/macOS
ls -d <KSP_ROOT>/GameData/RealismOverhaul/ 2>/dev/null && echo "RO installed" || echo "RO NOT installed"

# Windows (PowerShell)
Test-Path "<KSP_ROOT>\GameData\RealismOverhaul" -PathType Container
```

---

> 📖 **Quick-reference companion:** All detail from the references below is also available as a single searchable document — [RO-COMPLETE-REFERENCE.md](references/RO-COMPLETE-REFERENCE.md) (92 KB, 1,615 lines). Use `Ctrl+F` to find anything across all 8 categories at once.

## 1. WHAT IS REALISM OVERHAUL?

**Realism Overhaul (RO)** is the flagship mod suite of the KSP-RO GitHub organization (68 repositories). It is a comprehensive "multipatch to KSP" that transforms Kerbal Space Program into a realistic spaceflight simulator by modifying parts and gameplay to match real-world rocket science, engineering, and orbital mechanics.

- **KSP Compatibility:** 1.12.x only (v18.0.0.0+)
- **License:** CC-BY-SA
- **Organization:** https://github.com/KSP-RO
- **Release Thread:** https://forum.kerbalspaceprogram.com/topic/155700-112-ksp-ro-realism-overhaul-16-may-2022/
- **Support Discord:** https://discord.gg/ZGbR6nv
- **CKAN:**
  - Meta-package: `RealismOverhaul`
  - Express install: RP-1 Express Install
  - Install: `ckan install RealismOverhaul`

RO does NOT add new parts — it applies **Module Manager patches** to existing parts (stock and mod-added) to make their performance, dimensions, masses, and capabilities match real-world counterparts.

### Key Changes from Stock KSP

- **Engines:** Real sizes, real thrust/Isp, limited ignitions, ullage simulation, real fuels
- **Pods/Capsules:** Full-size, real-weight equivalents
- **Propellant Tanks:** Correct dry mass ratios, multiple tank types with real-world vehicle examples
- **Solar Panels:** Lower power output, lighter mass, decay over time
- **Reaction Wheels:** Much weaker (realistic torque, no magic attitude control)
- **ElectricCharge:** Displayed in watts in tooltips and Solar Panel menu
- **Crew mass:** 100 kg each (person + suit)

> **Complete repo index:** [references/00-repo-index.md](references/00-repo-index.md)
>
> All 68 repositories sorted by category with stars, forks, issues, and descriptions.

---

## 2. INSTALLATION & DEPENDENCIES

### 2.1 Official Dependency Tree (from `RealismOverhaul.netkan`)

**Hard DEPENDS** (14 mods — CKAN installs automatically):

| # | Mod | Purpose |
|---|-----|---------|
| 1 | ModuleManager | Config patching engine |
| 2 | TweakScaleRescaled-Redist | Part scaling support |
| 3 | AdvancedJetEngine (AJE) | Realistic jet engine thermodynamics |
| 4 | FAR | Ferram Aerospace Research — realistic aerodynamics |
| 5 | KerbalJointReinforcementContinued | Physics joint stabilization (prevents "Kraken") |
| 6 | RealChute | Realistic parachute behavior |
| 7 | RealChuteForStock | RealChute configs for stock parachutes |
| 8 | RealFuels | Real propellants, tank types, engine performance |
| 9 | ROUtils | Base library for all RO mods |
| 10 | RealHeat | Real heating model (conductive/convective/radiative) |
| 11 | RealPlume | Realistic exhaust plume effects |
| 12 | SmokeScreen | Particle effects plugin for RealPlume |
| 13 | KSPCommunityFixes | Community bug-fix compilation |
| 14 | KerbalChangelog | In-game changelog display |

### 2.2 Install Locations

| Source | Target |
|--------|--------|
| `GameData/RealismOverhaul` | `KSP/GameData/` |
| `GameData/EngineGroupController` | `KSP/GameData/` |

EngineGroupController is unique to RO — bundled with the standard install.

### 2.3 CONFLICTS (Must NOT Install)

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

> **Full installation instructions, dependency tree, conflict map, and .netkan analysis:**
> See [references/01-core-mods.md](references/01-core-mods.md) (RealismOverhaul section — Installation & Dependencies).

---

## 3. REPOSITORY CATEGORIES & DOCUMENTATION MAP

### 3.1 Core / Foundational Mods
> [references/01-core-mods.md](references/01-core-mods.md)

- **RealismOverhaul** — Flagship multipatch (9,716 commits, 175 releases, CC-BY-SA)
- **RP-1** (Realistic Progression One) — Historical career mode with tech tree (v4.4.0.0)
- **RealSolarSystem (RSS)** — 1:1 scale real solar system via Kopernicus
- **RealFuels** — Real propellants, tank types, ullage, tech levels (128 releases)
- **ProceduralParts** — Procedurally generated tanks/SRBs, any size and shape

**Also includes:** System requirements, False KSP Lessons (10 myths debunked), changelog highlights (v10.3.0–v18.0.0), FAQ & troubleshooting, known incompatibilities table.

### 3.2 Engine & Propulsion
> [references/02-engines-propulsion.md](references/02-engines-propulsion.md)

- **SolverEngines** — Developer plugin enabling AJE-style engine solvers (42 releases, LGPL)
- **AJE** (Advanced Jet Engine) — Real thermodynamics via NASA EngineSim + JSBSim (v2.17.0)
- **ROEngines** — Accurate 3D models for 50+ liquid engines, 19 solid, 13 maneuvering
- **RealPlume** — SmokeScreen-based realistic exhaust plume effects (v13.3.2)
- **RealPlume-StockConfigs** — Plume config assignments for stock-sized engines

**Also includes:** Engine technical details (ullage, limited ignitions, throttle limits, pressure-fed vs pump-fed, H-1 TEATEB ignition), RealPlume particle system architecture, AJE Brayton cycle thermodynamics, SABRE specs (Mach 5.5 / 600–800 kN), SolverEngines API reference, common engine FAQ.

### 3.3 Parts, Tanks & Capsules
> [references/03-parts-tanks-capsules.md](references/03-parts-tanks-capsules.md)

- **ROTanks** — Modular SSTU-based tanks (Atlas, Titan, Centaur, Saturn parts)
- **ROCapsules** — 12 spacecraft families (Apollo, Gemini, Mercury, Orion, Starliner, etc.)
- **ROHeatshields** — Dynamically resizable heatshields
- **ProceduralFairings** — Auto-reshaping fairings with 10 pre-set historic shapes
- **ProceduralFairings-ForEverything** — Patcher replacing stock fairings
- **StagedAnimation** — Stage-triggered animation plugin

**Also includes:** Tank type mapping (cryogenic, ServiceModule, balloon, structural, spherical), B9PartSwitch fuel switching workflow, tech levels by era (1950s–1980s+), engine clustering with thrust plates, complete ROCapsules part inventory, ProceduralFairings auto-reshaping algorithm, FAQ.

### 3.4 Life Support & Reliability
> [references/04-lifesupport-reliability.md](references/04-lifesupport-reliability.md)

- **TacLifeSupport** — Food, water, O2, electricity; background processing since v0.13.2
- **TestFlight** — Part reliability system; 8 failure types; logarithmic reliability curves
- **ROKerbalism** — RO/RP-1 config pack; historical life support, ISRU chains, radiation
- **Kerbalism** — ⚠️ OUTDATED (v3.11, Dec 2019, KSP 1.5–1.10) — use ROKerbalism instead
- **DeadlyReentry** — ⚠️ INCOMPATIBLE with RO v18.x (listed in .netkan conflicts)

**Also includes:** TestFlight reliability mechanics (base reliability, flight data accumulation, persistent tracking), DeadlyReentry heat model (3 modes — but CONFLICT with RO), Kerbalism radiation system (cosmic background + solar flares + planetary belts), G-force formula, FAQ.

### 3.5 Visual & Environmental
> [references/05-visuals-environment.md](references/05-visuals-environment.md)

- **RSS-Textures** — High-resolution planetary textures (2048/4096/8192 resolution tiers)
- **RSSVE** — Visual enhancement pack (EVE clouds + Scatterer atmospheric scattering)
- **RSS-CanaveralHD** — HD terrain for Cape Canaveral / KSC
- **RealHeat** — Real heating model (hard DEPENDS of RO); 3 heat transfer modes
- **RealISRU** — Realistic in-situ resource utilization; 5 resource chain tables
- **ROSolar** — Realistic solar panels with decay
- **RSSTimeFormatter** — Time formatting for RSS date systems

**Also includes:** Texture resolution vs VRAM tables, EVE config details (cloud layers, city lights, aurora), RealHeat heat transfer mode comparison, RealISRU resource chains with Earth analogues, visual FAQ.

### 3.6 Historical Spacecraft & Launch Vehicles
> [references/06-historical-spacecraft.md](references/06-historical-spacecraft.md)

- **SovietRockets** — Soyuz/Proton/Zenit variants (7 Soyuz, 5 Proton, 3 Zenit)
- **SovietSpacecraft** — Vostok, Voskhod, Soyuz, TKS/VA
- **SovietProbes** — Long-term Soviet probe project
- **USProbesPack** — Pioneer, Mariner, Ranger, Surveyor probes
- **USRockets** — Older/obscure US launchers
- **Antares-Cygnus** — Antares launch vehicle + Cygnus cargo (5 Antares variants)
- **SalyutStations** — DOS/OPS-style space stations
- **Skylab** — Broken and unbroken Skylab (Saturn V compatible)
- **FASA-RO** — FASA Realism Overhaul Edition (Saturn V / Apollo / Gemini)
- **RNMisc** — Raidernick's miscellaneous mods
- **US-Soviet Solar Panels Pack** — Standalone solar panels

**Also includes:** Explorer 1 ascent procedure, Hydyne fuel formulation, complete spec tables (crew, mass, dimensions, duration) for each spacecraft, Saturn V stage specs, Apollo CSM/LM specs, Antares/Cygnus variant comparison, FAQ.

### 3.7 Utilities & Tools
> [references/07-utilities-tools.md](references/07-utilities-tools.md)

- **KerbalJointReinforcement-Continued** — ⚠️ Hard DEPENDS of RO; auto-strut, physics stabilization
- **KerbalKonstructs** — Static object placement; in-game editor broken on 1.12.x
- **KSCSwitcher** — Moves KSC to new launch sites (9 supported sites)
- **RealAntennas** — Communications via Friis equation + Shannon-Hartley theorem
- **ContractConfigurator** — Config-based contract creation (⚠️ potentially outdated: v1.30.5, Oct 2020)
- **LunarTransferPlanner** — Lunar transfer planning tool
- **KJR-Next** (related) — Complete rewrite of KJR (compiled for 1.12.3, no dependencies)
- **RemoteTech vs RealAntennas** — Either/or (both listed in .netkan recommends)

**Also includes:** RealAntennas link budget calculation, ContractConfigurator 12 parameter types, KSCSwitcher launch site coordinates, KJR-C auto-strut mechanics, "Kraken" glossary, mod dependency chain table.

### 3.8 Infrastructure & Build Tools
> [references/08-infrastructure-build.md](references/08-infrastructure-build.md)

- **BuildTools** — 10-step CI/CD pipeline (CKAN API, version stamping, changelog generation)
- **ROLibrary** — Shared code library for ROx family (PartModule bases, physics helpers)
- **ROUtils** — Lowest-level base library (logging, config helpers, extension methods)
- **CFGProjectGenerator** — Generates .csproj from .cfg files
- **RP1AnalyticsWebApp** — Player analytics ("Spyware v2.0")
- **RP-1-ExpressInstall** — CKAN metapackage for RO/RP-1
- **ECM-Viewer** — Browser-based engine config parameter viewer
- **GoForLaunch** — Collaborative RP-0 career repository

**Also includes:** Build pipeline sequence, ROLibrary API data flow diagram, RP-1 Express Install dependency list, build infrastructure dependency graph.

---

## 4. CRITICAL: DEADLYREENTRY CONFLICT

**DeadlyReentry is INCOMPATIBLE with current Realism Overhaul (RO v18.x).**

The RO `.netkan` master file explicitly lists `DeadlyReentry` in its `conflicts:` section. CKAN will refuse to install both simultaneously. RO v18.x uses **RealHeat** (a hard DEPENDS) for thermal modeling, making DRE redundant and conflicting.

- For reentry heating in current RO: use **RealHeat + ROHeatshields**
- DeadlyReentry is still maintained as a KSP-RO repo and may return to compatibility in future versions
- The DRE section in [references/04-lifesupport-reliability.md](references/04-lifesupport-reliability.md) is provided for **historical documentation only**

---

## 5. FALSE KSP LESSONS (v18.0.0)

Full reference in [references/01-core-mods.md](references/01-core-mods.md) (False KSP Lessons section). Key myths debunked:

| # | Myth | Fact |
|---|------|------|
| 1 | MOAR BOOSTERS is best | High TWR causes aerodynamic destruction, mass penalty, inability to throttle down |
| 2 | Asparagus staging is optimal | Adds dry mass, complexity, drag; serial staging is simpler and often better |
| 3 | Nuclear engines are always best | Low TWR, massive boil-off for interplanetary — hypergolics often better |
| 4 | You need a 10 km/s interplanetary stage | Realistic transfers with aerocapture need far less |
| 5 | SSTO spaceplanes are efficient | High dry mass; staged rockets are more efficient for payload delivery |
| 6 | Ion engines are endgame superiority | Extremely low TWR; requires very long burns and massive solar arrays |
| 7 | You always need Reaction Wheels | RCS is lighter for large craft; real reaction wheels are weak |
| 8 | Solar panels are always efficient | Solar panel decay reduces output over time; RTGs better for long missions |
| 9 | More engines = more better | Clustering adds dry mass; use procedural tanks and thrust plates instead |
| 10 | TWR < 1 is always fine | Insufficient TWR causes gravity losses; aim for TWR 1.2–1.5 on launch |

---

## 6. KEY TECHNICAL TERMS

| Term | Definition | See |
|------|-----------|-----|
| **Ullage** | Settling propellants to tank bottoms before engine ignition | [references/02-engines-propulsion.md](references/02-engines-propulsion.md) (§6 Engine Technical Details) |
| **Vernier** | Small gimballed engine for fine trajectory control | Same file (§Appendix: Verniers) |
| **Pressure-fed** | Engine where high-pressure gas forces propellant (requires ServiceModule tank) | Same file (§Appendix: Pressure-Fed) |
| **Balloon tank** | Thin-walled tank relying on internal pressure for structure (Centaur) | [references/03-parts-tanks-capsules.md](references/03-parts-tanks-capsules.md) (ROTanks section) |
| **TEATEB** | Triethylaluminium-triethylborane — pyrotechnic hypergolic igniter fluid | [references/02-engines-propulsion.md](references/02-engines-propulsion.md) (§Appendix: Engine Ignition) |
| **Hydyne** | 60% UDMH + 40% diethylenetriamine — high-energy fuel for Juno I | [references/06-historical-spacecraft.md](references/06-historical-spacecraft.md) (USProbesPack section) |
| **Boil-off** | Cryogenic propellant evaporation over time (especially LH2) | [references/01-core-mods.md](references/01-core-mods.md) (RealFuels section) |
| **ServiceModule tank** | Pressurized tank type required for pressure-fed engines (~200 atm helium) | [references/03-parts-tanks-capsules.md](references/03-parts-tanks-capsules.md) (ROTanks section) |
| **Kraken** | Community term for KSP physics instability causing spontaneous vessel destruction | [references/07-utilities-tools.md](references/07-utilities-tools.md) (§Glossary) |

---

## 7. SUPPORT & TROUBLESHOOTING

- **Discord:** https://discord.gg/ZGbR6nv
- **GitHub Issues:** https://github.com/KSP-RO/RealismOverhaul/issues
- **Release Thread:** https://forum.kerbalspaceprogram.com/topic/155700-112-ksp-ro-realism-overhaul-16-may-2022/
- **Support Policy:** No support without logs (KSP.log / output_log.txt) and reproduction steps

### Common Issues Quick Reference

| Issue | Solution | See |
|-------|----------|-----|
| Engine won't fire (pressure-fed) | Use ServiceModule tank type | [03](references/03-parts-tanks-capsules.md) FAQ |
| Engine turning off too early (MechJeb) | Set minimum acceleration limit > 0 | [01](references/01-core-mods.md) FAQ |
| Zero ignitions | Check EngineIgnitor tooltip; some engines have limited ignitions | [02](references/02-engines-propulsion.md) FAQ |
| LH2 boil-off during interplanetary | Switch to hypergolics; use exterior radiator fins + large power supply | [01](references/01-core-mods.md) RealFuels appendix |
| Parts exploding on launch | Reduce TWR; check RealHeat multiplier; add heat shields | [01](references/01-core-mods.md) FAQ |
| RCS burning up during reentry | Place RCS above fuselage for aerodynamic shielding | [04](references/04-lifesupport-reliability.md) DRE appendix |
| No communication link | Check antenna dish diameter/power; RealAntennas uses Friis equation | [07](references/07-utilities-tools.md) RealAntennas FAQ |
| Vessel spontaneously destroyed | Likely "Kraken" physics instability; ensure KJR-C is installed (hard DEPENDS) | [07](references/07-utilities-tools.md) KJR section |

---

## 8. CROSS-MOD INTEGRATION NOTES

### Either/Or Pairs (from .netkan)

| Choice A | Choice B | Purpose |
|----------|----------|---------|
| RemoteTech | RealAntennas | Communications |
| Kerbalism-Config-RO | TACLS | Life support |
| TestFlight | TestLite | Part reliability |
| VenStockRevamp | Restock | Stock visual overhaul |

### Deprecated / Superseded Mods

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

---

## 9. QUICK REFERENCE: REPO MAP BY CATEGORY

| Category | File | Repos Covered |
|----------|------|---------------|
| Core | [01-core-mods](references/01-core-mods.md) | RealismOverhaul, RP-1, RealSolarSystem, RealFuels, ProceduralParts |
| Engines/Propulsion | [02-engines-propulsion](references/02-engines-propulsion.md) | SolverEngines, AJE, ROEngines, RealPlume, RealPlume-StockConfigs |
| Parts/Tanks/Capsules | [03-parts-tanks-capsules](references/03-parts-tanks-capsules.md) | ROTanks, ROCapsules, ROHeatshields, ProceduralFairings, PFFE, ProceduralSolidsLibrary, StagedAnimation |
| Life Support/Reliability | [04-lifesupport-reliability](references/04-lifesupport-reliability.md) | TacLifeSupport, TestFlight, ROKerbalism, Kerbalism, DeadlyReentry |
| Visuals/Environment | [05-visuals-environment](references/05-visuals-environment.md) | RSS-Textures, RSSVE, RSS-CanaveralHD, RealHeat, RealISRU, ROSolar, RSSTimeFormatter |
| Historical Spacecraft | [06-historical-spacecraft](references/06-historical-spacecraft.md) | SovietRockets, SovietSpacecraft, SovietProbes, USProbesPack, USRockets, Antares-Cygnus, SalyutStations, Skylab, FASA-RO, RNMisc, ROStations |
| Utilities/Tools | [07-utilities-tools](references/07-utilities-tools.md) | KJR-C, KerbalKonstructs, KSCSwitcher, RealAntennas, ContractConfigurator, LunarTransferPlanner, CustomPreLaunchChecks, FlightSchool, KerbalRenamer, CanaveralPads |
| Infrastructure/Build | [08-infrastructure-build](references/08-infrastructure-build.md) | BuildTools, BuildLibs, CFGProjectGenerator, ROLibrary, ROUtils, ROLoadingImages, RP-1-ExpressInstall, RP1AnalyticsWebApp, ECM-Viewer, GoForLaunch |
| Index (all 68) | [00-repo-index](references/00-repo-index.md) | Every KSP-RO repo with stats |

---

*End of SKILL.md. See referenced files in `references/` for detailed documentation on each category.*
