# kRPC Orbital Mechanics

## Maneuver Nodes

```python
# Create a maneuver node at apoapsis
node = vessel.control.add_node(
    vessel.orbit.time_to_apoapsis,  # Time from now
    prograde=1000,  # Delta-v in prograde direction (m/s)
    normal=0,
    radial=0
)

# Read node properties
print(f"Delta-v: {node.delta_v:.1f} m/s")
print(f"Time to node: {node.time_to:.1f} s")
print(f"Remaining delta-v: {node.remaining_delta_v:.1f} m/s")

# Execute the node
# 1. Point at node
autopilot.target_direction = node.prograde(vessel.orbital_reference_frame)
autopilot.engage()
autopilot.wait()

# 2. Burn until delta-v is consumed
thrust = vessel.available_thrust
isp = vessel.specific_impulse
g0 = 9.80665
burn_time = node.remaining_delta_v / (thrust / (vessel.mass * isp) - g0)

# Wait until node
ut = conn.space_center.ut
wait_time = node.time_to - (burn_time / 2)
conn.space_center.warp_to(ut + wait_time)

# Execute burn
control.throttle = 1.0
while node.remaining_delta_v > 0.1:
    pass
control.throttle = 0.0

# Remove node
node.remove()
```

## Hohmann Transfer

```python
from math import pi, sqrt, cos, acos

# Calculate Hohmann transfer
r1 = vessel.orbit.semi_major_axis
r2 = target_orbit.semi_major_axis
mu = conn.space_center.bodies['Kerbin'].gravitational_parameter

# Transfer orbit semi-major axis
a_transfer = (r1 + r2) / 2

# Delta-v for transfer burn
dv1 = sqrt(mu / r1) * (sqrt(2 * r2 / (r1 + r2)) - 1)
dv2 = sqrt(mu / r2) * (1 - sqrt(2 * r1 / (r1 + r2)))

# Phase angle for transfer
phase_angle = pi * (1 - (1 / (2 * sqrt(2))) * sqrt((r1 + r2) / r2) ** 3)

print(f"Transfer delta-v 1: {dv1:.1f} m/s")
print(f"Transfer delta-v 2: {dv2:.1f} m/s")
print(f"Phase angle: {phase_angle * 180 / pi:.1f}°")
```
