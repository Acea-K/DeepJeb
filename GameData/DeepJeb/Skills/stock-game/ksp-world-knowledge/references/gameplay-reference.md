# Game Systems & Gameplay Reference

## Career Mode Progression
1. Start with basic parts (flea SRB, Mk1 pod, basic fins)
2. Complete contracts (Test parts, reach altitude, crew report)
3. Earn science from experiments + biome mapping
4. Unlock tech tree nodes
5. Expand to Mun/Minmus missions
6. Interplanetary exploration

**Key upgrades (level 2 buildings):**
- EVA + flag planting: Astronaut Complex L2
- Maneuver nodes: Tracking Station L2 + Mission Control L2
- Surface samples / Fuel transfer: R&D L2
- Action groups: VAB/SPH L2

## Science Collection
| Experiment | Location | How to use |
|------------|----------|------------|
| Crew Report | Any | Right-click pod or command seat |
| EVA Report | Any (space, surface, flying) | Right-click Kerbal on EVA |
| Surface Sample | Surface only | Kerbal on EVA, right-click ground |
| Mystery Goo | Any | Deploy in part menu; reset after use |
| Materials Bay | Any | Deploy; reset by scientist |
| Thermometer | Any | Read and record |
| Barometer | Atmosphere only | Read and record |
| Gravioli Detector | Any | Read (different results by body) |
| Science Jr. | Atmosphere only | Read |
| Seismic Accelerometer | Impact or landed | Impact or wait for impacts |

**Science transmission:** Less efficient than returning physically (typical loss: 20–60%).

## CommNet
- **Direct:** Antenna directly to KSC
- **Relay:** Via relay satellites for occlusion behind planets
- **No connection = no control of unmanned probes**

## Funds & Reputation (Career)
- **Funds:** Earned from contracts, spent on parts
- **Reputation:** Affects contract rewards; high rep = better contracts
- **Strategies:** (Administration Building) Convert one resource to another

---

## Common Problems & Solutions

### Rocket Won't Lift Off
- Check staging, TWR > 1.0, throttle (Z for max), staging lock (Mod+L)
- Common oversight: Forgot to press space

### Rocket Flips During Ascent
- Too fast in lower atmosphere → reduce throttle near terminal velocity
- Too much drag on top → use fairings, move CoM forward
- Too steep turn → gentler gravity turn
- Not enough control → add fins, more reaction wheels

### Can't Reach Orbit
- Need ~3,200–3,400 m/s Δv
- Too much drag (no fairing, bad aerodynamics)
- Poor gravity turn (too steep or too shallow)

### Running Out of Fuel
- Calculate Δv before building
- Check for dead weight (empty tanks)
- Efficient gravity turn
- Use efficient engines for upper stages (Terrier, Poodle, Nerv)

### Ship Spins Out of Control
- Check SAS is on (T)
- Balanced RCS placement
- Asymmetric fuel drain
- Add reaction wheels
- For aircraft: check CoM vs CoL

### Can't Dock
1. Set target; match inclination at AN/DN
2. Use lower/higher orbit for close approach
3. Kill relative velocity at closest approach
4. Approach at <1 m/s for final 50m
5. Use RCS translation (HNIJKL), set docking port as "Control from Here"

### Rovers Flip in Low Gravity
- Drive gently, short taps of keys
- Disable SAS/reaction wheels while roving
- Turn off RCS while driving

### Kerbal Floating Away
- R for EVA jetpack; WASD translate, Shift/Ctrl up/down
- Aim for hatch, F to grab ladder

### Can't Activate Next Stage
- Staging locked (Mod+L)
- In docking mode / time warp
- No active command pod / mouse hovering over staging

### Can't Time Warp
- Throttle not at 0% (X)
- Ship spinning (enable SAS)
- In atmosphere / too close to terrain

### Re-Entry Overheating
- Heat shield on bottom, facing retrograde
- Periapsis at 35–45 km for Kerbin
- Slow down before thick atmosphere from interplanetary

### Game Performance
- Lower texture quality, use DX11
- Less scatterer/EVE = better FPS
- CPU single-thread performance matters most

### Other Issues
- **Can't save/load craft:** Check for corrupted .craft, version match, missing mod parts
- **Downgrading:** Steam Betas, keep backups; last 32-bit: 1.4.5
- **Black screen:** Update drivers, verify files, delete settings.cfg

---

## Tips & Advice

### Beginner Tips
1. Start in Sandbox, then Science/Career
2. Complete in-game tutorials
3. Use KSPedia (blue rocket book icon)
4. **Read the Δv map** before interplanetary missions
5. Quicksave (F5) FREQUENTLY
6. Staged boosters beat giant single rockets
7. Practice Mun landing first
8. YouTube: Scott Manley, Matt Lowne, EJ_SA, ShadowZone
9. Reddit: r/KerbalSpaceProgram (creations), r/KerbalAcademy (questions)

### Kerbal Personalities
- **Jebediah:** Fearless (badS = true)
- **Bill:** Engineer type, lower courage
- **Bob:** Scientist type, easily scared
- **Valentina:** First female Kerbal (v1.0)
- Each has "Courage" and "Stupidity" stats

### Fun Facts
- Physics: Keplerian on-rails for unfocused, N-body for active
- Physics tick: 60 Hz normal, 30–10 Hz during physics warp
- KSP has been used by NASA/ESA for educational outreach
- "Kraken" = physics glitches that destroy spacecraft
- No 32-bit support since v1.5 (2018)

### Common Q&A (Chinese Community / Bilibili FAQ)
- **Language:** Steam properties → Language tab
- **Can't start:** Delete settings.cfg in game root
- **Copy parts:** Hold Alt while clicking
- **Transfer priority:** Higher number = drains first
- **Surface features not showing:** Edit persistent.sfs → set ROCSeed to positive random number
- **Helicopters:** BG rotors + blades; counter-torque needed

---

## Vessel Sharing & Community

### KerbalX.com — Craft Sharing
**Website:** https://kerbalx.com/
- Automatic MOD detection, search by Mod/Stock, VAB/SPH
- Download .craft → place in `<KSP Root>/ships/VAB/` or `ships/SPH/`

### Other Craft Resources
- **KSP Forum Spacecraft Exchange:** https://forum.kerbalspaceprogram.com/forum/20-ksp1-spacecraft-exchange/
- **Steam Workshop:** https://steamcommunity.com/app/220200/workshop/

### KerbCat — Chinese KSP Community
**Website:** https://kerbcat.com/
- Chinese-translated guides, news, community articles
