using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using RealWeaponNames_Engine.Patches;
using RealWeaponNames_Engine.Services;

namespace RealWeaponNames_Engine
{
    [BepInPlugin(PluginGuid, PluginName, AppVersion.BepInSemVer)]
    public sealed class RealWeaponNamesPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.at747.realweaponnames";
        public const string PluginName = "Real Weapon Names (NATO QoL)";

        internal static RealWeaponNamesPlugin Instance { get; private set; }
        internal static ManualLogSource Log { get; private set; }
        internal static bool IsEnabled => Enabled != null && Enabled.Value;

        internal static ConfigEntry<bool> Enabled { get; private set; }

        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            Enabled = Config.Bind("General", "Enabled", true,
                "Replace fictional weapon names with real NATO-style designations in UI only.");

            if (!IsEnabled)
            {
                Logger.LogInfo($"{PluginName} {AppVersion.DisplayVersion} loaded (disabled via config).");
                return;
            }

            try
            {
                _harmony = new Harmony(PluginGuid);
                _harmony.PatchAll(typeof(RealWeaponNamesPlugin).Assembly);

                PatchManual(_harmony, EncyclopediaWeaponPanelPatch.TargetMethod(),
                    typeof(EncyclopediaWeaponPanelPatch), nameof(EncyclopediaWeaponPanelPatch.Postfix),
                    "EncyclopediaBrowser.WeaponStationDisplay.UpdateText");

                PatchManual(_harmony, EncyclopediaAfterLoadPatch.TargetMethod(),
                    typeof(EncyclopediaAfterLoadPatch), nameof(EncyclopediaAfterLoadPatch.Postfix),
                    "Encyclopedia.AfterLoad");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Harmony patch failed: {ex}");
                throw;
            }

            Logger.LogInfo($"{PluginName} {AppVersion.DisplayVersion} loaded (display labels at mount init + UI patches).");
        }

        internal static bool IsSafeForUiPatch()
        {
            return MainMenu.State != MainMenu.LoadingState.Loading;
        }

        private static void PatchManual(Harmony harmony, System.Reflection.MethodBase target, Type patchType, string postfixName, string label)
        {
            if (target == null)
            {
                Log.LogWarning($"{label} not found; related UI names may be unchanged.");
                return;
            }

            harmony.Patch(target, postfix: new HarmonyMethod(patchType, postfixName));
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            _harmony?.UnpatchSelf();
        }
    }
}
