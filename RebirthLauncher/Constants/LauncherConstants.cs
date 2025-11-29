using System.IO;

namespace RebirthLauncher.Constants
{
    public static class LauncherConstants
    {
        public const string AppDataFolder = "RebirthLauncher";
        public const string ServerListFileName = "servers.xml";
        public const string DefaultManifestUrl = "https://patch.cityofheroesrebirth.com/manifest.xml";

        public static string AppDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataFolder);

        public static string ServerListPath =>
            Path.Combine(AppDataPath, ServerListFileName);
    }
}