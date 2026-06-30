namespace RealWeaponNames_Engine
{
    /// <summary>
    /// at747 versioning — keep in sync with BepInSemVer, DisplayVersion, AssemblyInfo, CHANGELOG.
    /// </summary>
    internal static class AppVersion
    {
        public const string ReleaseBase = "1.0.0";
        public const string BepInSemVer = ReleaseBase;
        public const string VersionChannel = "DEV";
        public const int CycleBuildNumber = 1;

        /// <summary>QoL — cosmetic NATO weapon name replacements.</summary>
        public const string ChangeLetters = "Q";

        public const int SubNumber = 16;

        public const string DisplayVersion = "1.0.0 Build DEV1Q16";
    }
}
