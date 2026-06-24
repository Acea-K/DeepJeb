# Parsers & Tools

> A collection of open-source tools hosted on GitHub for parsing/generating/editing .craft files, along with relevant source code analysis

## 1. Craft File Reader (Node.js)

- **Repository**: https://github.com/kspcommunity/Craft-File-Reader
- **npm**: `@kspcommunity/craft-file-reader`
- **Language**: Node.js
- **License**: Includes license file

### Features

Reads .craft files and extracts vessel information and part lists. Can be combined with Mod Parts Lister data to check for mod parts.

### Parsing Logic (from index.js source code analysis)

```javascript
function craftRead(filename) {
    const data = fs.readFileSync(filename, 'utf8');
    const lines = data.split('\n');
    
    // Top-level fields
    if (line.startsWith("ship"))      → craftData.ship
    if (line.startsWith("description")) → craftData.description
    if (line.startsWith("version"))   → craftData.version
    if (line.startsWith("size"))      → craftData.size
    if (line.startsWith("vesselType"))→ craftData.vesselType
    if (line.startsWith("type"))      → craftData.type
    
    // PART blocks
    if (line.startsWith("PART")) {
        isPartSection = true;
        // Skip the first line "PART"
    }
    if (isPartSection && line.startsWith("}")) {
        isPartSection = false;
    }
    // Count parts
    if (isPartSection && line.startsWith("part =")) {
        craftData.totalPartCount++;
        const partName = line.split('=')[1].trim().split('_')[0];
        craftData.parts.push(partName);
    }
}
```

### Usage Example

```javascript
const processCraftFile = require('@kspcommunity/craft-file-reader');
const result = await processCraftFile('/path/to/craft.craft');
// result.craftDetails → { ship, description, version, type, size, vesselType, totalPartCount, parts }
// result.partsDetails → [{ partName, modName, ... }, ...]
```

## 2. craft-parser (Python)

- **Repository**: https://github.com/rmeno12/craft-parser
- **Language**: Python 3
- **Features**: Parses .craft files for mass distribution, supports 3D visualization

### Parsing Functions

```python
# Basic mass calculation
CraftParser.calculate_mass(craft_filepath, masses) → int

# 3D mass distribution
CraftParser.calculate_mass_distribution_3d(craft_filepath, masses) → [[mass, [x,y,z]], ...]

# Plot 3D scatter
matplotlib + mpl_toolkits.mplot3d
```

### Field Parsing Patterns Extracted from Source

```python
for index, line in enumerate(craft_lines):
    if line == "PART":
        # PART block starts
        partname = craft_lines[index + 2].split()[2].split('_')[0]
        # part = xxx found at line index+2
        # pos is in one of the lines between index+3 and index+6
        for i in range(3, 7):
            if "pos = " in craft_lines[index + i]:
                pos = [float(coord) for coord in craft_lines[index + i].split()[2].split(',')]
                break
```

This parsing pattern tells us:
- The second line of a PART block (index+2) is `part = xxx`
- `pos = x,y,z` is within the first few lines of the PART block (index+3 to index+6)
- Dots in part names are replaced with underscores: `replace('.', '_')`
- Only the text before the first underscore is taken: `split('_')[0]`

### Mass Data (stockmasses.json)

Contains dry mass data for all stock parts, categorized by Aerodynamics, Command, Propulsion, FuelTank, etc. Includes 200+ parts.

## 3. KML (Kerbal Markup Lister)

- **Repository**: https://github.com/my-th-os/KML
- **Language**: C# (.NET / Mono)
- **Features**: KSP persistent file editor (supports sfs and craft files)

### Features

- Dual GUI and CLI modes
- Tree view display of XML structure
- Supports add/delete/modify of parts/vessels/Kerbals
- Auto-repair docking port and contract issues
- Supports selection and export

### CLI Commands

```bash
KML.exe <save-file> --vessels      # List vessels
KML.exe <save-file> --tree         # Display tree structure
KML.exe <save-file> --repair       # Auto-repair issues
KML.exe <save-file> --warnings     # Check warnings
```

## 4. KSP-Recall

- **Repository**: https://github.com/net-lisias-ksp/KSP-Recall
- **Purpose**: Fixes part positioning bugs in KSP 1.9+

### Issues Related to .craft Format

| Issue | Problem | Solution |
|-------|---------|----------|
| #32 | KSP 1.9+ corrupts part positions when loading craft | Preserve position values from craft during loading |
| #41 | Attachment node offset when procedural parts are root | Fix node initialization |
| #118 | TweakScale-induced part position distortion | Trust the pos in the craft file, do not recalculate |

**Key Finding (Issue #118)**:
> "The position of a part in the craft file should not be modified during loading. Only the nodes need to move, since nodes are not saved to the craft file."

## 5. io_object_mu (Blender Import/Export)

- **Repository**: https://github.com/taniwha/io_object_mu
- **Purpose**: Blender plugin supporting import/export of KSP .mu model files and .craft files
- **Language**: Python
- **License**: GPL v2

### Features
- Import .mu and .craft files into Blender
- Export .mu model files
- Supports material nodes (Cycles renderer)
- Part light fixes, collision mesh hiding

## 6. io_kspblender (Blender .craft Importer)

- **Repository**: https://github.com/spencerarrasmith/io_kspblender
- **Purpose**: Add-on specifically for importing .craft files into Blender
- **Author**: Spencer Arrasmith
- **License**: GPL v2
- **Blender Version**: 2.7.3+

### Features
- **Most complete .craft parsing engine** (ksparser.py): parses all known PART fields
- Automatically finds corresponding .mu model files from GameData/Parts directory
- Supports symmetry parts, surface attachment, and stack attachment
- Struts and fuel lines automatically create stretch constraints
- Complex vertex operations for launch clamps
- Parts automatically grouped by separation stage (dstg)

### Parsed PART Fields (ksparser.py)
| Category | Fields |
|----------|--------|
| Basic | `part`, `partName`, `partNumber`, `pos`, `rot`, `attPos`, `attPos0`, `attRot`, `attRot0` |
| Hierarchy | `linklist` (link object list), `partNumber` hierarchy |
| Symmetry | `mir`, `symMethod`, `symlist` (sym object list) |
| Stage | `istg`, `dstg`, `sidx`, `sqor`, `sepI`, `attm` |
| Connection | `attNlist` (attN object list), `srfNlist` (srfN object list) |
| Target | `tgt`, `tgtpos`, `tgtrot`, `tgtdir`, `cData` |
| Modification | `modCost`, `modMass`, `modSize` |
| Cross-section | `xsections` (unused) |

### Coordinate System Conversion
```
Unity (Y-up, left-handed) → Blender (Z-up, right-handed)
- Position: Swap Y↔Z
- Rotation: Swap Y↔Z + negate W
- Quaternion: Unity format (x,y,z,w), Blender format (w,x,y,z)
```

### Usage Steps
1. Create `kspdir.txt` in KSP installation directory, write the KSP path
2. Enable the plugin in Blender
3. File → Import → KSP Craft (.craft)
4. Select a .craft file to auto-import and locate parts

### Part Lookup Logic (part_dict.py)
- Scans all .cfg files under GameData
- Extracts `name =` (part name) and `model =`/`mesh =` (model path)
- Supports Mod exception handling (B9 Aerospace and other irregularly named mods)

## 7. Tool Comparison

| Tool | Language | Read | Write | Edit | GUI | Use Case |
|------|----------|------|------|------|-----|----------|
| Craft File Reader | Node.js | ✅ | ❌ | ❌ | ❌ | Batch analysis, Web services |
| craft-parser | Python | ✅ | ❌ | ❌ | ✅(3D plot) | Mass distribution analysis |
| KML | C# | ✅ | ✅ | ✅ | ✅ | Manual editing, repairs |
| io_object_mu | Python (.mu) | ✅ | ✅ | ❌ | ✅(Blender) | .mu model editing |
| io_kspblender | Python | ✅ | ❌ | ❌ | ✅(Blender) | 3D visualization, 3D printing |
| Text editor | — | ✅ | ✅ | ✅ | ✅ | Quick manual modifications |

## 8. .loadmeta Companion Files

> Source: FileInfo.com, KSP Forums (topic/190027)

`.loadmeta` files are cache companion files introduced in KSP 1.2.2+, accompanying `.sfs` or `.craft` files.

### Features
- Stores hash values for fast file integrity verification
- Speeds up file reading during game loading (avoids re-parsing complete data)
- Plain text format, viewable with any text editor

### Common Issues
- **Missing does not affect the game**: Loading will just be slightly slower
- **Safe to delete**: The game will automatically rebuild them
- **Corruption**: Just delete the file

**Regarding .craft files**: .craft files downloaded from platforms like KerbalX sometimes do not include .loadmeta — this is normal. The game works fine without .loadmeta.

## 9. Tips for Hand-writing .craft Files

Community experience:

1. **Start simple**: Copy an existing small craft file and modify its content rather than writing from scratch
2. **Getting part names**: Check the `name =` field in .cfg files under GameData/Squad/Parts/
3. **Part tree order**: The root part (link=-1) must be the first PART block
4. **Calculating pos**: When stacking, the child part's pos = parent part's pos + child part's attPos
5. **Shortcut for rot**: In most cases, use `0,0,0,1` (no rotation)
6. **Testing**: Load and test after changing one part at a time, rather than modifying everything at once
