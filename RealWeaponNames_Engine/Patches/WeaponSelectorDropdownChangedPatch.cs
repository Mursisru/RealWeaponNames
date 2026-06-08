using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(WeaponSelector), nameof(WeaponSelector.DropdownChanged))]
    internal static class WeaponSelectorDropdownChangedPatch
    {
        private static void Postfix(WeaponSelector __instance)
        {
            if (!RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            WeaponSelectorUiHelper.RefreshDropdownLabels(__instance);
        }
    }
}
