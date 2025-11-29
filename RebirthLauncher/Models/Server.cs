using System.Collections.Generic;

namespace RebirthLauncher.Models
{
    /// <summary>
    /// Contains info about a particular CoH server, pulled from the manifest for that server
    /// </summary>
    public class Server
    {
        public string ManifestUrl { get; set; } = "";
        public string ServerName { get; set; } = "";
        public string Icon { get; set; } = "";
        public string BackgroundImage { get; set; } = "";
        public string StartParams { get; set; } = "";
        public string Exec { get; set; } = "";
        public bool IsValidated { get; set; } = false;
        public string ManifestHash { get; set; } = "";
        public List<BrowseButton> BrowseButtonList { get; set; } = new();
    }
}