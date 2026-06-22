# Infrastructure & Build Tools

Reference for development tools, build infrastructure, CI/CD workflows, and supporting applications in the Realism Overhaul ecosystem.

---

## BuildTools

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/BuildTools](https://github.com/KSP-RO/BuildTools) |
| **Description** | Reusable GitHub Actions workflows and composite actions for KSP-RO CI/CD |
| **Commits** | 53 |
| **Releases** | 0 |

Central repository of shared CI/CD components. All KSP-RO mod repositories reference these instead of duplicating workflow definitions.

### Composite Actions

| Action | Description |
|---|---|
| `checkout-BuildTools` | Checkout utility |
| `download-assemblies` | Download build assemblies |
| `download-assemblies-v2` | CKAN-based assembly download (preferred) |
| `process-changelog` | Generate kerbalConfig node from release notes |
| `remove-excess-dlls` | Clean up unnecessary DLLs |
| `update-assembly-info` | Replace `@version@` in assemblyInfo |
| `update-version-file` | Update version in `.version` file |
| `update-version-in-readme` | Update version links in README |

### Reusable Workflows

- `check-secret.yml` — verifies decryption password for protected assemblies
- `validate-cfg-files.yml` — validates `.cfg` files using [KSPMMCfgParser](https://github.com/KSP-CKAN/KSPMMCfgParser) by HebaruSan/CKAN

### CI/CD Pipeline (10 steps)

1. **Trigger:** Push to master, PR, or release tag
2. **Checkout:** Clone repository + BuildTools for shared actions
3. **Secret Check:** Verify decryption password available
4. **Assembly Download:** `download-assemblies-v2` pulls KSP assemblies via CKAN
5. **Build:** Compile C# against downloaded assemblies
6. **Validation:** `validate-cfg-files` runs KSPMMCfgParser on all `.cfg` files
7. **Changelog Processing:** Generate kerbalConfig node from release notes
8. **Version Updates:** Stamp version in assemblyInfo, `.version`, and README
9. **Cleanup:** `remove-excess-dlls` strips unnecessary DLLs
10. **Artifact:** Package and upload mod as CI artifact or GitHub release

---

## BuildLibs

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/BuildLibs](https://github.com/KSP-RO/BuildLibs) |
| **Description** | Encrypted library files for building KSP-RO mods |
| **Commits** | 5 |
| **Releases** | 0 |

Stores encrypted DLLs and assemblies (KSP game assemblies, dependencies) that cannot be publicly distributed due to licensing. Contents: `ExtraLibs/` and `KSP_x64_Data.zip`. Decryption key available to KSP-RO maintainers, checked via `check-secret` workflow. Not intended for end users.

---

## CFGProjectGenerator

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/CFGProjectGenerator](https://github.com/KSP-RO/CFGProjectGenerator) |
| **Description** | Generates `.csproj` files from `.cfg` files for IDE-based development |
| **Commits** | 6 |
| **Releases** | 1 |

Automates creation of Visual Studio project files that include all `.cfg` files from a mod's `GameData` directory. Allows editing both C# and `.cfg` files in a single IDE project.

**How it works:**
1. Reads template (`GameData.csproj.in`)
2. Walks up from template to find `GameData/` directory
3. Recursively finds all `.cfg` files
4. Replaces `<<content>>` placeholder with `<Content/>` blocks
5. Writes output `.csproj` alongside the `.in` file

**Usage:** `CFGProjectGenerator.exe path\to\inputfile.csproj.in` or `CFGProjectGenerator.exe path\to\repo\root` (auto-detects `.in` file). Example `GameData.csproj.in` provided in `Example/` directory.

---

## ROLibrary

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ROLibrary](https://github.com/KSP-RO/ROLibrary) |
| **Description** | Shared library for common code among ROx family mods |
| **Commits** | 216 |
| **Releases** | 30 |

Centralized shared C# code for ROx mods (ROCapsules, ROEngines, ROSolar, ROHeatshields, ROTanks). Ensures consistency and reduces code duplication.

### API Architecture

**Core classes:**
- **PartModule base classes** — shared implementations extended by ROx mods for engines, tanks, capsules, solar panels
- **Resource management** — standardized fuel switching, tank capacity calculations, resource transfer helpers
- **Physics calculation helpers** — shared math for thrust curves, Isp calculations, atmospheric performance modeling
- **Configuration parsing** — utilities for reading ModuleManager-applied configs consistently

**Data flow:** `ModuleManager patches → ROx mod .cfg files → ROLibrary API → ROx PartModules → In-game behavior`

**Dependencies:** ModuleManager, Realism Overhaul. Automatically installed when installing any ROx mod. Available via CKAN.

---

## ROUtils

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ROUtils](https://github.com/KSP-RO/ROUtils) |
| **Description** | Base library for KSP-RO mods |
| **License** | CC-BY-NC-SA 4.0 |
| **Commits** | 37 |
| **Releases** | 4 |

Lowest-level utility library for the entire KSP-RO ecosystem. While ROLibrary is ROx-specific, ROUtils is used across ALL KSP-RO mods.

### Core Capabilities

- **Logging infrastructure** — standardized debug and error logging with configurable verbosity
- **Configuration helpers** — common patterns for reading/applying ModuleManager configs with error handling
- **Extension methods** — C# convenience functions on KSP stock classes (Part, Vessel, CelestialBody, etc.)
- **Type converters** — conversions between KSP units, SI units, and real-world measurements
- **Assembly attribute helpers** — standardized version and metadata attributes

**Role:** Most fundamental library — RO itself depends on it. ROLibrary builds on ROUtils for the ROx family. Listed as required dependency in the official RO manual install guide. Available via CKAN and manually under CC-BY-NC-SA terms.

---

## ROLoadingImages

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ROLoadingImages](https://github.com/KSP-RO/ROLoadingImages) |
| **Description** | Replaces KSP loading screens with RSS/RO/RP-1 themed images |
| **License** | CC-BY-NC-SA 4.0 |
| **Commits** | 13 |
| **Releases** | 1 |

Lightweight cosmetic mod — simple texture replacement, no performance impact. Images sourced from RO/RP-1 Discord community ([Discord](https://discord.gg/V73jjNd)). Available via CKAN.

---

## RP-1-ExpressInstall

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/RP-1-ExpressInstall](https://github.com/KSP-RO/RP-1-ExpressInstall) |
| **Description** | CKAN metapackage for one-click RO/RP-1 installation |
| **Commits** | 28 |
| **Releases** | 2 |

Enables single-click installation of the complete RO/RP-1 mod pack via CKAN. Handles version locking and dependency resolution for 80+ interdependent mods.

**Metapackage details:** Identifier `RP-1-ExpressInstall`, latest version 2.0-r5, compatible with KSP 1.10/1.11/1.12, license CC-BY-NC-SA.

**Dependency list includes:**
- RealismOverhaul (core)
- RealSolarSystem + RSS Textures
- RP-1 (career mode)
- RealFuels, RealPlume, RealHeat
- KJR-Continued, FAR, RealChute
- Procedural Parts, Procedural Fairings
- All ROx family (ROEngines, ROCapsules, ROTanks, etc.)
- KSCSwitcher, Kerbal Konstructs
- MechJeb, HangarExtender, and other QoL mods

Handles version locking to prevent "dependency hell" from conflicting mod versions.

---

## RP1AnalyticsWebApp

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/RP1AnalyticsWebApp](https://github.com/KSP-RO/RP1AnalyticsWebApp) |
| **Description** | "Spyware v2.0" — web app for RP-1 career analytics |
| **Commits** | 266 |
| **Releases** | 0 |

Collects and visualizes opt-in career progression data from RP-1 players. Helps the dev team balance RP-1, compare careers, and identify progression blockers.

**Stack:** ASP.NET Core, MongoDB, Vite frontend, primarily on MS Azure. GitHub OAuth for admin. Swagger at `/swagger/`.

**Data collected (opt-in only):** Career progression (contracts, milestones), tech tree unlocks, facility upgrades, financial history, vessel statistics. No personal identifying information. Aggregated and anonymized.

**Analytics features:** Career timeline, tech tree heatmap, economic analysis, community comparison tools.

**Benefits:** Identifies unbalanced tech nodes, progression bottlenecks, contract reward issues. Validates gameplay curve against actual player experience.

**First admin user** must be created at database level (no self-registration).

---

## ECM-Viewer

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/ECM-Viewer](https://github.com/KSP-RO/ECM-Viewer) |
| **Description** | Web-based Engine Configurator Module viewer |
| **Commits** | 2 |
| **Releases** | 0 |

Static single-page web app (single `index.html`). Browsable interface for ECM configuration data:

- Displays engine parameters in human-readable tables
- Browse configs without opening raw `.cfg` files
- Shows thrust curves, fuel types, ignition limits, throttle ranges
- Useful for modders and players understanding engine behavior

ECM defines how real engines behave — each config specifies thrust, Isp, throttle range, ignition count, ullage requirements, and gimbal range.

---

## GoForLaunch

| Field | Detail |
|---|---|
| **Repository** | [KSP-RO/GoForLaunch](https://github.com/KSP-RO/GoForLaunch) |
| **Description** | Collaborative RP-0 career save |
| **Commits** | 114 |
| **Releases** | 0 |

Community-driven shared RP-0 career. Multiple players share one career, taking turns launching missions. Repository hosts save files, documentation, patches, mission logos, and CKAN config (`GFL-RO.ckan`). [Forum thread](http://forum.kerbalspaceprogram.com/index.php?/topic/132795-go-for-launch-cooperative-rorssrp-0/).

---

## FAQ

### CI/CD Pipeline

- **Missing secret:** `check-secret` failure means BuildLibs decryption password not configured in GitHub secrets
- **Assembly download failure:** `download-assemblies-v2` may fail if CKAN unreachable or KSP assemblies changed
- **CFG validation errors:** KSPMMCfgParser reports syntax errors with line numbers

### RP-1-ExpressInstall

- **Version conflicts:** Tweakscale Rescaled versions may conflict between express install and RO/FAR dependencies — CKAN may need manual resolution
- **Clean install recommended:** CKAN may refuse if conflicting mods exist; fresh KSP install is safest
- **ModuleManager:** Must match KSP version — build ≥1024 for KSP 1.12.x

### ROLibrary/ROUtils

- Match ROLibrary version to ROx mod versions — API mismatches cause runtime errors
- ROUtils must load before other RO mods; CKAN handles this automatically
- Manual installers: ensure `ROUtils.dll` in `GameData/ROUtils/Plugins/`

### RP1AnalyticsWebApp

- First admin created at database level (no self-registration)
- Application Insights telemetry only on Azure hosting

---

## Dependency Graph

```
ROUtils ───┬── RealismOverhaul ───┬── ROx mods (ROEngines, ROCapsules, etc.)
           │                      └── RP-1 ───┬── RP-1-ExpressInstall
           │                                  └── RP1AnalyticsWebApp
           ├── BuildTools ───┬── BuildLibs (encrypted assemblies)
           │                 └── CFGProjectGenerator
           ├── ROLibrary (ROx-specific shared code)
           └── ROLoadingImages (cosmetic)
```

## Glossary

| Term | Definition |
|---|---|
| **CKAN** | Comprehensive Kerbal Archive Network — mod manager and repository |
| **Metapackage** | Package defining dependencies but containing no code |
| **.netkan** | CKAN package metadata format (JSON) |
| **Composite Action** | GitHub Actions reusable step-level action |
| **Reusable Workflow** | GitHub Actions reusable job-level workflow |
| **KSPMMCfgParser** | Tool by HebaruSan/CKAN validating `.cfg` file syntax |
| **ECM** | Engine Configurator Module — defines real engine parameters in RO |
| **ROx** | RO-specific part mods (ROEngines, ROCapsules, ROTanks, ROSolar, ROHeatshields) |
| **Spyware v2.0** | In-joke name for RP-1's opt-in analytics system |

---

## Summary

| Mod | Primary Function | License | Status |
|---|---|---|---|
| BuildTools | CI/CD workflows & actions | — | Active |
| BuildLibs | Encrypted build assemblies | — | Internal |
| CFGProjectGenerator | Project file generation | — | Stable |
| ROLibrary | Shared ROx library | — | Active |
| ROUtils | Base library for RO mods | CC-BY-NC-SA 4.0 | Active |
| ROLoadingImages | Custom loading screens | CC-BY-NC-SA 4.0 | Stable |
| RP-1-ExpressInstall | CKAN one-click install | — | Active |
| RP1AnalyticsWebApp | Career analytics web app | — | Active |
| ECM-Viewer | Engine config viewer | — | Minimal |
| GoForLaunch | Collaborative career | — | Community |
