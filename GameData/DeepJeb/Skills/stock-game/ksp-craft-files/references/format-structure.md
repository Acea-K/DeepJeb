# File Format Structure Detailed Explanation

## 1. Top-Level Nodes

### ship
```
ship = My Rocket
```
Vessel name, the name displayed in-game.

### description
```
description = A heavy lifter for Mun missions
```
Description text, editable in the VAB/SPH editor.

### version
```
version = 1.12.5
```
The KSP version number under which the vessel was created.

### type
```
type = VAB
```
`VAB` or `SPH`, identifies which building the vessel was designed in. Determines the default coordinate system and symmetry mode in the editor.

### size
```
size = 2.5, 4.0, 2.5
```
`<x, y, z>` format, the overall bounding box dimensions of the vessel (meters). Used for preview zoom in VAB/SPH.

### vesselType
```
vesselType = Ship
```
Internal KSP vessel type identifier. Common values:

| Type Identifier | Description |
|---------|------|
| `Debris` | Debris |
| `Probe` | Unmanned probe |
| `Rover` | Rover |
| `Lander` | Lander |
| `Ship` | Ship |
| `Station` | Space station |
| `Base` | Base |
| `Plane` | Plane |
| `Relay` | Communications relay |
| `EVA` | EVA (Kerbal) |
| `Flag` | Flag |
| `DeployedScienceController` | Deployed science controller |
| `DeployedSciencePart` | Deployed science part |

## 2. PART Block Structure

Each PART block represents a part in the vessel, with blocks separated by `PART\n{` and `}`. Parts are organized in a **tree structure** —

### 2.1 Basic PART Block Example

```
PART
{
    part = Mark1Cockpit
    partName = Part_021b9e20-16d4-4abb-adc9-9beac1438972
    pos = 0.0, 0.5, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    attPos = 0.0, -0.5, 0.0
    attDir = 0.0, -1.0, 0.0
    attAng = 0.0, 0.0, 0.0
    link = -1
    mirror = 0
    sym = 0
    symMethod = Mirror
    istg = 0
    dstg = 0
    sqor = 0
    ridx = 0
    CrewCapacity = 1
    persistentId = 3489217349
}
```

### 2.2 PART with Modules and Resources

```
PART
{
    part = fuelTank
    partName = Part_6a2b1c30-...
    pos = 0.0, 1.5, 0.0
    rot = 0.0, 0.0, 0.0, 1.0
    ... ...
    MODULE
    {
        name = ModuleFuelTanks
        ...
    }
    RESOURCE
    {
        name = LiquidFuel
        amount = 400
        maxAmount = 400
    }
    RESOURCE
    {
        name = Oxidizer
        amount = 500
        maxAmount = 500
    }
}
```

### 2.3 Surface Attachment (Radial Mount) PART

```
PART
{
    part = radialEngineMini
    partName = Part_...
    pos = 0.0, 0.0, 0.625
    rot = 0.0, 0.0, 0.0, 1.0
    link = 0              // Parent part is the part at index 0
    attPos = -0.625, 0.5, 0.0
    attDir = -1.0, 0.0, 0.0
    mirror = 0
    sym = 1
    symMethod = Radial
    srfN = srfAttach, -0.625, 0.5, 0.0, -1.0, 0.0, 0.0, 0, 0
    // Surface attachment node information
}
```

### 2.4 Mirror Symmetry PART

```
PART
{
    part = radialEngineMini
    partName = Part_...
    pos = 0.0, 0.0, -0.625
    rot = 0.0, 0.0, 0.0, 1.0
    link = 0
    attPos = -0.625, 0.5, 0.0
    attDir = -1.0, 0.0, 0.0
    mirror = 1              // ← Mirror copy
    symMethod = Mirror
    sym = 1
    srfN = srfAttach, -0.625, 0.5, 0.0, -1.0, 0.0, 0.0, 0, 0
}
```

## 3. ConfigNode Parsing Rules for File Structure

KSP uses Squad's ConfigNode format to parse `.craft` files. The rules are as follows:

```
NODE_NAME
{
    key = value
    CHILD_NODE
    {
        ...
    }
}
```

- Key-value format: `key = value`
- Values can be strings, integers, floats, or comma-separated multi-values
- Child nodes are expressed as `NODE_NAME\n{ ... }`
- Comments start with `//`

## 4. Part Reference Order

The order of parts in the file is the order in which they are instantiated. The root part (first part's `link = -1`) is loaded first, and subsequent parts are loaded sequentially according to the tree structure.

- **link = -1**: Root part, no parent part
- **link = N** (N >= 0): Parent part is the Nth PART block in the file
