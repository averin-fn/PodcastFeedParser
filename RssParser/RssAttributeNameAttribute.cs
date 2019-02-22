using System;
using System.Collections.Generic;
using System.Text;

namespace PodcastRssParser
{
    public class RssAttributeNameAttribute : Attribute
    {
        private readonly string name;

        public RssAttributeNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
