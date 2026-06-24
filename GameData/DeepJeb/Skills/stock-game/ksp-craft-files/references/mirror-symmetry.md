# Mirror, Symmetry, and Part Tree Structure

> Compiled from KSP community discussions and editor behavior analysis

## 1. Part Tree Structure

The structure of a vessel in KSP is a **tree** — each part has exactly one parent part (except the root part).

```
          [Root Part: mk1pod]  ← link = -1, root
           /             \
          /               \
    [fuelTank]      [fuelTank]    ← link = 0, symmetric copy
         |                 |
    [LV-T30]           [LV-T30]   ← link = 1 and link = 2
```

### 1.1 Representation in .craft Files

```
PART  // Index 0 — Root part
{
    part = mk1pod
    link = -1
    ...
}
PART  // Index 1 — Stacked below the root part
{
    part = fuelTank
    link = 0
    ...
}
PART  // Index 2 — Mirror-symmetric fuel tank
{
    part = fuelTank
    link = 0
    mirror = 1
    ...
}
```

### 1.2 Limitations of the Tree Structure

From KSP forum `topic/187700` discussion:

> "In KSP 1, the vessel structure is tree-shaped, each part has only one parent node, all the way to the root part. This is a limitation for construction — it means you can't have circular dependencies (two parts being each other's parent)."

This limitation means:
- Parts cannot form circular structures
- Fuel lines can create cross-feed paths, but structurally it is still a tree
- KSP 2 had planned to introduce more flexible structures

## 2. Symmetry System

The KSP editor supports two symmetry modes, recorded in .craft files by the `symMethod` field.

### 2.1 Mirror Symmetry

Default for the SPH (Space Plane Hangar). Parts appear in pairs on either side of the symmetry axis.

```
symMethod = Mirror
```

In craft files:
```
// Original
PART (part = deltaWing, link = 0, mirror = 0, symMethod = Mirror)
// Mirror copy  
PART (part = deltaWing, link = 0, mirror = 1, symMethod = Mirror)
```

`mirror` field:
- `mirror = 0`: Original part
- `mirror = 1`: Mirror copy

### 2.2 Radial Symmetry

Default for the VAB (Vehicle Assembly Building). Parts are evenly distributed around a central axis.

```
symMethod = Radial
```

In craft files:
```
// Original
PART (part = radialEngineMini, link = 0, mirror = 0, symMethod = Radial, sym = 2)
// Symmetric copy 1
PART (part = radialEngineMini, link = 0, mirror = 0, sym = 2)  // mirror is not necessarily 1
// Symmetric copy 2 (more if sym=3)
```

### 2.3 sym Field

```
sym = <count>
```

- `sym = 0`: No symmetry
- `sym = 1`: Symmetric (paired)
- `sym = 2`: 3-way symmetry (equilateral triangle distribution) — sym value = symmetry count - 1?
- ...

**Note**: The exact encoding of the sym field may differ across KSP versions. A safer understanding is that the sym value describes the "type" of symmetry, rather than a simple count.

## 3. mirror Field Details

```
mirror = <0|1>
```

- `0`: Original part (or non-symmetric part)
- `1`: Mirror copy

### 3.1 Coordinate Handling When mirror = 1

The position of the mirror copy is automatically calculated by the game based on the symmetry mode and the original part's position. This means:

- The mirror copy's `pos` field is not necessarily equal to the original part's X value negated
- In some cases (especially for surface attachments), the mirror copy's pos is directly written as the calculated value
- Editing the mirror copy's pos may cause position desynchronization between the two sides

### 3.2 Mirror and Rotation

Mirrored parts "flip" the rotation quaternion — specifically, if the original part's quaternion is `(x, y, z, w)`, mirroring is equivalent to negating the components of the corresponding axis. The actual calculation depends on the symmetry axis:

```
Symmetry axis is X: rot_x_mirror = (x, -y, -z, w) or approximate
Symmetry axis is Y: rot_y_mirror = (-x, y, -z, w)
Symmetry axis is Z: rot_z_mirror = (-x, -y, z, w)
```

These are approximations; the actual computation is done by the Unity engine.

## 4. Symmetry Count and Editor Interaction

Users can toggle symmetry mode (Mirror↔Radial) in the editor with the R key, and adjust the symmetry count by clicking the symmetry ring:

| Operation | Result |
|-----------|--------|
| R key | Toggle Mirror/Radial mode |
| Shift + Click on symmetry ring | Increase symmetry count (2→4→6→8→...) |
| Click on symmetry ring | Cycle symmetry count (2→3→4→6→8) |

## 5. Subassembly Symmetry

Subassemblies are saved as separate .craft files in the `Subassemblies/` directory. When a subassembly is loaded, its part tree is inserted into the current vessel. Symmetry information is copied along with the parts.

## 6. Reddit Community Records

From `r/KerbalSpaceProgram` post `90xnr2`:

> "The vessel is made of parts structured like a tree, with the root part at the top and all parts connected to it. A part has a mirror-symmetric counterpart attached on the other side."

> "mirror = 0 is the original part, mirror = 1 is the mirrored copy."

> "pos = x, y, z is the position of the part in space."

> "rot = x, y, z, w is the rotation represented as a quaternion. Importantly, it is not a direction vector — it is a quaternion."
