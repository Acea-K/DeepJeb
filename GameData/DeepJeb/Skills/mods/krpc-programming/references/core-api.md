# kRPC Core API Reference

## Connection & Basic Usage

### Python Connection
```python
import krpc

# Basic connection (localhost, default ports)
conn = krpc.connect(name='My Script')

# Custom connection
conn = krpc.connect(
    name='Remote Control',
    address='192.168.1.100',
    rpc_port=50000,
    stream_port=50001
)

# Get active vessel
vessel = conn.space_center.active_vessel
print(vessel.name)  # Print vessel name
```

### C# Connection
```csharp
using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;

// Basic connection (auto-disposed)
using (var conn = new Connection("My Script"))
{
    var spaceCenter = Client.Create(conn);
    var vessel = spaceCenter.ActiveVessel;
    Console.WriteLine(vessel.Name);
}

// Custom connection
using (var conn = new Connection("Remote", "192.168.1.100", 50000, 50001))
{
    // ...
}
```

### Lua Connection
```lua
local krpc = require('krpc')
local conn = krpc.connect('My Script')
local vessel = conn.space_center.active_vessel
print(vessel.name)
```

---

## SpaceCenter Service API

### Accessing SpaceCenter
```python
# Python
space_center = conn.space_center
vessel = space_center.active_vessel

# List all vessels
for v in space_center.vessels:
    print(v.name)
```

### Vessel Properties
| Property | Type | Access | Description |
|----------|------|--------|-------------|
| `name` | String | R/W | Vessel name |
| `type` | VesselType | R/W | Vessel type (ship, station, probe, etc.) |
| `situation` | VesselSituation | R | Current situation (landed, orbiting, etc.) |
| `mass` | Double | R | Total mass (kg) |
| `dry_mass` | Double | R | Mass without resources (kg) |
| `thrust` | Double | R | Current total thrust (N) |
| `available_thrust` | Double | R | Available thrust from active engines (N) |
| `max_thrust` | Double | R | Maximum thrust (N) |
| `specific_impulse` | Double | R | Combined Isp of active engines (s) |
| `orbit` | Orbit | R | Current orbit object |
| `control` | Control | R | Control inputs |
| `auto_pilot` | AutoPilot | R | Autopilot object |
| `resources` | Resources | R | Resource information |
| `parts` | Parts | R | Part interaction |
| `comms` | Comms | R | CommNet information |
| `crew_capacity` | Int32 | R | Max crew capacity |
| `crew_count` | Int32 | R | Current crew count |
| `mission_time` | Double | R | Mission elapsed time (s) |
| `biome` | String | R | Current biome name |
| `moment_of_inertia` | Tuple | R | (pitch, roll, yaw) moment of inertia (kg·m²) |
| `maximum_torque` | Tuple | R | (pitch, roll, yaw) max available torque (N·m) |

### Control Inputs
```python
control = vessel.control

# Throttle (0.0 to 1.0)
control.throttle = 0.5

# Steering
control.pitch = 0.2      # -1 to 1
control.yaw = 0.0        # -1 to 1
control.roll = -0.1      # -1 to 1

# Buttons
control.sas = True       # Enable SAS
control.rcs = True       # Enable RCS
control.gear = True      # Deploy landing gear
control.brakes = True    # Apply brakes
control.lights = True    # Toggle lights
control.abort = True     # Abort action group

# Staging
control.next_stage()     # Activate next stage
control.activate_stage(3)  # Activate specific stage

# Custom action groups (1-10)
control.activate_action_group(1, True)
```

### Reference Frames

**Built-in Vessel Reference Frames:**
| Frame | Origin | Axes |
|-------|--------|------|
| `reference_frame` | Center of mass | x=right, y=forward, z=down |
| `orbital_reference_frame` | Center of mass | x=anti-radial, y=prograde, z=normal |
| `surface_reference_frame` | Center of mass | x=zenith, y=north, z=east |
| `surface_velocity_reference_frame` | Center of mass | y=velocity vector |

**Body Reference Frames:**
```python
# Kerbin's non-rotating reference frame
kerbin_frame = conn.space_center.bodies['Kerbin'].non_rotating_reference_frame

# Kerbin's rotating reference frame (surface-fixed)
kerbin_rotating = conn.space_center.bodies['Kerbin'].reference_frame
```

**Creating Custom Reference Frames:**
```python
# Reference frame at vessel position, oriented with orbital frame
custom_frame = conn.space_center.ReferenceFrame.create_relative(
    vessel.orbital_reference_frame,
    vessel.position(kerbin_frame)
)
```

---

## Flight Telemetry

### Flight Object
```python
# Get flight data in surface reference frame
flight = vessel.flight(vessel.surface_reference_frame)

print(f"Altitude: {flight.surface_altitude:.0f} m")
print(f"Latitude: {flight.latitude:.4f}")
print(f"Longitude: {flight.longitude:.4f}")
print(f"Vertical speed: {flight.vertical_speed:.1f} m/s")
print(f"Horizontal speed: {flight.horizontal_speed:.1f} m/s")
print(f"Mach: {flight.mach:.2f}")
print(f"G-force: {flight.g_force:.2f}")
print(f"Dynamic pressure: {flight.dynamic_pressure:.0f} Pa")
print(f"Atmospheric density: {flight.atmospheric_density:.4f} kg/m³")

# Orbital reference frame
orbital_flight = vessel.flight(vessel.orbital_reference_frame)
print(f"Orbital velocity: {orbital_flight.speed:.1f} m/s")
print(f"Prograde: {orbital_flight.prograde:.3f}")
print(f"Normal: {orbital_flight.normal:.3f}")
print(f"Radial: {orbital_flight.radial:.3f}")
```

### Orbit Information
```python
orbit = vessel.orbit

print(f"Apoapsis: {orbit.apoapsis_altitude:.0f} m")
print(f"Periapsis: {orbit.periapsis_altitude:.0f} m")
print(f"Inclination: {orbit.inclination:.2f}°")
print(f"Eccentricity: {orbit.eccentricity:.4f}")
print(f"Semi-major axis: {orbit.semi_major_axis:.0f} m")
print(f"Period: {orbit.period:.1f} s")
print(f"Time to apoapsis: {orbit.time_to_apoapsis:.1f} s")
print(f"Time to periapsis: {orbit.time_to_periapsis:.1f} s")
print(f"True anomaly: {orbit.true_anomaly:.2f}°")
print(f"Mean anomaly: {orbit.mean_anomaly:.4f}")
print(f"Orbital speed: {orbit.speed:.1f} m/s")

# Get position/velocity relative to a body
kerbin = conn.space_center.bodies['Kerbin']
pos = vessel.position(kerbin.non_rotating_reference_frame)
vel = vessel.velocity(kerbin.non_rotating_reference_frame)
```

---

## AutoPilot

### Basic Usage
```python
autopilot = vessel.auto_pilot

# Set target direction using pitch/heading
autopilot.target_pitch = 45.0    # degrees
autopilot.target_heading = 90.0  # degrees (east)
autopilot.target_roll = 0.0      # degrees

# Engage and wait for target
autopilot.engage()
autopilot.wait()  # Blocks until vessel reaches target orientation

# Set direction using reference frame vector
import krpc.types
ref_frame = vessel.surface_reference_frame
direction = (0, 1, 0)  # Straight up in surface frame
autopilot.target_direction = direction
autopilot.target_roll = float('nan')  # Don't control roll
autopilot.engage()
autopilot.wait()

# Disengage
autopilot.disengage()
```

### AutoPilot Configuration
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `stopping_time` | Tuple(float,float,float) | (0.5, 0.5, 0.5) | Max time to stop (pitch, roll, yaw) |
| `deceleration_time` | Tuple(float,float,float) | (5, 5, 5) | Deceleration time to target |
| `attenuation_angle` | Tuple(float,float,float) | (1, 1, 1) | Angle considered "close" to target |
| `time_to_peak` | Tuple(float,float,float) | (3, 3, 3) | PID auto-tuning time to peak |
| `overshoot` | Tuple(float,float,float) | (0.01, 0.01, 0.01) | PID auto-tuning overshoot |
| `auto_tune` | Boolean | True | Auto-tune PID parameters |
| `target_roll_threshold` | Float | 5.0 | Angle to start roll matching |

```python
# Aggressive turning (smaller deceleration_time)
autopilot.deceleration_time = (2, 2, 2)

# Gentle turning (larger deceleration_time)
autopilot.deceleration_time = (10, 10, 10)

# Stop quickly
autopilot.stopping_time = (0.3, 0.3, 0.3)

# Manual PID gains (disable auto_tune first)
autopilot.auto_tune = False
autopilot.pitch_pid_gain = (1, 0.1, 0.5)  # (Kp, Ki, Kd)
```

### Error Reporting
```python
# Check pointing error (requires autopilot engaged)
error = autopilot.error          # Overall error (degrees)
error_pitch = autopilot.error_pitch
error_heading = autopilot.error_heading
error_roll = autopilot.error_roll

print(f"Pointing error: {error:.2f}°")
```
