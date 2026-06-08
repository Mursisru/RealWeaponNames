using HarmonyLib;
using NuclearOption.MissionEditorScripts;
using RealWeaponNames_Engine.Services;
using UnityEngine.UI;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(RestrictedItem), nameof(RestrictedItem.SetItem), typeof(WeaponMount), typeof(RestrictionsTab))]
    internal static class RestrictedItemWeaponPatch
    {
        private static void Postfix(RestrictedItem __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var itemName = Traverse.Create(__instance).Field<Text>("itemName").Value;
            if (itemName == null || string.IsNullOrEmpty(itemName.text))
                return;

            itemName.text = WeaponDisplayNameResolver.ReplaceInComposite(itemName.text);
        }
    }
}
