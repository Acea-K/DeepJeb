# kRPC Vessel, Warp & Extending Reference

## Vessel Parts & Resources

### Accessing Parts
```python
parts = vessel.parts

# List all parts
for part in parts.all:
    print(f"{part.name}: {part.title}")

# Filter by name
engines = parts.with_name('liquidEngine')

# Get specific part
command_pod = parts.with_module('ModuleCommand')[0]

# Access modules
for module in command_pod.modules:
    print(f"  Module: {module.name}")
    for field in module.fields:
        print(f"    {field.name}: {field.value}")
```

### Controlling Parts
```python
# Activate/deactivate engine
engine = parts.with_name('liquidEngine')[0].engine
engine.active = True

# Set engine thrust limit
engine.thrust_limit = 0.8  # 80%

# Deploy solar panels
solar = parts.with_module('ModuleDeployableSolarPanel')[0]
solar.modules_with_name('ModuleDeployableSolarPanel')[0].set_action('DEPLOY', True)

# Control landing gear
gear = parts.with_module('ModuleLandingLeg')[0]
gear.modules_with_name('ModuleLandingLeg')[0].set_action('TOGGLE', True)

# Toggle lights
vessel.control.lights = True
```

### Resources
```python
resources = vessel.resources

# Get resource amount
fuel = resources.amount('LiquidFuel')
oxidizer = resources.amount('Oxidizer')
electricity = resources.amount('ElectricCharge')

print(f"Fuel: {fuel:.1f} L")
print(f"Oxidizer: {oxidizer:.1f} L")
print(f"Electricity: {electricity:.1f} L")

# Get capacity
fuel_capacity = resources.capacity('LiquidFuel')

# Stage-specific resources
stage_0_fuel = resources_in_stage(0, cumulative=False).amount('LiquidFuel')
```

---

## Warp & Time Control

```python
# Warp to absolute time (Universal Time)
ut = conn.space_center.ut
target_time = ut + 3600  # 1 hour from now
conn.space_center.warp_to(target_time)

# Warp to specific event
# Warp to apoapsis
time_to_ap = vessel.orbit.time_to_apoapsis
conn.space_center.warp_to(conn.space_center.ut + time_to_ap)

# Physical warp (faster than 100x)
# Note: Physics warp goes beyond 100x but can cause glitches
conn.space_center.physics_warp_factor = 3  # 4x physics speed
```

---

## Extending kRPC — Custom Services

### Service API Overview
Third-party mods can add new services to kRPC using C# attributes.

### Core Attributes
```csharp
using KRPC.Service;

// Define a service
[KRPCService(GameScene = GameScene.Flight)]
public static class MyModService
{
    // Define a procedure
    [KRPCProcedure]
    public static string GetStatus()
    {
        return "All systems operational";
    }

    // Define a class (instances passable to client)
    [KRPCClass]
    public class MyComponent
    {
        public float Value { get; set; }

        [KRPCMethod]
        public void Reset() { Value = 0; }
    }

    // Define a property
    [KRPCProperty]
    public static double Temperature { get; set; }
}
```

### Available Mod Services
| Service | Mod | Description |
|---------|-----|-------------|
| `MechJeb` | kRPC.MechJeb | Autopilot modules (ascent, landing, docking) |
| `FerramAerospaceResearch` | FAR | Advanced aerodynamics |
| `InfernalRobotics` | IR | Servo/motor control |
| `KerbalAlarmClock` | KAC | Alarm management |

---

## Community Resources

### Official
- **Documentation:** https://krpc.github.io/krpc/
- **GitHub:** https://github.com/krpc/krpc
- **Forum Thread:** https://forum.kerbalspaceprogram.com/topic/62902/
- **Tutorials:** https://krpc.github.io/krpc/tutorials.html

### Third-Party Extensions
- **kRPC.MechJeb:** https://genhis.github.io/KRPC.MechJeb/
- **kIPC (kOS/kRPC bridge):** https://forum.kerbalspaceprogram.com/topic/142979/
- **KNav:** https://github.com/Vivero/KNav
- **wernher (flight control):** https://github.com/theodoregoetz/wernher

---

## Appendix: Quick Reference

### Connection
```python
import krpc
conn = krpc.connect(name='Script')
vessel = conn.space_center.active_vessel
```

### Streams
```python
stream = conn.add_stream(vessel.flight().surface_altitude)
value = stream()
stream.rate = 10
```
