using PodcastRssParser;

namespace PodcastDataLib
{
    public class ItunesImage
    {
        [RssAttributeName("href")]
        public string Url { get; set; }
    }
}