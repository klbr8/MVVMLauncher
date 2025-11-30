// RebirthLauncher/Services/Bootstrap.cs
using System;
using System.Collections.Generic;
using System.IO;
using RebirthLauncher.Constants;
using RebirthLauncher.Helpers;

namespace RebirthLauncher.Services
{
    internal static class Bootstrap
    {
        public static bool EnableLogging { get; private set; } = false;

        public static int Run(string[] args)
        {
            var pendingLogs = new List<string>();

            // 1) Deletion-first: attempt to delete the single old launcher file.
            try
            {
                var oldPath = Path.Combine(LauncherConstants.AppDataFolderName, LauncherConstants.OldLauncherFileName);
                var deleted = FileHelpers.TryDeleteFile(oldPath, out var diag);
                pendingLogs.Add($"DeleteOld: {diag}");
            }
            catch (Exception ex)
            {
                pendingLogs.Add($"DeleteOld: unexpected exception: {ex.GetType().Name}: {ex.Message}");
            }

            // 2) Inline arg parsing
            var isUninstall = false;
            EnableLogging = false;
            if (args != null && args.Length > 0)
            {
                foreach (var raw in args)
                {
                    var a = (raw ?? string.Empty).Trim();
                    if (string.Equals(a, "/log", StringComparison.OrdinalIgnoreCase))
                        EnableLogging = true;
                    else if (string.Equals(a, "/uninstall", StringComparison.OrdinalIgnoreCase))
                        isUninstall = true;
                    else
                    {
                        // ignore unknown args
                    }
                }
            }

            if (EnableLogging)
            {
                foreach (var line in pendingLogs)
                    FileHelpers.AppendToLogFile(line);
                FileHelpers.AppendToLogFile("Bootstrap: /log enabled.");
            }

            // 3) Early uninstall path
            if (isUninstall)
            {
                if (EnableLogging) FileHelpers.AppendToLogFile("Bootstrap: uninstall requested.");
                try
                {
                    PerformUninstall(EnableLogging);
                    if (EnableLogging) FileHelpers.AppendToLogFile("Bootstrap: uninstall completed.");
                    return LauncherConstants.ExitCodeUninstall;
                }
                catch (Exception ex)
                {
                    if (EnableLogging) FileHelpers.AppendToLogFile($"Bootstrap: uninstall failed: {ex}");
                    return LauncherConstants.ExitCodeBootstrapFailure;
                }
            }

            if (EnableLogging) FileHelpers.AppendToLogFile("Bootstrap: continuing normal startup.");
            return LauncherConstants.ExitCodeSuccess;
        }

        private static void PerformUninstall(bool enableLogging)
        {
            try
            {
                var appFolder = LauncherConstants.AppDataFolderName;
                var tempFile = Path.Combine(appFolder, "temp_update_marker.tmp");
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    if (enableLogging) FileHelpers.AppendToLogFile($"Uninstall: deleted {tempFile}");
                }
                // Additional uninstall steps per Implementation.md (data/game client) go here.
            }
            catch (Exception ex)
            {
                if (enableLogging) FileHelpers.AppendToLogFile($"Uninstall: exception: {ex}");
                throw;
            }
        }
    }
}