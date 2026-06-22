# Life Support & Reliability

Life support systems, part reliability and testing, radiation simulation, science overhaul, and deadly reentry mechanics.

---

## ⚠️ CRITICAL: DeadlyReentry Compatibility

**DeadlyReentry is INCOMPATIBLE with Realism Overhaul v18.x and newer.** The RO `.netkan` explicitly lists `DeadlyReentry` in its `conflicts:` section — CKAN will refuse to install both. RO v18.x provides its own integrated heating and thermal damage system via **RealHeat** (a hard dependency), making DeadlyReentry unnecessary and conflicting.

For reentry heating: **RealHeat + RO's built-in thermal configs** replace DeadlyReentry.
For heatshields: **ROHeatshields** — see [03-parts-tanks-capsules.md](03-parts-tanks-capsules.md#roheatshields).

The DeadlyReentry documentation below is provided for historical reference only.

---

## ⚠️ Life Support: Kerbalism vs TACLS (Mutually Exclusive)

The RO `.netkan` recommends **one of** `Kerbalism-Config-RO` or `TACLS` for life support. They are mutually exclusive — install only one.

- **Kerbalism-Config-RO** (via ROKerbalism): Comprehensive life support + radiation + science + stress simulation. Recommended for deep simulation.
- **TACLS (TAC Life Support)**: Simpler resource-based life support (food, water, oxygen). Recommended for basic LS without the full Kerbalism feature set.

The upstream Kerbalism mod without ROKerbalism config is **NOT compatible** with current RO.

---

## TacLifeSupport

A comprehensive life support system for Kerbal Space Program, originally created by Thunder Aerospace Corporation (TAC). Kerbals need food, water, oxygen, and electricity to survive — both in vessel and during EVA.

**Repo:** [KSP-RO/TacLifeSupport](https://github.com/KSP-RO/TacLifeSupport) — 32 releases. CC BY-NC-SA 3.0.
**Maintainers:** danfarnsy & JPLRepo (via KSP-RO)
**Dependency:** REPOSoftTechKSPUtils (background resource processing)

### Resource Requirements

| Resource | Purpose | Survival Time Without |
|---|---|---|
| **Food** | Basic nutrition | 30 days |
| **Water** | Hydration | 3 days |
| **Oxygen** | Breathing | 2 hours |
| **Electricity** | Air quality & climate control | 2 hours |

Waste products: CarbonDioxide, Waste, WasteWater.

### Key Mechanics

- **Crewed pods** come pre-stocked with 3 days of resources
- **EVA suits** carry a half-day supply of each resource, drawn from the pod the Kerbal exited
- **Background simulation** — Kerbals consume resources even when the vessel is not active; they can die if left unattended
- **Time warp safety** — attempts to stop time warp when resources run low
- **Recyclers** — use recycler parts to extend mission duration by recovering waste products

### Background Resource Processing History

| Version | Capability |
|---------|-----------|
| v0.13.2+ | EC consumption/production for unloaded vessels (solar panels + generators) |
| v0.13.12+ | Added Near Future fission generators, FuelCells support |
| v0.16.0+ | Track ANY mod part using stock resource converters to generate EC (off by default) |
| v0.17.0+ | Fix Kopernicus Solar Panel background processing |
| v0.18.0+ | Fix Kopernicus solar panels; fix NRE spam on Resource Converters |

### RO Config Location Change

```
OLD: GameData/ThunderAerospace/TacLifeSupport/PluginData/LifeSupport.cfg
NEW: GameData/ThunderAerospace/TacLifeSupport/LifeSupport.cfg
```

### Parts

- **HexCan containers** for Food, Water, Oxygen (from Greys, CC-BY 3.0)
- Life support monitoring window showing estimated supply duration for each vessel

---

## TestFlight

A configurable, extensible part research and reliability system. Engineers generate **flight data** from actual launches — this data persists across builds and improves reliability for the same part types.

**Repo:** [KSP-RO/TestFlight](https://github.com/KSP-RO/TestFlight) — 123 releases. CC BY-NC-SA 4.0.

### Core Concept

Launch "Super Rocket 1" with a Mainsail engine → generates 10,000 units of flight data → Build "Super Rocket 2" with the same Mainsail → it starts with 10,000 flight data units already banked. More flights = more data = more reliable parts.

### FlightData System

- **Persistent across builds** — Flight data carries over between rocket designs
- **Part-specific tracking** — Different engine models do NOT share data
- **Profession bonuses** — Engineer Kerbals generate more flight data per flight

### Reliability Curve

Base reliability starts low for each new part type. Flight data accumulates with each successful launch. The curve follows a **logarithmic/asymptotic model** — rapid improvement from early data, diminishing returns as a part matures. Engine-specific configs exist for most real engines (LR-87, LR-91, J-2, RL-10, RD-180, etc.).

### Failure Types

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

### Failure Mechanics

| Aspect | Detail |
|--------|--------|
| **Check frequency** | Periodic during flight; configurable per part |
| **Reliability threshold** | Parts below certain reliability fire more frequent checks |
| **Failure severity** | Configurable per failure type (minor vs catastrophic) |
| **Repair system** | Some failures repairable in-flight by engineers via EVA |
| **TestFlight API** | Allows custom failure types, data recording, reliability calculations |

### Ignition Reliability (RealFuels Integration)

TestFlight integrates with RealFuels' `ModuleEngineRF`. Engines with limited ignitions can fail to start if reliability is low. The `additionalFailureChance` parameter allows extra TestFlight-based ignition failure probability. First-stage engines typically have 1 ignition; some upper stages have multiple restarts.

### Configurability

User-configurable: minimum/maximum reliability ratings, failure rates, repair costs. Pluggable PartModules give full control over reliability calculation, failure types, and per-part configuration.

### FAQ: TestFlight

**Q: My engine keeps failing.** New engines have low flight data and higher failure rates. Fly more test flights with the same engine type.

**Q: How do I check reliability?** Right-click menu in flight shows accumulated flight data and current estimated reliability percentage.

**Q: Why does my upper stage engine fail to ignite?** Check ullage (propellant settling), remaining ignition count, electric charge, and engine reliability rating.

**Q: Can I disable TestFlight?** Remove from GameData or set reliability to 100% via config. However, RO balance assumes TestFlight is present.

---

## ROKerbalism

Kerbalism configuration pack for Realism Overhaul and RP-1. Adapts the comprehensive Kerbalism mod with historically realistic spacecraft capabilities and life support systems.

**Repo:** [KSP-RO/ROKerbalism](https://github.com/KSP-RO/ROKerbalism) — 38 releases.

### Key Differences from Stock Kerbalism

| Feature | ROKerbalism Behavior |
|---|---|
| **Reliability** | Disabled — conflicts with TestFlight |
| **Spacecraft Customization** | Not available unless using advanced near-future parts |
| **Life Support Systems** | Each capsule has its real historical LS configuration |
| **Recycling** | Not available until ISS era (~2000s) |
| **Science Over Time** | All experiments take time (Kerbalism 3.0+) |
| **RP-1 Integration** | Life support subsystems unlockable via tech tree; old capsules upgradeable |

### Life Support & Simulation

In-depth crewed mission simulation including radiation, stress, and equipment. Prevents unrealistic scenarios (e.g., crewed Jupiter flyby in a Mercury capsule). Historical parts have real life support configurations.

### Communications

Stock CommNet is insufficient. **Recommended:** [RealAntennas](https://github.com/KSP-RO/RealAntennas). **RemoteTech is incompatible** with ROKerbalism.

### Supported Mods

ROCapsules, Ven's Stock Revamp, FASA (ROCapsules preferred), SXT, CxAerospace, RN Soyuz, BDB (partial), Tokamak Industries.

### Installation

1. Use a new install or uninstall any other life support mods (TACLS, USI-LS)
2. Install RealismOverhaul and dependencies
3. **(Recommended)** Install RP-1 for career mode
4. Copy **Kerbalism** and **KerbalismConfig** folders to `GameData`

> The core Kerbalism folder from stock works, but you **must** get the config folder from ROKerbalism.

---

## Kerbalism (Upstream Reference)

> ⚠️ **NOT compatible with current RO.** The upstream Kerbalism mod is documented here for reference. For RO installations, use **ROKerbalism** (above). Newer Kerbalism versions (v3.18+) exist upstream but are not maintained for RO/RP-1.

**Original Repo:** [Kerbalism/Kerbalism](https://github.com/Kerbalism/Kerbalism)
**Author:** Sir Mortimer — Unlicense (Public Domain)

### Core Features (Upstream)

**Life Support:** Kerbals consume food, water, oxygen. Death occurs if not provided. Recycling and production in situ.

**Stress System:** Without adequate living space, atmospheric pressure, and comforts, Kerbals become stressed and make mission-threatening mistakes.

**Radiation:** Simulates space radiation environment — cosmic background, solar flares, planetary radiation belts. Vessels must be adequately shielded. EVA is extremely dangerous during solar flare events.

Three radiation sources:

| Source | Countermeasure |
|--------|---------------|
| **Cosmic Background Radiation** | Minimal shielding; long missions need dedicated shielding |
| **Solar Radiation / Flares** | Active monitoring; shielded storm shelters; flare warnings |
| **Planetary Radiation Belts** | Mission planning to avoid or quickly transit through belts |

Shielding is provided by part mass + dedicated radiation shielding parts. Different materials provide different shielding effectiveness per unit mass. Kerbals accumulate a total radiation dose — exceeding limits causes health effects, reduced performance, and eventual death.

**Reliability & Failures:** Components have limited operational lifetimes. Engines have limited ignitions and limited burn time.

**ISRU:** Replaces stock "ore to everything" with semi-realistic extraction and conversion rules.

**Science Over Time:** Experiments produce data over time (up to several years). Data transmits over time. Eliminates stock lab "infinite science" exploit. Adds many probe, satellite, and late-game manned experiments.

**Background Processing:** All vessels simulated continuously with low performance overhead.

### Requirements

| Dependency | Required For |
|---|---|
| Module Manager | Both packages |
| HarmonyKSP | Both packages |
| KSPCommunityFixes | Both packages |
| CommunityResourcePack | KerbalismConfig only |

### Configuration Packs

| Pack | Description |
|------|-------------|
| Official KerbalismConfig | Stock-scale, "current space tech" scope |
| ROKerbalism | Official config for RO/RP-1 |
| SIMPLEX | Stockalike simplified LS/ISRU |
| SkyhawkKerbalism | BDB-focused with revamped systems |
| LessRealThanReal(ism) | RP-1 based at stock scales without RO |

---

## DeadlyReentry — ⚠️ INCOMPATIBLE (Historical)

**Repository:** [KSP-RO/DeadlyReentry](https://github.com/KSP-RO/DeadlyReentry)
**Original Author:** NathanKell | **Maintainer:** Starwaster — CC BY-SA 4.0

DeadlyReentry (DRE) overhauls KSP's atmospheric heating and structural damage systems. **Incompatible with RO v18.x+** — use RealHeat instead.

### Atmospheric Reentry Heating (Historical)

- Calculates atmospheric density from temperature and pressure (Earth-like atmosphere)
- Convective, conductive, and radiative heating during atmospheric entry
- Part-by-part thermal simulation — each part has its own temperature, thermal mass, and heat tolerance
- Parts visually burn up when exceeding tolerance (FX density exponent default 0.7)
- **As of v7.9.0, DRE no longer implements its own reentry heating** — since KSP 1.0, DRE tweaks stock parameters to make reentry deadlier

### Heat Transfer Mechanisms

| Mechanism | Description |
|-----------|-------------|
| **Convective heating** | Heat from plasma sheath to part surface. Dominant during peak heating. |
| **Conductive heating** | Internal heat transfer between adjacent parts. Heatshields conduct heat to parts behind them. |
| **Radiative heating** | Infrared from hot parts and shock-heated atmosphere. Also heat rejection to space. |

Key factors: atmospheric density (Earth-like model), vessel velocity (not part-relative), part thermal properties, occlusion shielding (parts behind heatshields receive reduced heating).

### G-Force Damage Formula

```
gToleranceMult * sqrt(6 * crashTolerance)
```

- Default `gToleranceMult` = 2.5
- Parts typically survive up to **12–15 Gs**
- Kerbals can survive ~9G from LEO reentry (steep), but prolonged high-G causes blackout/injury
- Structural G-limits: parts have configurable tolerances; exceeding causes structural failures

### Heat Shield Types (Historical Reference)

| Type | Example | Behavior |
|------|---------|----------|
| **Ablative** | Apollo, Dragon, Orion | Burns away material; good for high peak heating |
| **TPS Tiles** | Space Shuttle | Absorbs and re-radiates heat; good for sustained low heat flux |
| **PICA** | Stardust, Dragon | Advanced ablative; very high heat tolerance |

In RO, heatshields convert Ablator into CharredAblator during reentry; only untouched ablator is consumed at sensible rates (RO v11.0).

### Multiplier Tuning

Default heat multiplier in RO: ~25x (accounts for 8 km/s orbital velocities at Earth scale). Adjustable via **Alt+D+R**. LEO-rated shields will NOT survive translunar or interplanetary reentry velocities. Shallow reentries reduce peak heat flux but increase total heat load. Steep reentries (negative perigee, Mercury-style) have high peak heating, low total heat load, ~9G deceleration.

### Release History

| Version | KSP Version | Notes |
|---------|------------|-------|
| v5.2 | 0.24.2 | Original NathanKell release |
| v5.3 | 0.24.2+ | New density model, chute-cutting logic, burnup FX |
| v6.0 | 0.25 | Hand-off to Starwaster |
| v8.0.0–v8.1.3 | 1.8+ / 1.10+ | Modern versions under Starwaster |

### FAQ: DeadlyReentry (Historical)

**Q: Craft burns up on reentry even with heatshield.** Verify shield facing retrograde. Check heat multiplier (Alt+D+R). LEO reentry requires ~25x multiplier in RO.

**Q: What's the difference between ablative and non-ablative heatshields?** Ablative shields burn away material — good for high-peak heating. Non-ablative/radiative shields absorb and re-radiate — better for sustained moderate heat flux.

**Q: Kerbals keep dying from G-forces.** Limit reentry acceleration via entry angle. Use lifting reentry profiles or multiple aerobraking passes.

**Q: How do I adjust DeadlyReentry settings?** Press **Alt+D+R** in flight.

---

## FAQ: General

**Q: My engines keep turning off early.** Check: 1) TestFlight reliability failures, 2) MechJeb "Limit Acceleration" (if minimum is 0, it may throttle to 0), 3) RealFuels ignition count.

**Q: Why do my craft weigh so much more than expected?** Each crew member adds 100 kg (person + suit) to part mass.

**Q: Life support resources depleting too fast?** Verify storage for mission duration. Check recycler functionality in TACLS. In Kerbalism, verify consumption appropriate for crew count.

**Q: Can I combine TAC Life Support with Kerbalism?** **No.** The RO `.netkan` recommends one or the other — they are mutually exclusive.

**Q: Parachutes aren't working or burn up.** Use RealChute (RO standard). Adjust max chute temperature via tweakables.

---

## Appendix: RCS Thruster Heat Vulnerability

RCS thrusters burn up before other parts during atmospheric reentry due to low `maxTemp` threshold. Mitigation: place RCS thrusters above the fuselage or behind wings for aerodynamic shielding. Modern RO configs have addressed this for most part packs, but older part packs may still be affected.
