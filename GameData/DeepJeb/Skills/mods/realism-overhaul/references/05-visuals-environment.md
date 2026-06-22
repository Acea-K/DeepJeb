# Visuals & Environment

Visual enhancement, terrain, and environmental simulation mods in the Realism Overhaul ecosystem.

---

## RSS — Real Solar System (Reference)

RSS is the foundational mod that replaces the Kerbol system with a realistic 1:1 scale recreation of our solar system. It is a core dependency for Realism Overhaul and RP-1. See [01-core-mods.md](01-core-mods.md) for the main RSS documentation.

Companion visual mods: RSSVE (clouds/visual enhancements), KSC Switcher (launch sites), Kopernicus (required).

Planetary imagery derived from work by Steve Albers, NASA/JPL, and Celestia Motherlode.

---

## RSS-Textures

High-resolution planetary texture maps for RSS. Contains the image data — elevation maps, color maps, and surface detail textures — used by RSS to render planets.

**Repo:** [KSP-RO/RSS-Textures](https://github.com/KSP-RO/RSS-Textures) — 16 releases.

### Resolution Tiers

| Resolution | VRAM Usage | Recommended Use |
|------------|-----------|-----------------|
| **2048** | ~500 MB | Low-memory systems, integrated GPUs |
| **4096** | ~1–2 GB | Most systems — recommended default |
| **8192** | ~4–6 GB | High-end systems with 6+ GB VRAM |
| **16384** | ~8–16 GB | Extreme detail; requires powerful GPU |

### Known Issues

1. **Memory Crashes on 16384:** Using 16384 textures on systems with <8 GB VRAM causes out-of-memory crashes during scene transitions
2. **Missing Texture Artifacts:** Textures not merged correctly into GameData → grey sphere or checkerboard patterns
3. **Texture Stitching:** At lower resolutions (2048, 4096), visible seams may appear between adjacent texture tiles on Earth
4. **Partial Updates:** When upgrading resolution, delete old texture folders completely before merging new ones

### Installation

Merge the contents of the chosen resolution folder into `GameData/`. Do not nest in an extra subfolder.

---

## RSSVE (Real Solar System Visual Enhancements)

Visual enhancement pack for RSS — cloud layers, atmospheric effects, and visual improvements. Works with Environmental Visual Enhancements (EVE) and Scatterer for photorealistic visuals.

**Repo:** [KSP-RO/RSSVE](https://github.com/KSP-RO/RSSVE) (original by PhineasFreak) — 13 releases. CC-BY-NC-SA 4.0. **Mod pack redistribution not allowed.**

**Dependencies:** RSS, EVE, Scatterer.

### EVE Configuration

- **Cloud layers** for Earth with realistic coverage (stratus, cumulus, cirrus)
- **City lights** layer for nighttime visible-light imagery
- **Aurora effects** at high latitudes
- **Custom particle effects** for atmospheric phenomena

Configs are optimized for RSS planet definitions and may not work on stock-scale systems.

### Scatterer Integration

- **Atmospheric scattering** — realistic sky color, sunset/sunrise gradients
- **Ocean scattering** — subsurface scattering for realistic water appearance
- **Shadow effects** — planet shadows on atmosphere during eclipses

### Version Compatibility

> **Critical:** Install the specific versions of EVE, Scatterer, and RSSVE listed on the RSSVE release page. Using newer versions can cause visual glitches or crashes. This is the most common source of RSSVE problems.

### Known Issues

- **Cloud flickering** at high time warp — reduce to 10x or lower
- **Atmospheric scattering mismatch** if Scatterer version doesn't match RSSVE
- **Performance impact:** RSSVE + Scatterer can reduce frame rates 20–50% on mid-range systems

### FAQ: RSSVE

**Q: Clouds aren't showing.** Verify EVE at correct version per RSSVE release. Check `EnvironmentalVisualEnhancements.dll` in GameData. Ensure Scatterer installed.

**Q: Game crashes on load.** Usually version mismatch. Use exactly the EVE and Scatterer versions on the RSSVE release page.

**Q: Clouds flicker badly.** Reduce time warp to 10x or lower. Known limitation of EVE rendering.

**Q: Aurora effects not visible.** Require high latitudes (above 60° N/S) and active solar weather. Check from polar orbital view.

**Q: Performance is very poor.** RSSVE + Scatterer significantly impacts frame rates. Try disabling Scatterer effects or reducing cloud layer count.

---

## RSS-CanaveralHD

High-definition terrain elevation and color maps for Cape Canaveral region (Kennedy Space Center, CCAFS, and surroundings). Replaces default RSS terrain with more accurate local geography.

**Repo:** [KSP-RO/RSS-CanaveralHD](https://github.com/KSP-RO/RSS-CanaveralHD) — 2 releases. CC-BY-NC-SA 4.0.

Combines work from:
- **KatnissCapeCanaveral** by Katniss218 — original terrain work
- **RSSHiDefCape** by AnticlockwisePropeller — high-resolution height/color maps

Heightmap derived from U.S. Geological Survey data (Public Domain). Requires RSS.

---

## RealHeat

A real heating (thermal energy content) model for KSP. Replaces KSP's stock thermal model — tracks **heat energy in Joules** rather than just temperature. A hard dependency of RO v18.x.

**Repo:** [KSP-RO/RealHeat](https://github.com/KSP-RO/RealHeat) — 16 releases.

**Dependencies:** ModuleManager. Realism Overhaul recommended.

### Heat vs Temperature

| Concept | Stock KSP | RealHeat |
|---------|-----------|----------|
| **Primary quantity** | Temperature (K) | Heat energy (J) |
| **Heat transfer** | Simplified linear | Realistic conduction, convection, radiation |
| **Part interaction** | Parts treated independently | Heat flux between connected parts |
| **Thermal mass** | Abstract | Real — larger parts hold more heat |
| **Transient behavior** | Instant equilibrium | Real thermal time constants |

### Three Modes of Heat Transfer

1. **Conduction** — Heat transfer between connected parts. Depends on material thermal conductivity. Hot parts conduct heat to cooler adjacent parts. Heatshields conduct reentry heat into the capsule behind them. Radiator panels conduct heat from the vessel into the radiator surface.

2. **Convection** — Heat transfer to/from atmosphere. Dominant during atmospheric flight and reentry. Depends on atmospheric density, velocity, and part geometry. Engine bells transfer heat to surrounding air or vacuum.

3. **Radiation** — Infrared emission and solar absorption. All hot parts radiate heat to space (Stefan-Boltzmann law). Parts in shadow cool faster. Solar radiation absorbed on sun-facing surfaces. Planetary infrared (albedo effect) affects spacecraft in low orbit.

### Operational Impact

- **Engine heating:** Models thermal energy from combustion — requires thermal management for sustained burns
- **Solar panel heating:** Panels heat up under direct sunlight; performance degrades if overheated
- **Radiator sizing:** Requires properly sized radiators for crewed vessels and high-power systems
- **Cryogenic boiloff:** More realistic thermal modeling affects propellant boiloff rates (especially hydrolox)

### FAQ: RealHeat

**Q: Parts are overheating when they shouldn't.** RealHeat models physical heat transfer — parts in direct sunlight or near hot engines will heat realistically. Ensure adequate radiator surface area.

**Q: Engines overheat during long burns.** Sustained burns produce significant heat. Use engine plate radiators or limit burn times.

**Q: How do radiators work in RealHeat?** Radiators conduct heat from parts to their surface and radiate it to space. Effectiveness depends on temperature difference (hotter = more effective) and surface area.

**Q: What's the difference between RealHeat and DeadlyReentry heating?** RealHeat is a general thermal model replacement affecting all heat transfer. As of RO v18.x, RealHeat is used instead of DeadlyReentry (which is incompatible).

---

## RealISRU

Realistic In-Situ Resource Utilization for KSP. Overhauls stock ISRU with real-world chemical processes for extracting and processing resources from planetary bodies.

**Repo:** [KSP-RO/RealISRU](https://github.com/KSP-RO/RealISRU)

**Dependencies:** Realism Overhaul, ModuleManager, Community Resource Pack.

### Five Resource Chains

| Input Resource | Process | Output Resource | Real-World Analogue |
|----------------|---------|-----------------|---------------------|
| **Water (H₂O)** | Electrolysis | Oxygen + Hydrogen (LH₂) | Splitting water into breathable O₂ and propellant |
| **Mars Atmosphere (CO₂)** | Sabatier / Zirconia | Oxygen + Methane | MOXIE / ISRU for Mars return |
| **Regolith (generic)** | Carbothermal reduction | Oxygen + Metals | Lunar/Martian oxygen extraction |
| **Water Ice** | Purification & electrolysis | Propellant + Life Support | Lunar polar ice processing |
| **Hydrated Minerals** | Thermal extraction | Water | In-situ water from minerals |

### Conversion Equipment

- **Electrolyzer** — Splits water into hydrogen and oxygen
- **Sabatier Reactor** — Combines hydrogen with CO₂ to produce methane and water
- **Carbothermal Reactor** — Extracts oxygen from lunar/Martian regolith
- **Water Purifier** — Processes dirty water/ice into clean water

### Extraction Sources

- **Planetary surfaces** — scoop/mining from regolith
- **Atmospheres** — filtering atmospheric gases (Martian CO₂, Venusian atmosphere)
- **Ice deposits** — identified via biome maps in RSS

### Pre-Built Craft Files

Included in `Ships/VAB/`:
- **Lunar ISRU Base** — water ice processing + propellant production
- **Mars ISRU Lander** — atmosphere processing + methane production
- **Asteroid Miner** — small-scale asteroid resource extraction

### FAQ: RealISRU

**Q: ISRU equipment isn't processing.** Check: 1) Adequate power, 2) Correct input resources present, 3) Output storage not full, 4) Equipment on correct celestial body.

**Q: How do I find water ice on the Moon?** Concentrated in permanently shadowed craters at the lunar poles. Use biome maps or resource overlay tools.

**Q: Which planets support atmosphere processing?** Mars (CO₂ atmosphere) and Venus (thick CO₂ atmosphere). Earth's atmosphere is not useful for propellant extraction.

---

## ROSolar

Centralized collection of solar panel models from various mods, properly rescaled and configured for Realism Overhaul. Part of the "ROx" family (alongside ROCapsules, ROEngines, etc.).

**Repo:** [KSP-RO/ROSolar](https://github.com/KSP-RO/ROSolar) — 11 releases.

**Dependencies:** Realism Overhaul, ModuleManager. Source mods for external models (optional).

Reduces part clutter by providing panels in one package. Solar panel performance changes appropriately when the model is rescaled. All panels use RO-standard values for power output, mass, and deployment mechanics.

Models are sourced from community mods — some bundled directly (with permission), others configured only if the source mod is already installed. Each part references the original modder.

---

## RSSTimeFormatter

Replaces KSP's stock time system with a real-world Gregorian/proleptic calendar. Displays actual dates (e.g., "1957 October 4") instead of "Year 1, Day 1". Essential for historical mission replication in RP-1.

**Repo:** [KSP-RO/RSSTimeFormatter](https://github.com/KSP-RO/RSSTimeFormatter) — 16 releases. Installed as `RSSDateTime` in GameData.

**Dependencies:** Real Solar System, ModuleManager. RP-1 recommended for career integration.

- Real calendar dates from configured epoch
- Customizable starting date
- Lightweight C# plugin

---

## FAQ: General Visual/Environment

**Q: Earth shows up as a grey sphere or checkerboard.** Textures not installed correctly. Ensure contents of the chosen resolution folder are merged directly into `GameData/`, not nested.

**Q: Game crashes when switching to map view.** Texture resolution may be too high for available VRAM. Step down: 16384 → 8192 → 4096.

**Q: Seams/tears visible on Earth's surface.** Known artifact at lower resolutions (2048, 4096). Use 8192 to minimize seams. Ensure no conflicting texture mods.

**Q: How do I upgrade to higher-resolution textures?** Delete the old texture folder completely from GameData before installing higher-resolution ones.

**Q: Missing textures on specific parts.** Check all RO dependencies installed. Verify KSP.log for missing texture references. Use CKAN for dependency resolution.

**Q: ModuleManager stuck at "Applying patches."** First load after installing RO can take 5–15 minutes. If hung >30 minutes, check for mod conflicts or insufficient RAM.

**Q: Random crashes in VAB/SPH.** Usually memory pressure. Reduce texture resolution, remove unused part packs.

**Q: Oceans appear black/glitched.** Scatterer version mismatch. Reinstall Scatterer configs from the RSSVE release package.

---

## Summary

| Mod | Primary Function | License |
|-----|-----------------|---------|
| RSS-Textures | High-res planetary textures | — |
| RSSVE | Cloud layers & visual enhancements | CC-BY-NC-SA 4.0 |
| RSS-CanaveralHD | HD terrain for Cape Canaveral | CC-BY-NC-SA 4.0 |
| RealHeat | Physics-based thermal model | — |
| RealISRU | Realistic resource extraction | — |
| ROSolar | Consolidated solar panel parts | — |
| RSSTimeFormatter | Real-world calendar dates | — |
