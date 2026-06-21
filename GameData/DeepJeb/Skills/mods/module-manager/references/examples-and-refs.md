# Module Manager Use Cases, Troubleshooting & Community

## Common Use Cases

### Add a Module to an Existing Part

```
@PART[Mark1-2Pod]
{
    MODULE
    {
        name = ModuleScienceExperiment
        experimentID = crewReport
    }
}
```

### Modify Engine Parameters

```
@PART[liquidEngine2]
{
    @MODULE[ModuleEngines*]
    {
        @maxThrust *= 1.5        // 50% more thrust
        @heatProduction *= 0.8   // 20% less heat
    }
}
```

### Clone and Modify a Part

```
+PART[myOriginalPart]
{
    @name = myClonedPart
    @title = My Cloned Part
    @MODULE[ModuleEngines]
    {
        @maxThrust = 200
    }
}
```

### Remove a Module from All Parts

```
@PART[*]:HAS[#MODULE[ModuleToRemove]]
{
    !MODULE[ModuleToRemove] {}
}
```

### Conditional Patching Based on Installed Mods

```
@PART[somePart]:NEEDS[RealSolarSystem]
{
    @rescaleFactor = 1.0         // Only applies if RSS is installed
}
```

### Set Defaults Without Overwriting

```
@PART[somePart]
{
    &maxTemp = 2500              // Only set if not already defined
    &crashTolerance = 12
}
```

---

## Common Issues & Solutions

### MM Hangs at "0 patches loaded"
- Usually a broken DLL or .cfg file in GameData
- Check KSP.log for the specific file that causes the freeze
- Remove recently added mods one by one to isolate

### Patch Doesn't Apply
- Check `:NEEDS` conditions — does the required mod actually exist?
- Check ordering directives — another mod may have overwritten your change
- Check wildcard patterns — `*` and `?` must match exactly
- Run KSP once and check `ModuleManager.ConfigCache` to see the final patched state

### Module Manager Slows Loading
- Normal for large mod packs — MM reads and patches every .cfg
- Enable **cache** (`ModuleManager.Cache` created after first run) to speed up subsequent loads
- Remove unused mods to reduce patching overhead

### Patch Applies But Values Are Wrong
- Verify arithmetic operators (`+=`, `*=`, `!=`)
- Check for conflicting patches from other mods
- Use `:FINAL` as a diagnostic (temporary) to ensure your patch runs last

### "MM Not Installed" Error from Another Mod
- Ensure `ModuleManager.dll` is directly in `GameData/` (not in a subfolder)
- Check CKAN — some mod packs install MM automatically

---

## Community Resources

### Official
- **GitHub Repository:** https://github.com/sarbian/ModuleManager
- **Syntax Wiki:** https://github.com/sarbian/ModuleManager/wiki/Module-Manager-Syntax
- **Patch Ordering:** https://github.com/sarbian/ModuleManager/wiki/Patch-Ordering
- **Build Server:** https://ksp.sarbian.com/jenkins/job/ModuleManager/

### Tutorials
- **KSP Forum:** Module Manager tutorial thread
- **The Great Unofficial MM Help Thread:** https://forum.kerbalspaceprogram.com/topic/208450-the-great-unofficial-module-manager-help-thread
- **Reddit r/KerbalAcademy:** Various MM guides
