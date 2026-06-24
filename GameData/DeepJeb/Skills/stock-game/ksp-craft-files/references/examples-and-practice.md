# Complete .craft File Example & Practice

> A complete example showing the .craft file structure of a typical two-stage launch vehicle. Includes explanatory comments.

## 1. Complete Rocket Example

```
ship = Simple Rocket v2
description = A simple two-stage rocket for orbit insertion
version = 1.12.5
type = VAB
size = 1.25, 8.5, 1.25
vesselType = Ship

PART
{
    // Index 0 — Root part: Mk1 Command Pod
    part = mk1pod
    partName = Part_4578b3e0-d152-4f24-ba6f-8f3c7e8a9b01
    pos = 0.0, 8.0, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    attPos = 0.0, 0.0, 0.0
    attDir = 0.0, -1.0, 0.0
    attAng = 0.0, 0.0, 0.0
    link = -1
    mirror = 0
    sym = 0
    symMethod = Mirror
    istg = 0
    dstg = 0
    sqor = 0
    ridx = -1
    CrewCapacity = 1
    persistentId = 2948304671
    
    MODULE
    {
        name = ModuleCommand
        minimumCrew = 1
        hibernateOn = False
        RESOURCE
        {
            name = ElectricCharge
            rate = 0.05
        }
    }
}

PART
{
    // Index 1 — First stage fuel tank (attached to command pod bottom)
    part = fuelTank
    partName = Part_b712c9a0-c301-4e8e-8bfa-2d5f9e6a3c02
    pos = 0.0, 7.0, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    attPos = 0.0, -0.5, 0.0
    attDir = 0.0, -1.0, 0.0
    attAng = 0.0, 0.0, 0.0
    link = 0
    mirror = 0
    sym = 0
    symMethod = Mirror
    istg = 0
    dstg = -1
    sqor = 0
    ridx = -1
    CrewCapacity = 0
    
    RESOURCE
    {
        name = LiquidFuel
        amount = 200
        maxAmount = 200
    }
    RESOURCE
    {
        name = Oxidizer
        amount = 245
        maxAmount = 245
    }
}

PART
{
    // Index 2 — Long fuel tank (Stage 1 main fuel)
    part = fuelTank_long
    partName = Part_9a8b7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d
    pos = 0.0, 5.56, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    attPos = 0.0, -0.94, 0.0
    attDir = 0.0, -1.0, 0.0
    attAng = 0.0, 0.0, 0.0
    link = 1
    mirror = 0
    sym = 0
    symMethod = Mirror
    istg = 0
    dstg = -1
    sqor = 0
    ridx = -1
    
    RESOURCE
    {
        name = LiquidFuel
        amount = 400
        maxAmount = 400
    }
    RESOURCE
    {
        name = Oxidizer
        amount = 490
        maxAmount = 490
    }
}

PART
{
    // Index 3 — First stage engine LV-T30
    part = liquidEngine
    partName = Part_1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d
    pos = 0.0, 3.84, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    attPos = 0.0, -0.88, 0.0
    attDir = 0.0, -1.0, 0.0
    attAng = 0.0, 0.0, 0.0
    link = 2
    mirror = 0
    sym = 0
    symMethod = Mirror
    istg = 1
    dstg = 0
    sqor = 0
    ridx = -1
    
    MODULE
    {
        name = ModuleEngines
        thrustVectoringCapability = 0.5
        minThrust = 0
        maxThrust = 215
    }
}
```

## 2. Example with Radial Parts

```
ship = Rocket with Boosters
description = Example with radially attached solid boosters
version = 1.12.5
type = VAB
size = 2.5, 12.0, 2.5
vesselType = Ship

PART
{
    // Root part
    part = mk1-3pod
    partName = Part_root_uuid...
    pos = 0.0, 11.0, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    link = -1
    mirror = 0
    sym = 0
    symMethod = Mirror
    ...
}

PART
{
    // First stage fuel tank
    part = Rockomax32_BW
    partName = Part_main_tank_uuid...
    pos = 0.0, 7.0, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    link = 0
    mirror = 0
    ...
}

PART
{
    // First stage engine
    part = liquidEngine1-2    // Mainsail
    partName = Part_main_engine_uuid...
    pos = 0.0, 2.0, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    link = 1
    mirror = 0
    ...
}

PART
{
    // Radial solid booster A (original side)
    // Surface-attached to the main fuel tank side
    part = solidBooster
    partName = Part_boosterA_uuid...
    pos = 1.88, 5.0, 0.0
    rot = 1.0, 0.0, 0.0, 0.0      // Rotated 90 degrees outward
    link = 1
    mirror = 0                      // Original part
    sym = 1
    symMethod = Radial
    attPos = 0.0, -0.5, 0.0
    attDir = 1.0, 0.0, 0.0         // Connecting to the right
    srfN = srfAttach, 0.0, -0.5, 0.0, 1.0, 0.0, 0.0, 0, 0
    ...
}

PART
{
    // Radial solid booster B (symmetry copy)
    part = solidBooster
    partName = Part_boosterB_uuid...
    pos = -1.88, 5.0, 0.0
    rot = 1.0, 0.0, 0.0, 0.0
    link = 1
    mirror = 0                      // Not a mirror copy, but radial symmetry
    sym = 1
    symMethod = Radial
    attPos = 0.0, -0.5, 0.0
    attDir = -1.0, 0.0, 0.0        // Connecting to the left
    srfN = srfAttach, 0.0, -0.5, 0.0, -1.0, 0.0, 0.0, 0, 0
    ...
}
```

## 3. Quick Guide to Writing .craft Files

### Build Steps

```
1. Determine the root part → Write the first PART block (link = -1)
2. Attach child parts to the root → Write the second PART block (link = 0)
3. Continue adding → each time link points to the correct parent part index
4. Set up symmetry parts → Write the corresponding symmetry copies
5. Add RESOURCE and MODULE blocks
6. Save as plain text UTF-8 (no BOM)
7. Place in KSP/saves/<save>/Ships/VAB/ or SPH/
```

### Empirical Formula for Calculating pos Values

```
For stack attachment (Y-axis upward):

Child part's pos.Y = Parent part's pos.Y + Parent part's bottom node offset + Child part's top node offset

// Example:
// Parent part fuelTank (1m tall, bottom node at Y=-0.5): pos.Y = 7.0
// Child part fuelTank_long (2m tall, top node at Y=1.0): child pos.Y = 7.0 - 0.5 + 1.0 = 7.5
// But in practice, the child's attPos gives the relative position, and the game calculates from there
```

**The most reliable method**: Build a simple rocket in-game, save it, examine the pos value patterns in the .craft file, then edit manually.
