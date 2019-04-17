using System;

namespace RssParserLib
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
