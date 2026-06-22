# Engine & Propulsion Mods

Core engine and propulsion mods in the KSP-RO ecosystem: SolverEngines (simulation framework), AJE (jet engines), ROEngines (3D models), RealPlume and RealPlume-StockConfigs (visual effects).

---

## SolverEngines

- **Repository:** [KSP-RO/SolverEngines](https://github.com/KSP-RO/SolverEngines)
- **Authors:** NathanKell and blowfish (based on camlost's AJE work)
- **License:** LGPL
- **Releases:** 42

SolverEngines is a developer-oriented plugin that splits KSP's engine system into two components: an engine module (KSP interaction) and an engine solver (physics calculations). **It is not an end-user mod** — do not install it unless another mod requires it.

### Core Architecture

| Component | Description |
|-----------|-------------|
| `ModuleEnginesSolver` | Engine module (derives from `ModuleEnginesFX`) |
| `EngineSolver` | Physics solver (calculates thrust, Isp, fuel flow) |
| Replacement intake module | Derives from `ModuleResourceIntake` |
| Flight/engine stats GUI | Real-time engine telemetry display |
| `ModuleAnimateEmissive` | Replaces `ModuleAnimateHeat` for emissive heat animation |

### Key Features

- **THRUST_TRANSFORM Support (v3.5+):** Named thrust transforms with optional `overallMultiplier` and per-transform `multiplier` entries
- **Auto Overheat Bar:** Creates overheat bars when `engineTemp` reaches `maxEngineTemp`
- **Configurable GUI:** Displays engine telemetry; special panels for air-breathing engines; toolbar integration (blizzy78 or stock)
- **Customizable Units:** All GUI fields support unit changes

### Reference Implementations

- **AJE** — Air-breathing jet engine solver
- **RealFuels** — `ModuleEnginesRF` for liquid rocket engines

### For Developers

```csharp
[assembly: KSPAssemblyDependency("SolverEngines", 1, 0)]
```

Subclass `EngineSolver` (thrust, Isp, fuel flow), subclass `ModuleEnginesSolver` (override `CreateEngine()`), override optional hooks (`UpdateFlightConditions`, `UpdateThrottle`, `OnStart`, `OnLoad`, `UpdatePropellantStatus`).

### Changelog Highlights

| Version | Key Changes |
|---------|-------------|
| v3.14.0 | Added virtual `UpdatePropellantStatus` |
| v3.12.0 | Fixed kg vs tons compatibility for `fuelFlowGui` |
| v3.10.0 | Uses own `massFlowGui` field (Waterfall compatibility) |
| v3.5 | `THRUST_TRANSFORM` nodes support |
| v3.0 | **Breaking:** `EngineThermodynamics` refactored to struct |

---

## AJE (Advanced Jet Engine)

- **Repository:** [KSP-RO/AJE](https://github.com/KSP-RO/AJE)
- **License:** LGPL v2
- **Releases:** 22
- **Forum:** [KSP Forum](https://forum.kerbalspaceprogram.com/topic/139868-18-111-advanced-jet-engine-v2170-june-26)

AJE replaces KSP's simplified jet engine model with a physics-based simulation incorporating real thermodynamic cycles, compressor/turbine performance, and atmospheric modeling. Built on SolverEngines.

### Key Features

- Simulates the full Brayton cycle: inlet → compressor → combustor → turbine → (afterburner) → nozzle
- Supports turbojets, turbofans, turboprops, ramjets, and scramjets
- Real-world engine configs for historical and modern aircraft engines
- Incorporates code from **NASA EngineSim** and **JSBSim** (LGPLv2)

### Thermodynamic Parameters Modeled

Compressor pressure ratio and efficiency, turbine inlet temperature (TIT), bypass ratio (turbofans), inlet recovery factor, nozzle expansion ratio, mechanical losses.

### SABRE / LAPCAT Engine Details

| Parameter | Value |
|-----------|-------|
| Sea level thrust | 600–800 kN per engine (per REL docs, NOT the ~1960 kN Wikipedia figure) |
| TWR | ~0.6 per REL documentation |
| Air-breathing speed limit | ~Mach 5.5 (~1.7 km/s) before serious overheating |
| Precoolers | **Cosmetic/non-functional** — community workaround: add `thermalMassModifier = 5` and `skinMassPerArea = 2` |

**Known issue:** Some RP-1 users report problems with newer AJE versions; downgrading to v2.10.0 fixes it. SABRE's Isp in jet mode may be too low.

### Dependencies

- Module Manager
- SolverEngines (bundled with AJE)

---

## ROEngines

- **Repository:** [KSP-RO/ROEngines](https://github.com/KSP-RO/ROEngines)
- **Releases:** 45

High-quality, accurate 3D engine models replacing generic Squad engines for RO. Solves the historical problem of duplicate engine versions across multiple mods.

### Key Features

- **50+ liquid engines** covering American, Soviet/Russian, European, and other programs
- **19 solid engines** including boosters, upper stage solids, and separation motors
- **13 small maneuvering/landing engines** (RCS thrusters, apogee motors, landing engines)
- **Launch clamps** from FASA (by Frizzank)
- **Patch Manager integration** — choose which engine variants to keep or hide via the KSP UI

### Patch Manager Usage

1. Space Center → Patch Manager icon
2. Click ROEngines button
3. **Active (green):** Only ROEngines version shows; duplicates hidden
4. **Disabled (red):** Allow duplicate engines to appear
5. Click "Apply All" → restart KSP

### Liquid Engines (Partial List)

| Engine | Source | |
|--------|--------|---|
| A-4, Aerobee, Agena | RealEngines, Taerobee, BDB | |
| AJ10 variants (137, 190, Early/Mid/Advanced) | BDB, SSTU | |
| BE-3, BE-4 | NicheParts, Provenance Aerospace | |
| C-1, E-1, F-1, F-1B, G-1 | BDB, Astral Manufactures | |
| Gamma-2/8/301, HM7 | CRE, Astral Manufactures | |
| H-1C/D, J-2, J-2S, J-2T, J-2X | BDB, SSTU | |
| LE-5, LE-7 | Forgotten Real Engines | |
| LMAE, LMDE | RealEngines | |
| LR101, LR105, LR79, LR87/LR91/LR89 variants | BDB | |
| M-1, MB-35, MB-60, Merlin Family | BDB, SSTU | |
| NERVA, NERVA II, XE Prime | BDB | |
| NK-33, NK-43, RD series (0105-301) | RealEngines/Soviet Rockets | |
| RL10 variants, RS-68, RS-25 SSME | BDB/RMM | |
| Rutherford, RZ.20, RZ Series | NicheParts, CRE/BDB | |
| Viking-2/4/5, XLR10/11/25/41/43/99 | Forgotten Real Engines, Various | |

### Solid Engines (19)

Aerobee Aerojet, Agena Retro/Spin, AJ60, AJ-260 SL/FL, Alcyone-1, Algol 1/2, Altair I/II/III, Antares 1/2, Baby Sergeant, Castor 1/2/4/4AXL/30/30XL/120, GEM40/46/60/63, M55, Nike-M5E1, SR19, SR73, SRMU, R-103, Star-5D/8/13/17/37/48B, UA-1200 Series.

### Dependencies

- Module Manager
- Realism Overhaul
- B9PartSwitch
- ROLibrary (bundled)
- Patch Manager (bundled)

---

## RealPlume

- **Repository:** [KSP-RO/RealPlume](https://github.com/KSP-RO/RealPlume)
- **Authors:** Zorg, Blowfish, Felger
- **License:** CC-BY-NC-SA
- **Releases:** 30
- **Forum:** [KSP Forum](https://forum.kerbalspaceprogram.com/topic/188033-ksp112x-realplume-stock-v408-realplume-v1332-25jun2021)

RealPlume replaces KSP's stock engine exhaust with realistic expanding plumes. Built on SmokeScreen (by Sarbian).

### Three-Layer Architecture

1. **SmokeScreen** — Replaces stock particle system with GPU-driven shuriken particles enabling expanding plume behavior
2. **RealPlume** — Particle models, textures, sound FX libraries, and prefabricated plume templates
3. **RealPlume-StockConfigs** — Module Manager patches assigning prefabs to specific engines

### Key Features

- Plumes visually expand as atmospheric pressure drops (3–5× wider in vacuum)
- Pre-tuned generic plume prefabs attachable with a single config node
- Engine-type specific profiles: kerolox (yellow-orange, sooty), hydrolox (pale blue-violet, semi-transparent), hypergolic (yellow-white), solid (bright white with smoke), nuclear (violet-blue, faint)
- Waterfall compatibility (RealPlume-Stock v4.0.8+ auto-detects and removes RealPlume configs when Waterfall plume present)

### New Plume Prefabs (v13.3.0)

17 new prefabs from Nertea and JadeOfMaar:

| Category | New Prefabs |
|----------|-------------|
| Chemical | HTP, Pentaborane, Kerolox Lower Aspirated, Kerolox SL Film Cooled, Methalox airbreathing (RAPIER), Aerospike, Aerozine50 vacuum, Alcolox |
| Cryogenic/Hydrolox | Sea level plumes fade red→blue; additional shock cones on Cryogenic_LowerSSME_CE |
| Nuclear | LH2 solid core, LH2+LOX augmented, Closed/open cycle gas core |
| Electric (WIP) | VASIMIR Xenon/Argon, MPDT (lithium) |
| Sound | 2 new sounds from Beale; wav→ogg conversion (halved mod size) |

### Dependencies

| Mod | Type |
|-----|------|
| Module Manager | Required |
| SmokeScreen | Required |
| RealPlumeConfigs | Required (engine-specific assignments) |
| EngineLighting | Recommended |

**Conflicts:** HotRockets (incompatible).

---

## RealPlume-StockConfigs

- **Repository:** [KSP-RO/RealPlume-StockConfigs](https://github.com/KSP-RO/RealPlume-StockConfigs)
- **License:** CC-BY-NC-SA
- **Releases:** 54

Module Manager configs that assign RealPlume exhaust effects to all stock KSP engines and many popular mod engines. Bridges the gap between RealPlume's effect library and actual in-game parts.

### Dependencies

- RealPlume (required)
- SmokeScreen (required, via RealPlume)
- Module Manager (required)

### Relationship

```
RealPlume (Core) → plume effect templates
    ↓
RealPlume-StockConfigs → assigns templates to engines
    ↓
In-game engines → render realistic exhaust plumes
```

---

## Dependency Overview

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

**Key:** SolverEngines is the simulation framework used by AJE and RealFuels. ROEngines provides visual models. RealPlume + RealPlume-StockConfigs provide visual exhaust effects.

---

## Engine Technical Details

### Ullage Simulation

Propellants require **ullage** — acceleration to settle liquids before engine ignition in microgravity.

- **Required for:** All pump-fed liquid engines in microgravity
- **Not required for:** Pressure-fed hypergolic engines (RCS thrusters)
- **Methods:** Hot-staging, ullage motors, RCS burns, spin-stabilization

### Limited Ignitions

| Engine Type | Typical Ignitions | Examples |
|-------------|-------------------|----------|
| First-stage pump-fed | 1 | RD-107, F-1, LR-79 |
| Upper stage pump-fed | 1–3 | RL10 (multiple), J-2 (2), Kestrel |
| Pressure-fed hypergolic | Unlimited | R-4D, LEROS, SuperDraco |
| Solid motors | 1 (cannot restart) | Castors, GEMs, Alcyone |

### Throttle Limits

Very few engines throttle significantly:

| Engine | Throttle Range | Notes |
|--------|---------------|-------|
| LMDE | ~10%–100% | Deepest throttling liquid engine |
| RS-25 (SSME) | ~70%–109% | Throttled to reduce G-loads |
| SuperDraco | ~20%–100% | Deep-throttling hypergolic |
| BE-3 | ~18%–100% | Deep-throttling hydrolox |
| Most pump-fed | Fixed (100%) | Cannot throttle at all |

Real rockets manage G-loads by shutting engines down early, not throttling.

### Pressure-Fed vs Pump-Fed

| Type | Fuel Delivery | Tank Requirement | Examples |
|------|--------------|------------------|----------|
| Pressure-fed | High-pressure gas forces fuel in | ServiceModule or Fuselage tank | TRW LM Descent Engine |
| Pump-fed | Turbopump delivers fuel | Standard tank types | RL-10, RD-180, RS-68 |

**RL-10 note:** Despite being used on Centaur, RL-10 is **NOT** pressure-fed — it uses a turbopump.

---

## Engine Ignition Requirements

**H-1 engine example:** Requires TEATEB (pyrotechnic igniter fluid), ElectricCharge, Kerosene, and LqdOxygen. Limited ignitions.

**General rule:** Hover over an engine in the VAB — the tooltip (provided by RealFuels) shows ignition requirements (blue text), remaining ignitions, and whether pressure-fed.

---

## Common Engine Issues

### Engines Won't Light / Flameout Quickly

Check in order:
1. **Ullage failure** — propellants not settled (most common upper-stage cause)
2. **No electric charge** — ignition requires spark
3. **Zero ignitions remaining** — engine has used all available starts
4. **Incorrect tank type** — must be ServiceModule for pressure-fed engines
5. **RealFuels configuration** — verify engine config allows current conditions

### Zero Ignitions Remaining

Each engine has a finite number defined in RealFuels configs. To disable: edit config files, find `ignitions = X`, change or remove.

### Engines Turning Off During Flight

- **TestFlight:** May simulate reliability failures (more flights = more reliable)
- **MechJeb:** "Limit Acceleration" minimum at 0 can throttle engines to 0%
- **Propellant feed:** Check tank crossfeed configuration
- **Throttle limits:** Verify you're not exceeding engine limits

---

## False KSP Lessons (Engine-Related)

| Stock KSP Myth | RO Reality |
|----------------|------------|
| All engines throttle to 0.1% | Very few engines throttle at all |
| Infinite restarts | Most engines have 1–3 ignitions max |
| Moar Boosters always helps | Higher TWR = heavier engines = less delta-v |
| All propellants are equal | Kerolox, hydrolox, hypergolics differ dramatically |

---

## Verniers & Ullage Motors

**Verniers:** Small engines with significant gimbal for attitude control during/after main engine burn. Example: Atlas sustainer (350 kN main + 2× 5 kN verniers).

**Ullage motors:** Liquid or solid motors firing briefly before main ignition to settle propellants. RCS can substitute if fed by propellant bladders.

**Engine clustering:** For 2–6 MN thrust range, use Procedural Fairings thrust plate with multiple nodes. Available engines: RD-191 (2 MN), RD-180 (4 MN), RS-68 (3.4 MN).

---

See also: [00-repo-index.md](00-repo-index.md), [01-core-mods.md](01-core-mods.md)
