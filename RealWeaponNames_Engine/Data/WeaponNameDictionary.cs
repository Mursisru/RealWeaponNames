using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RealWeaponNames_Engine.Data
{
    internal static class WeaponNameDictionary
    {
        private static readonly Regex KtSpacedPattern = new Regex(@"\((\d+)\s*kt\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex KtCompactPattern = new Regex(@"\((\d+)kt\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex GunVariantPrefixPattern = new Regex(
            @"^(\d+(?:\.\d+)?mm)\s+(?:Rotary|Revolver|Internal)\s+(.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex GunMachineGunPattern = new Regex(
            @"^(\d+(?:\.\d+)?mm)\s+Machine\s+Gun$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Dictionary<string, string> FullNames = BuildFullNames();
        private static readonly Dictionary<string, string> ShortNameOverrides = BuildShortNameOverrides();
        private static readonly string[] KeysLongestFirst = BuildKeysLongestFirst();

        public static IReadOnlyDictionary<string, string> DisplayNames => FullNames;

        public static IReadOnlyList<string> KeysByLengthDesc => KeysLongestFirst;

        public static bool TryGetDisplayName(string original, out string display)
        {
            if (original == null)
            {
                display = null;
                return false;
            }

            if (FullNames.TryGetValue(original, out display))
                return true;

            var normalized = NormalizeLookupKey(original);
            if (normalized != original && FullNames.TryGetValue(normalized, out display))
                return true;

            display = null;
            return false;
        }

        public static bool TryGetShortName(string originalFullName, string displayFullName, string originalShortName, out string shortName)
        {
            if (originalFullName != null && ShortNameOverrides.TryGetValue(originalFullName, out shortName))
                return true;

            if (originalFullName != null)
            {
                var normalized = NormalizeLookupKey(originalFullName);
                if (normalized != originalFullName && ShortNameOverrides.TryGetValue(normalized, out shortName))
                    return true;
            }

            if (originalShortName != null && FullNames.TryGetValue(originalShortName, out shortName))
                return true;

            if (originalShortName != null && ShortNameOverrides.TryGetValue(originalShortName, out shortName))
                return true;

            if (displayFullName != null)
            {
                shortName = DeriveShortFromFull(displayFullName);
                return true;
            }

            shortName = originalShortName;
            return false;
        }

        public static string DeriveShortFromFull(string displayFullName)
        {
            if (string.IsNullOrEmpty(displayFullName))
                return displayFullName;

            var spaceIndex = displayFullName.IndexOf(' ');
            return spaceIndex > 0 ? displayFullName.Substring(0, spaceIndex) : displayFullName;
        }

        internal static string NormalizeLookupKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;

            var normalized = key.Trim();
            normalized = KtSpacedPattern.Replace(normalized, "($1 kt)");
            normalized = Regex.Replace(normalized, @"\s+", " ");

            var gunVariant = GunVariantPrefixPattern.Match(normalized);
            if (gunVariant.Success)
            {
                normalized = gunVariant.Groups[1].Value + " " + CanonicalizeGunSuffix(gunVariant.Groups[2].Value);
            }
            else
            {
                var machineGun = GunMachineGunPattern.Match(normalized);
                if (machineGun.Success)
                    normalized = machineGun.Groups[1].Value + " MG";
            }

            return normalized;
        }

        private static string[] BuildKeysLongestFirst()
        {
            var keys = new string[FullNames.Count];
            FullNames.Keys.CopyTo(keys, 0);
            Array.Sort(keys, (a, b) => b.Length.CompareTo(a.Length));
            return keys;
        }

        private static void RegisterEntry(Dictionary<string, string> map, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            map[key] = value;

            var spacedKt = KtCompactPattern.Replace(key, "($1 kt)");
            if (!map.ContainsKey(spacedKt))
                map[spacedKt] = value;

            var compactKt = KtSpacedPattern.Replace(key, "($1kt)");
            if (!map.ContainsKey(compactKt))
                map[compactKt] = value;

            var dotlessMk = key.Replace("Mk.II", "Mk II");
            if (dotlessMk != key && !map.ContainsKey(dotlessMk))
                map[dotlessMk] = value;

            var dottedMk = key.Replace("Mk II", "Mk.II");
            if (dottedMk != key && !map.ContainsKey(dottedMk))
                map[dottedMk] = value;
        }

        private static string CanonicalizeGunSuffix(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                return suffix;

            if (suffix.Equals("Gun", StringComparison.OrdinalIgnoreCase))
                return "Cannon";

            var lower = suffix.ToLowerInvariant();
            switch (lower)
            {
                case "cannon": return "Cannon";
                case "mg": return "MG";
                case "autocannon": return "Autocannon";
                case "grenade launcher": return "Grenade Launcher";
                case "railgun": return "Railgun";
                default: return suffix;
            }
        }

        private static void RegisterGunAliases(Dictionary<string, string> map, string canonicalKey, string value)
        {
            RegisterEntry(map, canonicalKey, value);

            var parts = Regex.Match(canonicalKey, @"^(\d+(?:\.\d+)?mm)\s+(.+)$");
            if (!parts.Success)
                return;

            var caliber = parts.Groups[1].Value;
            var gunType = parts.Groups[2].Value;

            foreach (var prefix in new[] { "Rotary", "Revolver", "Internal" })
                RegisterEntry(map, caliber + " " + prefix + " " + gunType, value);

            if (gunType == "Cannon")
            {
                RegisterEntry(map, caliber + " Rotary Gun", value);
                RegisterEntry(map, caliber + " Revolver Gun", value);
                RegisterEntry(map, caliber + " Internal Gun", value);
            }

            if (gunType == "MG")
            {
                RegisterEntry(map, caliber + " Machine Gun", value);
                RegisterEntry(map, caliber + " Rotary MG", value);
            }

            if (gunType == "Autocannon")
                RegisterEntry(map, caliber + " Rotary Autocannon", value);
        }

        private static void RegisterGunShortAliases(Dictionary<string, string> map, string canonicalKey, string shortName)
        {
            map[canonicalKey] = shortName;

            var parts = Regex.Match(canonicalKey, @"^(\d+(?:\.\d+)?mm)\s+(.+)$");
            if (!parts.Success)
                return;

            var caliber = parts.Groups[1].Value;
            var gunType = parts.Groups[2].Value;

            foreach (var prefix in new[] { "Rotary", "Revolver", "Internal" })
                map[caliber + " " + prefix + " " + gunType] = shortName;

            if (gunType == "Cannon")
            {
                map[caliber + " Rotary Gun"] = shortName;
                map[caliber + " Revolver Gun"] = shortName;
                map[caliber + " Internal Gun"] = shortName;
            }

            if (gunType == "MG")
            {
                map[caliber + " Machine Gun"] = shortName;
                map[caliber + " Rotary MG"] = shortName;
            }

            if (gunType == "Autocannon")
                map[caliber + " Rotary Autocannon"] = shortName;
        }

        private static Dictionary<string, string> BuildFullNames()
        {
            var map = new Dictionary<string, string>(StringComparer.Ordinal);

            // Air-to-air
            RegisterEntry(map, "IRM-S1", "AIM-92 Stinger");
            RegisterEntry(map, "IRM-S2", "AIM-9X Sidewinder");
            RegisterEntry(map, "MMR-S3", "MICA IR");
            RegisterEntry(map, "AAM-29 Scythe", "AIM-120 AMRAAM");
            RegisterEntry(map, "AAM-36 Scimitar", "Meteor");

            // Air-to-surface
            RegisterEntry(map, "AGR-18 Lynchpin", "AGR-20 APKWS");
            RegisterEntry(map, "AGR-24 Kingpin", "Laser Zuni");
            RegisterEntry(map, "AT-145", "AGM-176 Griffin");
            RegisterEntry(map, "AGM-48", "AGM-114 Hellfire");
            RegisterEntry(map, "Eyeball Mk.II", "Spike NLOS");
            RegisterEntry(map, "AGM-68", "AGM-65 Maverick");
            RegisterEntry(map, "ATP-1", "BGM-71 TOW");
            RegisterEntry(map, "Tusko-B", "MGM-140 ATACMS");
            RegisterEntry(map, "ARAD-116", "AGM-88 HARM");
            RegisterEntry(map, "AGM-99", "AGM-84E SLAM");
            RegisterEntry(map, "ALM-C450", "AGM-158 JASSM");
            RegisterEntry(map, "ALND-4 (20 kt)", "AGM-86 ALCM");
            RegisterEntry(map, "AShM-300", "AGM-84 Harpoon");
            RegisterEntry(map, "Piledriver TBM", "MGM-31 Pershing II");

            // Bombs / submunitions
            RegisterEntry(map, "GS25", "BLU-108");
            RegisterEntry(map, "PAB-80LR", "GBU-39 SDB");
            RegisterEntry(map, "PAB-125", "Mk-81 JDAM");
            RegisterEntry(map, "PAB-250", "GBU-38 JDAM");
            RegisterEntry(map, "PAB-250LR", "GBU-38 JDAM-ER");
            RegisterEntry(map, "GPO-500", "GBU-12 Paveway II");
            RegisterEntry(map, "GBM-500LR", "AGM-154 JSOW");
            RegisterEntry(map, "GPO-2P Auger", "GBU-24 Paveway III");
            RegisterEntry(map, "Demolition Bomb", "GBU-43/B MOAB");
            RegisterEntry(map, "GPO-N", "B61");

            // Guided shells
            RegisterEntry(map, "76mm Guided Shell", "OTO Melara 76mm DART");
            RegisterEntry(map, "127mm Guided Shell", "OTO Melara 127mm Vulcano");

            // Surface-to-air missiles
            RegisterEntry(map, "RAM-45", "RIM-116 RAM");
            RegisterEntry(map, "NL-98", "RIM-66 Standard (SM-2)");
            RegisterEntry(map, "StratoLance R9", "RIM-174 Standard (SM-6)");

            // Guns and cannons
            RegisterGunAliases(map, "12.7mm MG", "M2 Browning (.50 BMG)");
            RegisterGunAliases(map, "20mm Cannon", "M61 Vulcan");
            RegisterGunAliases(map, "20mm Autocannon", "M61 Vulcan");
            RegisterGunAliases(map, "23mm Autocannon", "GSh-23L");
            RegisterGunAliases(map, "25mm Cannon", "M242 Bushmaster");
            RegisterGunAliases(map, "25mm Autocannon", "M242 Bushmaster");
            RegisterGunAliases(map, "27mm Cannon", "Mauser BK-27");
            RegisterGunAliases(map, "27mm Autocannon", "Mauser BK-27");
            RegisterGunAliases(map, "30mm Cannon", "GAU-8 Avenger");
            RegisterGunAliases(map, "35mm Autocannon", "Oerlikon Millennium 35mm");
            RegisterEntry(map, "40mm Grenade Launcher", "Mk 19 GMG");
            RegisterEntry(map, "40mm GMG", "Mk 19 GMG");
            RegisterGunAliases(map, "40mm Cannon", "Bofors 40mm");
            RegisterGunAliases(map, "57mm Cannon", "Bofors 57mm");
            RegisterGunAliases(map, "76mm Cannon", "OTO Melara 76mm");
            RegisterGunAliases(map, "127mm Cannon", "127mm Mk 45 Mod 4");
            RegisterGunAliases(map, "130mm Cannon", "Rheinmetall 130mm L/51");
            RegisterEntry(map, "155mm Railgun", "BAE Systems 155mm Railgun");

            // Laser systems
            RegisterEntry(map, "20kW pulse laser", "AN/SEQ-3 LaWS");
            RegisterEntry(map, "80kW laser", "HELIOS");
            RegisterEntry(map, "120kW laser", "DragonFire");

            // Utility pods
            RegisterEntry(map, "ECM Pod", "AN/ALQ-131 ECM Pod");
            RegisterEntry(map, "Radar Jamming Pod", "AN/ALQ-99 Tactical Jamming Pod");
            RegisterEntry(map, "Radome", "AN/APG-65 Radar Pod");

            return map;
        }

        private static Dictionary<string, string> BuildShortNameOverrides()
        {
            var map = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "MMR-S3", "MICA" },
                { "AAM-29 Scythe", "AIM-120" },
                { "AAM-36 Scimitar", "Meteor" },
                { "AGR-18 Lynchpin", "APKWS" },
                { "AGR-24 Kingpin", "Zuni" },
                { "Eyeball Mk.II", "Spike" },
                { "Tusko-B", "ATACMS" },
                { "ALM-C450", "JASSM" },
                { "ALND-4 (20 kt)", "ALCM" },
                { "ALND-4 (20kt)", "ALCM" },
                { "AShM-300", "Harpoon" },
                { "Piledriver TBM", "Pershing" },
                { "PAB-80LR", "SDB" },
                { "PAB-125", "Mk-81" },
                { "PAB-250", "GBU-38" },
                { "PAB-250LR", "JDAM-ER" },
                { "GPO-500", "GBU-12" },
                { "GBM-500LR", "JSOW" },
                { "GPO-2P Auger", "GBU-24" },
                { "Demolition Bomb", "MOAB" },
                { "76mm Guided Shell", "76mm DART" },
                { "127mm Guided Shell", "127mm Vulcano" },

                { "RAM-45", "RAM" },
                { "NL-98", "SM-2" },
                { "StratoLance R9", "SM-6" },

                { "40mm Grenade Launcher", "Mk 19" },
                { "40mm GMG", "Mk 19" },
                { "155mm Railgun", "155mm RG" },

                { "20kW pulse laser", "LaWS" },
                { "80kW laser", "HELIOS" },
                { "120kW laser", "DragonFire" },

                { "ECM Pod", "ALQ-131" },
                { "Radar Jamming Pod", "ALQ-99" },
                { "Radome", "APG-65" },
            };

            RegisterGunShortAliases(map, "12.7mm MG", ".50 BMG");
            RegisterGunShortAliases(map, "20mm Cannon", "M61");
            RegisterGunShortAliases(map, "20mm Autocannon", "M61");
            RegisterGunShortAliases(map, "23mm Autocannon", "GSh-23");
            RegisterGunShortAliases(map, "25mm Cannon", "M242");
            RegisterGunShortAliases(map, "25mm Autocannon", "M242");
            RegisterGunShortAliases(map, "27mm Cannon", "BK-27");
            RegisterGunShortAliases(map, "27mm Autocannon", "BK-27");
            RegisterGunShortAliases(map, "30mm Cannon", "GAU-8");
            RegisterGunShortAliases(map, "35mm Autocannon", "35mm");
            RegisterGunShortAliases(map, "40mm Cannon", "Bofors 40");
            RegisterGunShortAliases(map, "57mm Cannon", "Bofors 57");
            RegisterGunShortAliases(map, "76mm Cannon", "76mm");
            RegisterGunShortAliases(map, "127mm Cannon", "Mk 45");
            RegisterGunShortAliases(map, "130mm Cannon", "130mm");

            return map;
        }
    }
}
