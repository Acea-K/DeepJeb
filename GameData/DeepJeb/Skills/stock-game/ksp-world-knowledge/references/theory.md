# Fundamental Orbital Physics

## Kepler's Laws of Planetary Motion

Kepler's three laws describe how celestial bodies (and spacecraft) move under gravity. Every orbit in KSP follows these rules.

**First Law (Law of Ellipses):** Every orbit is an ellipse with the central body at one focus.
- In KSP: all stable orbits are ellipses. A circular orbit is simply an ellipse with zero eccentricity.
- Key parameters: semi-major axis (a), eccentricity (e), periapsis (closest point), apoapsis (farthest point).

**Second Law (Law of Equal Areas):** A body sweeps out equal areas in equal time intervals.
- A spacecraft moves faster near periapsis and slower near apoapsis.
- In KSP: this is why ejection burns done at periapsis are more efficient (Oberth effect), and why capture burns at periapsis require less Δv.

**Third Law (Harmonic Law):** The square of the orbital period is proportional to the cube of the semi-major axis.
```
T² ∝ a³        or        T = 2π × √(a³ / μ)
```
- **T**: orbital period (seconds)
- **a**: semi-major axis (meters)
- **μ**: gravitational parameter of the central body (m³/s², e.g. Kerbin μ ≈ 3.53×10¹²)
- This means higher orbits have disproportionately longer periods — a 4× higher orbit takes 8× longer to complete.

**KSP relevance:**
- Phasing orbits for rendezvous use Kepler's Third Law: a lower orbit has a shorter period, so it catches up to a higher target.
- Transfer window timing relies on Kepler's laws to compute optimal phase angles between planets.

---

## Orbital Energy: Kinetic & Potential

A spacecraft in orbit has two forms of mechanical energy:

**Kinetic Energy (KE):** Energy of motion.
```
KE = ½ m v²
```

**Gravitational Potential Energy (PE):** Energy of altitude, always negative in a bound orbit.
```
PE = -G M m / r    (or equivalently)   PE = -μ m / r
```

- **m**: spacecraft mass (kg)
- **v**: orbital velocity (m/s)
- **r**: distance from center of the central body (m)
- **μ**: gravitational parameter (m³/s²)

**Total Orbital Energy (conserved in a vacuum):**
```
E_total = KE + PE = ½ m v² - μ m / r   =   -μ m / (2a)
```
- For a circular orbit: `v = √(μ / r)`, so `KE = μ m / (2r)` and `PE = -μ m / r`, giving `E_total = -μ m / (2r)`.
- For an elliptical orbit: total energy depends only on semi-major axis **a**, not eccentricity.

**Energy & Δv in KSP:**
- Burning prograde adds KE, raising the opposite side of the orbit (higher apoapsis).
- Burning retrograde subtracts KE, lowering the opposite side (lower periapsis).
- At a given altitude, the most efficient place to change orbital energy is at the point where velocity is highest — periapsis (the Oberth effect).
- Escape (parabolic) orbit: E_total = 0, meaning `v = √(2μ / r)` — the escape velocity.
- Hyperbolic (interplanetary) orbit: E_total > 0, excess velocity remains after escaping.

---

## Conservation of Momentum & Gravity Assist

**Linear Momentum:**
```
p = m v
```
In the absence of external forces, total momentum of a closed system is conserved.

**Momentum Exchange in Gravity Assists (Slingshot):**

A gravity assist uses a planet's motion to change a spacecraft's velocity without burning fuel. Despite common intuition, the spacecraft does NOT "bounce off" the planet's atmosphere — it exchanges momentum with the planet through gravity.

**How it works:**
1. As the spacecraft approaches a planet, the planet's gravity pulls it in, accelerating it relative to the planet.
2. The spacecraft follows a hyperbolic trajectory around the planet, bending its direction.
3. As it leaves, the spacecraft has the same speed *relative to the planet* (conservation of energy in the planet's frame), but its velocity *relative to the Sun* has changed.

**Why the spacecraft gains speed:**
```
Δv_spacecraft = - (m_planet / m_sc) × Δv_planet   ≈ 0
```
Because a planet is astronomically more massive than a spacecraft, the planet's velocity change is negligible. From the Sun's reference frame, the spacecraft's velocity vector has been rotated and its magnitude changed by effectively "stealing" a tiny amount of the planet's orbital momentum.

**Analogy:** A small ball thrown at a moving train bounces off with more speed than it came in — the ball gains momentum from the train, while the train barely slows down.

**In KSP:**
- Gravity assists are used to reach outer planets (Jool, Eeloo) with minimal Δv.
- Jool is the most common gravity assist target in KSP because of its large mass and multiple moons.
- Eve and Tylo are also useful for powerful assists due to their high gravity.
- A gravity assist can either **speed up** (fly behind the planet's orbital path) or **slow down** (fly in front) the spacecraft relative to the Sun.
- Careful trajectory planning is required — the approach angle and periapsis altitude determine the bending angle and final velocity.

**The 3-body limitation:** KSP's patched-conic approximation (on-rails) does not simulate real n-body gravity during time warp. Gravity assists are calculated at the SOI boundary transition, not continuously. In stock KSP, a gravity assist is a single impulse at the SOI edge; mods like **Principia** add true n-body physics for realistic continuous gravity assists.
