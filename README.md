# Real Weapon Names (NATO QoL)

BepInEx immersion mod for **Nuclear Option**. Replaces fictional in-game weapon names with real NATO-style designations in UI only (loadout menus, HUD, encyclopedia, tactical map, notifications). Does not change weapon physics, guidance, damage, or AI.

**Version:** 1.0.0 Build PR-R1Q1 (pre-release)

## Install

1. Install [BepInEx 5 x64](https://docs.bepinex.dev/) for Nuclear Option.
2. Copy `RealWeaponNames_Engine.dll` to `BepInEx/plugins/`.
3. Launch the game. Config: `BepInEx/config/com.at747.realweaponnames.cfg`.

## Config

| Key | Default | Description |
|-----|---------|-------------|
| `General.Enabled` | `true` | Enable name replacements |

## Build

Requires .NET Framework 4.8 and Nuclear Option installed at path in `Directory.Build.props` (`NuclearOptionRoot`).

```powershell
msbuild RealWeaponNames_Engine\RealWeaponNames_Engine.csproj /p:Configuration=Release
```

Output: `RealWeaponNames_Engine\bin\Release\RealWeaponNames_Engine.dll`

## Coverage

- 57+ weapon name mappings (missiles, bombs, guns, SAM, lasers, pods).
- Aircraft gun aliases (`20mm Rotary Cannon`, `Autocannon` variants, etc.).
- UI patches: loadout, HUD, encyclopedia, radial menu, map markers, action reports.
