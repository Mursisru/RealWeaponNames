using HarmonyLib;
using RealWeaponNames_Engine.Services;
using UnityEngine.UI;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(WeaponIndicator), nameof(WeaponIndicator.Refresh))]
    internal static class WeaponIndicatorPatch
    {
        private static void Postfix(WeaponIndicator __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var aircraft = Traverse.Create(__instance).Field("aircraft").GetValue<object>();
            if (aircraft == null)
                return;

            var weaponManager = Traverse.Create(aircraft).Field("weaponManager").GetValue<object>();
            if (weaponManager == null)
                return;

            var station = Traverse.Create(weaponManager).Field("currentWeaponStation").GetValue<object>();
            if (station == null)
                return;

            var weaponInfo = Traverse.Create(station).Property("WeaponInfo").GetValue<WeaponInfo>();
            if (weaponInfo == null)
                return;

            var nameText = Traverse.Create(__instance).Field<Text>("weaponName").Value;
            if (nameText == null)
                return;

            nameText.text = WeaponDisplayNameResolver.ResolveShortName(weaponInfo);
        }
    }
}
