using RssParserLib;

namespace RssParserSample.Data
{
    public class Enclosure
    {
        [RssAttributeName("length")]
        public string Lenght { get; set; }

        [RssAttributeName("type")]
        public string Type { get; set; }

        [RssAttributeName("url")]
        public string Url { get; set; }
    }
}