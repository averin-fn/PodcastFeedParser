﻿using PodcastRssParser;

namespace PodcastDataLib
{
    public class Image
    {
        [RssElementName("url")]
        public string Url { get; set; }

        [RssElementName("title")]
        public string Title { get; set; }

        [RssElementName("link")]
        public string Link { get; set; }
    }
}