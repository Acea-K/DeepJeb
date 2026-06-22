# Core & Foundational Mods

The five pillars of Realism Overhaul: RealismOverhaul, RP-1, RealSolarSystem, RealFuels, and ProceduralParts. Together they transform KSP into a realistic rocketry simulator.

---

## System Requirements

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

## 1. RealismOverhaul

- **Repository:** [KSP-RO/RealismOverhaul](https://github.com/KSP-RO/RealismOverhaul)
- **License:** CC-BY-SA
- **Forum:** [KSP Forums](https://forum.kerbalspaceprogram.com/index.php?/topic/155700-112-realism-overhaul-v1101-may-18/)
- **Discord:** https://discord.gg/V73jjNd

RO is the flagship mod — a multipatch that applies Module Manager configs to stock and mod parts, making their performance, dimensions, masses, and capabilities match real-world counterparts.

### Key Features

- Realistic engine thrust, Isp, mass, and dimensions
- Realistic tank sizing based on real rocket stages and material technologies
- Correct capsule masses, crew capacity, and avionics
- Thousands of Module Manager patches covering nearly every part
- Compatibility with FAR, RealFuels, and many popular mods
- `EngineGroupController` installed alongside RO (unique to RO)

### Dependencies (Hard DEPENDS from .netkan)

| Mod | Purpose |
|-----|---------|
| ModuleManager | Config patching framework |
| TweakScaleRescaled-Redist | Rescaling support |
| AdvancedJetEngine (AJE) | Realistic jet engine simulation |
| FAR (Ferram Aerospace Research) | Realistic aerodynamics |
| KerbalJointReinforcementContinued | Physics stabilizer |
| RealChute + RealChuteForStock | Realistic parachute simulation |
| RealFuels | Real propellant types, tank configs, ullage, ignition limits |
| ROUtils | Shared base library |
| RealHeat | Realistic thermal model |
| RealPlume + SmokeScreen | Realistic engine exhaust effects |
| KSPCommunityFixes | Bug fixes for KSP 1.12.x |
| KerbalChangelog | In-game changelog display |

RealSolarSystem, SolverEngines, and ProceduralParts are **recommends** (not hard-required).

### Installation

- **CKAN (Recommended):** Search "RealismOverhaul" — auto-resolves dependencies.
- **Manual:** Download release/nightly, extract to `KSP/GameData/`.
- **Nightly:** https://nightly.link/KSP-RO/RealismOverhaul/workflows/buildAndTest/master/RealismOverhaul.zip

Installs to `GameData/RealismOverhaul` and `GameData/EngineGroupController`.

---

## 2. RP-1 (Realistic Progression One)

- **Repository:** [KSP-RO/RP-1](https://github.com/KSP-RO/RP-1)
- **License:** CC-BY-NC-SA-4.0
- **Forum:** [KSP Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/190040-161-173-rp-1-realistic-progression-one-v12/)
- **Discord:** https://discord.gg/V73jjNd

RP-1 is a comprehensive career mode for Realism Overhaul, providing historically-inspired progression from early sounding rockets through the Apollo era and beyond.

### Key Features

- Historical tech tree and contracts following real spaceflight history
- Fully icon-rebuilt tech tree designed for RO/RP-1
- Difficulty levels: Easy/Normal (recommended for beginners), Moderate/Hard
- Historical part placement in the tech tree
- Minimal install requirements, broad mod compatibility

### Required Mods

RealismOverhaul, RealSolarSystem, RealFuels, ProceduralParts, SolverEngines, Community Resource Pack, Module Manager.

### Installation

1. **CKAN:** Search "RP-1" — handles all dependencies.
2. **Manual:** Download from [GitHub Releases](https://github.com/KSP-RO/RP-1/releases) or [Nightly](https://nightly.link/KSP-RO/RP-1/workflows/build/master/RP-1.zip), extract to `KSP/GameData/`.

### Community Gameplay Strategies

- **Early career:** Invest 100–150 points in VAB with level 4–5 pad **before** heavy science investment. ~7k funds/15 days on sounding rockets with 20 VAB points.
- **Orbital launches:** 20t pad orbital launches possible with RD-101 + 2×AJ10-27 + XASR stages but difficult. Recommended: 60t pad with proper orbital engines.
- **LR105 class engines** cannot be air-lit until tech tier 5 or 7.
- Mixed Soviet/US rocket combinations can yield useful early sustainer + booster combos.

> "Yes, you can turn the difficulty down and starting on easy is probably the right thing to do. Nevertheless I can promise you that RO/RP-1 is going to be difficult at first if you haven't played it before." — siimav

---

## 3. RealSolarSystem (RSS)

- **Repository:** [KSP-RO/RealSolarSystem](https://github.com/KSP-RO/RealSolarSystem)
- **License:** CC-BY-NC-SA
- **Author:** NathanKell
- **Forum:** [KSP Forums](https://forum.kerbalspaceprogram.com/index.php?/topic/177216-112-real-solar-system)

RSS replaces the Kerbol system with a 1:1 scale Solar System — Earth at 6,371 km radius, Sun at 1 AU, and all major bodies with correct sizes, masses, orbits, rotation periods, and axial tilts.

### Key Features

- All inner planets, outer planets, dwarf planets, and major moons with accurate orbital mechanics
- Custom launch sites via KSCSwitcher (Cape Canaveral, Baikonur, etc.)
- Biome maps for all planetary bodies
- Configurable texture resolutions: 2048, 4096, or 8192
- Powered by Kopernicus

### Dependencies

| Mod | Purpose |
|-----|---------|
| Kopernicus | Planet-modding framework |
| CustomBarnKit | Customizable building upgrades |
| KSCSwitcher | Multiple launch site support |
| Module Manager | Config patch framework |

### Texture Installation

Download RSS Textures from [ScaledRSS-Textures](https://github.com/KSP-RO/ScaledRSS-Textures) releases. Install to `GameData/RSS-Textures/`. Choose resolution based on VRAM.

> **KSP 1.4.x Incompatibility:** RSS (via Kopernicus) is NOT compatible with KSP 1.4.x. Use KSP 1.12.x.

---

## 4. RealFuels

- **Repository:** [KSP-RO/RealFuels](https://github.com/KSP-RO/RealFuels)
- **License:** CC-BY-SA
- **Author:** NathanKell
- **Forum:** [KSP Forums](https://forum.kerbalspaceprogram.com/index.php?/topic/58236-18-real-fuels)

RealFuels overhauls KSP's fuel and engine system: real-world propellants, realistic tank masses, and physically accurate engine performance. 1 unit = 1 liter at STP.

### Key Features

#### Fuel & Tank System

- Customizable tanks with exact fuel amounts; real-world dry masses
- Multiple real fuel types: RP-1, LH2, LCH4, hypergolics (NTO/UDMH, NTO/AZ50, MON/MMH), alcohol/LOx, solid fuels
- Seven tank types with different mass ratios and capabilities
- ModuleEnginesRF powered by SolverEngines

### Tank Types

| Tank Type | Description | Use Case |
|-----------|-------------|----------|
| Default | Regular construction, minimal insulation | Titan/Saturn stages |
| Structural | Heavier, reinforced | Aircraft, spaceplanes |
| ServiceModule | Pressure-fed capable (200 atm) | Service modules, RCS |
| Fuselage | Heavier ServiceModule variant | Aircraft, spaceplanes |
| Cryogenic | Highly insulated for cryo fuels | Cryogenic upper stages |
| Balloon | Thin-walled, very light, fragile | Atlas sustainer |
| BalloonCryo | Balloon + cryo insulation | Centaur upper stage |

### Fuel Mixtures

#### Chemical (Liquid)

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

#### Nuclear (Liquid)

| Fuel | Isp | Thrust | Notes |
|------|-----|--------|-------|
| Liquid Hydrogen | Highest | Lowest | Benchmark; worse density than hydrolox |
| Liquid Methane | Lower | Higher | Often better total delta-V than LH2 despite lower Isp |
| Liquid Ammonia | Lower | Moderate | Moderate density |
| LOx-Augmented NTR | 65–70% of LH2 mode | ~8× increase | Consumes LOx as reaction mass |

### Tech Levels (Engine Progression)

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

### Engine Type Classifications

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

### Ullage & Limited Ignitions

- **Ignitions:** Limited number of starts (or unlimited with `ignitions = -1`)
- **Ullage Check:** Propellant must be "Very Stable" or engine risks flameout
- **Pressure-Fed:** Requires ServiceModule tank type (200 atm)
- **Ignition Resources:** Consumes resources (e.g., ElectricCharge) per start

### GUI Usage

- **Tank GUI:** Shows wet/dry mass, volume; autoconfigure buttons for mixtures used by vessel engines; right-click for "remove all" / "configure for"
- **Engine/RCS GUI:** Buttons for configuration and tech level changes; stats for current config
- **RCS Note:** STACK_PRIORITY_SEARCH fuels need CrossFeedEnabler mod

### Dependencies

Module Manager, Community Resource Pack, SolverEngines.

### Installation

Delete old `ModularFuelTanks`, `RealFuels`, `CommunityResourcePack`, `SolverEngines` folders. Extract to `KSP/GameData`. Install an engine pack (Stockalike RF Configs or RealismOverhaul).

---

## 5. ProceduralParts

- **Repository:** [KSP-RO/ProceduralParts](https://github.com/KSP-RO/ProceduralParts)
- **License:** CC-BY-SA 3.0 Unported
- **Forum:** [KSP Forums](https://forum.kerbalspaceprogram.com/index.php?/topic/204080-18-procedural-parts)

Procedurally generate rocket parts in any size and shape — the spiritual successor to StretchyTanks/StretchySRBs.

### Key Features

- Procedural tanks, SRBs, and other parts at any size
- Customizable shape (diameter, length, profile)
- Multiple texture options and surface finishes
- Full RealFuels and MFT compatibility (RF v6.1+, MFT 5.0.1+)
- Tank types for RealFuels, Kethane, EPL, TAC Life Support
- Procedural heat shields (Deadly Reentry integration)
- FAR drag models auto-update
- SRB support with low/high-altitude switching
- Tech-limited sizing (customizable via Module Manager)

### Tech Limits Customization

```cfg
@PART[proceduralTank*]
{
    @MODULE[ProceduralPart]
    {
        @TECHLIMIT,* {
            @lengthMax *= 3
            @volumeMax *= 3
            @diameterMax *= 2
        }
    }
}
```

For RealFuels SRB, target `proceduralSRBRealFuels`.

### Known Issues

| Issue | Resolution |
|-------|-----------|
| Stock decouplers missing with ProceduralParts installed | SmartTanks conflict — remove or disable SmartTanks |
| Limited part dimensions | Enable all part upgrades in sandbox via Difficulty Settings |

---

## False KSP Lessons

> *"A lot you will need to unlearn."* — Moving from Kerbin to Earth.

### Myth 1: MOAR BOOSTERS is a pilot's best friend

High TWR has major downsides: risk of aerodynamic destruction, massive mass penalty from heavy engines, inability to throttle down. EELV upper stages burn 10–20 minutes at TWR < 0.2. Optimal LEO: small 30s solids, 3-minute core, 6-minute upper stage. **Delta-V is a function of mass ratio. Lighter engines = lighter stages = higher mass ratio.**

### Myth 2: Rocket engines all throttle down to 0.1%

Very few engines throttle. Deep throttling (landing): LMDE got to ~10% max. Shallow throttling (reduce G-loads): RS-25 throttles to ~70%. Most engines don't throttle at all; G-loads managed by shutting down engines early.

### Myth 3: Rockets must throttle down due to air resistance

Real rockets throttle at Max Q for structural loading, not efficiency. Optimal flight profile is **"bang-bang control"** (100% throttle or 0% coasting). Post-1.00 KSP LKO delta-V: ~3,300 m/s.

### Myth 4: Rocket engines are infinitely restartable

Restarting is complex and limited. The **ullage problem**: propellants float away from feed lines in freefall. Solutions: ullage motors, RCS burns to settle propellants. Most first-stage engines: 1 ignition. Pressure-fed hypergolic engines: effectively infinite.

### Myth 5: Burn up to get apoapsis, then burn right to circularize

Most real LVs perform a single continuous burn interrupted only by staging. Many burn **well after apogee** (Saturn V, hydrolox upper stages). The "apogee" becomes the **perigee** of your parking orbit. Gravity turn takes ~3 minutes of a 7–12+ minute ascent.

### Myth 6: All propellants are created equal

| Propellant | Density | Vacuum Isp | Notes |
|------------|---------|------------|-------|
| Kerolox (RP-1/LOX) | ~1 kg/L | ~350s | High density |
| Hydrolox (LH2/LOX) | 1/2.84 of kerolox | ~460s | Low density, high Isp |
| Hypergolic storables | Up to 2+ kg/L | Much lower | Room-temp storable, no boil-off |

NTR: Uses LH2 (0.07085 kg/L), up to 1000s Isp; can also use ammonia, methane, water.

### Myth 7: Rocket engines and fuel tanks are heavy

Real engines have very high TWR (maxing over 150:1). Atlas D sustainer: loaded 113 tonnes, dry 2.347 tonnes. In KSP, only liquid engines/tanks underperform (too heavy by factor of 3–8).

### Myth 8: Reaction wheels are magical all-powerful devices

Attitude control uses gimbaled thrust and RCS. Reaction wheels have limited torque, especially under thrust, and can only apply torque temporarily (spin up → need RCS to spin down). Used for fine, low-torque applications (ISS, telescopes).

### Myth 9: Orbital rendezvous is easy from any starting orbit

Real rendezvous requires waiting for launch windows, launching into the same plane (with small "dogleg" corrections), placing chaser in slightly lower/behind orbit, and maintaining low-velocity approaches.

### Myth 10: A heatshield is enough for any reentry; shallower is better

LEO-rated shields won't survive translunar/interplanetary reentry. Shallower reentries: lower peak heat flux, higher total heat load. Steeper reentries (Mercury negative perigee): high peak heating, low total load, 9G. Lifting reentries (STS): much higher total load, lower peak flux.

**Heat shield types:** Ablative (Apollo, Dragon, Orion), TPS tiles (Space Shuttle), PICA (Stardust, Dragon).

---

## Dependency Tree (KSP 1.12.x)

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

| Mod | Category | Description |
|-----|----------|-------------|
| Module Manager | Core framework | Patch-loading framework |
| Kopernicus | Planet framework | Planet modification via RSS |
| ModularFlightIntegrator | Flight integration | Use version from Kopernicus |
| CommunityResourcePack | Resources | Standardized resource definitions |
| SolverEngines | Engine simulation | ModuleEnginesRF physics |
| RealFuels | Fuels | Real propellants, ullage, ignitions |
| RealSolarSystem | Planets | 1:1 scale solar system |
| RealismOverhaul | Core | Realistic stats/sizes |
| ProceduralParts | Parts | Procedural fuel tanks |
| AJE | Engines | Realistic jet engines |
| FAR | Aerodynamics | Realistic aerodynamics |
| KJR | Physics | Structural integrity |
| RealChute | Parachutes | Realistic parachutes |
| RealHeat | Thermal | Realistic thermal mechanics |
| RealPlume | Visuals | Realistic exhaust effects |
| SmokeScreen | Visuals | Extended FX plugin |
| ROUtils | Library | Shared code library |
| KSPCommunityFixes | Patches | KSP bug fixes |
| EVE | Visuals | Environmental visual enhancements |
| Scatterer | Visuals | Atmospheric scattering |
| RSS Textures | Textures | 2048/4096/8192 resolution |
| RSSVE | Visuals | RSS visual enhancement pack |
| 000_Harmony | Framework | Auto-installed |
| 000_KSPBurst | Framework | Auto-installed |
| EngineGroupController | Utility | Auto-installed |

---

## Recommended Additional Mods

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

## Mods to Avoid

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
| Sol/RSS-Reborn | Not officially supported |
| Volumetric clouds (Blackrack) | Not available in RSS |
| VisibleMapsActive | Should be avoided |

> **General rule:** Assume any mod not explicitly marked as RSS/RO/RP-1 compatible is incompatible.

---

## FAQ & Troubleshooting

### Installation Issues

**CKAN won't let me install RO?** Start with clean KSP 1.12.x install (build.txt version 1024+). Check for conflicting mods.

**CKAN Express Install conflicts (RP-1)?** Known: Tweakscale Rescaled Redistributable v3.2.2 vs v2.4.8.6; Stockalike RF Configs conflicts. Check version compatibilities.

**Manual installation tips?** Delete old RealismOverhaul folder first. Use ONLY the ModularFlightIntegrator from Kopernicus.

### Gameplay Troubleshooting

**Engines turning off too early?** Two common causes:
1. TestFlight may simulate reliability failures (more flights = more reliable)
2. MechJeb "Limit Acceleration" minimum set to 0 may throttle real engines to 0

**Engines won't light or flameout quickly?** Check in order:
1. Ullage simulation — propellants not settled
2. Electric charge — spark required for ignition
3. Tank type — verify correct type (ServiceModule for pressure-fed)
4. Ignition count — check remaining ignitions in RealFuels engine config
5. Engine config — verify fuel type matching

**Zero ignitions remaining?** Edit RealFuels config files, find `ignitions = X`, modify or remove.

**How to deal with ullage in upper stages?** Hot-staging for ascent; ullage motors or small RCS burns for orbital maneuvers.

**Crew mass too high?** Each crew member = 100 kg (person + suit). This is realistic.

**Venus entry tips?** Lunar-rated heat shields survive Venus entry from hyperbolic trajectories. Set periapsis >100 km for safer entry. Multiple aerobraking passes possible. Very thick atmosphere — may not need parachutes.

### Support Policy

> **No support without logs and reproduction steps.**

- **Discord:** https://discord.gg/ZGbR6nv or https://discord.gg/V73jjNd
- Provide KSP.log / output_log.txt and clear reproduction steps
- Check GitHub issues before posting

---

## LH2 Boil-Off

Liquid Hydrogen boil-off requires active management:
- Use **exterior radiator fin** parts mounted on cryogenic tanks
- Refrigeration draws significant **ElectricCharge** — a major power drain
- For interplanetary trips, use **room-temperature hypergolics** instead; LH2 impractical beyond days/weeks
- LH2 tanks must use **Cryogenic** or **BalloonCryo** tank type

## Mass Units

All masses in Realism Overhaul are in **metric tons** (tonnes). 1 RO tonne = 1,000 kg. Applies to part masses, engine masses, tank dry masses, and payload masses.

---

See also: [00-repo-index.md](00-repo-index.md), [02-engines-propulsion.md](02-engines-propulsion.md)
