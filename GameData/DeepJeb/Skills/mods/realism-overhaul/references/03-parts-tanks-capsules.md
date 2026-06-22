# Parts, Tanks & Capsules

Modular tanks, historically accurate crew capsules, adjustable heatshields, procedural fairings, solid rocket motor components, and staging animation utilities.

---

## ROTanks

Modular, procedural fuel tanks and structural components for Realism Overhaul, based on modified code from SSTU by Shadowmage45. Tanks are fully resizable in diameter and length, with texture switching and multiple tank type options (cryogenic, service module, balloon, etc.).

**Repo:** [KSP-RO/ROTanks](https://github.com/KSP-RO/ROTanks) — 19 releases, CC BY-NC-SA (original SSTU assets)

**Dependencies:** Textures Unlimited, ROLibrary.

### Tank Type Mapping

| Tank Type | Description | Typical Uses |
|-----------|-------------|--------------|
| **Cryogenic** | Active boil-off simulation for supercooled propellants | LH2, LOX, methalox |
| **Service Module** | Pressure-fed tank configuration with helium pressurization | Hypergolic propellants (MMH/NTO) |
| **Balloon** | Thin-wall tension-bearing tank (requires pressurization) | Atlas stage, Centaur |
| **Structural** | Non-fuel-bearing structural components | Interstages, adapters |
| **Inert** | Decorative/non-functional | Static models, fairings |
| **Spherical** | CryoTanks-style inline spherical tanks | Compact designs |

### Fuel Switching via B9PartSwitch

Right-click the tank in the VAB/SPH, select from tank types, configure propellant ratios and fill levels. Tank mass, cost, and tech level requirements adjust automatically.

### Tech Levels

| Era | Available Tanks | Materials |
|-----|-----------------|-----------|
| **1950s** | Steel balloon, basic SM tanks | Steel, aluminum |
| **1960s** | Aluminum cryo, common SM, balloon | Aluminum alloys, stainless steel |
| **1970s** | Aluminum-lithium cryo | Al-Li alloys, titanium |
| **1980s+** | Composite cryo, advanced SM | Carbon composites, Al-Li |

### Historical Tank Geometries

- **Atlas:** Balloon tank (stainless steel, pressure-stabilized)
- **Centaur:** Balloon tank (stainless steel, cryogenic)
- **Titan II:** Stiffened skin tanks (aluminum)
- **Saturn S-IC, S-II, S-IVB:** Common bulkhead tanks (aluminum)
- **Saturn S-IVB:** Single common bulkhead separating LH2 and LOX

### Source Assets

| Source Modder | Assets Contributed |
|---|---|
| Shadowmage45 (SSTU) | Core modular tank system, inline spherical tanks |
| Frizzank (FASA) | Atlas tank, Atlas skirt, Atlas mount, Titan 2 interstage |
| CobaltWolf (BDB) | LDC Titan interstage decoupler, Titan 2 decoupler, mounts, Centaur-D/V, Saturn SII |
| Beale (Tantares) | Interstage trusses |
| Katniss | MS-II, MS-V, STS, S-IVB interstages |
| Hephaistos | Vulcan mount |
| Akron (Coatl Aerospace) | Probe bodies: Rectangle Tall, Square, Hexagon, Pioneer 10, Voyager, STEREO |
| Nertea (CryoTanks) | Inline spherical tanks |

Probe bodies also include: Diapason, Explorer 33, Helios, Tri-Prism, OGO, OSO, Ranger, Sputnik 3, Venera.

### FAQ: ROTanks

**Q: My tank keeps losing propellant (boil-off).** Cryogenic tanks simulate boil-off. Use Service Module tanks for hypergolic propellants to avoid evaporation.

**Q: Tank mass seems wrong.** Each tank type has different mass ratios. In RP-1 career, mass ratios improve with tech level progression.

**Q: Service module tanks aren't working with pressure-fed engines.** Service Module tank type is required for pressure-fed engines. If the tank is set to Cryogenic, hypergolic pressurization may not work.

### Appendix: Balloon Tanks

Balloon (pressure-stabilized) tanks are thin-walled and rely on internal pressure for structural integrity. In RO, balloon tanks are lighter but more fragile than conventional stiffened-skin tanks. RealFuels' "pressurized" tank type is for pressure-fed engines only, not for balloon tanks.

### Appendix: ServiceModule Tank

Required for pressure-fed engines (TRW LM Descent Engine, R-4D, SuperDraco). Uses helium pressurization to force propellants into the engine at ~200 atm. Cannot be used with pump-fed engines — the high tank pressure would overwhelm the turbopump inlet.

---

## ROCapsules

A curated collection of historically accurate crew capsule models, reducing install bloat by providing a single source for many historical spacecraft. All models, animations, and textures credited to original modders.

**Repo:** [KSP-RO/ROCapsules](https://github.com/KSP-RO/ROCapsules) — 38 releases

**Dependencies:** Module Manager, Realism Overhaul, B9PartSwitch, Textures Unlimited, ROLibrary. KSPWheel (for Dynasoar), ModuleDepthMask (for Gemini).

### Apollo (DECQ)

| Part | Description |
|------|-------------|
| Command Module | Apollo CM with detailed interior |
| Drogue Docking Port | Apollo drogue parachute docking system |
| Drogue Parachute Pack | Drogue parachute deployment system |
| CM Forward Heat Shield | Forward heatshield for CM |
| High-Gain Antenna | S-band high-gain antenna |
| Launch Escape System | LES tower with abort motors |
| LES Boattail Plug | Aerodynamic boattail cover |
| Main Parachute Pack | Three main parachute cluster |
| Service Module RCS Block | Quad RCS thruster block |
| Service Module | Apollo SM with SPS engine |

### Apollo Block III+ (DECQ)

| Part | Description |
|------|-------------|
| Command Module | Updated Block III+ CM |
| CM Heat Shield | Block III+ heat shield |

### Apollo Block III/V (BDB by CobaltWolf)

| Part | Description |
|------|-------------|
| Decoupler | SM-CM decoupler |
| Block III Service Engine (LMAE) | Service propulsion engine |
| Block V Service Engine (TR-201) | Updated service engine |
| Block III HGA | High-gain antenna |
| Block V HGA | Updated HGA |
| Block III+ Mission Module | Extended mission module |
| Block III+ SM | Extended service module |
| Block IV Mission Module | Block IV mission module |
| Block V Solar Array | Solar panel array |
| Apollo Docking Mechanism Probe | Docking probe assembly |

### GE Apollo D2 (AlternateApollo by Mcdouble)

| Part | Description |
|------|-------------|
| Heatshield Adapter | Adapter for heatshield |
| AJ10-133-LH engine | Service propulsion engine |
| HGA | High-gain antenna |
| Block I/II Abort Motors | Launch abort motors |
| Abort Motor Decoupler | LES decoupler |
| Nose Cone | Aerodynamic nose cone |
| Descent Module | D2 descent/crew module |
| Drogue Docking Port | Drogue parachute docking port |
| Docking Probe | Docking mechanism |
| Interstages 1–3 | Three interstage adapters |
| Mission Modules 1–2 | Two mission module variants |
| Parachute Pack | Main parachute system |
| SM RCS Block | Service module RCS block |
| Service Module | Main service module |
| Skirt Sections 1–2 | SM skirt fairings |
| Solar Array | Solar panel deployment |

### Dynasoar / X-20 Moroz (IronCretin & Well; LonesomeRobots Aerospace by SilentVelcro)

| Part | Description |
|------|-------------|
| Cockpit | Crew cockpit with windows |
| Window Cover | Protective window cover |
| Wing | Delta wing surface |
| Elevon | Elevon control surface |
| Rudder | Rudder control surface |
| Front/Rear Skids | Landing skid gear |
| Cargo Bay | Internal payload bay |
| Cabin | Crew cabin section |
| Crew Tube | Crew transfer tunnel |
| Equipment Compartment | Avionics bay |
| Aft Bay | Aft equipment bay |
| Telemetry Antenna | Communications antenna |
| Docking Arm | Docking arm mechanism |
| Docking Rod | Docking alignment rod |

### Gemini (BDB by CobaltWolf; FASA by Frizzank)

**Bluedog Design Bureau:**

| Part | Description |
|------|-------------|
| Agena Target Vehicle Docking Port | Docking port for Agena |
| UHF Antenna | Communications antenna |
| Cabin | Gemini crew cabin |
| Adapter Equipment Section | Equipment bay adapter |
| Nose Fairing | Aerodynamic nose fairing |
| Rendezvous & Recovery Section Fairing | Parachute/recovery fairing |
| Aerodynamic Nose Fairing | Nose fairing variant |
| Recovery Main Parachutes | Main parachutes |
| Recovery Drogue Parachutes | Drogue parachutes |
| OAMS Thruster Pack | Orbit Attitude and Maneuvering System |
| Re-entry Control System | RCS for reentry |
| Adapter Retrograde Section | Retro rocket section adapter |
| Gemini B Adapter Retrograde Section | Gemini B retro adapter |
| Gemini B Equipment Section | Gemini B equipment bay |

**FASA:**

| Part | Description |
|------|-------------|
| Antenna | Communications antenna |
| Flight Pack | Gemini flight pack |
| Flight Pack Control Surface | Flight pack control surface |
| Wings | Gemini wings (paraglider) |
| Wing Control Surface | Wing control surface |

### Big Gemini (BDB by CobaltWolf)

| Part | Description |
|------|-------------|
| Cabin | Big Gemini crew cabin |
| Decoupler | Stage decoupler |
| Docking Adapter | Docking port adapter |
| Heatshield | Reentry heat shield |
| LES | Launch Escape System |
| MOL Docking Port | Manned Orbiting Lab docking port |
| Service Module | Big Gemini service module |

### LEM / Apollo Lunar Module (DECQ)

| Part | Description |
|------|-------------|
| LM Ascent Stage | Lunar module ascent stage with cabin |
| LM Decoupler | Ascent/descent stage separation |
| LM Descent Stage | Lunar module descent stage |
| LM Descent Engine | Throttleable descent engine |
| Lunar Roving Vehicle | Foldable lunar rover |

### Mercury (FASA by Frizzank)

| Part | Description |
|------|-------------|
| Mercury-Atlas Adapter | Adapter for Atlas launch vehicle |
| Mercury-Redstone Adapter | Adapter for Redstone launch vehicle |
| Command Pod | Mercury crew capsule |
| LES | Launch Escape System tower |
| Nose Fairing & Antenna | Nose fairing with antenna |
| Parachute Pack | Main and reserve parachutes |
| Posigrade Solid Rocket Motor | Separation motor |
| RCS Roll Thrusters | Hydrogen peroxide roll jets |
| Retro Solid Rocket Pack | Three retro rocket pack |
| Retro Strap & Decoupler | Retro pack mounting |

### Orion / SLS (DECQ)

| Part | Description |
|------|-------------|
| Crew Module | Orion CM |
| European Service Module (ESM) | ESM built by ESA |
| ESM Fairing | Aerodynamic fairing for ESM |
| Decoupler | Stage separation mechanism |
| Forward Heatshield & NASA Docking System | Forward shield + NDS |
| Heatshield | Main ablative heatshield |
| Launch Abort System | LAS with abort motors |
| Main Parachute | Three-parachute cluster |
| RCS | Orion RCS thrusters |
| ESM Solar Panels | Four solar array wings |

### CST-100 Starliner (Hanson Ma, Mesh Edits by Vader111)

| Part | Description |
|------|-------------|
| Crew Module | Starliner crew cabin |
| Heat Shield | Ablative heat shield |
| Parachute Pack | Parachute deployment system |
| Nose Cone | Aerodynamic nose cone |
| Launch Abort Engines | LAS with abort motors |
| Service Module | Starliner SM |

### Vostok / Voskhod (Soviet Spacecraft by RaiderNick)

| Part | Description |
|------|-------------|
| Voskhod Airlock | EVA airlock for Voskhod 2 |
| Voskhod Descent Module | Voskhod crew capsule |
| Voskhod Retro Package | Retro rocket package |
| Voskhod Retro Decoupler | Retro rocket decoupler |
| Vostok Descent Module | Vostok crew capsule |
| Vostok/Voskhod Decoupler | Common decoupler |
| Vostok/Voskhod Parachute | Common parachute system |
| Vostok/Voskhod Service Module | Common service module |

### Docking Ports

| Part | Description | Source |
|------|-------------|--------|
| APAS 89/95 Active | Androgynous Peripheral Attach System (active) | BDB |
| APAS 89/95 Passive | APAS docking ring (passive) | BDB |
| NASA Docking System Active | International docking standard (active) | CST-100 by Hanson Ma |
| NASA Docking System Passive | International docking standard (passive) | CST-100 by Hanson Ma |

### FAQ: ROCapsules

**Q: Gemini capsule parts are missing.** Gemini requires ModuleDepthMask. Verify this plugin is installed.

**Q: Dynasoar parts don't work properly.** Dynasoar requires KSPWheel for landing gear functionality.

**Q: Apollo LES won't separate.** Check that staging is correctly configured. The LES decoupler and abort motor must be staged in the correct sequence.

---

## ROHeatshields

A single, versatile heatshield part that can be dynamically resized in diameter and configured with various geometries and thermal protection levels — from simple heat sinks to Apollo-class BLEO ablative shielding.

**Repo:** [KSP-RO/ROHeatshields](https://github.com/KSP-RO/ROHeatshields) — 11 releases. MIT (code), CC-BY-NC-SA 4.0 (assets from Coatl Aerospace, SSTU, BDB).

**Dependencies:** Textures Unlimited, ROLibrary.

### Features

- Fully resizable diameter
- Multiple geometric profiles (conical, flat, ogive)
- Configurable thermal protection level (heat sink → BLEO ablative)
- Textures Unlimited coloring support
- RP-1 career integration for tech-unlock and pricing

### Heat Shield Type Selection

| Protection Level | Suitable For |
|-----------------|--------------|
| **Heat sink** | LEO only |
| **Low ablative** | LEO to lunar |
| **High ablative (BLEO)** | Beyond low Earth orbit, interplanetary return |

---

## ProceduralFairings

Auto-reshaping payload fairings that automatically reshape to enclose any attached payload. Originally created by e-dog, maintained by rsparkyc then pap1723.

**Repo:** [KSP-RO/ProceduralFairings](https://github.com/KSP-RO/ProceduralFairings) — 19 releases. CC-BY 4.0. **Mod pack redistribution not allowed.**

**Dependencies:** Module Manager, KSPCommunityFixes, ROUtils. Parts are under the **Payload** tab in VAB/SPH.

### Auto-Reshaping Mechanics

1. **Mesh detection:** Scans all parts attached above the fairing base
2. **Collision volume calculation:** Computes minimum fairing volume needed to enclose the payload
3. **Dynamic reshaping:** Fairing walls automatically conform to the payload shape
4. **Real-time updates:** Shape updates in real-time as you add or move parts

The algorithm uses convex hull approximation for payload enclosure, interpolates between base and nose cone profiles, subdivides segments for smooth curves, and calculates nose cone shape separately from the base section.

### Fairing Shape Customization

| Parameter | Range | Description |
|-----------|-------|-------------|
| `baseConeShape1-4` | 0–1 | Controls base section profile curvature |
| `baseConeSegments` | 2–50 | Vertical segments in base |
| `noseConeShape1-4` | 0–1 | Controls nose section profile curvature |
| `noseConeSegments` | 2–50 | Vertical segments in nose |
| `noseHeightRatio` | 0.5–5 | Nose cone height as fraction of base diameter |

### Historic Fairing Shape Reference

| Shape | baseConeShape1-4 | baseConeSegments | noseConeShape1-4 | noseConeSegments | noseHeightRatio |
|-------|------------------|------------------|------------------|------------------|-----------------|
| **Conic** | 0.3, 0.3, 0.7, 0.7 | 7 | 0.1, 0, 0.7, 0.7 | 11 | 2 |
| **Ogive (Egg)** | 0.3, 0.2, 1, 0.5 | 7 | 0.5, 0, 1, 0.7 | 11 | 2 |
| **Cone-Egg** | 0.3, 0.3, 0.7, 0.7 | 3 | 0.5, 0, 1, 0.7 | 11 | 2 |
| **Atlas V** | 0.1, 1, 0, 0.7 | 3 | 0.1, 0, 0.7, 0.7 | 11 | 3.5 |
| **Delta** | 0, 0, 0, 0 | 3 | 0.3, 0, 1, 0.8 | 11 | 2 |
| **Jupiter/Titan** | 0.3, 0.3, 0.7, 0.7 | 3 | 0, 0, 0.7, 0.2 | 3 | 2 |
| **Long March** | 0.7, 0.7, 0.3, 0.3 | 5 | 0.2, 0, 0.7, 0.2 | 50 | 2.8 |
| **Proton** | 0, 0, 0, 0 | 4 | 1.25, 0.2, 0.1, 0.8 | 3 | 3 |
| **Soyuz** | 0.3, 0.3, 0.9, 1 | 2 | 0.54, 0, 0.52, 0.035 | 10 | 2.2 |

### Inline Fairings

Place the lower fairing base under the section to be enclosed, then place an inverted fairing base above it. Low-profile base rings available for tight interstage configurations.

### Interstage Bases

Place an interstage base at the separation point — it acts as both decoupler and fairing mount point. Configure staging: fairing jettison → stage separation.

### Career Tech Level Limitations

Configurable in `PF_Settings.cfg`:

| Tech Level | Min Diameter | Max Diameter | Max Height |
|------------|-------------|-------------|------------|
| Early | 0.625m | 2.5m | 5m |
| Mid | 0.625m | 5m | 10m |
| Advanced | 0.625m | 10m+ | 20m+ |

### FAQ: ProceduralFairings

**Q: Fairings don't automatically reshape.** Try removing and re-attaching the fairing base. Ensure payload is fully connected and KSPCommunityFixes is installed.

**Q: Fairings clip through the payload.** Increase fairing diameter or adjust shape parameters. Some payload shapes are too complex for auto-reshaping.

**Q: Can't create inline fairings.** Place an inverted fairing base above the enclosed section. You may need low-profile base rings for tight interstage spaces.

**Q: Fairings won't separate.** Check staging sequence. Ensure fairing is set to jettison in the right-click menu.

---

## ProceduralFairings-ForEverything (PFFE)

Patches mods that include their own fairing systems (stock or custom) to instead use Procedural Fairings, removing VAB clutter by eliminating individual pre-set fairing shapes.

**Repo:** [KSP-RO/ProceduralFairings-ForEverything](https://github.com/KSP-RO/ProceduralFairings-ForEverything) — 8 releases. CC-BY-NC-SA 4.0. CKAN installation strongly recommended.

Contributors: Felger, blackheart612, Ravenchant, MeCripp, ferram4, PhineasFreak, NathanKell, pap1723.

---

## ProceduralSolidsLibrary

Configuration data library for procedural solid rocket motor components. Provides the engineering data that powers the procedural SRB system in RP-1 career mode.

**Repo:** [KSP-RO/ProceduralSolidsLibrary](https://github.com/KSP-RO/ProceduralSolidsLibrary)

### Configuration Files

| File | Purpose |
|------|---------|
| `CasingMaterials.cfg` | Casing materials: mass ratios, temperature tolerances (steel, titanium, composites) |
| `GrainGeometries.cfg` | Grain geometries: burn profiles for cylindrical, star, finocyl shapes |
| `Nozzles.cfg` | Nozzle data: expansion ratios, materials, mass properties |
| `Propellants.cfg` | Propellant formulations: PBAN, HTPB, NEPE-75, Double-base with Isp, burn rate, density |

### Burn Profile Reference

| Grain Geometry | Burn Profile |
|---------------|-------------|
| **Cylindrical** | Neutral burn (constant thrust) |
| **Star** | Progressive then regressive |
| **Finocyl** | Complex profile (common in large boosters) |

Typically bundled with RP-1 or installed as a CKAN dependency. Requires a procedural parts framework (e.g. ProceduralParts) to actually construct motors.

---

## StagedAnimation

A lightweight KSP plugin that plays an animation on a part when it is staged, without performing any decoupling action. Designed as a drop-in replacement for ModuleAnimatedDecoupler — staging icon still appears as a decoupler.

**Repo:** [KSP-RO/StagedAnimation](https://github.com/KSP-RO/StagedAnimation) — 2 releases. Animation code based on work by Starwaster.

Also provides `ModuleAnimateGenericExtra`, extending the stock `ModuleAnimateGeneric` with:
- **`deployLimitName`** — Custom label for the Deploy Limit slider
- **`showToggle`** — If `False`, hides the Toggle event from the right-click menu

Use cases: staging-driven solar panel deployment, antenna extension, cover jettison, interstage fairing opening, ullage motor firing.

---

## FAQ: General

**Q: Crewed spacecraft weighs much more than expected.** Each crew member adds 100 kg (person + suit) to part mass. This is a RO feature.

**Q: Parts don't appear in the VAB.** Check mod installation, Module Manager, and RO patch application in KSP log.

**Q: SSTU doesn't work with RO.** SSTU is incompatible with RO/RP-1 for KSP 1.6.1+. Use ROTanks and ROEngines instead.

**Q: Can I use Community Tech Tree with RP-1?** No — RP-1 uses its own historical tech tree.

**Q: Can I use RealPlume-StockConfigs with RO?** No — RO uses its own plume configs tailored to real-scale engines.

**Q: Game crashes on load with RO installed.** Common causes: insufficient RAM (16+ GB recommended), outdated mods, missing dependencies. Try CKAN Express Install.
