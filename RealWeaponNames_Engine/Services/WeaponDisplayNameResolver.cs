using RealWeaponNames_Engine.Data;

namespace RealWeaponNames_Engine.Services
{
    internal static class WeaponDisplayNameResolver
    {
        public static void ApplyWeaponInfoLabels(WeaponInfo info)
        {
            if (info == null)
                return;

            var originalFull = info.weaponName;
            var originalShort = info.shortName;

            string displayFull;
            if (WeaponNameDictionary.TryGetDisplayName(originalFull, out displayFull))
                info.weaponName = displayFull;

            string displayShort;
            if (WeaponNameDictionary.TryGetShortName(originalFull, displayFull ?? originalFull, originalShort, out displayShort)
                && !string.IsNullOrEmpty(displayShort))
            {
                info.shortName = displayShort;
            }
        }

        public static void ApplyUnitDefinitionLabels(UnitDefinition definition)
        {
            if (definition == null)
                return;

            if (!string.IsNullOrEmpty(definition.code))
                definition.code = ReplaceInComposite(definition.code);

            if (!string.IsNullOrEmpty(definition.unitName))
                definition.unitName = ReplaceInComposite(definition.unitName);

            if (!string.IsNullOrEmpty(definition.bogeyName))
                definition.bogeyName = ReplaceInComposite(definition.bogeyName);
        }

        public static string ResolveFullName(WeaponInfo info, bool cargo = false)
        {
            if (info == null)
                return null;

            var name = Replace(info.weaponName);
            return cargo ? ("Cargo (" + name + ")") : name;
        }

        public static string ResolveShortName(WeaponInfo info)
        {
            if (info == null)
                return null;

            if (!string.IsNullOrEmpty(info.shortName))
                return info.shortName;

            return Replace(info.weaponName);
        }

        public static string Replace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string display;
            return WeaponNameDictionary.TryGetDisplayName(text, out display) ? display : text;
        }

        public static string ReplaceInComposite(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var result = text;
            var keys = WeaponNameDictionary.KeysByLengthDesc;
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (result.IndexOf(key, System.StringComparison.Ordinal) < 0)
                    continue;

                string display;
                if (WeaponNameDictionary.TryGetDisplayName(key, out display))
                    result = result.Replace(key, display);
            }

            return result;
        }

        public static bool MightContainMappedName(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            var keys = WeaponNameDictionary.KeysByLengthDesc;
            for (var i = 0; i < keys.Count; i++)
            {
                if (text.IndexOf(keys[i], System.StringComparison.Ordinal) >= 0)
                    return true;
            }

            var normalized = WeaponNameDictionary.NormalizeLookupKey(text);
            if (normalized != text)
            {
                for (var i = 0; i < keys.Count; i++)
                {
                    if (normalized.IndexOf(keys[i], System.StringComparison.Ordinal) >= 0)
                        return true;
                }
            }

            return false;
        }
    }
}
