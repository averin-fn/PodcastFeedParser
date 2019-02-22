using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PodcastRssParser
{
    public class RssParser
    {
        private Dictionary<string, string> Definitions = new Dictionary<string, string>();

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
                try
                {
                    XDocument rss = XDocument.Parse(value);
                    Trace.WriteLine("RSS parse success",
                        "Info");
                    GetDefinitions(rss);
                    ParseElement(rss.Element("rss")
                        .Element("channel"), ref result);
                    Trace.WriteLine("Podcast parsing completed",
                        "Info");
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message,
                        "Error");
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// Get defenitions from XML document
        /// </summary>
        /// <param name="doc">XML document</param>
        private void GetDefinitions(XDocument doc)
        {
            try
            {
                foreach (var attr in doc.Element("rss").Attributes())
                {
                    string[] array = attr.Name.ToString().Split('}');
                    if (array.Length > 1)
                    {
                        Definitions.Add(array.Last(), attr.Value);
                    }
                }
                Trace.WriteLine("Get definitions success",
                    "Info");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message,
                    "Error");
            }
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

            if (array.Length > 1)
            {
                Definitions.TryGetValue(array.First(), out string value);
                return  value != null ? 
                    $"{{{value}}}{array.Last()}"
                    : null;
            }
            return attrName;
        }

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
                                try
                                {
                                    property.SetValue(result, e.FirstOrDefault().Value);
                                }
                                catch(Exception ex)
                                {
                                    Trace.WriteLine($"\n" +
                                        $"Property: {property.Name}\n" +
                                        $"XElement: {e.FirstOrDefault()}" +
                                        $"Message: {ex.Message}",
                                        "Error");
                                }
                                break;
                            }

                            if (property.PropertyType.IsGenericType)
                            {
                                var genericType = property.PropertyType.GenericTypeArguments.First();
                                var collection = (IList)typeof(List<>).MakeGenericType(genericType).GetConstructor(Type.EmptyTypes).Invoke(null);

                                foreach (var item in e)
                                {
                                    var obj = Activator.CreateInstance(genericType);
                                    ParseElement(item, ref obj);
                                    collection.Add(obj);
                                }

                                try
                                {
                                    property.SetValue(result, (ICollection)collection);
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine($"\n" +
                                        $"Property: {property.Name}\n" +
                                        $"XElement: {e.FirstOrDefault()}" +
                                        $"Message: {ex.Message}",
                                        "Error");
                                }
                            }

                            if (property.PropertyType.IsClass)
                            {
                                var obj = Activator.CreateInstance(property.PropertyType);
                                ParseElement(e.FirstOrDefault(), ref obj);
                                try
                                {
                                    property.SetValue(result, obj);
                                }
                                catch(Exception ex)
                                {
                                    Trace.WriteLine($"\n" +
                                        $"Property: {property.Name}\n" +
                                        $"XElement: {e.FirstOrDefault()}" +
                                        $"Message: {ex.Message}",
                                        "Error");
                                }
                                break;
                            }
                        }
                    }

                    if (attr.AttributeType == typeof(RssAttributeNameAttribute))
                    {
                        var attrValue = attr.ConstructorArguments.First().Value.ToString();
                        var e = element.Attribute(attrValue);
                        if(e == null) break;
                        
                        if (property.PropertyType == typeof(string))
                        {
                            try
                            {
                                property.SetValue(result, e.Value);
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine($"\n" +
                                    $"Property: {property.Name}\n" +
                                    $"XElement: {e}" +
                                    $"Message: {ex.Message}", 
                                    "Error");
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
