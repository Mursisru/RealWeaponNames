using HarmonyLib;
using RealWeaponNames_Engine.Services;
using TMPro;

namespace RealWeaponNames_Engine.Services
{
    internal static class WeaponSelectorUiHelper
    {
        internal static void RefreshDropdownLabels(WeaponSelector selector)
        {
            if (selector == null || !RealWeaponNamesPlugin.IsEnabled || !RealWeaponNamesPlugin.IsSafeForUiPatch())
                return;

            var dropdown = Traverse.Create(selector).Field<TMP_Dropdown>("dropdown").Value;
            if (dropdown == null || dropdown.options == null)
                return;

            for (var i = 0; i < dropdown.options.Count; i++)
            {
                var option = dropdown.options[i];
                if (option == null || string.IsNullOrEmpty(option.text))
                    continue;

                option.text = WeaponDisplayNameResolver.ReplaceInComposite(option.text);
            }

            TryRefreshCaption(dropdown);
            SyncDropdownText(selector, dropdown);
        }

        private static void TryRefreshCaption(TMP_Dropdown dropdown)
        {
            if (dropdown == null || dropdown.options == null || dropdown.options.Count == 0)
                return;

            var index = dropdown.value;
            if (index < 0 || index >= dropdown.options.Count)
                return;

            dropdown.RefreshShownValue();
        }

        private static void SyncDropdownText(WeaponSelector selector, TMP_Dropdown dropdown)
        {
            if (dropdown == null || dropdown.options == null || dropdown.options.Count == 0)
                return;

            var index = dropdown.value;
            if (index < 0 || index >= dropdown.options.Count)
                return;

            var dropdownText = Traverse.Create(selector).Field<TextMeshProUGUI>("dropdownText").Value;
            if (dropdownText == null)
                return;

            dropdownText.text = dropdown.options[index].text;
        }
    }
}
