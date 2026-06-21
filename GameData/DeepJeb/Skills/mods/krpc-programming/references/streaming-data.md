# kRPC Streaming Data

## Why Streams?

Polling methods repeatedly (e.g., in a while loop) creates high overhead. Streams execute on the server and push updates efficiently.

```python
# BAD: Polling (high overhead)
while True:
    alt = vessel.flight().surface_altitude
    print(alt)

# GOOD: Streaming (low overhead)
alt_stream = conn.add_stream(
    vessel.flight().surface_altitude
)
while True:
    print(alt_stream())  # or alt_stream()
```

## Creating Streams

```python
# Stream a property
pos_stream = conn.add_stream(
    vessel.position,
    vessel.orbital_reference_frame
)

# Stream a method
alt_stream = conn.add_stream(
    vessel.flight, vessel.surface_reference_frame
).surface_altitude

# Set update rate (Hz, 0 = unlimited)
alt_stream.rate = 10  # 10 updates per second
```

## Waiting for Conditions

```python
# Wait until altitude > 10000m
alt_stream = conn.add_stream(
    vessel.flight().surface_altitude
)
while alt_stream() < 10000:
    pass

# Using condition variables (no busy-wait)
import threading
with alt_stream.condition:
    while alt_stream() < 10000:
        alt_stream.wait()
```

## Callbacks

```python
def on_alt_change(value):
    print(f"Altitude changed to: {value:.0f} m")

alt_stream = conn.add_stream(
    vessel.flight().surface_altitude
)
alt_stream.add_callback(on_alt_change)
alt_stream.start()
# ...
alt_stream.remove_callback(on_alt_change)
```

## Custom Events

```python
# Create event: wait until periapsis
event = vessel.orbit.event(
    conn.space_center.FlightEvent.Periapsis
)
event.wait()

# Create event with expression
expr = conn.krpc.Expression.greater_than(
    conn.krpc.Expression.call(
        vessel.flight().mean_altitude.get_call()
    ),
    conn.krpc.Expression.constant_double(50000)
)
event = conn.krpc.Event(expr)
event.start()
event.wait()
```
