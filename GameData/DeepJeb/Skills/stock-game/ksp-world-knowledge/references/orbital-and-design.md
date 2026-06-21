# Orbital Mechanics & Spacecraft Design Reference

## Core Mechanics & Equations

### Delta-V (Δv) — The Rocket Equation
```
Δv = ln(M_start / M_dry) × Isp × 9.81 m/s²
```
- **M_start**: Total mass of the stage (full fuel)
- **M_dry**: Mass of the stage (empty fuel)
- **Isp**: Specific impulse of the engine (in seconds)
- **9.81 m/s²**: Standard gravity constant (Kerbin surface gravity)

**Simplified for stock fuel tanks:**
- Standard tank full/empty mass ratio ≈ 9
- Maximum Δv ≈ 21.58 × Isp (per stage, no payload)

### Thrust-to-Weight Ratio (TWR)
```
TWR = F_thrust / (m_total × g_local)
```
| TWR | Behavior |
|-----|----------|
| < 1.0 | Cannot lift off |
| 1.0–1.3 | Very slow ascent, risky |
| 1.3–2.0 | **Ideal for Kerbin launch** |
| > 2.0 | Aggressive ascent, structural stress risk |

### Specific Impulse (Isp)
- **Atmospheric Isp:** Lower (engines less efficient in atmosphere)
- **Vacuum Isp:** Higher (engines more efficient in space)
- **Mixed engine stages:** `Isp_combined = (F₁ + F₂ + ...) / (F₁/Isp₁ + F₂/Isp₂ + ...)`

### Key Engine Reference (Stock v1.12.5)
| Engine | Vac Isp (s) | Atm Isp (s) | Max Δv (m/s) | Notes |
|--------|-------------|-------------|--------------|-------|
| LV-T30 "Reliant" | 310 | 285 | 6680 | Great first-stage engine |
| LV-T45 "Swivel" | 320 | 270 | 6895 | Has gimbal, good for control |
| RE-I5 "Skipper" | 320 | 280 | 6895 | Good heavy lifter upper stage |
| RE-M3 "Mainsail" | 310 | 285 | 6680 | Heavy lifter |
| KS-25 "Vector" | 315 | 295 | 6787 | High thrust, gimbal |
| LV-N "Nerv" | 800 | 185 | 17238 | Nuclear, no oxidizer needed, heavy |
| LV-909 "Terrier" | 345 | 85 | 7434 | Best small vacuum engine |
| RE-L10 "Poodle" | 350 | 90 | 7542 | Great medium vacuum engine |
| IX-6315 "Dawn" | 4200 | — | 57099 | Xenon ion, very low thrust |
| O-10 "Puff" | 250 | 240 | 5249 | Monopropellant engine |
| CR-7 R.A.P.I.E.R. | 305 | 285/320\* | 6572 | Airbreathing + rocket mode |
| J-20 "Juno" | — | 200 (jet) | — | Starter jet engine |
| Whiplash | — | 250 (jet) | — | High speed turbojet |
| Panther | — | 200/220\* | — | Afterburning jet |
| Goliath | — | 250 (jet) | — | Large turbofan |

\*R.A.P.I.E.R.: 285 atm in rocket mode, 320 in airbreathing mode. Panther: 200 dry, 220 afterburning.

---

## Fundamental Orbital Physics

> Theoretical orbital physics extracted to [references/theory.md](references/theory.md), covering Kepler's laws of planetary motion, orbital kinetic/potential energy, and conservation of momentum for gravity assists.

---

## Delta-V Planning & Maps

### Online Delta-V Planners
- **Primary:** https://ksp.loicviennois.com/ — Interactive Δv calculator with transfer windows
- **Interactive Map:** https://deltavmap.github.io/ — Visual static map with all Δv values
- **Visual Calculator:** https://ksp-visual-calculator.blaarkies.com/ — Multi-purpose calculator

### Delta-V Requirements (from 80 km Kerbin orbit)

**One-way to low orbit:**
| Destination | Δv (m/s) | Notes |
|-------------|----------|-------|
| Mun | ~860 | Hohmann transfer |
| Minmus | ~930 | + inclination adjustment |
| Eve | ~1,180 | Best window: −54° |
| Duna | ~1,060 | Best window: +44° |
| Moho | ~2,200–4,300 | Very window-dependent |
| Dres | ~2,480 | |
| Jool system | ~1,980 | Best window: +96° |
| Eeloo | ~2,770 | Best window: +101° |

**Round trip from 80 km LKO:**
| Destination | Round trip Δv | Difficulty |
|-------------|---------------|------------|
| Mun | ~5,640 m/s | Beginner |
| Minmus | ~5,790 m/s | Beginner |
| Duna | ~6,600 m/s | Intermediate |
| Eve | ~12,000+ m/s | Expert |
| Moho | ~10,000+ m/s | Expert |
| Jool (Laythe) | ~8,000+ m/s | Expert |

---

## Orbital Mechanics

### Key Orbital Terms
| Symbol | Name | Description |
|--------|------|-------------|
| ⨀ | Prograde | Direction of travel. Burn to increase orbit. |
| ⨂ | Retrograde | Opposite of travel. Burn to decrease orbit. |
| △ | Normal | Change inclination north |
| ▽ | Anti-normal | Change inclination south |
| ⦻ | Radial in | Rotate orbit in-plane (toward body) |
| ⊙ | Radial out | Rotate orbit in-plane (away from body) |
| Ap | Apoapsis | Highest point in orbit |
| Pe | Periapsis | Lowest point in orbit |
| SOI | Sphere of Influence | Region where a body's gravity dominates |

### Getting to Orbit (from Kerbin)
1. Launch straight up briefly
2. Begin gradual gravity turn (pitch east / 90° heading)
3. Reach ~45° pitch by 10 km altitude
4. Follow prograde marker through ascent
5. Circularize at apoapsis (70–80 km minimum)
6. Total Δv: ~3,200–3,400 m/s

**Terminal velocity guide:**
| Altitude | Terminal Velocity |
|----------|-------------------|
| Sea level | ~100 m/s |
| 5 km | ~170 m/s |
| 10 km | ~260 m/s |
| 15 km | ~400 m/s |
| 30 km | ~1,000 m/s |

### Hohmann Transfer
1. Burn prograde at periapsis to raise apoapsis to target orbit
2. Coast to apoapsis
3. Burn prograde again to circularize

### Oberth Effect
Burning closer to a gravitational body gives more Δv efficiency.

### Interplanetary Transfer Windows
| Destination | Phase Angle | Frequency |
|-------------|-------------|-----------|
| Moho | ~109° ahead | Every ~0.6 years |
| Eve | ~54° behind | Every ~2.1 years |
| Duna | ~44° ahead | Every ~2.25 years |
| Dres | ~82° ahead | Every ~2.6 years |
| Jool | ~96° ahead | Every ~4.8 years |
| Eeloo | ~101° ahead | Every ~5.0 years |

### Rendezvous & Docking
1. **Achieve orbit** — Stable orbit roughly in target's orbital plane
2. **Align inclination** — Normal/anti-normal burns at AN/DN nodes
3. **Sync orbits** — Lower/higher orbit to let target catch up
4. **Close approach** — Burn prograde/retrograde; aim for <2.5 km
5. **Match velocity & dock** — Kill relative velocity; approach <1 m/s

**Key:** Lower orbit catches up. If target is ahead, get lower. If behind, get higher.

### Aerobraking
| Body | Periapsis | Notes |
|------|-----------|-------|
| Kerbin | ~35–45 km | Carry heat shield |
| Duna | ~12–15 km | Thin atmosphere, precise setup |
| Eve | ~60–70 km | Very thick, be careful |
| Jool | ~120–140 km | Extreme heat danger |
| Laythe | ~25–35 km | Similar to Kerbin |

---

## Spacecraft Design

### Rocket Design Principles
1. **TWR > 1** at every stage (preferably 1.3–2.0 in atmosphere)
2. **Staging:** Shed dead weight (empty tanks and engines)
3. **Asparagus staging:** Fuel feeds inward; drop outer tanks when empty
4. **Struts:** Use EAS-4 Strut Connectors to reinforce weak joints
5. **Fairings:** Reduce drag in atmosphere
6. **Rule of thumb:** Each stage ~3× the fuel of the stage above it
7. **Δv for Kerbin launch:** ~3,400 m/s

### Stock Part Size Classes
| Size | Examples |
|------|----------|
| 0.625 m | Ant engine, Oscar tanks (probes) |
| 1.25 m | Standard (manned capsules, most rockets) |
| 1.875 m | Making History DLC adapter |
| 2.5 m | Large (heavy lifters) |
| 3.75 m | Making History DLC |
| 5.0 m | Making History DLC (SLS-class) |

### Common Rocket Configurations
- **Single stick:** One central stack, simple
- **Radial boosters:** SRBs or liquid boosters on sides
- **Asparagus:** Multiple radial tanks feeding into center — highest efficiency
- **Tandem staging:** Multiple stages on top (Saturn V style)

### Aircraft / Spaceplane Design
- **CoM** must be in front of **CoL** for stability
- **CoT** must pass through CoM for straight flight
- Main wings: Roll control; Tail: Pitch; Vertical fin: Yaw
- Takeoff: ~80–120 m/s; Landing approach: ~50 m/s

### Construction Tricks (International Communities)
- **Struts weigh nothing in flight** — use freely
- **5× Terrier** lighter & more efficient than 1× Poodle
- **4× LV-T30/LV-T45** outperforms 1× Skipper
- **3 legs** = stable tripod; **4 legs** = better tip-over resistance
- **Ion engines** excel at fine adjustments on low-gravity bodies
- **Unshielded solar panels** more mass-efficient than shielded
- **Separators** heavier than decouplers — use decouplers unless clearance needed

### Action Groups
- **Custom groups 1–0:** Toggle anything (solar panels, science, engine modes)
- **Default:** Gear (G), Lights (U), Brakes (B), SAS (T), RCS (R), Abort
- **Axis groups:** Bind robotic parts (BG DLC) to pitch/yaw/roll/throttle
