---
name: ksp-craft-files
description: >-
  Use when you need to understand, parse, generate, or debug Kerbal Space Program
  .craft files. Covers file format structure, all PART fields, coordinate system
  and rotation (quaternions), attachment nodes (stack/surface), mirror symmetry
  and part tree, radial size standards (0.625m-7.5m), stock part mass/dimension
  reference, parsing tools and libraries, troubleshooting position/rotation bugs,
  and .craft vs .sfs persistence comparison. Includes annotated file examples for
  simple rockets and radial booster configurations. Compiled from multiple parser
  implementations (io_kspblender/ksparser.py, craft-parser, Craft-File-Reader),
  KSP-Recall issue analysis, KSP forums, and community documentation.
version: 1.0.0
author: Acea
license: MIT
metadata:
  hermes:
    tags: [gaming, ksp, file-format, kerbal-space-program, craft-files]
    related_skills: [game-file-format-research]
platforms: [linux, macos, windows]
---

# KSP Craft Files — Knowledge Base

## Overview

Kerbal Space Program `.craft` files are plain-text files that describe a player-designed spacecraft, stored in `KSP/saves/<save>/Ships/VAB/` or `SPH/`. Each file defines a tree of parts — their positions, rotations, attachments, resources, modules, stage configuration, and symmetry relationships.

This knowledge base covers the format in detail, compiled from the source code of multiple independent parsers (Python, Node.js, C#), KSP-Recall bug analysis, forum discussions, and community documentation.

## When to Use

- You need to **read or parse** a .craft file programmatically
- You need to **generate** a .craft file (hand-writing or script-generated subassemblies)
- You are **debugging** a broken craft (position offset, rotation flip, missing parts, symmetry loss)
- You need to **convert** between .craft and .sfs persistence format
- You need **part dimensions or mass data** for building or modding
- You are researching **KSP's internal data model** (part tree, attachment nodes, coordinate system)

## Quick Reference

### File Structure Skeleton

```
ship = <name>
description = <text>
version = <KSP version>
type = VAB|SPH
size = <x,y,z>
vesselType = Ship|Plane|Lander|Probe|Station|Rover|Base|Relay|...

PART { ... }
PART { ... }
```

### Essential PART Fields

```
PART
{
    part = <name>_<number>          # full name with instance number
    partName = <cfg_name>           # clean part name (from .cfg name=)
    partNumber = <int>              # higher = higher in hierarchy
    pos = <x>, <y>, <z>            # world position (Unity Y-up, 1u=1m)
    rot = <x>, <y>, <z>, <w>       # quaternion rotation
    attPos = <x>, <y>, <z>         # attachment point in parent local space
    attDir = <dx>, <dy>, <dz>      # attachment direction
    link = <parent_index|-1>       # -1 = root part
    mirror = 0|1                   # 0=original 1=mirrored copy
    symMethod = Mirror|Radial
    istg = <stage>
    dstg = <decouple_stage>
    attm = 0|1                     # 0=stack 1=surface attach

    attN = <loc>, <part>_<number>  # node attachment (multi-line)
    srfN = srfAttach, <part>_<num> # surface attachment (multi-line)
    sym = <part>_<number>          # symmetry counterpart (multi-line)
    tgt = <part>_<number>          # strut/fuel-line target

    MODULE { name = ModuleEngines ... }
    RESOURCE { name = LiquidFuel amount = X maxAmount = X }
}
```

### Common Quaternions

| Rotation | `rot` value |
|----------|-------------|
| Identity (no rotation) | `0, 0, 0, 1` |
| Y-axis 90° | `0, 0.707, 0, 0.707` |
| Y-axis 180° | `0, 1, 0, 0` |
| Y-axis -90° | `0, -0.707, 0, 0.707` |
| X-axis 90° | `0.707, 0, 0, 0.707` |
| Z-axis 90° | `0, 0, 0.707, 0.707` |

### Radial Sizes

| Name | Diameter | Node Size |
|------|----------|-----------|
| XS / Size0 | 0.625m | 0 |
| SM / Size1 / Mk1 | 1.25m | 1 |
| Size1.5 (MH) | 1.875m | 1.5 |
| MD / Size2 / Mk2 | 2.5m (airfoil) | 2 |
| LG / Size3 / Mk3 | 3.75m (large airfoil) | 3 |
| Size4 (MH) | 5.0m | 4 |
| Size5 (Mod) | 7.5m+ | 5 |

### Common Stock Part Names

| Internal Name | In-Game Name |
|---------------|--------------|
| `mk1pod` | Mk1 Command Pod |
| `mk1-3pod` | Mk1-3 Command Pod |
| `Mark1Cockpit` | Mk1 Cockpit |
| `fuelTank` | FL-T400 Fuel Tank |
| `fuelTank_long` | FL-T800 Fuel Tank |
| `liquidEngine` | LV-T30 "Reliant" |
| `liquidEngine2` | LV-T45 "Swivel" |
| `liquidEngine1-2` | "Mainsail" Engine |
| `nuclearEngine` | LV-N "NERV" |
| `solidBooster` | RT-10 Solid Booster |
| `probeCoreCube` | QBE Probe Core |

## Reference Files

Detailed content is extracted into flat reference files under `references/`. Each covers one topic:

| File | Content |
|------|---------|
| [format-structure.md](references/format-structure.md) | Top-level nodes, PART block structure, ConfigNode parsing rules, part reference ordering |
| [part-fields.md](references/part-fields.md) | Complete field reference: required, attachment, symmetry, hierarchy, target, modifier, module, resource |
| [coordinates-and-rotation.md](references/coordinates-and-rotation.md) | Unity Y-up coordinate system, `pos`, quaternion `rot`, `attPos`/`attDir`, KSP-Recall position bugs |
| [nodes-and-attachments.md](references/nodes-and-attachments.md) | Stack vs surface vs docking nodes, `attN`/`srfN` format, Mk2/Mk3 special nodes, `attachRules` |
| [mirror-symmetry.md](references/mirror-symmetry.md) | Tree structure, mirror/radial symmetry, `mirror`/`sym`/`symMethod`/`mir` fields |
| [part-size-standards.md](references/part-size-standards.md) | All radial sizes (0.625m–7.5m), Mk1/Mk2/Mk3 cross-sections, exact dimensions of core parts |
| [stock-part-reference.md](references/stock-part-reference.md) | Mass table (200+ stock parts), resource densities |
| [parser-tools.md](references/parser-tools.md) | Craft-File-Reader (Node.js), craft-parser (Python), io_kspblender (Blender), KML (C#), .loadmeta files |
| [troubleshooting.md](references/troubleshooting.md) | Position offset, rotation flip, crash-on-load, symmetry loss, cross-version issues, part mismatch |
| [examples-and-practice.md](references/examples-and-practice.md) | Annotated two-stage rocket, radial booster example, hand-writing guide, position calculation |
| [craft-vs-sfs.md](references/craft-vs-sfs.md) | .craft vs .sfs comparison, ORBIT block fields, SIT values, extraction method |

### Raw Source Code (in `references/`)

The following files were extracted directly from the [io_kspblender](https://github.com/spencerarrasmith/io_kspblender) parser library and are the primary source of truth for the field mapping in this knowledge base:

| File | Lines | Purpose |
|------|-------|---------|
| [ksparser.py](references/ksparser.py) | 257 | Core .craft file parser — `kspcraft()` and `part()` classes, full field list, Unity→Blender coordinate conversion |
| [import_craft.py](references/import_craft.py) | 852 | Blender import pipeline — part placement, strut/fuel-line constraints, material setup, 3D-print helpers |
| [part_dict.py](references/part_dict.py) | 190 | Part dictionary builder — scans GameData .cfg files to map part names → .mu model paths |

> **License note**: The three `.py` files above are derived works of the io_kspblender project and retain their **original GNU General Public License v2 (GPL-2.0)** grant. They are included under the aggregation clause of GPL-2.0 §3 as independent works for reference and analysis only. The GPL-2.0 license text is included at [references/LICENSE.GPL-2.0](references/LICENSE.GPL-2.0). All other files in this skill are distributed under the MIT license as stated in the frontmatter.

## Common Pitfalls

1. **`part` vs `partName` are different** — `part = fuelTank_4293084140` (includes instance number), `partName = fuelTank` (clean cfg name). Some parsers only read one.

2. **`rot` is a quaternion, not Euler angles** — `rot = x, y, z, w` where x²+y²+z²+w² must equal 1. `0,0,0,1` = no rotation. Don't mistake for a `(pitch, yaw, roll)` Euler triple.

3. **Positions are trusted on load** — KSP does NOT recompute part positions from attachment nodes when loading a craft. The `pos` value is authoritative. TweakScale/ProceduralParts bugs arise because nodes change size but pos stays the same.

4. **Nodes are NOT saved in craft files** — `attachNodes` are regenerated from `.cfg` files at load time. If a part was scaled after saving, the regenerated nodes won't match the saved positions.

5. **`mirror=1` rotation should not be edited** — Manually editing a mirrored part's `rot` breaks asymmetry. Always edit the original (`mirror=0`) and let the game compute the copy.

6. **`link` index is 0-based, in file order** — The root part has `link=-1`. Child parts reference their parent by its position in the PART list (first PART=0, second=1...).

7. **`partNumber` determines hierarchy** — Higher numeric value = higher in the part tree, independent of file order. Used by link, attN, srfN, and sym references.

8. **Missing parts = mod dependency** — If a craft references `part = someModPart_12345` and the mod isn't installed, the game silently drops the part. Check `GameData/` for the mod.

9. **KSP 1.9+ position bug** — KSP 1.9 introduced a regression where craft positions are shifted on load. Install KSP-Recall to fix, or manually adjust `pos` values.

## Verification Checklist

- [ ] Frontmatter has `name`, `description` (≤1024 chars), `version`, `author`, `license`, `metadata.hermes.{tags, related_skills}`
- [ ] SKILL.md starts with `---` at byte 0
- [ ] Reference files are flat in `references/` — no subdirectories
- [ ] Reference files have no YAML frontmatter
- [ ] Reference filenames are lowercase-kebab, no numerical prefixes
- [ ] Cross-references use relative flat paths: `[topic.md](topic.md#section)`
- [ ] Every `pos`, `rot` example uses the correct Unity Y-up coordinate convention
- [ ] All stock part names match actual `name=` values from Squad .cfg files
- [ ] Quaternions in examples are normalized (x²+y²+z²+w² ≈ 1.0)
