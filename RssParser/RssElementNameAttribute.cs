using System;
using System.Collections.Generic;
using System.Text;

namespace PodcastRssParser
{
    public class RssElementNameAttribute : Attribute
    {
        private readonly string name;

        public RssElementNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
