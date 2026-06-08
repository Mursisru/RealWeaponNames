# Changelog

## 1.0.0 Build PR-R1Q1

- First GitHub release mirror of Engine **DEV1Q16** (GitHub **Latest** tag **v1.0.0**).
- 57 canonical NATO name mappings; aircraft gun aliases; map/encyclopedia patches.

## 1.0.0 Build DEV1Q16

- AGM-99 display name: `AGM-84E SLAM`.

## 1.0.0 Build DEV1Q15

- AGM-99 display name: `AGM-84K SLAM`.

## 1.0.0 Build DEV1Q14

- AGM-99 display name: `AGM-84H/K SLAM-ER` → `AGM-84H SLAM-ER`.

## 1.0.0 Build DEV1Q13

- Gun names from sharedassets scan: `20/25/27mm Autocannon`, `23mm Autocannon`, `40mm GMG`, `127mm Cannon` (+ rotary aliases).

## 1.0.0 Build DEV1Q12

- Aircraft gun aliases: `20mm Rotary Cannon`, `Rotary Gun`, `Revolver/Internal` variants map to the same NATO names as ship cannons.
- Normalize gun lookup keys (`Rotary`/`Revolver`/`Internal`/`Machine Gun` → canonical caliber keys).

## 1.0.0 Build DEV1Q5

- Fix infinite loading: defer ApplyAll out of Encyclopedia.Preload callback (deadlock with FindObjectsOfTypeAll).
- Remove ApplyAll from plugin Awake; add try/catch around patch init.

## 1.0.0 Build DEV1Q4

- Fix key aliases: `ALND-4 (20kt)` vs `(20 kt)`, Mk.II variants, auto kt normalization.
- Loadout map labels: refresh WeaponSelector on SetValue/UpdateWeapons/ShowHardpoints.
- HUD font matching for weapon labels (FlightHud/CombatHud font on Text and TMP).

## 1.0.0 Build DEV1Q3

- Added SAM, gun/cannon, laser, and utility pod name mappings (21 entries).

## 1.0.0 Build DEV1Q2

- Fix coverage: patch after `Encyclopedia.AfterLoad`, `UnitDefinition.unitName`, all `WeaponMount.mountName`.
- UI safety-net: loadout dropdown (`WeaponSelector`), encyclopedia spawn title, mission editor restrictions.

## 1.0.0 Build DEV1Q1

- Initial release: cosmetic replacement of 31 fictional weapon names with NATO-style designations.
- One-time `WeaponInfo` patch at load plus Harmony safety-net patches for loadout, HUD, radial menu, encyclopedia, and action reports.
- Config toggle `General.Enabled`.
