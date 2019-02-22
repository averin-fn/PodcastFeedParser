using PodcastDataLib;
using PodcastRssParser;
using System.IO;
using System.Net;
using Xunit;

namespace PodcastRssParserTest
{
    public class Test
    {
        private readonly RssParser _parser = new RssParser();
        private readonly string _userAgent = 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/64.0.3282.140 " +
            "Safari/537.36 " +
            "Edge/18.17763";

        [Theory]
        [InlineData("https://beardycast.libsyn.com/rss")]
        [InlineData("https://rss.simplecast.com/podcasts/4121/rss")]
        [InlineData("https://rss.simplecast.com/podcasts/5513/rss")]
        [InlineData("https://www.12or19.com/podcast?format=rss")]
        [InlineData("http://feeds.soundcloud.com/users/soundcloud:users:317505434/sounds.rss")]
        [InlineData("http://feeds.rucast.net/radio-t")]
        public void ParseTest(string url)
        {
            var feed = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = _userAgent;
            WebResponse response = req.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                feed = reader.ReadToEnd();
            }

            var podcast = _parser.Parse<Podcast>(feed);

            Assert.True(podcast.Episodes.Count > 0,
                $"{podcast.Title}: Count of episodes must be more then 0.");

            Assert.True(podcast.Title != null &&
                podcast.Episodes != null,
                $"{podcast.Title}: Podcast must be not null.");

            Assert.True(podcast.Itunes_Image != null,
                $"{podcast.Title}" +
                $"Image must be not null");

            foreach (var episode in podcast.Episodes)
            {
                Assert.True(episode.Title != null &&
                    !string.IsNullOrWhiteSpace(episode.Enclosure.Url),
                    $"{podcast.Title} - {episode.Title}: " +
                    $"Fields (Title, Enclosure.Url) must be not null or white space.");

                Assert.True(episode.Image != null,
                    $"{podcast.Title} - {episode.Title}: " +
                    $"Image must be not null");
            }
        }
    }
}
