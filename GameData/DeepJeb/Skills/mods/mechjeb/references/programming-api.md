# MechJeb Programming API Reference

## kRPC.MechJeb (External Scripting)

**kRPC.MechJeb** is a server-side addon to [kRPC](https://krpc.github.io/krpc) that exposes MechJeb's autopilot settings as remote procedures callable from Python, C#, or any kRPC client language.

**Setup:**
1. Install both kRPC and MechJeb2 in KSP
2. Install kRPC.MechJeb server plugin in GameData
3. Connect a kRPC client (Python recommended)

**Core API Pattern:**
```python
import krpc
conn = krpc.connect()
vessel = conn.space_center.active_vessel

# Get MechJeb module
ascent = conn.mechjeb.ascent_autopilot(vessel)

# Read current values
print(ascent.desired_orbit_altitude)   # Read target altitude
print(ascent.desired_inclination)      # Read target inclination
print(ascent.enabled)                  # Check if active

# Modify values in real-time
ascent.desired_orbit_altitude = 200000  # Set 200 km target
ascent.desired_inclination = 28.5       # Set inclination
ascent.enabled = True                   # Activate autopilot
```

### Available Autopilot Modules via kRPC

**AscentAutopilot:**
| Property | Type | R/W | Description |
|----------|------|-----|-------------|
| `enabled` | Boolean | get/set | Enable/disable ascent guidance |
| `ascent_path_index` | Int32 | get/set | 0=Classic, 1=GravityTurn, 2=PVG |
| `desired_inclination` | Double | get/set | Target inclination in degrees |
| `desired_orbit_altitude` | Double | get/set | Target altitude in km |
| `force_roll` | Boolean | get/set | Force roll during ascent |
| `vertical_roll` | Double | get/set | Roll angle during vertical climb |
| `turn_roll` | Double | get/set | Roll angle during pitch-over |
| `autostage` | Boolean | get/set | Auto-stage when depleted |
| `skip_circularization` | Boolean | get/set | Skip final circularization burn |
| `limit_aoa` | Boolean | get/set | Enable angle-of-attack limiting |
| `max_aoa` | Double | get/set | Maximum angle of attack |

**Classic Ascent Profile (`ascent_path_classic`):**
| Property | Type | R/W | Description |
|----------|------|-----|-------------|
| `auto_path` | Boolean | get/set | Auto altitude-based turn |
| `auto_turn_percent` | Single | get/set | 0-1, turn shape percentage |
| `auto_turn_speed_factor` | Single | get/set | 0-1, speed factor |
| `turn_start_altitude` | Double | get/set | Manual turn start altitude |
| `turn_start_velocity` | Double | get/set | Manual turn start velocity |
| `turn_end_altitude` | Double | get/set | Turn completion altitude |
| `turn_end_angle` | Double | get/set | Final flight path angle |
| `turn_shape_exponent` | Double | get/set | 0-1, controls steepness |

**Other Available Modules:**
- `rendezvous_autopilot` — Automate rendezvous sequence
- `docking_autopilot` — Terminal docking guidance
- `landing_guidance` — Precision landing
- `rover_autopilot` — Waypoint driving
- `throttle_controller` — Thrust limits
- `staging_controller` — Stage parameters
- `rcs_balancer` — RCS output tuning

---

## kOS.MechJeb2.Addon (kOS Integration)

**kOS.MechJeb2.Addon** provides a direct integration layer between kOS and MechJeb2, enabling kOS scripts to access MechJeb data and functionality.

**Setup:**
1. Install both kOS and MechJeb2
2. Install kOS.MechJeb2.Addon (available on CKAN)

**Usage in kOS:**
```
// Get MechJeb module
local mj is addons:mechjeb.

// Access ascent guidance
local ascent is mj:ascent.
set ascent:desired_altitude to 200000.
set ascent:desired_inclination to 28.5.
set ascent:enabled to true.

// Access landing guidance
local land is mj:landing.
set land:target to latlng(0.1, 74.6).
set land:enabled to true.

// Read vessel information
print mj:vessel:deltav.
print mj:vessel:orbital_period.
```

---

## Module Manager Patches

MechJeb can be configured via Module Manager patches (see [module-manager.skill] for full MM syntax reference). Create `.cfg` files to modify default behavior:

```
// Example: Disable specific features
@MechJebSettings
{
    %enableAscentGuidance = false
    %enableLandingGuidance = false
}

// Example: Modify default ascent profile
@MechJebAscentSettings
{
    %defaultTurnStartAltitude = 200
    %defaultTurnEndAltitude = 70000
}
```

---

## Programmatic Access via C# (In-Game)

For advanced users writing plugins or addons, MechJeb exposes its internal API through C# assemblies:

```csharp
// Access active vessel's MechJeb
MechJebCore mechjeb = vessel.GetMechJebModule<MechJebCore>();

// Get ascent module
MechJebModuleAscent ascent = mechjeb.GetComputerModule<MechJebModuleAscent>();

// Read/write values
double alt = ascent.desiredOrbitAltitude;
ascent.desiredOrbitAltitude = 300000;
ascent.enabled = true;

// Get landing module
MechJebModuleLandingGuidance land = mechjeb.GetComputerModule<MechJebModuleLandingGuidance>();
land.enabled = true;
```
