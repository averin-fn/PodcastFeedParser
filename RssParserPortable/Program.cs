using PodcastDataLib;
using System;
using PodcastRssParser;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace RssParserPortable
{
    class Program
    {
        private static readonly string _url = "https://www.12or19.com/podcast?format=rss";
        private static readonly string _userAgent = 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/64.0.3282.140 " +
            "Safari/537.36 " +
            "Edge/18.17763";

        static void Main(string[] args)
        {
            string feed = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
                req.UserAgent = _userAgent;
                WebResponse response = req.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    feed = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{ex.Message}",
                    "Error");
            }

            if (!string.IsNullOrWhiteSpace(feed))
            {
                var parser = new RssParser();
                var podcast = parser.Parse<Podcast>(feed);

                Console.WriteLine($"Channel Title: {podcast.Title}\n");

                if (podcast.Episodes != null)
                {
                    foreach (var item in podcast.Episodes)
                    {
                        Console.WriteLine(
                            $"Episode: {item.Itunes_Episode}\n" +
                            $"Title: {item.Title}\n");
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
