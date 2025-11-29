// RebirthLauncher/Constants/LauncherConstants.cs
using System;
using System.IO;

namespace RebirthLauncher.Constants
{
    internal static class LauncherConstants
    {
        // Base folder for launcher data (local app data)
        internal static readonly string AppDataFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RebirthLauncher");

        // Files (not user editable)
        internal static readonly string SettingsFilePath = Path.Combine(AppDataFolder, "settings.xml");
        internal static readonly string ServersFilePath = Path.Combine(AppDataFolder, "servers.xml");
        internal static readonly string LogFilePath = Path.Combine(AppDataFolder, "launcher.log");

        // Remote resources
        // Replace these with your actual URLs; keep them internal and stable.
        internal static readonly string VersionUrl = "https://example.com/rebirthlauncher/version.txt";
        internal static readonly string DefaultServerManifestUrl = "https://example.com/rebirthlauncher/default_manifest.xml";

        // Network retry policy (small, conservative defaults)
        internal static readonly int NetworkRetryCount = 2; // total attempts
        internal static readonly int NetworkRetryDelayMs = 500;

        // Logging rotation threshold (bytes)
        internal static readonly long LogMaxBytes = 5_000_000; // 5 MB

        // Other small defaults
        internal static readonly int DefaultHttpTimeoutSeconds = 100; // if you later choose to set HttpClient.Timeout
    }
}