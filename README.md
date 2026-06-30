**Developer:** Mursisru

# Real Weapon Names (NATO QoL)

[![Nuclear Option](https://img.shields.io/badge/Game-Nuclear%20Option-blue)](https://store.steampowered.com/app/2168680/Nuclear_Option/) [![BepInEx 5](https://img.shields.io/badge/Loader-BepInEx%205-orange)](https://docs.bepinex.dev/) [![Version](https://img.shields.io/badge/Version-1.0.0-green)]() [![License](https://img.shields.io/badge/License-MIT-lightgrey)](LICENSE)


BepInEx immersion mod for **Nuclear Option**. Replaces fictional in-game weapon names with real NATO-style designations in UI only (loadout menus, HUD, encyclopedia, notifications). Does not change weapon physics, guidance, damage, or AI.

**Version:** 1.0.0 Build DEV1Q1

## Install

> [!IMPORTANT]
> **BepInEx 5 (x64) required** - install [BepInEx](https://docs.bepinex.dev/) before this mod.

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

---

## Keywords

nuclear-option, bepinex, harmony, mod, realweaponnames, csharp, unity
