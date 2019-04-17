using RssParserLib;

namespace RssParserSample.Data
{
    public class ItunesImage
    {
        [RssAttributeName("href")]
        public string Url { get; set; }
    }
}