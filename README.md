**Developer:** Mursisru

# Real Weapon Names (NATO QoL)

[![Nuclear Option](https://img.shields.io/badge/Game-Nuclear%20Option-blue)](https://store.steampowered.com/app/2168680/Nuclear_Option/)
[![BepInEx 5](https://img.shields.io/badge/Loader-BepInEx%205-orange)](https://docs.bepinex.dev/)
[![Version](https://img.shields.io/badge/Version-1.0.0-green)](https://github.com/Mursisru/RealWeaponNames/releases/tag/v1.0.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow)](https://github.com/Mursisru/RealWeaponNames/blob/main/LICENSE)

BepInEx immersion mod for **[Nuclear Option](https://store.steampowered.com/app/2168680/Nuclear_Option/)**. Replaces fictional in-game weapon names with real NATO-style designations in **UI only** (loadout menus, HUD, encyclopedia, notifications). Does **not** change weapon physics, guidance, damage, or AI.

**Plugin GUID:** `com.at747.realweaponnames`  
**Version:** `1.0.0` · dev `1.0.0 Build DEV1Q16`

> [!NOTE]
> **Multiplayer safe** — cosmetic client-side strings only; no gameplay or network changes.

---

## Critical warnings

> [!IMPORTANT]
> **BepInEx 5 (x64) required** - install [BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html) before this mod.

> [!NOTE]
> **Multiplayer safe** - cosmetic client-side UI strings only; no gameplay or network changes.

## Install

> [!IMPORTANT]
> **BepInEx 5 (x64) required** — install [BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html) before this mod.

1. Download **`RealWeaponNames_Engine.dll`** from [Releases](https://github.com/Mursisru/RealWeaponNames/releases) or build Release.
2. Copy to:

   ```text
   Nuclear Option\BepInEx\plugins\RealWeaponNames_Engine.dll
   ```

3. Launch once. Config: `BepInEx\config\com.at747.realweaponnames.cfg`

## Requirements

- **[Nuclear Option](https://store.steampowered.com/app/2168680/Nuclear_Option/)** (Steam)
- **[BepInEx 5](https://docs.bepinex.dev/)** x64

---

## Configuration

| Key | Default | Description |
|-----|---------|-------------|
| `General.Enabled` | `true` | Enable NATO-style name replacements |

---

## Build

Requires .NET Framework 4.8 and Nuclear Option at `NuclearOptionRoot` in `Directory.Build.props`.

```powershell
msbuild RealWeaponNames_Engine\RealWeaponNames_Engine.csproj /p:Configuration=Release
```

Output: `RealWeaponNames_Engine\bin\Release\RealWeaponNames_Engine.dll`

---

## License

MIT — see [LICENSE](LICENSE).

---

## Keywords

nuclear-option, bepinex, harmony, mod, real-weapon-names, nato, qol, csharp, unity
