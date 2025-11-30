// RebirthLauncher/Constants/LauncherConstants.cs
using System;
using System.IO;

namespace RebirthLauncher.Constants
{
    public static class LauncherConstants
    {
        // Local/AppData folder name for launcher data
        public static readonly string AppDataFolderName = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RebirthLauncher");

        // Old launcher filename to check/delete at startup (single exact name; no wildcards)
        public const string OldLauncherFileName = "RebirthLauncher.exe.old";

        // Log file name (appended to when /log is present)
        public const string LogFileName = "rebirth_launcher.log";

        // Version file and update zip names (used in later sub-steps)
        private const string _versionFileName = "version.txt";
        private const string _updateZipName = "RebirthLauncher_Update.zip";

        // Remote base components (private). Public properties expose absolute URLs.
        private const string _remoteBase = "https://example.com/rebirth"; // placeholder; can be overridden later

        /// <summary>
        /// Absolute URL to version.txt (constructed from private members).
        /// </summary>
        public static string VersionUrl => $"{_remoteBase.TrimEnd('/')}/{_versionFileName}";

        /// <summary>
        /// Absolute URL to the update zip (constructed from private members).
        /// </summary>
        public static string UpdateZipUrl => $"{_remoteBase.TrimEnd('/')}/{_updateZipName}";

        // Exit codes
        public const int ExitCodeSuccess = 0;
        public const int ExitCodeUninstall = 2;
        public const int ExitCodeBootstrapFailure = 1;
    }
}