using System.Reflection;
using HarmonyLib;
using RealWeaponNames_Engine.Services;
using TMPro;

namespace RealWeaponNames_Engine.Patches
{
    internal static class EncyclopediaWeaponPanelPatch
    {
        internal static MethodBase TargetMethod()
        {
            var nestedType = AccessTools.Inner(typeof(EncyclopediaBrowser), "WeaponStationDisplay");
            return AccessTools.Method(nestedType, "UpdateText");
        }

        internal static void Postfix(object __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var nameText = Traverse.Create(__instance).Field<TMP_Text>("nameText").Value;
            if (nameText == null || string.IsNullOrEmpty(nameText.text))
                return;

            nameText.text = WeaponDisplayNameResolver.ReplaceInComposite(nameText.text);
        }
    }
}
