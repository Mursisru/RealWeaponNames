using HarmonyLib;
using RealWeaponNames_Engine.Services;
using TMPro;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(EncyclopediaBrowser), "SpawnUnit")]
    internal static class EncyclopediaBrowserSpawnUnitPatch
    {
        private static void Postfix(EncyclopediaBrowser __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var unitName = Traverse.Create(__instance).Field<TMP_Text>("unitName").Value;
            if (unitName == null || string.IsNullOrEmpty(unitName.text))
                return;

            unitName.text = WeaponDisplayNameResolver.ReplaceInComposite(unitName.text);
        }
    }
}
