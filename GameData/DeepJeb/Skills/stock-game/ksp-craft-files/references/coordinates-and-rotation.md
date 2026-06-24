# Coordinate Systems, Position and Rotation

> This document is compiled from KSP community discussions, craft file parser source code, and KSP-Recall bug analysis

## 1. KSP Coordinate System (Unity Coordinate System)

KSP uses a **left-handed coordinate system** (Unity standard):

- **X axis**: Rightward (viewed from the front of the VAB/SPH)
- **Y axis**: Upward
- **Z axis**: Forward (toward the back wall of the VAB/SPH)
- **1 unit = 1 meter**

### 1.1 Coordinate System Reference

```
       +Y (Upward)
        |
        |
        +----→ +X (Rightward)
       /
      /
    ↓ +Z (Forward, from screen into the game)
```

### 1.2 VAB vs SPH Coordinate Differences

| Editor | Description |
|--------|------|
| **VAB** | Y axis upward (rockets built vertically), Z is the "forward" direction |
| **SPH** | Z axis corresponds to horizontal forward (aircraft built horizontally) |

> Note: The KSP kOS documentation mentions SHIP-RAW and SOI-RAW coordinate systems, with the latter relative to celestial bodies. Coordinates in .craft files are editor local coordinates.

## 2. pos Field — Position

### Format
```
pos = <x>, <y>, <z>
```

Three floats, comma-separated. The position is the part's coordinates in the editor coordinate system.

### Explanation

- The root part (`link = -1`) is typically near the origin: `pos = 0.0, 0.0, 0.0`
- Subsequent parts store positions in **editor world coordinates**, not parent part local coordinates
- However, in some versions, pos is an offset **relative to the parent part**? This is a point of debate:
  - KSP-Recall Issue #118 discussion states: **part positions in the craft file do not need to be changed on load**, the game trusts the position values in the craft file
  - However, the craft-parser 3D distribution plotting code directly uses pos values to draw scatter plots, indicating pos is "absolute" or at least usable world coordinates

**Practical conclusion**: pos values are considered "correct positions" when the game loads them — the game does not recalculate them (unless encountering a bug). When saving, the editor writes a snapshot of the part's position **relative to the editor origin** or **the part's current world position**.

### Common Coordinate Scenarios

```
// A two-stage rocket
PART  // Root command pod
{
    part = mk1pod
    pos = 0.0, 1.0, 0.0     // 1 meter above the origin
    link = -1
}
PART  // Fuel tank (stacked below the command pod)
{
    part = fuelTank
    pos = 0.0, 0.0, 0.0     // 1 meter below the command pod (in relative position terms)
    link = 0
    attPos = 0.0, -0.5, 0.0
    attDir = 0.0, -1.0, 0.0
}
```

## 3. rot Field — Rotation Quaternion

### Format
```
rot = <x>, <y>, <z>, <w>
```

Four floats, representing a normalized quaternion.

### Introduction to Quaternions

A quaternion is a way to represent 3D rotation using 4 components:

- `q = (x * i + y * j + z * k + w)` where `x² + y² + z² + w² = 1`
- `(x, y, z)` is the rotation axis direction multiplied by sin(θ/2)
- `w` = cos(θ/2)
- `θ` is the rotation angle around that axis

### Common Quaternion Values

| rot value | Meaning |
|--------|------|
| `0, 0, 0, 1` | No rotation (identity quaternion) |
| `0, 0, 0, -1` | No rotation (equivalent) |
| `0, 0, 1, 0` | 180° rotation around Z axis |
| `0, 1, 0, 0` | 180° rotation around Y axis |
| `1, 0, 0, 0` | 180° rotation around X axis |
| `0, 0.707, 0, 0.707` | 90° rotation around Y axis |
| `0, -0.707, 0, 0.707` | -90° rotation around Y axis |

### Record from KSP Community Discussions

From KSP forum `topic/51800` (summary):

> "rot appears to be a **half-angle unit vector** — meaning the vector length is sin(θ/2), the corresponding axis direction is (x, y, z), and w = cos(θ/2)"
>
> "I've been putting the x and z components of the unit vector pointing from PART to its connected part into the 2nd and 4th components of the quaternion… this is actually the Unity standard way."

So **rot in `.craft` files is the standard Unity quaternion**.

### Key Considerations for rot

1. **Normalization requirement**: The quaternion must be normalized (length == 1), otherwise the game may behave unexpectedly
2. **Equivalence**: `q` and `-q` represent the same rotation, so `0,0,0,1` and `0,0,0,-1` are equivalent
3. **Editor vs flight**: The rotation in the editor may differ from in-flight — some parts are affected by physics after launch and acquire rotation
4. **Old version compatibility**: Very early KSP versions may have used a different rotation representation

## 4. attPos and attDir — Attachment Position and Direction

### attPos (Attachment Position)

```
attPos = <x>, <y>, <z>
```

The connection point position on this part, in **the parent part's local coordinate space**.

### attDir (Attachment Direction)

```
attDir = <dx>, <dy>, <dz>
```

The direction vector pointing to the parent part, in the parent part's local coordinate space. For example:

- `attDir = 0.0, -1.0, 0.0`：Connected from below (standard stack, parent part is above)
- `attDir = 1.0, 0.0, 0.0`：Connected from the right
- `attDir = 0.0, 0.0, 1.0`：Connected from the front

### Relationship Between attPos and pos

```
pos(child) = pos(parent) + directional offset of attPos(child)
```

Simplified understanding:
- attPos is the connection position of the child part **relative to the parent part**
- pos is the position of the child part **in editor world space**
- The two should be consistent (at least in theory)

## 5. attAng — Attachment Angle

```
attAng = <x>, <y>, <z>
```

Represents the attachment rotation using Euler angles (degrees? radians?). In many craft files, this value is `0.0, 0.0, 0.0` (no additional rotation).

## 6. Position-Related Bugs (from KSP-Recall)

### 6.1 Part Position Offset Bug (KSP-Recall Issue #41)

When Procedural Parts is used as the root part, attachment nodes become **offset** from the part, causing child part positions to be incorrect.

### 6.2 Position Recalculation on Load Issue (Issue #118)

> "My understanding is that the part positions in the craft file do not need to be changed on load. Only the nodes need to be moved because they are not saved with the craft file."

Key insight:
- **The craft file saves the part's final render position**, which is trusted when loaded
- **Attachment nodes (attachNodes) are NOT saved to the craft file** — they are regenerated from the .cfg configuration every time a part is loaded
- This means that if a part was scaled (TweakScale) or deformed (Procedural Parts) after saving, the node positions on load may not match the part positions recorded in the craft file

### 6.3 KSP 1.9 + Craft File Corruption (KSP-Recall)

KSP 1.9+ may incorrectly modify part positions when loading craft files. KSP-Recall has specific workarounds to prevent this behavior.

## 7. kOS and Craft Coordinates

The reference frames defined in kOS documentation differ from coordinates directly used in craft files:

| kOS Frame | Description |
|--------|------|
| SHIP-RAW | Origin at the CPU vessel, rotation same as KSP raw grid |
| SOI-RAW | Celestial body-centered, rotation same as KSP raw grid |

> The pos coordinates in craft files are different from the frames above — they are **editor space coordinates**, not physical world coordinates in flight.
