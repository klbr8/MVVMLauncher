using System.Collections.Generic;
using System.Xml.Serialization;

namespace RebirthLauncher.Models
{
    [XmlRoot("ServerList")]
    public class ServerList
    {
        [XmlElement("Server")]
        public List<Server> Servers { get; set; } = new();
    }
}