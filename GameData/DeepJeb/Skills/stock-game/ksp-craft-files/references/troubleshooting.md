# Common Issues & Debugging

> Compiled from KSP-Recall Issue discussions, community forum Reddit Q&A, and parser development experience

## 1. Part Position Errors

### Symptoms
- After loading a vessel, some parts are not in the correct position
- Parts are floating or clipping into each other
- Parts bouncing after loading

### Possible Causes

#### 1.1 Caused by TweakScale / Procedural Parts
**Issue #118 (KSP-RO/ProceduralParts)**:
- TweakScale scales parts during loading, modifying model dimensions, but the pos in .craft still has the pre-scaling values
- The game trusts the pos in .craft, but visually the parts appear misaligned
- **Solution**: Disable position recalculation during loading in KSP-Recall or ProceduralParts settings

#### 1.2 KSP 1.9+ craft loading bug
**Issue #32 (KSP-Recall)**:
- KSP 1.9+ incorrectly modifies part positions during loading
- Manifests as position offset after saving and reloading
- **Solution**: Install the KSP-Recall Mod, or manually correct the pos values in .craft

#### 1.3 Node initialization at zero
**Issue #41 (KSP-Recall)**:
- When Procedural Parts are used as the root part, attachment nodes are not properly initialized
- Child parts have correct pos in .craft, but the node position is (0,0,0)
- **Solution**: Force node position initialization during loading

### Debugging Methods

1. Open the .craft file with a text editor and check if pos values are reasonable
2. Compare pos patterns of normal parts:
   ```
   Normal stacking: Y values of pos increase/decrease along the part centerline
   root: pos = (0.0, 0.5, 0.0)
   child1: pos = (0.0, -0.44, 0.0)  // Below root
   child2: pos = (0.0, -1.38, 0.0)  // Below child1
   ```
3. Check if attPos and attDir correctly point to the parent part
4. Verify the link index correctly points to the parent part

## 2. Rotation Errors

### Symptoms
- Part orientation is wrong
- Engine facing the wrong direction
- Wings flipped

### Possible Causes

#### 2.1 Quaternion errors
- `rot` is not a unit quaternion (length ≠ 1)
- Quaternion component order is wrong (game expects `x,y,z,w`)
- w component is 0 (theoretically represents a 180° rotation, but is often an error value)

#### 2.2 Mirror-induced orientation flip
- The rot of mirror copies (mirror=1) is auto-calculated
- Manually editing the rot of a mirror copy can cause asymmetric anomalies
- **Solution**: Do not manually modify the rot of parts with mirror=1; instead modify the original part (mirror=0)

### Quaternion Validation
```
// Quaternion (x,y,z,w) must satisfy:
x² + y² + z² + w² = 1.0   // Must be normalized

// 0°, 0, 0, 0, 1
// 90° around Y axis, 0, 0.707, 0, 0.707   (0.5² + 0.5² ≈ 0.5, so √0.5 ≈ 0.707)
// 180° around Y axis, 0, 1, 0, 0
```

## 3. Crashes or Errors on Loading

### Common Errors

#### "Part not found"
- The .craft references a Mod part that is not installed
- Check if the part in `part = xxx` exists in GameData
- **Solution**: Install the corresponding Mod, or delete that PART block from the .craft

#### "Malformed craft file"
- Mismatched braces (missing `}` or `{`)
- Encoding issues (saving as UTF-8 with BOM may cause problems)
- Line ending format (use LF consistently)
- **Solution**: Check brace pairing with a text editor

#### Part cannot connect
- Node size mismatch (e.g., a size=0 node trying to connect to a size=2 node)
- The attachRules in .cfg do not allow that connection type
- **Solution**: Use an adapter part

## 4. Symmetry Sync Loss

### Symptoms
- Left-right symmetric parts are no longer symmetric
- One side does not change when the other is edited

### Possible Causes
- Manually edited the pos or rot of a mirror=1 part
- The sym field has been corrupted
- **Solution**: Re-apply symmetry or rebuild in the game

## 5. Cross-Version Compatibility Issues

| From Version | To Version | Possible Issues |
|--------------|------------|----------------|
| < 1.0 | 1.0+ | Major aerodynamic model changes, some part attributes incompatible |
| 1.0.x | 1.2.x | Communication system introduced, missing antenna parts |
| 1.3.x | 1.4.x | Resource system changes |
| 1.7.x | 1.8.x | Unity upgrade, model scaling changes |
| 1.8.x | 1.9.x | Part positioning system changes (KSP-Recall Issue #32) |
| < 1.12 | 1.12.x | EVA construction system adds new fields |

## 6. Precautions for Manual .craft File Editing

### 6.1 Text Editor Selection
- **Recommended**: VS Code, Notepad++, Sublime Text (syntax highlighting helps with checking)
- **Avoid**: Word, WPS, and other rich text editors (they may introduce invisible characters)

### 6.2 Backup Before Editing
```bash
cp mycraft.craft mycraft.craft.bak
```

### 6.3 Principles of Safe Modification
1. Only change a small amount at a time
2. Test in the game immediately after modification
3. First confirm you are editing the correct field (editing the wrong field is worse than not editing at all)
4. Do not arbitrarily delete RESOURCE or MODULE blocks (unless you are sure they are not needed)

### 6.4 Useful Edit Patterns
```
// Quickly find a specific part
Search: part = <partName>

// Find the root part
Search: link = -1

// View all symmetry copies
Search: mirror = 1
```

## 7. Using Text Editing Techniques for Troubleshooting

### Quick Part Position Fix

```bash
# Offset pos values as a whole (e.g., shift all parts' Y coordinates down by 1 meter)
# Note: This is just a concept; actual modification requires line-by-line changes
# It is recommended to use a text editor that supports column editing or regex replacement
```

### Updating Part Version Numbers

Replace `version = <old>` with `version = <new>` (though usually unnecessary, KSP handles compatibility).

### Removing Crash-causing Mod Parts

Find the non-existent `part = xxx` entry and delete the entire corresponding PART block (including everything from `PART` to `}`).
