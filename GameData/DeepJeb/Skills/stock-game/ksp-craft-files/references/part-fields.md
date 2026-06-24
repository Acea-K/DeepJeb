# PART Fields Complete Reference

> Compiled from multiple parser implementations (craft-parser, Craft-File-Reader) and community discussions

> Field mapping sources: io_kspblender's ksparser.py (most complete craft parser implementation), craft-parser, KSP-Recall source code analysis

## 1. Required Fields

| Field | Format | Description | Example |
|------|------|------|------|
| `part` | String | Part full name (with instance number suffix: `<cfg name>_<number>`) | `part = fuelTank_4293084140` |
| `partName` | String | Part short name (without number, corresponds to `name` in .cfg) | `partName = fuelTank` |
| `partNumber` | Integer | Numeric ID, **higher value = higher in hierarchy** (used to determine parent-child relationships) | `partNumber = 4293084140` |
| `pos` | `x, y, z` | Position in editor coordinate system (meters) | `pos = 0.0, 1.5, 0.0` |
| `rot` | `x, y, z, w` | Rotation represented as a quaternion (Unity format, w is scalar part) | `rot = 0.0, 0.0, 0.0, 1.0` |

## 2. Attachment-Related Fields

| Field | Format | Description | Typical Value |
|------|------|------|--------|
| `attPos` | `x, y, z` | Attachment point position relative to parent part | `attPos = 0.0, -0.5, 0.0` |
| `attDir` | `x, y, z` | Attachment direction vector | `attDir = 0.0, -1.0, 0.0` ← downward |
| `attAng` | `x, y, z` | Attachment angle (Euler angles) | `attAng = 0.0, 0.0, 0.0` |
| `attN` | Variable | Node attachment info line | `attN = top, 0.0, 0.5, 0.0, 0.0, 1.0, 0.0, 0, 0` |
| `srfN` | Variable | Surface attachment info line | `srfN = srfAttach, x,y,z, dx,dy,dz, 0, 0` |
| `link` | Integer | Index of parent part in the file (-1 for root part) | `link = 0` |

### 2.1 attN Format (Stack Node)

```
attN = <nodeName>, <posX>, <posY>, <posZ>, <dirX>, <dirY>, <dirZ>, <nodeType>, <resourceFlow>
```

- `nodeName`：Node name (e.g., `top`, `bottom`, `left`, `right`, etc.)
- `pos`：Node position (in local coordinates of the part)
- `dir`：Node direction vector
- `nodeType`：0=normal 1=can dock
- `resourceFlow`：0=cannot flow 1=can flow

### 2.2 srfN Format (Surface Attachment Node)

```
srfN = srfAttach, <posX>, <posY>, <posZ>, <dirX>, <dirY>, <dirZ>, <nodeType>, <resourceFlow>
```

Same format as attN, but `nodeName` is fixed to `srfAttach`.

## 3. Symmetry and Mirror

| Field | Format | Description | Typical Value |
|------|------|------|--------|
| `mirror` | 0 or 1 | Whether this is a mirror copy | `mirror = 1` |
| `sym` | Integer | Symmetry mode encoding | `sym = 0` (no symmetry), `sym = 1` (mirror) |
| `symMethod` | String | Symmetry method name | `Mirror` (mirror symmetry), `Radial` (radial symmetry) |
| `mir` | `x,y,z` | Mirror vector | `mir = 1, 1, 1` |
| `symCounterpartIdx` | Integer | Symmetry counterpart part index | - |
| `cpIdx` | Integer | Counterpart index (older versions) | - |

## 4. Hierarchy and Grouping

| Field | Format | Description | Typical Value |
|------|------|------|--------|
| `istg` | Integer | Independent stage number | `istg = 0` |
| `dstg` | Integer | Decouple stage number | `dstg = -1` |
| `sqor` | Integer | Stage queue order | `sqor = 0` |
| `ridx` | Integer | Resource index | `ridx = -1` |
| `sidx` | Integer | Stage sequence number | `sidx = -1` |
| `sepI` | Integer | Separation index | `sepI = 0` |
| `uid` | Integer | Unique ID | `uid = 0` |

## 5. Connection Relationships (Multi-line Fields)

These fields may appear multiple times in a .craft file, representing connection relationships between parts.

### 5.1 link — Parent Part Reference
```
link = <part_name>_<partNumber>
```
Records the parent part of this part. The game determines hierarchy based on the partNumber value.

### 5.2 attN — Stack Node Attachment (Multi-line)
```
attN = <loc>, <part_name>_<partNumber>
```

| Part | Description |
|------|------|
| `loc` | Node location name (e.g., `bottom`, `top`) |
| `part` | Full name of the child part connected to this node |
| `partNumber` | Numeric ID of the child part |

### 5.3 srfN — Surface Attachment (Multi-line)
```
srfN = srfAttach, <part_name>_<partNumber>
```

| Part | Description |
|------|------|
| Type | Fixed as `srfAttach` |
| `part` | Full name of the surface-attached part |
| `partNumber` | Numeric ID of the surface-attached part |

### 5.4 sym — Symmetry Copy (Multi-line)
```
sym = <part_name>_<partNumber>
```
Records the symmetry counterpart of this part.

## 6. Target/Strut Related Fields (for struts/fuel lines)

| Field | Format | Description |
|------|------|------|
| `tgt` | String | Target part full name (other end of strut/fuel line) |
| `tgtpos` | `x, y, z` | Target position |
| `tgtrot` | `x, y, z, w` | Target rotation |
| `tgtdir` | `x, y, z` | Target direction |
| `cData` | Variable | Connection data (contains target info) |

## 7. Modification Value Fields

| Field | Format | Description |
|------|------|------|
| `modCost` | Float | Modified additional cost (usually 0) |
| `modMass` | Float | Modified additional mass (usually 0) |
| `modSize` | `(x, y, z)` | Modified dimensions |

## 8. Initial Position/Rotation

| Field | Format | Description |
|------|------|------|
| `attPos` | `x, y, z` | Attachment position (parent part local coordinates) |
| `attPos0` | `x, y, z` | Initial attachment position |
| `attRot` | `x, y, z, w` | Attachment rotation |
| `attRot0` | `x, y, z, w` | Initial attachment rotation |

> Note: `attPos0` and `attRot0` are `(0,0,0)` for most parts, indicating no initial offset.

## 9. Surface Attachment Flag

| Field | Format | Description |
|------|------|------|
| `attm` | 0 or 1 | Whether it is surface attachment (1 = surface attach, 0 = stack attach) |

## 10. Part Properties

| Field | Format | Description |
|------|------|------|
| `CrewCapacity` | Integer | Crew capacity |
| `persistentId` | Long integer | Persistent ID (cross-save matching) |
| `flag` | String | Flag texture path |
| `color` | String | Color scheme |
| `Title` | String | In-game display name (overrides cfg) |
| `fuelCrossFeed` | Boolean | Whether cross-part fuel transfer is allowed |
| `temperatureModifier` | Float | Temperature modifier coefficient |
| `skinTemperatureModifier` | Float | Skin temperature modifier coefficient |
| `heatConductivity` | Float | Thermal conductivity |
| `skinHeatConductivity` | Float | Skin thermal conductivity |
| `radiatorHeadroom` | Float | Radiator headroom |
| `thermalMassModifier` | Float | Thermal mass modifier coefficient |
| `emissiveConstant` | Float | Emissive constant |
| `dragModelType` | String | Drag model type |
| `maximum_drag` | Float | Maximum drag coefficient |
| `minimum_drag` | Float | Minimum drag coefficient |
| `angularDrag` | Float | Angular drag coefficient |
| `crashTolerance` | Float | Crash tolerance |
| `maxTemp` | Float | Maximum temperature (K) |
| `skinMaxTemp` | Float | Maximum skin temperature (K) |
| `stagingIcon` | String | Staging icon type |
| `stageOffset` | Integer | Stage offset |
| `stageBefore` | Boolean | Stage before |
| `stageAfter` | Boolean | Stage after |
| `rescaleFactor` | Float | Rescale factor (for mods) |

## 11. MODULE Sub-blocks

Parts can contain any number of `MODULE` blocks, each identified by the `name` field:

```
MODULE
{
    name = ModuleEngines
    thrustVectoringCapability = 0.5
    minThrust = 0
    maxThrust = 215
    ...
}
```

Common module names:

| Module Name | Purpose |
|--------|------|
| `ModuleEngines` | Engine parameters |
| `ModuleFuelTanks` | Fuel tank parameters |
| `ModuleRCS` | RCS thrusters |
| `ModuleReactionWheel` | Reaction wheel/SAS |
| `ModuleLandingLeg` | Landing legs |
| `ModuleParachute` | Parachutes |
| `ModuleDockingNode` | Docking ports |
| `ModuleDeployableSolarPanel` | Solar panels |
| `ModuleScienceExperiment` | Science experiments |
| `ModuleResourceConverter` | Resource conversion |
| `ModuleControlSurface` | Control surfaces |
| `ModuleCommand` | Command pod |

## 12. RESOURCE Sub-blocks

```
RESOURCE
{
    name = LiquidFuel
    amount = 400
    maxAmount = 400
    flowState = True      // Whether flow is allowed (optional)
}
```

- `name`：Resource name
- `amount`：Current amount
- `maxAmount`：Maximum capacity
- `flowState`：Flow state (optional, default True)

Common resource names: `LiquidFuel`, `Oxidizer`, `MonoPropellant`, `ElectricCharge`, `XenonGas`, `SolidFuel`, `Ore`, `Ablator`
