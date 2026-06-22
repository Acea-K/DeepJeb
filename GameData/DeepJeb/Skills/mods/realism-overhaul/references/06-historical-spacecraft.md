# Historical Spacecraft Mods

Reference for historical spacecraft and launch vehicle mods in the Realism Overhaul ecosystem. These mods replicate real-world rockets, spacecraft, probes, and space stations.

---

## SovietRockets

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/SovietRockets](https://github.com/KSP-RO/SovietRockets) |
| **Description** | Various versions of the Soviet Soyuz and Proton rockets |
| **License** | CC BY-NC-ND 3.0 US |
| **Maintainer** | KSP-RO team |
| **Commits** | 245 |
| **Releases** | 39 |

**Launch vehicles:** Soyuz (multiple variants), Proton (K, K/D, etc.), Zenit-2, Fregat-1 upper stage.

**Features:** Old School Fairings support, legacy RemoteTech support (RealAntennas recommended for modern RO), pre-built craft files, multiple variants per rocket family.

**Recommended companion mods:** [Salyut Stations](https://github.com/KSP-RO/SalyutStations), [Soviet Probes](https://github.com/KSP-RO/SovietProbes) — both designed specifically for these launchers.

### Soyuz Variants

| Variant | Stage Config | Payload to LEO | Notable Features |
|---------|-------------|----------------|------------------|
| **Soyuz (R-7 Semyorka)** | 4-stage (R-7 core + 4 boosters) | ~6.5 tonnes | Original ICBM-derived launcher |
| **Soyuz-U** | 3-stage (R-7 + Blok I upper) | ~7.0 tonnes | Most-flown variant (780+ launches) |
| **Soyuz-U2** | 3-stage (synthetic kerosene) | ~7.2 tonnes | Used Syntin fuel; higher performance |
| **Soyuz-FG** | 3-stage (upgraded engines) | ~7.5 tonnes | Introduced digital flight control |
| **Soyuz-2.1a** | 3-stage (digital) | ~7.0 tonnes | Modernized; no longer human-rated |
| **Soyuz-2.1b** | 3-stage (RD-0124 upper) | ~8.2 tonnes | Upgraded upper stage |
| **Soyuz-ST** | 3-stage (tropicalized) | ~9.0 tonnes | Launched from Kourou, French Guiana |

### Proton Variants

| Variant | Stage Config | Payload to LEO/GTO | Notable Features |
|---------|-------------|--------------------|------------------|
| **Proton (UR-500)** | 2-stage | ~12 tonnes LEO | Original heavy ICBM |
| **Proton-K** | 3-stage | ~20 t LEO / ~5 t GTO | Workhorse for space station modules |
| **Proton-K/D** | 3-stage + D upper | ~5.7 t GTO | Block D upper stage for lunar/GTO |
| **Proton-K/DM** | 3-stage + DM upper | ~4.5 t GTO | DM upper stage (restartable) |
| **Proton-M** | 3-stage (+ Breeze-M) | ~22 t LEO / ~6.5 t GTO | Modernized with Breeze-M upper stage |

### Zenit Details

- **Zenit-2** — 2-stage, ~13.7 tonnes to LEO, RD-171 engine (4-chamber, 4-nozzle)
- **Zenit-3SL** (Sea Launch) — 3-stage with Block DM-SL, ~6 tonnes to GTO
- **Zenit-3F** (Zenit-2SB/Fregat) — Fregat-SB upper stage for higher energy orbits

### Fregat Upper Stage

- Hypergolic upper stage for Soyuz and Zenit
- Multi-restart capability (up to 20+ ignitions)
- Dry mass: ~930 kg, Propellant: N₂O₄/UDMH, Isp: ~333s

Credits: Thorton & Igel (Soyuz, Fregat1, some Proton parts), DECQ (R7 textures), buran.ru (Zenit meshes).

---

## SovietSpacecraft

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/SovietSpacecraft](https://github.com/KSP-RO/SovietSpacecraft) |
| **Description** | Soviet spacecraft — Soyuz, Vostok, Voskhod, and TKS/VA |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 114 |
| **Releases** | 26 |

**Spacecraft:** Soyuz 7K-T, 7K-OKA, 7K-OKP, 7K-T-AF, 7K-LOK (lunar orbiter), 7K-LK (lunar lander), Progress 7K-TG (cargo), Vostok, Voskhod, TKS/VA.

**Requires:** [SovietRockets](https://github.com/KSP-RO/SovietRockets) for launch vehicles. Strongly recommended: [Salyut Stations](https://github.com/KSP-RO/SalyutStations).

**Features:** Pre-configured craft files (recommended — manual assembly difficult), legacy RemoteTech support (RealAntennas recommended for modern RO).

### Vostok

| Spec | Detail |
|------|--------|
| **Crew** | 1 cosmonaut |
| **Mass** | ~4.7 tonnes |
| **Length / Diameter** | ~4.4 m / ~2.4 m |
| **Duration** | Up to 5 days |
| **Reentry** | Ejection seat (cosmonaut ejected at ~7 km) |
| **Missions** | Vostok 1–6 (1961–1963) |

### Voskhod

| Spec | Detail |
|------|--------|
| **Crew** | 2–3 cosmonauts |
| **Mass** | ~5.7 tonnes |
| **Length / Diameter** | ~5.0 m / ~2.4 m |
| **Duration** | Up to 22 days |
| **Notable** | First 3-crew mission (Voskhod 1); First spacewalk — Leonov (Voskhod 2) |

### Soyuz Variants

| Variant | Mass | Length | Crew | Purpose |
|---------|------|--------|------|---------|
| **7K-OK** | ~6.6 t | ~7.5 m | 2–3 | Orbital ops; first-generation |
| **7K-T** | ~6.8 t | ~7.5 m | 2 | Second-gen; batteries instead of solar panels |
| **7K-T-AF** | ~6.8 t | ~7.5 m | 2 | Astrophysical observatory (Orion telescope) |
| **7K-LOK** | ~9.9 t | ~10 m | 2 | Lunar orbiter (Soviet lunar program) |
| **7K-LK** | ~5.5 t | ~5 m | 1 | Lunar lander (never flew crewed) |
| **Progress 7K-TG** | ~7.2 t | ~7.9 m | 0 | Uncrewed cargo |

### TKS/VA

| Spec | Detail |
|------|--------|
| **Crew** | 2–3 |
| **Mass (fully loaded)** | ~21.6 tonnes |
| **VA capsule mass** | ~4.8 tonnes |
| **Length / Diameter** | ~17 m / ~4.1 m (FGB module) |
| **Purpose** | Almaz military stations; FGB modules later used for Mir core and ISS Zarya |

---

## SovietProbes

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/SovietProbes](https://github.com/KSP-RO/SovietProbes) |
| **Description** | Luna and Sputnik series probes |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 88 |
| **Releases** | 24 |

**Probes:** Sputnik 1/2/3 (complete), Luna 2 (impactor), Luna 3 (far-side photographer), Luna 9 (soft lander), Luna 10 (orbiter) — complete. Molniya, Polyot — planned.

**Requires:** [SovietRockets](https://github.com/KSP-RO/SovietRockets). Legacy RemoteTech support (RealAntennas recommended). Craft files included.

---

## USProbesPack

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/USProbesPack](https://github.com/KSP-RO/USProbesPack) |
| **Description** | NASA and US military probes |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 246 |
| **Releases** | 40 |

**Probes:** Pioneer 6/7/8/9, Pioneer 10/11, Voyager, Galileo, Magellan, New Horizons, TDRS, EOS, NEO satellites, Vanguard-1. Legacy RemoteTech support (RealAntennas recommended). Many meshes direct from NASA/JPL.

### Pioneer Probes

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| **Pioneer 6/7/8/9** | Solar orbit | ~63–147 kg | Interplanetary space weather network |
| **Pioneer 10** | Jupiter flyby | ~258 kg | First across asteroid belt; Pioneer plaque |
| **Pioneer 11** | Jupiter/Saturn | ~259 kg | First Saturn flyby; Jupiter gravity assist |

### Mariner Probes

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| **Mariner 2** | Venus flyby | ~203 kg | First successful Venus flyby (1962) |
| **Mariner 4** | Mars flyby | ~261 kg | First Mars flyby; first close-up images (1965) |
| **Mariner 5** | Venus flyby | ~245 kg | Venus atmosphere measurements |
| **Mariner 6/7** | Mars flyby | ~413 kg each | Mars flyby photography |
| **Mariner 9** | Mars orbiter | ~558 kg | First spacecraft to orbit another planet (1971) |
| **Mariner 10** | Venus/Mercury | ~474 kg | First Mercury flyby; first gravity assist |

### Ranger Probes

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| **Ranger 7** | Lunar impactor | ~367 kg | First US lunar close-up photos (1964) |
| **Ranger 8** | Lunar impactor | ~367 kg | Mare Tranquillitatis (Apollo 11 site) |
| **Ranger 9** | Lunar impactor | ~367 kg | Crater Alphonsus photography |

Block III design: 6 TV cameras, transmitted until impact. No landing capability.

### Surveyor Probes

| Probe | Target | Mass | Key Features |
|-------|--------|------|--------------|
| **Surveyor 1** | Lunar lander | ~292 kg | First US soft landing (1966) |
| **Surveyor 3** | Lunar lander | ~302 kg | Visited by Apollo 12 crew |
| **Surveyor 5** | Lunar lander | ~305 kg | Alpha-scattering soil analysis |
| **Surveyor 7** | Lunar lander | ~306 kg | Tycho crater; robotic arm |

Soft landing with retro-rockets and vernier engines. Three landing legs, stable on slopes up to ~20°.

---

## USRockets

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/USRockets](https://github.com/KSP-RO/USRockets) |
| **Description** | Older, obscure US launchers |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 287 |
| **Releases** | 30 |

Focuses on less common US launch vehicles. Old School Fairings support, pre-built craft files. Periodically expanded with additional launchers.

---

## Antares-Cygnus

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/Antares-Cygnus](https://github.com/KSP-RO/Antares-Cygnus) |
| **Description** | Antares launch vehicle, Cygnus and Cygnus Extended cargo spacecraft |
| **License** | CC BY-NC-ND 3.0 US |
| **Authors** | Kartoffelkuchen & Raidernick |
| **Commits** | 49 |
| **Releases** | 13 |

**Features:** Legacy RemoteTech support (RealAntennas recommended), KIS support, pre-built craft files.

### Antares Variants

| Variant | Stage 1 | Stage 2 | Payload to LEO | Notes |
|---------|---------|---------|----------------|-------|
| **Antares 110** | Castor 30A (solid) | ATK solid | ~5.0 t | Original |
| **Antares 120** | 2x RD-181 (kerolox) | Castor 30XL | ~6.1 t | Upgraded first stage |
| **Antares 130** | 2x RD-181 | Briz-M (hypergolic) | ~6.8 t | Restartable upper |
| **Antares 230** | 2x RD-181 | Castor 30XL | ~6.1 t | Current operational |
| **Antares 230+** | 2x RD-181 | Castor 30XL (enhanced) | ~6.5 t | Latest (2025+) |

**First stage (Antares 230):** 2x RD-181, RP-1/LOX, ~3,840 kN total thrust.
**Second stage (Castor 30XL):** Solid motor, ~140 s burn, ~497 kN thrust.

### Cygnus Variants

| Variant | Pressurized Volume | Total Mass | Propellant | Features |
|---------|-------------------|------------|------------|----------|
| **Cygnus Standard** | ~18 m³ | ~5.6 t | NTO/MMH | Original ISS resupply |
| **Cygnus Enhanced** | ~27 m³ | ~8.0 t | NTO/MMH | Extended cargo capacity |
| **Cygnus PCM** | ~27 m³ | ~8.0 t | NTO/MMH | Enhanced power generation |

**Service module:** Ultraflex solar arrays, hypergolic bipropellant RCS, CBM berthing (not direct docking), incendiary reentry at end of mission.
**CRS-2 (PCM):** Up to 3,750 kg pressurized, 1,250 kg unpressurized cargo. 3.5 kW average power.

---

## SalyutStations

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/SalyutStations](https://github.com/KSP-RO/SalyutStations) |
| **Description** | Salyut 1–7 and Almaz stations (DOS and OPS series) |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 122 |
| **Releases** | 30 |

**Stations:** Salyut 1, 4, 6, 7 (DOS series) and Almaz 2, 3, 5 (OPS military series). Fully functional replicas with historical color schemes.

**Requires:** [SovietRockets](https://github.com/KSP-RO/SovietRockets). Recommended: [SovietSpacecraft](https://github.com/KSP-RO/SovietSpacecraft) (crewed ferry craft). Legacy RemoteTech support (RealAntennas recommended). Craft files included.

---

## Skylab

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/Skylab](https://github.com/KSP-RO/Skylab) |
| **Description** | Skylab station — broken and unbroken versions |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 57 |
| **Releases** | 15 |

**Components:** Main OWS module (converted S-IVB stage), left solar array (damaged config), micrometeorite shield (EVA repair), nosecone with ATM and docking adapter, original unbroken variant.

**Saturn V auto-patching (priority):** DECQ (highest) > OLDD (middle) > FASA (lowest). Default config works if none installed.

**Launch guidance:** Steep ascent profile, level off only at end (~200 m/s DV remaining). Must use Skylab ASAS for control. Not designed for beginners. Requires ModuleManager.

Credits: Usonian (mesh), NoMrBond (internal seats), Snjo (Firespitter).

---

## FASA-RO

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/FASA-RO](https://github.com/KSP-RO/FASA-RO) |
| **Description** | FASA — Realism Overhaul Edition (Stock Compatible) |
| **License** | CC-BY-SA |
| **Commits** | 57 |
| **Releases** | 11 |

RO-compatible edition of FASA (Frizzank's American Space Agency). Includes RO configs/patches for Mercury, Gemini, Apollo and associated launchers.

**Requires original FASA mod parts** — FASA-RO provides only RO patches.

### Saturn V Stages

| Stage | Engine(s) | Burn Time | Thrust |
|-------|-----------|-----------|--------|
| **S-IC** | 5x F-1 | ~150–168 s | ~35,100 kN SL each |
| **S-II** | 5x J-2 | ~360–390 s | ~5,150 kN vac each |
| **S-IVB** | 1x J-2 | ~470 s + ~335 s (TLI) | ~1,033 kN vac |

### Apollo CSM/LM

| Vehicle | Mass | Crew | Key Features |
|---------|------|------|--------------|
| **Command Module** | ~5.6 t | 3 | Ablative heatshield |
| **Service Module** | ~24.5 t (prop) | 0 | SPS engine (310s Isp) |
| **Lunar Module** | ~15.2 t (fueled) | 2 | Descent/ascent stages |

### Gemini

| Vehicle | Mass | Crew | Features |
|---------|------|------|----------|
| **Gemini Capsule** | ~3.8 t | 2 | Ejection seats |
| **Titan II GLV** | ~154 t (liftoff) | — | Hypergolic launcher |
| **Agena Target** | ~3.2 t | 0 | Docking target |

Saturn V compatibility: FASA, OLDD, DECQ. Skylab auto-detects installed mod and applies patches.

---

## RNMisc

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/RNMisc](https://github.com/KSP-RO/RNMisc) |
| **Description** | Miscellaneous spacecraft parts — MIR Docking Port, Spacelab |
| **License** | CC BY-NC-ND 3.0 US |
| **Commits** | 17 |
| **Releases** | 8 |

Collection of parts that don't fit other specific mods. MIR Docking Port (ISS Community pack), Spacelab pressurized laboratory (by gandalf).

---

## ROStations

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ROStations](https://github.com/KSP-RO/ROStations) |
| **Description** | Station parts and configs for RO, with BDB integration docs |
| **Commits** | 13 |

ModuleManager patches and part definitions for space station components. Includes Skylab BDB integration notes. Requires Realism Overhaul and ModuleManager.

---

## Summary

| Mod | Primary Content | License | Status |
|---|---|---|---|
| SovietRockets | Soyuz, Proton, Zenit | CC BY-NC-ND 3.0 | Active |
| SovietSpacecraft | Soyuz, Vostok, Voskhod, TKS/VA | CC BY-NC-ND 3.0 | Active |
| SovietProbes | Luna, Sputnik probes | CC BY-NC-ND 3.0 | Active |
| USProbesPack | NASA/US military probes | CC BY-NC-ND 3.0 | Active |
| USRockets | Obscure US launchers | CC BY-NC-ND 3.0 | Active |
| Antares-Cygnus | Antares launcher, Cygnus cargo | CC BY-NC-ND 3.0 | Active |
| SalyutStations | Salyut 1–7, Almaz stations | CC BY-NC-ND 3.0 | Active |
| Skylab | Skylab station (broken/unbroken) | CC BY-NC-ND 3.0 | Active |
| FASA-RO | Mercury, Gemini, Apollo (RO edition) | CC-BY-SA | Active |
| RNMisc | Miscellaneous parts | CC BY-NC-ND 3.0 | Active |
| ROStations | Station parts/configs | — | Active |

---

## FAQ

### SovietRockets

**Q: Why won't my Soyuz/Proton engine stage separate?**
Check decoupler staging sequence. SovietRockets uses realistic staging events. Use included craft files as reference.

**Q: My Proton rocket keeps exploding on the pad.**
Proton uses hypergolic propellants (N₂O₄/UDMH). Check that RealFuels tank types are set correctly for hypergolic fuels.

**Q: Which Soyuz variant for crew?**
Soyuz-U/2.1a are historically accurate. Use craft files or RO documentation.

### SovietSpacecraft

**Q: Vostok/Voskhod won't separate from service module.**
Verify separation sequence in craft files. Reentry mode differs from US capsules (ejection seat for Vostok).

**Q: Why is TKS/VA so heavy?**
~21 tonnes fully loaded. FGB module is very massive. Use Proton-K or Proton-M.

**Q: No solar panels on Soyuz 7K-T?**
Correct — 7K-T used batteries for reduced mass. Mission limited to ~2 days. Upgrade to 7K-OK for solar panels.

**Q: Can I use Soyuz craft files without SovietRockets?**
No. SovietSpacecraft requires SovietRockets.

### USProbesPack

**Q: Pioneer/Voyager has no power.**
Pioneer/Voyager use RTGs — ensure RTG part is attached and has sufficient fuel.

**Q: Mariner probe not communicating?**
Check antenna deployment and power. Early probes had low-power transmitters. RealAntennas recommended for interplanetary distances.

**Q: Launcher for Ranger/Surveyor?**
Ranger used Atlas-Agena; Surveyor used Atlas-Centaur. Install USRockets or appropriate US launcher mods.

### FASA-RO

**Q: Parts missing or scaled incorrectly.**
FASA-RO provides only RO patches — original FASA mod must be installed first.

**Q: Saturn V keeps exploding or underpowered.**
Verify staging matches craft files. Use steep ascent profile — "only level off at the end." Apollo LM descent engine throttles ~10–60%.

**Q: Gemini won't separate from Titan II.**
Gemini uses "fire-in-the-hole" staging. Use included craft file staging. Verify retro-rocket activation.

**Q: Which Saturn V for Skylab?**
Skylab auto-detects FASA, OLDD, or DECQ. DECQ highest priority.

### Antares-Cygnus

**Q: Cygnus won't dock with ISS.**
Cygnus uses CBM (Common Berthing Mechanism) — berthed by robotic arm, not direct docking.

**Q: Which Antares variant is current?**
Antares 230+ (latest). For RP-1 careers, Antares 230 typically available from the 2010s era.

**Q: Cygnus has no RCS/propellant?**
Cygnus uses hypergolic bipropellant RCS. Check service module tanks have NTO/MMH.

### General

**Q: RemoteTech vs RealAntennas?**
Soviet and US mods include legacy RemoteTech support. For modern RO, use RealAntennas. RemoteTech standalone not recommended.

**Q: Will adding part packs break my save?**
Adding part packs works via ModuleManager patches. Removing packs may cause missing part errors. Always back up saves.

---

## Appendix: Explorer 1 Ascent Procedure

Explorer 1 launched on a Juno I (modified Jupiter-C). To replicate in RO:

1. Change engine config from Ethanol/LOx to **Hydyne/LOx**
2. Fly ascent until booster burnout, decouple booster cluster
3. RCS orient to **90° heading, 0° pitch** (horizontal, eastward)
4. Spin up upper stage and decouple payload at ~10 seconds to apogee
5. Fire solid upper stages (Baby Sergeant rockets) in succession
6. Spin-stabilization required — Juno I upper cluster was spin-stabilized

**Hydyne:** 60% UDMH + 40% DET. Storable hypergolic fuel, higher performance than ethanol. Available as RealFuels propellant option for Jupiter-C / Juno I engine config.
