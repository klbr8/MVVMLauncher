using System.Xml.Serialization;

namespace RebirthLauncher.Models
{
    [XmlRoot(ElementName = "launch")]
    public class Launch
    {

        [XmlAttribute(AttributeName = "params")]
        public string Params { get; set; } = "";

        [XmlAttribute(AttributeName = "order")]
        public int Order { get; set; }

        [XmlAttribute(AttributeName = "exec")]
        public string Exec { get; set; } = "";

        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; } = "";

        [XmlAttribute(AttributeName = "motd")]
        public string Motd { get; set; } = "";

        [XmlAttribute(AttributeName = "website")]
        public string Website { get; set; } = "";

        [XmlText]
        public string Text { get; set; } = "";
    }

    [XmlRoot(ElementName = "profiles")]
    public class Profiles
    {

        [XmlElement(ElementName = "launch")]
        public Launch Launch { get; set; } = new Launch();
    }

    [XmlRoot(ElementName = "file")]
    public class OFile
    {

        [XmlElement(ElementName = "url")]
        public string Url { get; set; } = "";

        [XmlAttribute(AttributeName = "md5")]
        public string Hash { get; set; } = "";

        [XmlAttribute(AttributeName = "size")]
        public ulong Size { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; } = "";

        [XmlText]
        public string Text { get; set; } = "";
    }

    [XmlRoot(ElementName = "filelist")]
    public class Filelist
    {

        [XmlElement(ElementName = "file")]
        public List<OFile> File { get; set; } = new List<OFile>();
    }

    [XmlRoot(ElementName = "buttonlist")]
    public class UIButtonList
    {

        [XmlElement(ElementName = "button")]
        public List<UIButton> UIButton { get; set; } = new List<UIButton>();
    }

    [XmlRoot(ElementName = "button")]
    public class UIButton
    {

        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; } = "";

        [XmlText]
        public string Text { get; set; } = "";

    }

    [XmlRoot(ElementName = "forum")]
    public class Forum
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; } = "";

        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; } = "";
    }

    [XmlRoot(ElementName = "forums")]
    public class Forums
    {

        [XmlElement(ElementName = "forum")]
        public Forum Forum { get; set; } = new Forum();
    }

    [XmlRoot(ElementName = "poster_image")]
    public class PosterImage
    {

        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; } = "";
    }

    [XmlRoot(ElementName = "manifest")]
    public class Manifest
    {

        [XmlElement(ElementName = "label")]
        public string Label { get; set; } = "";

        [XmlElement(ElementName = "profiles")]
        public Profiles Profiles { get; set; } = new Profiles();

        [XmlElement(ElementName = "filelist")]
        public Filelist Filelist { get; set; } = new Filelist();

        [XmlElement(ElementName = "forums")]
        public Forums Forums { get; set; } = new Forums();

        [XmlElement(ElementName = "webpage")]
        public string Webpage { get; set; } = "";

        [XmlElement(ElementName = "poster_image")]
        public PosterImage PosterImage { get; set; } = new PosterImage();

        [XmlElement(ElementName = "buttonlist")]
        public UIButtonList UIButtonList { get; set; } = new UIButtonList();
    }
}
