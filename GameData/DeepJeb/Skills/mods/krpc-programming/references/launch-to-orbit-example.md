# kRPC Complete Example — Launch to Orbit

Full Python script for launching from Kerbin to a 100 km circular orbit:

```python
import krpc
import time

conn = krpc.connect(name='Launch Script')
vessel = conn.space_center.active_vessel

# Set up streams
altitude = conn.add_stream(
    vessel.flight(vessel.surface_reference_frame).surface_altitude
)
surface_speed = conn.add_stream(
    vessel.flight(vessel.surface_reference_frame).horizontal_speed
)

# Configure vessel
vessel.control.sas = True
vessel.control.rcs = False

# Launch
print("Launching...")
vessel.control.throttle = 1.0
vessel.control.activate_next_stage()

# Gravity turn
while altitude() < 10000:
    time.sleep(0.1)

print("Starting gravity turn...")
vessel.auto_pilot.target_pitch_and_heading(45, 90)
vessel.auto_pilot.engage()

while altitude() < 20000:
    time.sleep(0.1)

vessel.auto_pilot.target_pitch_and_heading(20, 90)

while altitude() < 40000:
    time.sleep(0.1)

vessel.auto_pilot.target_pitch_and_heading(10, 90)

# Coast to apoapsis
while vessel.orbit.apoapsis_altitude < 100000:
    time.sleep(0.1)

vessel.control.throttle = 0.0
print(f"Apoapsis: {vessel.orbit.apoapsis_altitude:.0f} m")

# Wait for apoapsis
while vessel.orbit.time_to_apoapsis > 10:
    time.sleep(0.1)

# Circularization burn
vessel.auto_pilot.target_pitch_and_heading(0, 90)
vessel.auto_pilot.wait()

# Calculate burn time
dv = vessel.orbit.speed - (
    conn.space_center.bodies['Kerbin'].gravitational_parameter
    / vessel.orbit.semi_major_axis
) ** 0.5
isp = vessel.specific_impulse
g0 = 9.80665
thrust = vessel.available_thrust
burn_time = dv / (thrust / (vessel.mass * isp) - g0)

vessel.control.throttle = 1.0
time.sleep(burn_time)
vessel.control.throttle = 0.0

vessel.auto_pilot.disengage()
print("Orbit achieved!")
```
