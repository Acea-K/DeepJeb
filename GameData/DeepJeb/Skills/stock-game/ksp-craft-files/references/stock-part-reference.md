# Stock Part Reference Data

> Data sources: `stockmasses.json` from the craft-parser repository, KSP Wiki Parts pages, actual .cfg file parsing. Limited by original source scope; some parts may be missing.

## 1. Stock Part Mass Table

The following mass data is in **tons (t)**, reverse-engineered from `stockmasses.json` (based on KSP 1.4.x-1.12.x versions; actual values may vary slightly between versions).

### 1.1 Aerodynamics & Wings

| Part Name | Mass (t) | Notes |
|-----------|----------|-------|
| noseCone | 0.03 | 1.25m nose cone |
| standardNoseCone | 0.01 | 0.625m small nose cone |
| pointyNoseConeA | 0.075 | Pointy nose cone A |
| pointyNoseConeB | 0.075 | Pointy nose cone B |
| rocketNoseCone | 0.2 | 2.5m nose cone |
| rocketNoseConeSize3 | 0.4 | 3.75m nose cone |
| noseConeAdapter | 0.1 | Adapter nose cone |
| deltaWing | 0.2 | Delta wing |
| delta_small | 0.05 | Small delta wing |
| sweptWing | 0.275 | Swept wing |
| sweptWing1 | 0.113 | Swept wing 1 |
| sweptWing2 | 0.226 | Swept wing 2 |
| wingConnector | 0.2 | Wing connector 1 |
| wingConnector2 | 0.2 | Wing connector 2 |
| wingConnector3 | 0.1 | Wing connector 3 |
| wingConnector4 | 0.05 | Wing connector 4 |
| wingConnector5 | 0.05 | Wing connector 5 |
| wingStrake | 0.05 | Wing strake |
| structuralWing | 0.1 | Structural wing 1 |
| structuralWing2 | 0.1 | Structural wing 2 |
| structuralWing3 | 0.05 | Structural wing 3 |
| structuralWing4 | 0.025 | Structural wing 4 |
| airlinerMainWing | 0.78 | Airliner main wing |
| airlinerTailFin | 0.36 | Airliner tail fin |
| airlinerCtrlSrf | 0.17 | Airliner control surface |
| wingShuttleDelta | 0.5 | Space shuttle delta wing |
| wingShuttleElevon1 | 0.15 | Space shuttle elevon 1 |
| wingShuttleElevon2 | 0.23 | Space shuttle elevon 2 |
| wingShuttleRudder | 0.45 | Space shuttle rudder |
| wingShuttleStrake | 0.1 | Space shuttle strake |

### 1.2 Control Surfaces

| Part Name | Mass (t) |
|-----------|----------|
| AdvancedCanard | 0.08 |
| CanardController | 0.1 |
| StandardCtrlSrf | 0.05 |
| smallCtrlSrf | 0.04 |
| elevon2 | 0.06 |
| elevon3 | 0.08 |
| elevon5 | 0.08 |
| tailfin | 0.125 |
| basicFin | 0.01 |
| airbrake1 | 0.05 |
| R8winglet | 0.1 |
| winglet | 0.037 |
| winglet3 | 0.078 |

### 1.3 Command Pods

| Part Name | Mass (t) | Crew |
|-----------|----------|------|
| mk1pod | 0.8 | 1 |
| mk1-3pod | 2.6 | 3 |
| Mark1Cockpit | 1.25 | 1 |
| Mark2Cockpit | 1.0 | 2 |
| mk2Cockpit_Inline | 2.0 | 2 |
| mk2Cockpit_Standard | 2.0 | 2 |
| mk3Cockpit_Shuttle | 3.5 | 4 |
| crewCabin | 2.5 | 4 |
| MK1CrewCabin | 1.0 | 2 |
| mk2CrewCabin | 2.0 | 4 |
| mk2LanderCabin | 2.5 | 2 |
| mk2LanderCabin_v2 | 1.355 | 2 |
| mk3CrewCabin | 6.5 | 8 |
| landerCabinSmall | 0.6 | 1 |
| cupola | 1.76 | 1 |
| seatExternalCmd | 0.05 | 1 |
| probeCoreCube | 0.07 | — |
| probeCoreHex | 0.1 | — |
| probeCoreOcto | 0.1 | — |
| probeCoreOcto2 | 0.04 | — |
| probeCoreSphere | 0.05 | — |
| probeStackLarge | 0.5 | — |
| probeStackSmall | 0.1 | — |
| HECS2_ProbeCore | 0.2 | — |
| mk2DroneCore | 0.2 | — |

### 1.4 Engines

| Part Name | Mass (t) | Thrust (kN) |
|-----------|----------|-------------|
| microEngine | 0.02 | 2 |
| liquidEngineMini | 0.1 | 20 |
| radialEngineMini | 0.02 | 2 |
| smallRadialEngine | 0.09 | 12 |
| liquidEngine3 | 0.5 | 70 |
| liquidEngine (LV-T30) | 1.25 | 215 |
| liquidEngine2 (LV-T45) | 1.5 | 167 |
| liquidEngine1-2 (Mainsail) | 6.0 | 1500 |
| liquidEngineMainsail_v2 | 6.0 | 1500 |
| liquidEngine2-2 (Skipper) | 1.75 | 650 |
| engineLargeSkipper_v2 | 3.0 | 650 |
| radialLiquidEngine1-2 | 0.9 | 120 |
| SSME | 4.0 | 912 |
| nuclearEngine (NERV) | 3.0 | 60 |
| toroidalAerospike | 1.0 | 175 |
| omsEngine | 0.09 | 4 |
| ionEngine | 0.25 | 2E-3 |
| vernierEngine | 0.08 | 1 |
| Size3AdvancedEngine | 9.0 | 3750 |
| Size3EngineCluster | 15.0 | 6000 |
| JetEngine (J-20) | 1.5 | 80 |
| turboJet | 1.2 | 135 |
| turboFanEngine | 1.8 | 150 |
| turboFanSize2 | 4.5 | 360 |
| miniJetEngine | 0.25 | 28 |
| RAPIER | 2.0 | 105/180 |
| solidBooster (RT-10) | 0.75 | 250 |
| solidBooster_sm | 0.45 | 135 |
| solidBooster1-1 (BACC) | 1.5 | 310 |
| Size2LFB (S3 KS-25) | 10.5 | 2700 |
| MassiveBooster (Kickback) | 4.5 | 630 |
| Thoroughbred | 10.0 | 1700 |
| Clydesdale | 21.0 | 3000 |
| Mite | 0.075 | 20 |
| Shrimp | 0.15 | 50 |
| sepMotor1 | 0.0125 | 12 |

## 2. Resource Density Reference

| Resource Name | t/unit | L/unit | Notes |
|---------------|--------|--------|-------|
| LiquidFuel | 0.0005 | 1 | 0.5 kg/L = 0.5 t/1000L |
| Oxidizer | 0.0005 | 1 | Same as above |
| MonoPropellant | 0.001 | 1 | 1 kg/L |
| ElectricCharge | 0 | — | Massless |
| XenonGas | 0.00001 | ? | 0.01 kg/EC |
| SolidFuel | 0.0075 | 1 | 7.5 kg/L |
| Ore | 0.001 | 1 | 1 kg/L |
| Ablator | 0.001 | 1 | 1 kg/L |

## 3. Notes

- The mass data above is compiled from `stockmasses.json`, which is based on .cfg parsing of the KSP Squad Parts directory
- Some values may vary between different KSP versions
- Mod part mass data must be looked up in the corresponding Mod's .cfg files individually
- Full weight of a fuel tank = dry mass + fuel mass. Fuel mass = fuel amount × density
