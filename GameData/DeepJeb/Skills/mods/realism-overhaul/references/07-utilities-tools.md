# Utilities & Tools

Reference for utility mods, tools, and gameplay-enhancing plugins in the Realism Overhaul ecosystem.

> **KJR-Continued is a DEPENDS of RO v18.x.** Listed as a hard dependency in the official RO `.netkan`. CKAN installs it automatically — do not remove from a working RO installation.

> **Communications: RemoteTech vs RealAntennas (either/or).** The RO `.netkan` recommends one of RemoteTech or RealAntennas. They serve the same purpose (realistic antenna/link simulation) and should not both be installed. RealAntennas is the modern, RO-recommended option.

---

## Kerbal Joint Reinforcement Continued (KJR-C)

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/Kerbal-Joint-Reinforcement-Continued](https://github.com/KSP-RO/Kerbal-Joint-Reinforcement-Continued) |
| **Description** | Physics stabilizer — community continuation of ferram4's original |
| **Maintainer** | KSP-RO team |

Prevents Kraken (physics instability) by reinforcing vessel joints during high-stress events: loading, decoupling, launch. Essential for large/complex craft in RO/RP-1.

### Features

- **Physics easing** — slowly dials up external forces on planetary surfaces, reduces loading stress
- **Launch clamp easing** — prevents launch clamp shift on load
- **Interstage connection reinforcement** — parts connected to decouplers are connected to each other
- **Connection stiffness** — larger parts get stiffer; sequential stack parts get extra anti-wobble connection
- **World-space joints** — heaviest parts connect directly to ground when on pad with launch clamps

### Configuration (config.xml)

**General values:**

| Parameter | Default | Description |
|---|---|---|
| `reinforceAttachNodes` | 1 | Toggle all joint stiffening |
| `multiPartAttachNodeReinforcement` | 1 | Extra stiffening for stack parts |
| `reinforceDecouplersFurther` | 1 | Interstage connection stiffening |
| `reinforceLaunchClampsFurther` | 1 | Launch clamp stiffening |
| `worldSpaceJoints` | 1 | Ground-connected heaviest parts on pad |
| `useVolumeNotArea` | 1 | Use volume not area for connection strength |
| `debug` | 0 | Debug output toggle |
| `massForAdjustment` | 0.01 | Parts below this mass are not stiffened |

**Strength values:**

| Parameter | Default | Description |
|---|---|---|
| `breakForceMultiplier` | 1 | Scale failure strength for forces |
| `breakTorqueMultiplier` | 1 | Scale failure strength for torque |
| `breakStrengthPerArea` | 1500 | Strength = area × this value |
| `breakTorquePerMOI` | 6000 | Torque strength based on moment of inertia |

### Related: KJR-Next (KJRn)

Complete rewrite of KJR by Rudolf Meier. Alternative to KJR-Continued. Fully automatic, compatible with DLC robotic parts and Infernal Robotics Next. Compiled for KSP 1.12.3 (works 1.4+). Better performance — no stuttering/freezes. Intelligently reinforces joints rather than building struts everywhere. Available on CKAN. [Forum thread](https://forum.kerbalspaceprogram.com/topic/184206-kerbal-joint-reinforcement-next).

### Known Issues

- Overconstrained rockets (too many launch clamps) may spontaneously disintegrate from phantom forces
- Certain PartModule types are exempt from stiffening (pWings, KerbalEVA, procedural wings, etc.)

---

## Kerbal Konstructs

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/Kerbal-Konstructs](https://github.com/KSP-RO/Kerbal-Konstructs) |
| **Description** | Static object placement (launch sites, air bases, fuel stations) with in-game editor |
| **License** | MIT (plugin), Custom (assets) |
| **Maintainer** | KSP-RO team (NathanKell) |
| **Commits** | 814 |
| **Releases** | 17 |

Spiritual successor to KerbTown. Place static objects on any celestial body. Modders and players can add buildings, launch pads, runways, landing guides. KSP-RO fork abandons KerbTown compatibility for improved performance and features.

### Features

- Static object placement on any celestial body
- In-game base editor (create/modify bases)
- Interactive objects — fuel stations, air-race checkpoints
- Dependency for many RO launch site mods (CanaveralPads, etc.)

### Known Issues

- **In-game editor broken on KSP 1.12.x** — editor for creating/modifying statics non-functional. Existing statics display and facilities work. ([GitHub Issue #2](https://github.com/KSP-RO/Kerbal-Konstructs/issues/2))
- Instance editor not showing on KSP 1.11.1
- KK not appearing — usually missing CustomPreLaunchChecks dependency
- Remove MiniAVC v1.0.3.2 if experiencing problems

### Installation

- **Recommended:** CKAN (handles dependencies automatically)
- **Manual:** Download from [GitHub Releases](https://github.com/KSP-RO/Kerbal-Konstructs/releases), merge `GameData/KerbalKonstructs/`
- **Requires:** [CustomPreLaunchChecks](https://github.com/KSP-RO/CustomPreLaunchChecks/) — without it, KK will not function

**Compatible mods (KSP 1.12.x):** Kerbin Side Remastered, KSC Harbor, KSC Extended, Water Launch Sites.

[Forum thread](https://forum.kerbalspaceprogram.com/topic/204210-ksp-18-kerbal-konstructs-continued)

---

## KSCSwitcher

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/KSCSwitcher](https://github.com/KSP-RO/KSCSwitcher) |
| **Description** | Moves KSC to a new location on Earth |
| **Commits** | 63 |
| **Releases** | 13 |

Relocates the Kerbal Space Center to any Earth coordinate. Essential for RO/RP-1 historical launches (Cape Canaveral, Baikonur, Kourou, etc.). Primarily designed for RSS/RO. Listed as recommended in the official RO manual install guide. Integrates with Kerbal Konstructs for physical launch pad placement.

### Launch Sites (via coordinate configuration)

| Site | Latitude | Notes |
|------|----------|-------|
| **Cape Canaveral, FL** | 28.5°N | Kennedy Space Center / CCSFS |
| **Baikonur Cosmodrome, Kazakhstan** | 45.9°N | Russian/Soviet primary site |
| **Kourou, French Guiana** | 5.2°N | European Spaceport (optimal equatorial/GTO) |
| **Vandenberg SFB, CA** | 34.7°N | Polar orbit launches |
| **Plesetsk Cosmodrome, Russia** | 62.8°N | High-inclination launches |
| **Wallops Flight Facility, VA** | 37.9°N | NASA suborbital/small orbital |
| **Tanegashima Space Center, Japan** | 30.4°N | JAXA launches |
| **Satish Dhawan Space Centre, India** | 13.7°N | ISRO launches |
| **Jiuquan Satellite Launch Center, China** | 40.9°N | CNSA launches |

Compatible with CanaveralPads, Tundra Space Center, and other KK-based launch site mods. Settings via ModuleManager patches or in-game UI. Works with sandbox RO (does not require RP-1). Multiple facilities configurable simultaneously.

---

## RealAntennas

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/RealAntennas](https://github.com/KSP-RO/RealAntennas) |
| **Description** | Physics-based RF communications model replacing stock CommNet range system |
| **Commits** | 403 |
| **Releases** | 14 |

Replaces KSP's stock CommNet with realistic link budget calculations. Implements proper antenna gain, transmit power, RF frequency/bandwidth, receiver noise modeling, variable data rates, and multiple modulation schemes.

### Core Model

```
RxPower = TxPower + TxGain - FreeSpacePathLoss + RxGain
SNR = RxPower - Rx_NoiseFloor
```

### Friis Transmission Equation

Free-space path loss: `FSPL(dB) = 20 × log10(distance) + 20 × log10(frequency) + 32.44`

### Shannon-Hartley Theorem

Achievable data rate: `C = B × log2(1 + SNR)` where B = bandwidth.

Noise floor: `NoiseFloor(dBm) = 10 × log10(k × T × B) + 30` (k = Boltzmann's constant, T = system noise temperature).

### Dish Mechanics

- Dish diameter directly affects gain — larger = stronger signals at longer distances
- Pointing accuracy matters — off-axis alignment reduces effective gain
- Beam width narrows with frequency and dish size — high-gain alignment more critical

### Key Features

- **Multiple antennas per node** — each configurable for a different band
- **Asymmetric links** — data rate in one direction can differ significantly from the other
- **No antenna combining** — at most one link between any node pair based on best individual antenna pair in each direction
- Multiple modulation schemes: BPSK/QPSK/8PSK/QAM varieties
- Band-specific behavior: higher frequencies (Ka-band) offer more bandwidth but greater atmospheric attenuation; lower frequencies (UHF) more robust but less throughput
- Performance optimized with Unity Profiler to minimize GC and runtime issues

Works with stock CommNet as a drop-in replacement.

---

## ContractConfigurator

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ContractConfigurator](https://github.com/KSP-RO/ContractConfigurator) |
| **Description** | Config-file-based contract creation framework |
| **License** | MIT (core), GPL v2.0 (CC_RemoteTech.dll) |
| **Original Author** | nightingale |
| **Commits** | 2,222 |
| **Releases** | 38 |

Framework for defining new contracts via CFG files rather than C#. Foundation for RP-1 contract packs. [Forum thread](https://forum.kerbalspaceprogram.com/topic/91625-1101-contract-configurator-v1305-2020-10-05) (last release: v1.30.5, Oct 2020 — may be outdated for KSP 1.12+; newer versions may exist under KSP-RO maintenance).

### Contract Sections

| Section | Purpose |
|---|---|
| **Parameters** | Contract objectives (reach orbit, return safely, etc.) |
| **Requirements** | Prerequisites (prior contracts, tech level) |
| **Data Nodes** | Custom data storage for contract state |
| **Behavior** | Contract logic and flow |
| **Rewards** | Payouts (funds, science, reputation) |

### Config Skeleton

```cfg
CONTRACT_TYPE
{
    name = MyContract
    title = My Contract Title
    description = Description text
    synopsis = Short summary

    REQUIREMENT
    {
        name = RequirementName
        type = ...
    }

    PARAMETER
    {
        name = ParameterName
        type = ...
    }

    advanceFunds = X
    rewardFunds = Y
    rewardScience = Z
    rewardReputation = W
}
```

### Parameter Types (12 key types)

| Parameter Type | Purpose |
|---|---|
| `VesselParameterGroup` | Group requirements for a specific vessel |
| `ReachState` | Require vessel to reach a specific state |
| `OrbitParameter` | Specific orbital parameters |
| `SpecificVessel` | Target a specific named vessel |
| `PartParameter` | Require specific part on vessel |
| `ScienceParameter` | Require science data return |
| `DurationParameter` | Sustain a condition for a time |
| `CrewParameter` | Crew-related requirements |
| `ResourceParameter` | Resource thresholds |
| `CommsParameter` | Communication requirements |
| `Contracts.ActiveContracts` | Require other active contracts |
| `Contracts.CompletedContracts` | Require completed contracts |

New parameter types can be added with as little as a dozen lines of C#.

---

## LunarTransferPlanner

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/LunarTransferPlanner](https://github.com/KSP-RO/LunarTransferPlanner) |
| **Description** | GUI for planning lunar transfers in RSS |
| **License** | MIT |
| **Commits** | 1 |
| **Releases** | 1 |

Simple GUI for optimal lunar transfer launch windows in RSS.

**Features:** Configurable flight time (default 4 days), launch inclination display for immediate launch, shows next two minimum-inclination windows, optional Toolbar mod integration.

**Dependencies:** None required. Optional: Toolbar, Kerbal Alarm Clock (KAC wrapper included).

---

## CustomPreLaunchChecks

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/CustomPreLaunchChecks](https://github.com/KSP-RO/CustomPreLaunchChecks) |
| **Description** | Pre-launch validation checks |
| **Commits** | 11 |
| **Releases** | 1 |

Configurable pre-launch validation checks — vessel cannot launch until criteria are met. Useful for RO/RP-1 realistic launch procedures. Required dependency for Kerbal Konstructs. C# plugin with source code available.

---

## FlightSchool

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/FlightSchool](https://github.com/KSP-RO/FlightSchool) |
| **Description** | Kerbals must attend flight school before becoming pilots |
| **Commits** | 14 |

RP-1 career addon: newly hired kerbals must complete training before serving as pilots. Training consumes time and funds. Requires RP-1 for full functionality.

---

## KerbalRenamer

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/KerbalRenamer](https://github.com/KSP-RO/KerbalRenamer) |
| **Description** | Culturally diverse kerbal names from 55+ cultures |
| **Commits** | 58 |
| **Releases** | 13 |

Replaces default Kerbal names with culturally diverse, historically appropriate names from 55+ cultures. Important for RP-1 careers where kosmonauts/astronauts should have realistic names.

**Features:** Named profiles (e.g., "1951" for early space nations), CUSTOM profile, surname-first culture support (China, Hungary), binary gender system (M/F).

**Configuration:** Edit `kerbalrenamer.cfg`. Select one profile from settings. Add new cultures by copying existing entries and editing name lists.

---

## CanaveralPads

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/CanaveralPads](https://github.com/KSP-RO/CanaveralPads) |
| **Description** | Launch pads for RSS-CanaveralHD using KerbalKonstructs |
| **License** | CC-BY-NC-SA 4.0 |
| **Commits** | 15 |
| **Releases** | 2 |

Historically accurate Cape Canaveral/KSC launch complexes.

**Launch complexes:** LC-39 (Apollo/Space Shuttle, by Katniss), various other pads configured by Aviation365.

**Features:** Full building sets clipped into ground, configurable pads via building offsets (100m offset for unused buildings), historical accuracy.

**Dependencies:** RSS-CanaveralHD, Kerbal Konstructs (fixed version), OSSNTR, Tundra Space Center.

---

## Technical Details

### KJR-Continued Auto-Strut Mechanics

- Every part joint analyzed; stiffness assigned from size, mass, connection type
- Sequential stack parts get additional stiff-but-weak anti-wobble connection
- `multiPartAttachNodeReinforcement` controls extra stiffening for radial parts
- `useVolumeNotArea = 1` — connection strength scales with volume not cross-sectional area
- Parts below `massForAdjustment` (0.01) excluded from stiffening
- During physics loading ("coming off rails"), all joints temporarily strengthened to maximum
- World-space joints connect heaviest parts directly to ground on pad, preventing wobble

### RealAntennas Signal Calculation & Dish Mechanics

Beyond core link budget: dish diameter affects gain; pointing accuracy simulates off-axis alignment; beam width narrows with frequency and dish size; asymmetric links simulate realistic comms (ground station can hear weak spacecraft signal but spacecraft cannot receive high-rate data back); each antenna pair evaluated independently in both directions.

### ContractConfigurator API

Extensibility via hooks into KSP's contract system. Standard CFG file syntax — no coding required for basic packs. Repository: `GameData/` (configs), `source/` (C#), `test/` (unit tests), `assets/` (icons/UI).

### KSCSwitcher Compatibility

Works with Kerbal Konstructs for physical pad placement. Compatible with CanaveralPads, Tundra Space Center, and other KK-based mods. Settings via ModuleManager patches or in-game UI. Does not require RP-1. Multiple sites configurable simultaneously. Switching mid-career may cause issues with active contracts and recovery — restart new career for clean switching.

---

## FAQ

### KJR-Continued

- **Overconstrained rockets:** Too many launch clamps → spontaneous disintegration from phantom forces
- **Part conflicts:** Certain PartModule types exempt from stiffening
- **Performance:** Adjust `massForAdjustment` upward if experiencing lag with large craft

### RealAntennas

- **No connection:** Verify antenna band matches — UHF cannot talk to X-band dish at interplanetary distances
- **Asymmetric comms:** Vessel may receive commands but not transmit science — designed behavior
- **Dish pointing:** Steerable dishes must be oriented toward target
- **Performance:** Multiple antennas increase CPU load; use minimum needed

### ContractConfigurator

- **Contracts not appearing:** Check `.cfg` placement and ModuleManager processing (`ModuleManager.ConfigCache`)
- **Requirements not triggering:** Verify prerequisites completed, tech level unlocked
- **RP-1 conflicts:** Use only RP-1 contracts or explicitly compatible packs

### KSCSwitcher

- **Launch site not appearing:** Verify Kerbal Konstructs installed, static objects loaded
- **Wrong terrain elevation:** Adjust KSCSwitcher altitude offset
- **Mid-career switching:** May cause issues — restart for clean switching

### General

- **CKAN is recommended** — handles dependency resolution and version compatibility
- **Start with clean KSP install** for RO/RP-1 to avoid conflicts
- **Report issues** to [KSP-RO Discord](https://discord.gg/V73jjNd) with logs and reproduction steps
- **TestFlight** causes early engine shutdowns; reliability increases with more flights
- **MechJeb "Limit Acceleration"** set to minimum 0 can throttle real engines to 0 causing shutdowns

---

## Dependency Chain

| Mod | Depends On | Used By |
|---|---|---|
| KJR-Continued | — | All large craft, RO/RP-1 |
| Kerbal Konstructs | CustomPreLaunchChecks | CanaveralPads, Tundra Space Center, KSCSwitcher |
| KSCSwitcher | Kerbal Konstructs (optional) | RP-1 career mode |
| RealAntennas | — | RO/RSS (replaces stock CommNet) |
| ContractConfigurator | ModuleManager | RP-1 contracts |
| CustomPreLaunchChecks | — | Kerbal Konstructs |
| CanaveralPads | RSS-CanaveralHD, Kerbal Konstructs, OSSNTR, Tundra Space Center | RO/RP-1 launch sites |

## Glossary

| Term | Definition |
|---|---|
| **Ullage** | Settling propellants before ignition to prevent gas ingestion |
| **ModuleEngineRF** | RealFuels engine module (ignitions, throttle, fuel types) |
| **TestFlight** | Reliability simulation — engines become more reliable with use |
| **ModuleManager** | Patch-loading framework applying RO configs at game load |
| **Kraken** | KSP physics instability causing spontaneous vessel destruction |

---

## Summary

| Mod | Primary Function | License | Status |
|---|---|---|---|
| KJR-Continued | Physics stabilization | — | Active |
| Kerbal Konstructs | Static object placement | MIT / Custom | Active |
| KSCSwitcher | Launch site relocation | — | Active |
| RealAntennas | Realistic comms model | — | Active |
| ContractConfigurator | Config-based contracts | — | Active |
| LunarTransferPlanner | Lunar transfer planning | MIT | Stable |
| CustomPreLaunchChecks | Pre-launch validation | — | Stable |
| FlightSchool | Kerbal training system | — | Stable |
| KerbalRenamer | Cultural name generation | — | Active |
| CanaveralPads | Cape Canaveral launch pads | CC-BY-NC-SA 4.0 | Stable |
