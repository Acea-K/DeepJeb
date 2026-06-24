# .craft Files vs .sfs Persistent Saves Comparison

> Sources: KSP Wiki (Orbit), Reddit (r/KerbalAcademy), KSP Forum (topic/96546), FileInfo.com

## 1. Core Differences

| Dimension | .craft File | .sfs File (Persistent Save) |
|-----------|-------------|-----------------------------|
| Purpose | Vessel template in the editor | Launched vessel in the game world |
| Location | `Ships/VAB/` or `Ships/SPH/` | `saves/<save name>/persistent.sfs` |
| Orbital data | None | Yes (`ORBIT { ... }` block) |
| Situation | None | Yes (`SIT = LANDED`, etc.) |
| Mission info | None | Yes (crew, contracts, etc.) |
| File extension | `.craft` | `.sfs` |
| Companion file | None (but .craft files downloaded from KerbalX sometimes include `.loadmeta`) | `.loadmeta` (cache, optional) |

## 2. VESSEL Block in .sfs Files

The VESSEL block in .sfs is essentially a **superset** of .craft — it adds orbital/situation information on top of the `PART` blocks.

### 2.1 VESSEL Block Structure

```
VESSEL
{
    pid = <unique ID>
    name = <name>
    sit = LANDED|FLYING|ORBITING|SUB_ORBITAL|ESCAPING|PRELAUNCH|...
    landed = True|False
    landedAt = <location name>
    displaylandedAt = <display name>
    splashed = True|False
    met = <flight time (seconds)>
    lct = <last contact time>
    root = <root part index>
    lat = <latitude>
    lon = <longitude>
    alt = <altitude>
    hgt = <terrain height>
    nrm = <normal direction vector>
    rot = <rotation quaternion>
    CoM = <center of mass offset>
    CoMoffset = <center of mass offset amount>
    Stg = <number of stages>
    Prst = <persistent flag>
    ref = <reference frame>
    ctrl = <control state>
    
    // Orbital data (if in orbit)
    ORBIT
    {
        SMA = <semi-major axis>
        ECC = <eccentricity>
        INC = <inclination>
        LPE = <argument of periapsis>
        LAN = <longitude of ascending node>
        MNA = <mean anomaly>
        EPH = <epoch>
        REF = <reference body>
        OBJ = <celestial body name>
    }
    
    // VesselModules (optional)
    MODULE
    {
        name = ModuleDockingNode
        ...
    }
    
    // PART blocks (same as .craft)
    PART
    {
        ...
    }
}
```

### 2.2 SIT (Situation) Enum Values

| sit value | Meaning |
|-----------|---------|
| PRELAUNCH | Pre-launch (on launch pad/runway) |
| LANDED | Landed |
| SPLASHED | Splashed (on water) |
| FLYING | Flying (in atmosphere) |
| SUB_ORBITAL | Sub-orbital |
| ORBITING | In orbit |
| ESCAPING | Escaping |
| DOCKED | Docked |

### 2.3 ORBIT Block Fields

| Field | Meaning | Unit |
|-------|---------|------|
| SMA | Semi-major axis | meters |
| ECC | Eccentricity | — |
| INC | Inclination | degrees |
| LPE | Argument of periapsis | degrees |
| LAN | Longitude of ascending node | degrees |
| MNA | Mean anomaly | degrees |
| EPH | Epoch | seconds |
| REF | Reference body index | — |
| OBJ | Celestial body name | — |

## 3. Extracting .craft from .sfs

The community forum mentions a method for extracting a vessel from .sfs to .craft:

> "You can open persistent.sfs or quicksave.sfs, find the vessel's VESSEL block, copy the PART sections into a new .craft file."

### Extraction Steps

```
1. Open persistent.sfs
2. Search for the vessel name (in the VESSEL { name = <vessel name> line)
3. Find the corresponding VESSEL { ... } block
4. Delete the front part of the VESSEL block (pid, name, sit, landed, ORBIT, etc.)
5. Keep all PART { ... } blocks
6. Add the necessary top-level fields:
   ship = <vessel name>
   description = Extracted from persistence
   version = 1.12.5
   type = VAB
   size = 0, 0, 0
   vesselType = Ship
7. Save as a .craft file
8. Place into the Ships/VAB/ directory
```

**Important Notes**:
- Orbital vessel position coordinates are celestial body-relative, different from editor coordinates
- Part rotation is consistent between both formats
- PART blocks in .sfs typically contain more fields than .craft (such as `flightID`, `inStageIndex`, and other flight-time state)

## 4. Terminology Notes

Based on community usage, these three terms are essentially equivalent. See README §Core Concepts for details.

> **Note**: .craft files describe the "design blueprint" in the editor, while VESSEL blocks in .sfs describe entities that already exist in the game world (including orbit, status, etc.). Both use the same PART format, but VESSEL contains richer situational data.
