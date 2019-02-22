using PodcastRssParser;

namespace PodcastDataLib
{
    public class Owner
    {
        [RssElementName("itunes:name")]
        public string Name { get; set; }
        [RssElementName("itunes:email")]
        public string Email { get; set; }
    }
}