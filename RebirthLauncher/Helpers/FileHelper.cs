// RebirthLauncher/Helpers/FileHelpers.cs
using System.IO;
using System.Text;

namespace RebirthLauncher.Helpers
{
    internal static class FileHelpers
    {
        public static void AppendToLogFile(string message)
        {
            try
            {
                var folder = RebirthLauncher.Constants.LauncherConstants.AppDataFolderName;
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, RebirthLauncher.Constants.LauncherConstants.LogFileName);
                var line = $"{DateTime.UtcNow:O} {message}{Environment.NewLine}";
                File.AppendAllText(path, line, Encoding.UTF8);
            }
            catch
            {
                // Swallow logging exceptions.
            }
        }
        public static bool TryDeleteFile(string path, out string diagnostic)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    diagnostic = "TryDeleteFile: path was null or empty.";
                    return false;
                }

                if (!File.Exists(path))
                {
                    diagnostic = $"TryDeleteFile: file not found: {path}";
                    return false;
                }

                File.Delete(path);

                diagnostic = $"TryDeleteFile: deleted {path}";
                return true;
            }
            catch (Exception ex)
            {
                diagnostic = $"TryDeleteFile: failed to delete {path}: {ex.GetType().Name}: {ex.Message}";
                return false;
            }
        }
    }
}
