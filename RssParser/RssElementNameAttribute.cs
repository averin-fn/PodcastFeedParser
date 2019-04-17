using System;

namespace RssParserLib
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
