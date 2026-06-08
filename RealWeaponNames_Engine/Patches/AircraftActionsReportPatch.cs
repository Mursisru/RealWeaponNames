using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    [HarmonyPatch(typeof(AircraftActionsReport), nameof(AircraftActionsReport.ReportText))]
    internal static class AircraftActionsReportPatch
    {
        private static void Prefix(ref string report)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch() || string.IsNullOrEmpty(report))
                return;

            if (!WeaponDisplayNameResolver.MightContainMappedName(report))
                return;

            report = WeaponDisplayNameResolver.ReplaceInComposite(report);
        }
    }
}
