using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace RssParserLib
{
    public class RssParser
    {
        private Dictionary<string, string> Definitions;

        /// <summary>
        /// Parse RSS feed
        /// </summary>
        /// <typeparam name="T">Type of object that will be returned</typeparam>
        /// <param name="value">RSS feed</param>
        public T Parse<T>(string value) where T : new()
        {
            var result = new T();
            if (!string.IsNullOrWhiteSpace(value))
            {
                XDocument rss = XDocument.Parse(value);
                Definitions = GetDefinitions(rss);
                ParseElement(rss.Element("rss").Element("channel"), ref result);
            }
            return result;
        }

        /// <summary>
        /// Get definitions from XML document
        /// example: itunes for (itunes:title)
        /// </summary>
        private Dictionary<string, string> GetDefinitions(XDocument doc)
        {
            var result = new Dictionary<string, string>();

            try
            {
                foreach (var attr in doc.Element("rss").Attributes())
                {
                    string[] array = attr.Name.ToString().Split('}');
                    if (array.Length == 2)
                    {
                        result.Add(array[1], attr.Value);
                    }
                }
            }
            catch
            {
                throw new Exception("Get definition error.");
            }
            return result;
        }

        /// <summary>
        /// Compare RssElementNameAttribute with definitions
        /// </summary>
        /// <param name="attrName">Property attribute name for RSS parser
        /// example: itunes:title => {http://www.itunes.com/dtds/podcast-1.0.dtd}title
        /// </param>
        /// <returns>Comparable string or null</returns>
        private string ComparableRssAttrName(string attrName)
        {
            string[] array = attrName.Split(':');

            if (array.Length == 2)
            {
                if(Definitions.TryGetValue(array.First(), out string value))
                {
                    return $"{{{value}}}{array[1]}";
                }
                return null;
            }
            return attrName;
        }

        /// <summary>
        /// Parse element of xml
        /// </summary>
        private void ParseElement<T>(XElement element, ref T result)
        {
            if (element == null) return;

            PropertyInfo[] propertyInfos = result.GetType().GetProperties();

            foreach (var property in propertyInfos)
            {
                foreach (var attr in property.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(RssElementNameAttribute))
                    { 
                        var compareString = ComparableRssAttrName(attr.ConstructorArguments.First().Value.ToString());

                        if (compareString != null)
                        {
                            var e = element.Elements(compareString);

                            if (e == null || e.Count() == 0) break;

                            if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(result, e.FirstOrDefault().Value); break;
                            }

                            if (property.PropertyType.IsGenericType)
                            {
                                property.SetValue(result, SetPropertyGenericType(property, e)); break;
                            }

                            if (property.PropertyType.IsClass)
                            {
                                property.SetValue(result, SetPropertyClass(property, e)); break;
                            }
                        }
                    }

                    if (attr.AttributeType == typeof(RssAttributeNameAttribute))
                    {
                        var attrValue = attr.ConstructorArguments.First().Value.ToString();
                        var e = element.Attribute(attrValue);

                        if (e == null) break;
                        
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(result, e.Value); break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create collection objects to generic types
        /// </summary>
        private IList SetPropertyGenericType(PropertyInfo property, IEnumerable<XElement> e)
        {
            var genericType = property.PropertyType.GenericTypeArguments.First();
            var collection = (IList)typeof(List<>).MakeGenericType(genericType).GetConstructor(Type.EmptyTypes).Invoke(null);

            foreach (var item in e)
            {
                var obj = Activator.CreateInstance(genericType);
                ParseElement(item, ref obj);
                collection.Add(obj);
            }

            return collection;
        }


        /// <summary>
        /// Create specific class object
        /// </summary>
        private object SetPropertyClass(PropertyInfo property, IEnumerable<XElement> e)
        {
            var obj = Activator.CreateInstance(property.PropertyType);
            ParseElement(e.FirstOrDefault(), ref obj);
            return obj;
        }
    }
}
