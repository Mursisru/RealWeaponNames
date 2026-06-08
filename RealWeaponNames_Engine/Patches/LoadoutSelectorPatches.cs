using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(LoadoutSelector), nameof(LoadoutSelector.AssignAircraft))]
    internal static class LoadoutSelectorAssignAircraftPatch
    {
        private static void Postfix(LoadoutSelector __instance)
        {
            LoadoutSelectorPatchHelper.RefreshAllSelectors(__instance);
        }
    }

    [HarmonyPatch(typeof(LoadoutSelector), nameof(LoadoutSelector.LoadDefaults))]
    internal static class LoadoutSelectorLoadDefaultsPatch
    {
        private static void Postfix(LoadoutSelector __instance)
        {
            LoadoutSelectorPatchHelper.RefreshAllSelectors(__instance);
        }
    }

    [HarmonyPatch(typeof(LoadoutSelector), nameof(LoadoutSelector.UpdateWeapons))]
    internal static class LoadoutSelectorUpdateWeaponsPatch
    {
        private static void Postfix(LoadoutSelector __instance)
        {
            LoadoutSelectorPatchHelper.RefreshAllSelectors(__instance);
        }
    }

    internal static class LoadoutSelectorPatchHelper
    {
        internal static void RefreshAllSelectors(LoadoutSelector instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var selectors = Traverse.Create(instance).Field("weaponSelectors").GetValue<System.Collections.Generic.List<WeaponSelector>>();
            if (selectors == null)
                return;

            for (var i = 0; i < selectors.Count; i++)
                WeaponSelectorUiHelper.RefreshDropdownLabels(selectors[i]);
        }
    }
}
