using HarmonyLib;
using RealWeaponNames_Engine.Services;
using UnityEngine.UI;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(WeaponStatus), nameof(WeaponStatus.UpdateDisplay))]
    internal static class WeaponStatusDisplayPatch
    {
        private static void Postfix(WeaponStatus __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var station = Traverse.Create(__instance).Field("weaponStation").GetValue<object>();
            if (station == null)
                return;

            var weaponInfo = Traverse.Create(station).Property("WeaponInfo").GetValue<WeaponInfo>();
            if (weaponInfo == null)
                return;

            var cargo = Traverse.Create(station).Property("Cargo").GetValue<bool>();

            var nameText = Traverse.Create(__instance).Field<Text>("nameText").Value;
            if (nameText == null)
                return;

            nameText.text = WeaponDisplayNameResolver.ResolveFullName(weaponInfo, cargo);
        }
    }
}
