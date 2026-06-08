using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine.Patches
{
    /// <summary>
    /// Patches missile/unit definition display strings after Encyclopedia finishes loading.
    /// Big map TargetMarker reads unit.definition.code — not WeaponInfo.
    /// </summary>
    internal static class EncyclopediaAfterLoadPatch
    {
        internal static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Encyclopedia), "AfterLoad", new System.Type[0]);
        }

        internal static void Postfix(Encyclopedia __instance)
        {
            if (!RealWeaponNamesPlugin.IsEnabled || __instance == null)
                return;

            PatchDefinitions(__instance.missiles);
            PatchDefinitions(__instance.otherUnits);
        }

        private static void PatchDefinitions<T>(List<T> definitions) where T : UnitDefinition
        {
            if (definitions == null)
                return;

            for (var i = 0; i < definitions.Count; i++)
                WeaponDisplayNameResolver.ApplyUnitDefinitionLabels(definitions[i]);
        }
    }
}
