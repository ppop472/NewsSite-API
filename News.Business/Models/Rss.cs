using System.Xml.Serialization;

[XmlRoot("rss")]
public class Rss
{
    [XmlElement("channel")]
    public Channel[] Channels { get; set; } = [];
}

[XmlRoot("channel")]
public class Channel
{
    [XmlElement("title")]
    public string Title { get; set; } = string.Empty;
    [XmlElement("description")]
    public string Description { get; set; } = string.Empty;
    [XmlElement("link")]
    public string Link { get; set; } = string.Empty;
    [XmlElement("language")]
    public string Language { get; set; } = string.Empty;
    [XmlElement("lastBuildDate")]
    public string LastBuildDate { get; set; } = string.Empty;
    [XmlElement("item")]
    public Item[] Items { get; set; } = [];
}

[XmlRoot("guid")]
public class RssGuid
{
    [XmlText]
    public string Value { get; set; } = string.Empty;
}

[XmlRoot("item")]
public class Item
{
    [XmlElement("guid")]
    public RssGuid? Guid { get; set; }
    [XmlElement("title")]
    public string Title { get; set; } = string.Empty;
    [XmlElement("description")]
    public string Description { get; set; } = string.Empty;
    [XmlElement("creator", Namespace = "http://purl.org/dc/elements/1.1/")]
    public string Creator { get; set; } = string.Empty;
    [XmlElement("link")]
    public string Link { get; set; } = string.Empty;
    [XmlElement("pubDate")]
    public string PublishDate { get; set; } = string.Empty;
    [XmlElement("encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
    public string Content { get; set; } = string.Empty;
    [XmlElement("category")]
    public string[] Categories { get; set; } = [];
    [XmlElement("enclosure")]
    public Enclosure[] Enclosures { get; set; } = [];
}

public class Enclosure
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;
    [XmlAttribute("url")]
    public string Url { get; set; } = string.Empty;
}