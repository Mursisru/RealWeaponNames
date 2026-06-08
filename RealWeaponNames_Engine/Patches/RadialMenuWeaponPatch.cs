using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(RadialMenuAction), nameof(RadialMenuAction.SetWeapon))]
    internal static class RadialMenuWeaponPatch
    {
        private static void Postfix(RadialMenuAction __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            __instance.DisplayName = WeaponDisplayNameResolver.Replace(__instance.DisplayName);
        }
    }
}
