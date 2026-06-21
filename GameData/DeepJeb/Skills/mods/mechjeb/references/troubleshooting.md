# MechJeb Common Issues & Solutions

## MechJeb Doesn't Appear
- Ensure that the latest version of **Module Manager** is installed (see [module-manager.skill])
- In Career mode, verify you've unlocked the required tech node
- Some anti-virus software blocks MechJeb.dll — whitelist KSP folder

## Windows Protection Blocks Loading
- Install KSP outside `C:\Program Files (x86)\`
- Steam: Properties → Local Files → Move Install Folder
- Or simply copy the KSP directory elsewhere

## Ascent Guidance Fails / Rocket Flips
- Try reducing the "turn shape" percentage (more gradual turn)
- Increase turn start altitude for heavy rockets
- Check that the rocket is aerodynamically stable
- Ensure TWR is in the 1.3–2.0 range at launch
- Disable corrective steering for large craft

## Landing Guidance Misses Target
- Landing predictions are less accurate on atmospheric bodies
- Use Translatron for final approach on vacuum bodies
- Account for planetary rotation (treat as a moving target)

## kRPC Connection Issues
- Ensure kRPC server is running in KSP
- Check firewall settings for port 50000 (default)
- Use `conn = krpc.connect(name='MechJeb Control')` for named connections
- Some mods may conflict with kRPC's vessel control

## kOS.MechJeb2.Addon Not Found
- Verify both kOS and MechJeb2 are installed correctly
- Check that kOS.MechJeb2.Addon is in the GameData folder
- In Career mode, ensure you have an active vessel with both parts
