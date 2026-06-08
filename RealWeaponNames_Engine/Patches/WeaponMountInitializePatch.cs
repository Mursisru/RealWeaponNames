using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    /// <summary>
    /// Applies display labels when Encyclopedia initializes weapon mounts (runs during preload, not UI frame).
    /// </summary>
    [HarmonyPatch(typeof(WeaponMount), nameof(WeaponMount.Initialize))]
    internal static class WeaponMountInitializePatch
    {
        private static void Postfix(WeaponMount __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled)
                return;

            if (__instance.info != null)
                WeaponDisplayNameResolver.ApplyWeaponInfoLabels(__instance.info);

            if (!string.IsNullOrEmpty(__instance.mountName))
                __instance.mountName = WeaponDisplayNameResolver.ReplaceInComposite(__instance.mountName);
        }
    }
}
