using System.Collections.Generic;
using RssParserLib;

namespace RssParserSample.Data
{
    public class Podcast
    {
        [RssElementName("title")]
        public string Title { get; set; }

        [RssElementName("pubDate")]
        public string PubDate { get; set; }

        [RssElementName("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [RssElementName("generator")]
        public string Generator { get; set; }

        [RssElementName("link")]
        public string Link { get; set; }

        [RssElementName("language")]
        public string Language { get; set; }

        [RssElementName("copyright")]
        public string Copyright { get; set; }

        [RssElementName("docs")]
        public string Docs { get; set; }

        [RssElementName("managingEditor")]
        public string ManagingEditor { get; set; }

        [RssElementName("itunes:summary")]
        public string Itunes_Summary { get; set; }

        [RssElementName("image")]
        public Image Image { get; set; }

        [RssElementName("itunes:author")]
        public string Itunes_Author { get; set; }

        [RssElementName("itunes:keywords")]
        public string Itunes_Keywords { get; set; }

        [RssElementName("itunes:category")]
        public ICollection<Category> Categories { get; set; }

        [RssElementName("itunes:image")]
        public ItunesImage Itunes_Image { get; set; }

        [RssElementName("itunes:explicit")]
        public string Itunes_Explicit { get; set; }

        [RssElementName("itunes:owner")]
        public Owner Itunes_Owner { get; set; }

        [RssElementName("description")]
        public string Description { get; set; }

        [RssElementName("itunes:subtitle")]
        public string Itunes_Subtitle { get; set; }

        [RssElementName("itunes:type")]
        public string Itunes_Type { get; set; }

        [RssElementName("item")]
        public ICollection<Episode> Episodes { get; set; }
    }
}
