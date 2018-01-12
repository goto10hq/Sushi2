using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sushi2;

namespace Sushi2.Tests
{
    [TestClass]
    public class Extensions
    {
        [TestMethod]
        public void ToDbString()
        {
            Assert.AreEqual("foo", "   foo ".ToDbString());
            Assert.AreEqual("", Sushi2.Extensions.ToDbString(null));
            Assert.AreEqual("Yo", "Yo".ToDbString());
        }

        [TestMethod]
        public void ToNormalizedString()
        {
            Assert.AreEqual("foo", "   foo ".ToNormalizedString());
            Assert.AreEqual("", Sushi2.Extensions.ToNormalizedString(null));
            Assert.AreEqual("zlutoucky", " Žluťoučký ".ToNormalizedString());
        }

        [TestMethod]
        public void ToSortedString()
        {
            var texts = new List<string>
                        {
                            "abrakadabra",
                            "žlutý",
                            "úfo",
                            "zlo",
                            "Ět",
                            "zkazit",
                            "chobot",
                            "chčije a chčije",
                            "Číča",
                            "louka",
                            "007",
                            "ét",
                            "dům",
                            "Dúm",
                            "work",
                            "Číman",
                            "!",
                            "42",
                            "bříza",
                            "breeze",
                            "_"
                        };

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            var sorted = texts.Select(x => x).OrderBy(x => x.ToSortedString()).ToList();
            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.Czech;
            var linqSorted = texts.OrderBy(x => x.ToLower()).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                Assert.AreEqual(linqSorted[i], sorted[i]);
            }
        }

        [TestMethod]
        public void ToDateTime()
        {
            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(new DateTime(2017, 4, 6), "2017-04-06".ToDateTime());
            Assert.AreEqual(null, "foo".ToDateTime());
        }

        [TestMethod]
        public void ToByte()
        {
            const byte x = 100;
            const int r = 256;
            const int n = 42;

            Assert.AreEqual(x, "100".ToByte());
            Assert.AreEqual(null, "foo".ToByte());
            Assert.AreEqual(null, r.ToByte());
            Assert.AreEqual((byte)42, n.ToByte());
        }

        [TestMethod]
        public void ToInt16()
        {
            const short x = 100;
            const short r = 256;
            const long n = long.MaxValue;

            Assert.AreEqual(x, "100".ToInt16());
            Assert.AreEqual(null, "foo".ToInt16());
            Assert.AreEqual((short)256, r.ToInt16());
            Assert.AreEqual(null, n.ToInt16());
            Assert.AreEqual(short.MinValue, short.MinValue.ToInt16());
        }

        [TestMethod]
        public void ToInt32()
        {
            const int x = 100;
            const int r = -256;
            const long n = long.MaxValue;

            Assert.AreEqual(x, "100".ToInt32());
            Assert.AreEqual(null, "foo".ToInt32());
            Assert.AreEqual(-256, r.ToInt32());
            Assert.AreEqual(null, n.ToInt32());
            Assert.AreEqual(int.MinValue, int.MinValue.ToInt32());
        }

        [TestMethod]
        public void ToDouble()
        {
            const double x = 100.001;
            const double r = -256.33;

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(x, "100.001".ToDouble());
            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.Czech;
            Assert.AreEqual(x, "100,001".ToDouble());
            Assert.AreEqual(null, "100.001".ToDouble());

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(null, "foo".ToDouble());
            Assert.AreEqual(-256.33, r.ToDouble());
        }

        [TestMethod]
        public void ToInt64()
        {
            const int x = 100;
            const long r = -256;
            const long n = long.MaxValue;

            Assert.AreEqual(x, "100".ToInt64());
            Assert.AreEqual(null, "foo".ToInt64());
            Assert.AreEqual(-256, r.ToInt64());
            Assert.AreEqual(long.MaxValue, n.ToInt64());
        }

        [TestMethod]
        public void ToFloat()
        {
            const float x = 100.001f;
            const double r = -256.33;

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(x, "100.001".ToFloat());
            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.Czech;
            Assert.AreEqual(x, "100,001".ToFloat());
            Assert.AreEqual(null, "100.001".ToFloat());

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(null, "foo".ToFloat());
            Assert.AreEqual(-256.33f, r.ToFloat());
        }

        [TestMethod]
        public void ToDecimal()
        {
            const decimal x = 100.001m;
            const decimal r = -256.33m;

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(x, "100.001".ToDecimal());
            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.Czech;
            Assert.AreEqual(x, "100,001".ToDecimal());
            Assert.AreEqual(null, "100.001".ToDecimal());

            Thread.CurrentThread.CurrentCulture = Sushi2.Cultures.English;
            Assert.AreEqual(null, "foo".ToDecimal());
            Assert.AreEqual(-256.33m, r.ToDecimal());
        }

        [TestMethod]
        public void IsGuid()
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
                Assert.AreEqual(g.Value, g.Key.IsGuid(), $"Test failed for {g.Key} which should be {(g.Value ? "valid" : "invalid")}.");
            }

            Assert.AreEqual(true, Guid.NewGuid().IsGuid());
        }

        [TestMethod]
        public void ToGuid()
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
                var ng = g.Key.ToGuid();

                if (g.Value)
                    Assert.AreNotEqual(null, ng, $"Test failed for {g.Key} which should NOT be null.");
                else
                    Assert.AreEqual(null, ng, $"Test failed for {g.Key} which should be null.");
            }

            Assert.AreNotEqual(null, Guid.NewGuid().ToGuid());
        }

        [TestMethod]
        public void ToBoolean()
        {
            Assert.AreEqual(true, "true".ToBoolean());
            Assert.AreEqual(true, "True".ToBoolean());
            Assert.AreEqual(true, "1".ToBoolean());
            Assert.AreEqual(false, "false".ToBoolean());
            Assert.AreEqual(false, "FALSE".ToBoolean());
            Assert.AreEqual(false, "0".ToBoolean());

            Assert.AreEqual(true, ((int)1).ToBoolean());
            Assert.AreEqual(true, ((int)100).ToBoolean());
            Assert.AreEqual(false, ((int)0).ToBoolean());

            Assert.AreEqual(true, true.ToBoolean());
            Assert.AreEqual(false, false.ToBoolean());

            Assert.AreEqual(null, "XOXO".ToBoolean());
            Assert.AreEqual(null, "-".ToBoolean());
        }

        [TestMethod]
        public void ToStringWithoutDiacritics()
        {
            //TODO: test æ
            Assert.AreEqual("Prilis zlutoucky kun upel dabelske ody!", "Příliš žluťoučký kůň úpěl ďábelské ódy!".ToStringWithoutDiacritics());
            Assert.AreEqual("azk do kviz xnaca", "ázk do kvíz xnácá".ToStringWithoutDiacritics());
            Assert.AreEqual("AAAAAACEEEEIIIIDNOOOOOOUUUUYaaaaaaceeeeiiiinoooooouuuuyyAaAaAaCcCcDdDdEeEeEeEeGgGgIiIiIiKkLlLlLlLlNnNnNnOoOoRrRrRrSsSsSsTtTtUuUuUuUuYZzZzZzOoUu",
                            "ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïñòóôõöøùúûüýÿĀāĂăĄąĆćČčĎďĐđĒēĖėĘęĚěĞğĢģĪīĮįİıĶķĹĺĻļĽľŁłŃńŅņŇňŌōŐőŔŕŖŗŘřŚśŞşŠšŢţŤťŪūŮůŰűŲųŸŹźŻżŽžƠơƯư"
                                .ToStringWithoutDiacritics());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null string was inappropriately allowed.")]
        public void ToStringWithoutDiacriticsNullException()
        {
            string test = null;
            test.ToStringWithoutDiacritics();
        }

        [TestMethod]
        public void ToAccentInsensitiveSqlString()
        {
            Assert.AreEqual("f[ÓÖOoóö][ÓÖOoóö]", "foo".ToAccentInsensitiveSqlString());
            Assert.AreEqual("ch[ÓÖOoóö]", "cho".ToAccentInsensitiveSqlString());
            Assert.AreEqual("ch[ČčCc]ch!", "chcch!".ToAccentInsensitiveSqlString());
        }

        [TestMethod]
        public void ToMimeType()
        {
            Assert.AreEqual("image/jpeg", "foo.jpg".ToMimeType());
            Assert.AreEqual("image/jpeg", "foo.jpeg".ToMimeType());
            Assert.AreEqual("video/ogg", "video.ogv".ToMimeType());
            Assert.AreEqual("binary/octet-stream", "video.ioo".ToMimeType());
            Assert.AreEqual("binary/octet-stream", "bar".ToMimeType());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null string was inappropriately allowed.")]
        public void ToMimeTypeNullException()
        {
            string filename = null;
            filename.ToMimeType();
        }

        public class SmartTag : Attribute
        {
            public string Foo { get; }

            public SmartTag(string foo)
            {
                Foo = foo;
            }
        }

        public class FooTag : Attribute
        {
            public string Foo { get; }
        }

        public enum TestEnum
        {
            [SmartTag("goto10")]
            Goto10
        }

        [SmartTag("too smart")]
        public class Einstein
        {
            [SmartTag("name")]
            public string Name { get; set; }
        }

        [TestMethod]
        public void GetAttribute()
        {
            var a0 = TestEnum.Goto10.GetAttribute<SmartTag>();
            var a1 = TestEnum.Goto10.GetAttribute<FooTag>();

            Assert.AreNotEqual(null, a0);
            Assert.AreEqual("goto10", a0.Foo);
            Assert.AreEqual(null, a1);
        }

        [TestMethod]
        public void GetAttribute2()
        {
            var a0 = typeof(Einstein).GetAttribute<SmartTag>();
            var a1 = typeof(Einstein).GetAttribute<FooTag>();

            Assert.AreNotEqual(null, a0);
            Assert.AreEqual("too smart", a0.Foo);
            Assert.AreEqual(null, a1);
        }

        [TestMethod]
        public void GetAttributeValue()
        {
            var a0 = typeof(Einstein).GetAttributeValue((SmartTag a) => a.Foo);
            var a1 = typeof(Einstein).GetAttributeValue((FooTag a) => a.Foo);

            Assert.AreNotEqual(null, a0);
            Assert.AreEqual("too smart", a0);
            Assert.AreEqual(null, a1);
        }

        [TestMethod]
        public void GetPropertyValue()
        {
            var e = new Einstein { Name = "Cocona" };
            var a0 = e.GetPropertyValue("Name");
            var a1 = e.GetPropertyValue<string>("Name");
            var a2 = e.GetPropertyValue<string>("name");
            var n0 = e.GetPropertyValue<string>("FunnyBusiness");
            var n1 = e.GetPropertyValue<string>("Funny Business");

            Assert.AreNotEqual(null, a0);
            Assert.AreNotEqual(null, a1);
            Assert.AreEqual("Cocona", a0.ToString());
            Assert.AreEqual("Cocona", a1);
            Assert.AreEqual(null, a2);
            Assert.AreEqual(null, n0);
            Assert.AreEqual(null, n1);
        }

        [TestMethod]
        public void ToEpoch()
        {
            Assert.AreEqual(0, new DateTime(1970, 1, 1).ToEpoch());
            Assert.AreEqual(1483285244, new DateTime(2017, 1, 1, 15, 40, 44).ToEpoch());
            Assert.AreEqual(-1262276879, new DateTime(1930, 1, 1, 7, 32, 1).ToEpoch());
        }

        [TestMethod]
        public void ToIso8601WeekOfYear()
        {
            Assert.AreEqual(52, new DateTime(2017, 1, 1).ToIso8601WeekOfYear());
            Assert.AreEqual(1, new DateTime(2007, 12, 31).ToIso8601WeekOfYear());
            Assert.AreEqual(28, new DateTime(2017, 7, 15).ToIso8601WeekOfYear());
            Assert.AreEqual(28, new DateTime(2017, 7, 16).ToIso8601WeekOfYear());
            Assert.AreEqual(29, new DateTime(2017, 7, 17).ToIso8601WeekOfYear());
            Assert.AreEqual(29, new DateTime(2017, 7, 18).ToIso8601WeekOfYear());
            Assert.AreEqual(29, new DateTime(2017, 7, 23).ToIso8601WeekOfYear());
            Assert.AreEqual(30, new DateTime(2017, 7, 24).ToIso8601WeekOfYear());
            Assert.AreEqual(30, new DateTime(2017, 7, 29).ToIso8601WeekOfYear());
            Assert.AreEqual(30, new DateTime(2017, 7, 30).ToIso8601WeekOfYear());
        }

        [TestMethod]
        public void ToShuffledCollection()
        {
            var list = new List<int> { 2, 3, 4, 999, 1, 8, 777, 426, 5 };
            var list2 = new List<int> { 2, 3, 4, 999, 1, 8, 777, 426, 5 };

            Assert.AreEqual(list.Count, list2.Count);
            CollectionAssert.AreEqual(list, list2);

            var list3 = list2.ToShuffledCollection().ToList();
            Assert.AreEqual(list.Count, list2.Count);
            Assert.AreEqual(list.Count, list3.Count);
            CollectionAssert.AreEqual(list, list2);
            CollectionAssert.AreNotEqual(list, list3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null string was inappropriately allowed.")]
        public void ToImplodedStringNullException()
        {
            string test = null;
            test.ToImplodedString();
        }

        [TestMethod]
        public void ToImplodedString()
        {
            var list = new List<object> { "a", 3 };
            var list2 = new List<object> { "a", 3, "b" };

            Assert.AreEqual("a and 3", list.ToImplodedString(", ", " and "));
            Assert.AreEqual("a, 3 and b", list2.ToImplodedString(", ", " and "));
            Assert.AreEqual("a, 3, b", list2.ToImplodedString());
            Assert.AreEqual("a3b", list2.ToImplodedString(string.Empty));
        }

        [TestMethod]
        public void ToReplacedString()
        {
            Assert.AreEqual("hello Mr. Rob!", "hello Mr. Bob!".ToReplacedString("bob", "Rob"));
            Assert.AreEqual("hello Mr. Bob!", "hello Mr. Bob!".ToReplacedString("bob", "Rob", StringComparison.Ordinal));
            Assert.AreEqual("+++", "...".ToReplacedString(".", "+"));
            Assert.AreEqual("+.", "...".ToReplacedString("..", "+"));
        }
    }
}