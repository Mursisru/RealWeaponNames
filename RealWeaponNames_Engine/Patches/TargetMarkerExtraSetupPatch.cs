using HarmonyLib;
using RealWeaponNames_Engine.Services;
using UnityEngine.UI;

namespace RealWeaponNames_Engine.Patches
{
    /// <summary>
    /// Big map target labels use unit.definition.code in ExtraSetup (SPD/ALT/HDG block).
    /// </summary>
    [HarmonyPatch(typeof(TargetMarker), "ExtraSetup")]
    internal static class TargetMarkerExtraSetupPatch
    {
        private static void Postfix(TargetMarker __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var infoName = Traverse.Create(__instance).Field<Text>("infoName").Value;
            if (infoName == null || string.IsNullOrEmpty(infoName.text))
                return;

            infoName.text = WeaponDisplayNameResolver.ReplaceInComposite(infoName.text);
        }
    }
}
