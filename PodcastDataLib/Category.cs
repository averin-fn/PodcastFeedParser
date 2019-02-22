using PodcastRssParser;

namespace PodcastDataLib
{
    public class Category
    {
        [RssAttributeName("text")]
        public string Title { get; set; }

        [RssElementName("itunes:category")]
        public Category SubCategory { get; set; }
    }
}