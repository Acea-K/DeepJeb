# Attach Nodes and Attachment System

> Compiled from KSP Wiki Part Modelling Guidelines, community discussions, and .cfg configuration documentation

## 1. Attachment Node Types

KSP has three part connection methods, each represented differently in .craft files.

### 1.1 Stack Attachment / Node Attachment

Node names: `top`, `bottom`, etc.
Direction: Along the part axis

Applies to:
- Serial staging of rocket stages
- Vertical connections between fuel tanks
- Engine-to-fuel tank connections

```
// Defined in .cfg
node_stack_top = 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0
node_stack_bottom = 0.0, -1.0, 0.0, 0.0, -1.0, 0.0, 0
```

Node format: `node_stack_<name> = posX, posY, posZ, dirX, dirY, dirZ, size`

### 1.2 Surface Attachment

Node name: `srfAttach`
Allows parts to attach at any surface position on another part.

Applies to:
- Radial engines
- Science instruments
- Struts/trusses
- Side-mounted fuel tanks

```
// Defined in .cfg
node_attach = 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0, 1
// Or:
// attachRules = 1, 0, 1, 0, 1
```

### 1.3 Docking Node

Node name: `dockingNode`
When `nodeType = 1`, it indicates a dockable node.

```
// Defined in .cfg
node_stack_dockingPort = 0.0, 0.5, 0.0, 0.0, 1.0, 0.0, 1
//                                                          ↑ size=1 indicates dockable
```

## 2. Node Representation in .craft Files

### 2.1 attN Field (Stack Node Connection)

When a child part connects to a parent part via a stack node:

```
PART
{
    part = fuelTank
    partName = Part_...
    ...
    link = 0
    attPos = 0.0, -0.5, 0.0       // Connection point position on the parent part (parent part local coordinates)
    attDir = 0.0, -1.0, 0.0       // Direction pointing to the parent part
    attAng = 0.0, 0.0, 0.0        // Angle
    attN = bottom, 0.0, -0.5, 0.0, 0.0, -1.0, 0.0, 0, 1
    //     ↑node name  ↑position         ↑direction           ↑type↑flow
}
```

**attN Format Details:**
```
attN = <name>, <posX>, <posY>, <posZ>, <dirX>, <dirY>, <dirZ>, <nodeType>, <resourceFlow>
```

| Part | Description |
|------|-------------|
| `name` | Node name, matching `node_stack_<name>` in .cfg |
| `posX,Y,Z` | Node position in the part's local coordinates |
| `dirX,Y,Z` | Node orientation (unit vector) |
| `nodeType` | 0=normal node, 1=dockable node |
| `resourceFlow` | 0=no flow, 1=flow enabled |

### 2.2 srfN Field (Surface Connection)

When a part is surface-attached to another part:

```
PART
{
    part = radialEngineMini
    ...
    link = 0
    attPos = -0.625, 0.5, 0.0
    attDir = -1.0, 0.0, 0.0
    srfN = srfAttach, -0.625, 0.5, 0.0, -1.0, 0.0, 0.0, 0, 0
    //     ↑fixed       ↑position         ↑direction           ↑type↑flow
}
```

## 3. Node Definition Format in .cfg

### 3.1 Stack Node

```
node_stack_<name> = <posX>, <posY>, <posZ>, <dirX>, <dirY>, <dirZ>, <size>
// Example (1.25m fuel tank):
node_stack_top = 0.0, 0.5, 0.0, 0.0, 1.0, 0.0, 0
node_stack_bottom = 0.0, -0.5, 0.0, 0.0, -1.0, 0.0, 0
```

| Part | Description |
|------|-------------|
| `pos` | Node position in the part model (local coordinates) |
| `dir` | Node orientation (unit vector), usually aligned with the Y axis |
| `size` | Node size, matching the corresponding radial dimension: 0=0.625m, 1=1.25m, 2=2.5m, 3=3.75m, 4=5m |

### 3.2 Surface Node

```
node_attach = <posX>, <posY>, <posZ>, <dirX>, <dirY>, <dirZ>, <size>, <dockingEnabled>
// Example (radial engine):
node_attach = 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0, 1
```

### 3.3 Attachment Rules

```
attachRules = <allowStack>, <allowSrfAttach>, <allowCollision>, <allowStageSrfAttach>, <allowStageCollide>
// Example: 1, 0, 1, 0, 1
```

| Position | Meaning | Value |
|----------|---------|-------|
| 1 | Allow stack attachment | 0/1 |
| 2 | Allow surface attachment | 0/1 |
| 3 | Allow collision enabled | 0/1 |
| 4 | Allow surface attachment during staging | 0/1 |
| 5 | Allow collision during staging | 0/1 |

### 3.4 Mk2/Mk3 Special Nodes

Mk2 and Mk3 cross-section parts have special node structures:

```
Mk2 cross-section (wing shape)
node_stack_top = 0.0, 0.333, 0.0, 0.0, 1.0, 0.0, 1
node_stack_bottom = 0.0, -0.333, 0.0, 0.0, -1.0, 0.0, 1

Mk3 cross-section (larger wing shape)
node_stack_top = 0.0, 0.5, 0.0, 0.0, 1.0, 0.0, 2
node_stack_bottom = 0.0, -0.5, 0.0, 0.0, -1.0, 0.0, 2
```

## 4. Node Size Matching (size)

> See [part-size-standards.md](part-size-standards.md) §1.1 Standard Size Table for dimensions.

Node size matching rules: **The parent node's size must match the child node**, otherwise the editor will display an error or not show connection points.

## 5. Node Orientation Conventions

```
top / bottom: Along the Y axis (default stack direction)
left / right: Along the X axis (horizontal)
front / back: Along the Z axis (front/back)

Orientation convention:
  Points outward: top faces up, bottom faces down
  For non-standard directions: defined by the model
```

## 6. Parts with Multiple Nodes

Some parts have **more than 2 stack nodes** (e.g., various adapters, cross connectors):

```
node_stack_top = 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 1
node_stack_bottom = 0.0, -1.0, 0.0, 0.0, -1.0, 0.0, 1
node_stack_left = -0.5, 0.0, 0.0, -1.0, 0.0, 0.0, 1
node_stack_right = 0.5, 0.0, 0.0, 1.0, 0.0, 0.0, 1
```

In craft files, each node is represented by `attN`.
