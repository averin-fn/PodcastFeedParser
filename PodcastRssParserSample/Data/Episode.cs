using RssParserLib;

namespace RssParserSample.Data
{
    public class Episode
    {
        [RssElementName("title")]
        public string Title { get; set; }
        
        [RssElementName("itunes:title")]
        public string Itunes_Title { get; set; }

        [RssElementName("pubDate")]
        public string PubDate { get; set; }

        [RssElementName("guid")]
        public string Guid { get; set; }

        [RssElementName("link")]
        public string Link { get; set; }

        [RssElementName("itunes:image")]
        public ItunesImage Image { get; set; }

        [RssElementName("description")]
        public string Description { get; set; }

        [RssElementName("content:encoded")]
        public string Content_Encoded { get; set; }

        [RssElementName("enclosure")]
        public Enclosure Enclosure { get; set; }

        [RssElementName("itunes:duration")]
        public string Itunes_Duration { get; set; }

        [RssElementName("itunes:explicit")]
        public string Itunes_Explicit { get; set; }

        [RssElementName("itunes:keywords")]
        public string Itunes_Keywords { get; set; }

        [RssElementName("itunes:subtitle")]
        public string Itunes_Subtitle { get; set; }

        [RssElementName("itunes:summary")]
        public string Itunes_Summary { get; set; }

        [RssElementName("itunes:season")]
        public string Itunes_Season { get; set; }

        [RssElementName("itunes:episode")]
        public string Itunes_Episode { get; set; }

        [RssElementName("itunes:episodeType")]
        public string Itunes_EpisodeType { get; set; }

        [RssElementName("itunes:author")]
        public string Itunes_Author { get; set; }
    }
}