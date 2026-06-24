# Part Size Standards

> Compiled from KSP Wiki (Radial size, Part Modelling Guidelines), KSP Forum Size naming discussions. The KSP Wiki page is throttled by Anubis protection; some field information references Wayback Machine archives and community records.

## 1. Radial Size System

KSP part diameters are graded by standard sizes. Each size has an internal name and corresponding metric diameter.

### 1.1 Standard Size Table

| Internal Name | Diameter | Node Size | Series | Description |
|---------------|----------|-----------|--------|-------------|
| Size0 | 0.625m | 0 | XS (Extra Small) | Small probes, small struts |
| Size1 | 1.25m | 1 | SM (Small) / Mk1 | Standard engines/tanks |
| Size1.5 | 1.875m | 1.5 | — | Making History DLC addition |
| Size2 | 2.5m | 2 | MD (Medium) / Mk2 | Large rockets × planes |
| Size3 | 3.75m | 3 | LG (Large) / Mk3 | Heavy rockets × space shuttles |
| Size4 | 5.0m | 4 | XL | Making History DLC addition |
| Size5 | 7.5m | 5 | — | Mod-only (Restock+, etc.) |

### 1.2 KSP Forum Naming Explanations

From forum `topic/75702` ("Size names: get 'em right!"):

> "Although part diameters in KSP are 0.625m, 1.25m, 1.875m*, 2.5m, 3.75m, 5m** and 7.25m***, the game never refers to them that way."

Naming conventions:

| Common Name | Exact Diameter | Notes |
|-------------|---------------|-------|
| XS | 0.625m | Very small |
| S / SM | 1.25m | Mk1 series standard |
| Size 1.5 | 1.875m | Between S and M |
| M / MD | 2.5m | Mk2 series standard (non-circular cross-section) |
| L / LG | 3.75m | Mk3 series standard |
| XL | 5.0m | Making History |
| Extra Large | 7.5m+ | Mod only |

## 2. Mk Series Cross-Section Dimensions

### 2.1 Mk1 (1.25m Circular)

- Diameter: 1.25m
- Standard stack node size=1
- Applies to: Command pods, fuel tanks, engines
- Representative parts: `mk1pod`, `fuelTank`

### 2.2 Mk2 (2.5m Non-Circular)

- Approximate airfoil cross-section, height ~1.25m, width ~2.5m
- Standard stack node size=2 (but shape is airfoil)
- Built-in lift characteristics
- Applies to: Aircraft/spaceplanes
- Representative parts: `Mark2Cockpit`, `mk2Fuselage`

### 2.3 Mk3 (3.75m Large Airfoil)

- Larger airfoil cross-section, height ~2.5m, width ~3.75m
- Standard stack node size=3
- Applies to: Large space shuttles, heavy aircraft
- Representative parts: `mk3Cockpit_Shuttle`, `mk3Fuselage`

## 3. Exact Dimensions of Stock Core Parts

The following data is based on standard specifications from the KSP Wiki Parts page and modeling guidelines. **1 KSP unit = 1 meter.**

### 3.1 Command Pods

| Part Name | Diameter | Height | Mass (t) | Nodes (top/bottom) |
|-----------|----------|--------|----------|---------------------|
| mk1pod | 1.25m | 0.94m | 0.8 | Top/bottom size=1 |
| mk1-3pod | 1.25m | 1.25m | 2.6 | Bottom size=1 |
| Mark1Cockpit | 1.25m | ≈1.0m | 1.25 | Rear/front |
| Mark2Cockpit | Mk2 | ≈1.5m | 1.0 | Rear/front size=2 |
| mk2Cockpit_Inline | Mk2 | ≈2.0m | 2.0 | Rear/front size=2 |
| mk3Cockpit_Shuttle | Mk3 | ≈4.0m | 3.5 | Rear size=3 |
| landerCabinSmall | 1.25m | 0.75m | 0.6 | Top size=1 |
| cupola | 2.5m | 1.0m | 1.76 | Bottom size=2 |
| probeCoreCube | 0.625m | 0.625m | 0.07 | Each face size=0 |

### 3.2 Standard Fuel Tanks

| Part Name | Diameter | Height | Mass (t, empty) | Capacity (LF+O) |
|-----------|----------|--------|-----------------|-----------------|
| fuelTankSmallFlat | 1.25m | 0.25m | 0.0625 | 50+61 |
| fuelTankSmall | 1.25m | 0.56m | 0.125 | 100+122 |
| fuelTank | 1.25m | 0.94m | 0.25 | 200+245 |
| fuelTank_long | 1.25m | 1.88m | 0.5 | 400+490 |
| Rockomax8BW | 2.5m | 0.75m | 0.5 | 400+490 |
| Rockomax16_BW | 2.5m | 1.5m | 1.0 | 800+980 |
| Rockomax32_BW | 2.5m | 3.0m | 2.0 | 1600+1960 |
| Rockomax64_BW | 2.5m | 6.0m | 4.0 | 3200+3920 |
| Size3SmallTank | 3.75m | 1.5m | 2.25 | 1800+2200 |
| Size3MediumTank | 3.75m | 3.0m | 4.5 | 3600+4400 |
| Size3LargeTank | 3.75m | 6.0m | 9.0 | 7200+8800 |
| MK1Fuselage | Mk1 | 0.94m | 0.25 | 150 LF+O |
| mk2Fuselage | Mk2 | ≈2.0m | 0.35 | 200 LF+O |
| mk3Fuselage | Mk3 | ≈4.0m | 1.0 | 400 LF+O |

### 3.3 Engines

| Part Name | Mount Size | Mass (t) | Thrust (kN) | Nodes |
|-----------|------------|----------|-------------|-------|
| microEngine | 0.625m | 0.02 | 2 | Top/bottom size=0 |
| liquidEngineMini | 1.25m | 0.1 | 20 | Top/bottom size=1 |
| liquidEngine | 1.25m | 1.25 | 200 | Top/bottom size=1 |
| liquidEngine2 | 2.5m | 1.5 | 250 | Top/bottom size=2 |
| liquidEngine1-2 (Mainsail) | 2.5m | 6.0 | 1500 | Top/bottom size=2 |
| liquidEngine2-2 (Skipper) | 2.5m | 1.75 | 650 | Top/bottom size=2 |
| Size3AdvancedEngine | 3.75m | 9.0 | 3750 | Top/bottom size=3 |
| Size3EngineCluster | 3.75m | 15.0 | 6000 | Top/bottom size=3 |
| nuclearEngine (NERV) | 1.25m | 3.0 | 60 | Top/bottom size=1 |
| RAPIER | 1.25m | 2.0 | 105/180 | Top/bottom size=1 |
| radialEngineMini | Surface | 0.02 | 2 | Surface node |

### 3.4 Command Pod Node Positions

```
Note: All node positions are model-local coordinates in meters.
The positive Y axis direction is "upward".

mk1pod:
  node_stack_top = 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 1    (top connection point)
  node_stack_bottom = 0.0, -0.469, 0.0, 0.0, -1.0, 0.0, 1  (bottom connection point)

fuelTank (FL-T400):
  node_stack_top = 0.0, 0.5, 0.0, 0.0, 1.0, 0.0, 1
  node_stack_bottom = 0.0, -0.5, 0.0, 0.0, -1.0, 0.0, 1

Rockomax64_BW:
  node_stack_top = 0.0, 4.685, 0.0, 0.0, 1.0, 0.0, 2
  node_stack_bottom = 0.0, -4.685, 0.0, 0.0, -1.0, 0.0, 2
```

## 4. Unit System for Dimensions and Mass

KSP internally uses the International System of Units (SI):

| Dimension | Unit | Notes |
|-----------|------|-------|
| Length | meters (m) | 1 Unity unit = 1 meter |
| Mass | tonnes (t) | 1 t = 1000 kg |
| Force | kilonewtons (kN) | Thrust unit |
| Fuel | liters (L) | Corresponding mass depends on density |
| Electricity | units (EC) | No corresponding SI value |
| Temperature | kelvin (K) | |

## 5. Scaling Factor and TweakScale

Mods like TweakScale can add scaling information to .craft files:

```
rescaleFactor = 0.625  // Scale to 62.5% of original size
// TweakScale may also add:
MODULE
{
    name = TweakScale
    currentScale = <scaleValue>
    defaultScale = <defaultValue>
    ...
}
```

Note: **TweakScale does not directly modify the pos/rot values in .craft**; instead, it dynamically calculates them at load time. This is the root cause of some position offset issues discussed in Issue #118.
