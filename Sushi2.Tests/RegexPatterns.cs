using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sushi2.Tests
{
    [TestClass]
    public class RegexPatterns
    {
        [TestMethod]
        public void Email()
        {
            var emails = new Dictionary<string, bool>
                         {
                             { "mr@robot.com", true },
                             { "tsumugi+kawaii@i-live-here.in", true },
                             //{ "email@123.123.123.123", true },
                             { "_______@example.com", true },
                             { "indiana-jones@12345.museum", true },
                             { "email@111.222.333.44444", false },
                             { "classic.fake@omg..com", false },
                             //{ "email@-example.com", false },
                             { "email@example", false },
                             { "email@example.com (Mr. Robot)", false },
                             { "classic..fake@omg.com", false },
                             { "classicfake.@omg.com", false },
                             { ".classicfake@omg.com", false },
                             { "email@example@example.com", false },
                             { "email.example.com", false },
                             { "plain", false },
                             { "@example.com", false },
                             { "#@%^%#$@#$@#.com", false },
                             { "robot@yoyo.systems", true }
                         };

            foreach (var e in emails)
            {
                Assert.AreEqual(e.Value, Regex.IsMatch(e.Key, Sushi2.RegexPatterns.Email), $"Test failed for {e.Key} which should be {(e.Value ? "valid" : "invalid")}.");
            }
        }

        [TestMethod]
        public void Url()
        {
            var urls = new Dictionary<string, bool>
                       {
                           { "http://www.goto10.cz", true },
                           { "http://frohikey.com", true },
                           { "http://google.com/?search=drone%20plex", true },
                           { "http://google.com/?search=drone%20plex&type=link", true },
                           { "https://google.com/?search=drone%20plex&amp;type=link", true },
                           { "https://foo.com/my_post/aloha", true },
                           { "https://foo.com/my_post/aloha_(ehlo)", true },
                           { "http://✪df.ws/123", true },
                           { "http://userid:password@example.com:8080", true },
                           { "http://userid@example.com:8080", true },
                           { "http://142.42.1.1/", true },
                           { "http://142.42.1.1:8080/", true },
                           { "http://➡.ws/䨹", true },
                           { "http://foo.com/blah_(wikipedia)_blah#cite-1", true },
                           { "http://foo.com/(something)?after=parens", true },
                           { "http://code.google.com/events/#&product=browser", true },
                           { "http://-.~_!$&'()*+,;=:%40:80%2f::::::@example.com", true },

                           { "frohikey.com", false },
                           { "https://frohikey.com&amp;x", false },
                           { "http://foo.bar?q=Spaces should be encoded", false },
                           { "http://", false },
                           { "http://.", false },
                           { "http://..", false },
                           { "http://../", false },
                           { "http://?", false },
                           { "http://??", false },
                           { "http://??/", false },
                           { "http://#", false },
                           { "http://##", false },
                           { "http://##/", false },
                           { "//", false },
                           { "rdar://1234", false },
                           { "https:// ha ha", false },
                           { "http://-error-.invalid/", false },
                           { "http://0.0.0.0", false },
                           { "http://10.1.1.255", false },
                           { "http://10.1.1.0", false },
                           { "http://123.123.123", false }
                       };

            foreach (var u in urls)
            {
                Assert.AreEqual(u.Value, Regex.IsMatch(u.Key, Sushi2.RegexPatterns.Url), $"Test failed for {u.Key} which should be {(u.Value ? "valid" : "invalid")}.");
            }
        }

        [TestMethod]
        public void GuidTest()
        {
            var guids = new Dictionary<string, bool>
                         {
                             { "21CD5E48-0AD5-4DB7-8667-19A77D4CF89E", true },
                             { "B3767076-6C3E-42BC-B501-AF0D385179D8", true },
                             { "28303425741D4253903B8D3FDCB0DA73", true },
                             { "{26E348C6-DA63-485E-A35B-F8DC5DA84ABC}", true },

                            { "FOO-FOO-FOO-FOO-FOO", false },
                            { "!26E348C6-DA63-485E-A35B-F8DC5DA84ABC!", false },
                            { "17ED8272-9280-4B14-8816-82525FF3B0C", false },
                            { "38062CB7-6893-4BC4-8AB3-41722Z60E8FA", false },
                         };

            foreach (var g in guids)
            {
                Assert.AreEqual(g.Value, Regex.IsMatch(g.Key, Sushi2.RegexPatterns.Guid), $"Test failed for {g.Key} which should be {(g.Value ? "valid" : "invalid")}.");
            }
        }
    }
}