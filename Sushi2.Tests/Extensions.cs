﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void ToCzechSortedStringWeirdChars()
        {
            Assert.AreEqual(",,someaething#* .* eaetc. night or e*b*y", "„something“ – etc. night or ďáy".ToCzechSortedString());
        }
        
        [TestMethod]
        public void ToCzechSortedString()
        {
            var texts = new List<string>
                        {
                            "abrakadabra",
                            //"žlutý",
                            "úfo",
                            "zlo",
                            "Ět",
                            "zkazit",
                            "chobot",
                            "chčije a chčije",
                            "žluťoučký kůň",
                            "žluťoučký kúň",
                            "Číča",
                            "louka",
                            "007",
                            "ét",
                            "dům",
                            "ďum",
                            "ďúm",
                            "Dúm",
                            "work",
                            "Číman",
                            "!",
                            "42",
                            //"šasek",
                            "sašek",
                            "šálek",
                            "bříza",
                            "breeze",
                            "_",
                            " e ",
                            "Žluťoučký kúň",
                            "Zluťoučký kúň",
                            "Chobot",
                            "CHOBOT",
                            "Joker´s bar"                            
                        };

            var sorted = texts.Select(x => x).OrderBy(x => x.ToCzechSortedString(), StringComparer.Create(Sushi2.Cultures.English, false)).ToList();
            var orderedTexts = texts.Select(x => x).OrderBy(x => x, StringComparer.Create(Sushi2.Cultures.Czech, false)).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                Assert.AreEqual(orderedTexts[i], sorted[i]);
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
            Assert.AreEqual(x, "100 ".ToInt16());
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
            Assert.AreEqual(x, " 100 ".ToInt32());
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
            Assert.AreEqual(x, " 100.001 ".ToDouble());
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
            Assert.AreEqual(x, " 100 ".ToInt64());
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
            Assert.AreEqual(x, " 100.001 ".ToFloat());
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
            Assert.AreEqual(x, " 100.001 ".ToDecimal());
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
            Assert.AreEqual(true, " True ".ToBoolean());
            Assert.AreEqual(true, "1".ToBoolean());
            Assert.AreEqual(true, " 1 ".ToBoolean());
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
            Assert.AreEqual("Prilis zlutoucky kun upel dabelske ody!", "Příliš žluťoučký kůň úpěl ďábelské ódy!".ToStringWithoutDiacritics());
            Assert.AreEqual("azk do kviz xnaca", "ázk do kvíz xnácá".ToStringWithoutDiacritics());
            Assert.AreEqual("AAAAAACEEEEIIIIDNOOOOOOUUUUYaaaaaaceeeeiiiinoooooouuuuyyAaAaAaCcCcDdDdEeEeEeEeGgGgIiIiIiKkLlLlLlLlNnNnNnOoOoRrRrRrSsSsSsTtTtUuUuUuUuYZzZzZzOoUu",
                            "ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïñòóôõöøùúûüýÿĀāĂăĄąĆćČčĎďĐđĒēĖėĘęĚěĞğĢģĪīĮįİıĶķĹĺĻļĽľŁłŃńŅņŇňŌōŐőŔŕŖŗŘřŚśŞşŠšŢţŤťŪūŮůŰűŲųŸŹźŻżŽžƠơƯư"
                                .ToStringWithoutDiacritics());
            Assert.AreEqual("ae", "æ".ToStringWithoutDiacritics());
            Assert.AreEqual("Olga", "Ольга".ToStringWithoutDiacritics());
            Assert.AreEqual("Tatyana", "Татьяна".ToStringWithoutDiacritics());
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

        [AttributeUsage(AttributeTargets.All)]
        class SmartTag : Attribute
        {
            public string Foo { get; }

            public SmartTag(string foo)
            {
                Foo = foo;
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        class FooTag : Attribute
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

        [TestMethod]
        public void ToFilename()
        {
            Assert.AreEqual("aaa", "aaa".ToFilename());
            Assert.AreEqual("aaa", "aAa".ToFilename());
            Assert.AreEqual("a_3", "a 3".ToFilename());
            Assert.AreEqual("a_3", "a 3".ToFilename());
            Assert.AreEqual("a_-_3", "a 3".ToFilename("_-_"));
            Assert.AreEqual("caca__jo.txt", "čaČa!.jo.txt".ToFilename(removeDoubledSafeParts: false));
            Assert.AreEqual("caca_jo.txt", "čaČa!.jo.txt".ToFilename());
            Assert.AreEqual("caca_jo.txt", "čaČa.jo.txt".ToFilename(removeDoubledSafeParts: false));
            Assert.AreEqual("caca_.jo.txt", "čaČa!.jo.txt".ToFilename(removeAdditionalDots: false));
        }

        public enum Robot
        {
            [System.ComponentModel.Description("First")]
            One = 1,
            [System.ComponentModel.Description("Second")]
            Two = 2,
            Three = 3
        }

        [TestMethod]
        public void EnumToolsDescription()
        {
            Assert.AreEqual("First", EnumTools.GetEnumFieldDescription(Robot.One));
            Assert.AreNotEqual("First", EnumTools.GetEnumFieldDescription(Robot.Two));
            Assert.AreEqual("Three", EnumTools.GetEnumFieldDescription(Robot.Three));
        }

        [TestMethod]
        public void EnumToolsParse()
        {
            Assert.AreEqual(Robot.One, EnumTools.Parse<Robot>("1"));
            Assert.AreEqual(Robot.One, EnumTools.Parse<Robot>("One"));
            Assert.AreEqual(Robot.One, EnumTools.Parse<Robot>(Robot.One));
            Assert.AreEqual(Robot.One, EnumTools.Parse<Robot>(1));
            Assert.AreEqual(null, EnumTools.Parse<Robot>(4));
        }

        [TestMethod]
        public void EnumToolsGetNext()
        {
            Assert.AreEqual(Robot.Two, EnumTools.GetNext(Robot.One));
            Assert.AreEqual(Robot.Three, EnumTools.GetNext(Robot.Two));
            Assert.AreEqual(Robot.One, EnumTools.GetNext(Robot.Three));
        }

        [TestMethod]
        public void EnumToolsGetPrevious()
        {
            Assert.AreEqual(Robot.Three, EnumTools.GetPrevious(Robot.One));
            Assert.AreEqual(Robot.One, EnumTools.GetPrevious(Robot.Two));
            Assert.AreEqual(Robot.Two, EnumTools.GetPrevious(Robot.Three));
        }

        [TestMethod]
        public void ToSlug()
        {
            Assert.AreEqual("aaa", "aaa".ToSlug());
            Assert.AreEqual("aaa", "aAa".ToSlug());
            Assert.AreEqual("a-3", "a 3".ToSlug());
            Assert.AreEqual("a_-_3", "a 3".ToSlug("_-_"));
            Assert.AreEqual("caca--jo-txt", "čaČa!.jo.txt".ToSlug(removeDoubledSafeParts: false));
            Assert.AreEqual("caca-jo-txt", "čaČa!.jo.txt".ToSlug());
            Assert.AreEqual("caca-jo-txt", "čaČa.jo.txt".ToSlug(removeDoubledSafeParts: false));
            Assert.AreEqual("-", "__".ToSlug());
            Assert.AreEqual("000", "_000_".ToSlug());
            Assert.AreEqual("hello-hi", "hello hi aloha".ToSlug(maxLength: 10));
            Assert.AreEqual("hello-hi-aloha", "hello hi aloha".ToSlug(maxLength: 14));
            Assert.AreEqual("hel", "hello hi aloha".ToSlug(maxLength: 3));
            Assert.AreEqual("hello-hi-aloha", "!hello hi aloha!".ToSlug());
            Assert.AreEqual("stati-o-poligrafii-i-pechatnom-dele", "Статьи о полиграфии и печатном деле".ToSlug());
            Assert.AreEqual("olga", "Ольга".ToSlug());
        }

        [TestMethod]
        public void ShortGuid()
        {
            Assert.AreEqual("45b1_sF00kiU0QIRQ2Jo9Q", new Guid("FEF596E3-74C1-48D2-94D1-0211436268F5").ToShortGuid());
            Assert.AreEqual(new Guid("FEF596E3-74C1-48D2-94D1-0211436268F5"), "45b1_sF00kiU0QIRQ2Jo9Q".FromShortGuid());
            Assert.AreEqual("XRayF23_0USI0Nv5mYUwYQ", new Guid("17B2165D-FF6D-44D1-88D0-DBF999853061").ToShortGuid());
            Assert.AreEqual(new Guid("17B2165D-FF6D-44D1-88D0-DBF999853061"), "XRayF23_0USI0Nv5mYUwYQ".FromShortGuid());
            Assert.IsNull("XRayF23_0USI0Nv5m!!".FromShortGuid());
            Assert.IsNull("xx".FromShortGuid());

            for (var i = 0; i < 10000; i++)
                Assert.AreEqual(22, new Guid().ToShortGuid().Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null string was inappropriately allowed.")]
        public void ShortGuidNullArgument()
        {
            string test = null;
            test.FromShortGuid();
        }

        [TestMethod]
        public void FileSize()
        {
            var total = 1024 * 1024 * 1024 * 1 + 1024 * 1024 * 780 + 1024 * 2 + 5;
            var fs = total.ToFileSize();
            Assert.AreEqual(total, fs.Size);

            Assert.AreEqual(0, fs.Exabytes);
            Assert.AreEqual(0, fs.Petabytes);
            Assert.AreEqual(0, fs.Terabytes);
            Assert.AreEqual(1, fs.Gigabytes);
            Assert.AreEqual(780, fs.Megabytes);
            Assert.AreEqual(2, fs.Kilobytes);
            Assert.AreEqual(5, fs.Bytes);

            Assert.AreEqual(0, fs.TotalExabytes);
            Assert.AreEqual(0, fs.TotalPetabytes);
            Assert.AreEqual(0, fs.TotalTerabytes);
            Assert.AreEqual(1.762, Math.Round(fs.TotalGigabytes, 3));
            Assert.AreEqual(0, fs.TotalMegabytes);
            Assert.AreEqual(0, fs.TotalKilobytes);
            Assert.AreEqual(0, fs.TotalBytes);

            Assert.AreEqual("1.762 GB", fs.ToString(Sushi2.FileSize.Format.Brief, Sushi2.Cultures.English));
            Assert.AreEqual("1 GB 780 MB 2 KB 5 B", fs.ToString(Sushi2.FileSize.Format.Detail, Sushi2.Cultures.English));
        }

        [TestMethod]
        public void ZeroFileSize()
        {
            var total = 0;
            var fs = total.ToFileSize();
            Assert.AreEqual(total, fs.Size);

            Assert.AreEqual(0, fs.Exabytes);
            Assert.AreEqual(0, fs.Petabytes);
            Assert.AreEqual(0, fs.Terabytes);
            Assert.AreEqual(0, fs.Gigabytes);
            Assert.AreEqual(0, fs.Megabytes);
            Assert.AreEqual(0, fs.Kilobytes);
            Assert.AreEqual(0, fs.Bytes);

            Assert.AreEqual(0, fs.TotalExabytes);
            Assert.AreEqual(0, fs.TotalPetabytes);
            Assert.AreEqual(0, fs.TotalTerabytes);
            Assert.AreEqual(0, 0);
            Assert.AreEqual(0, fs.TotalMegabytes);
            Assert.AreEqual(0, fs.TotalKilobytes);
            Assert.AreEqual(0, fs.TotalBytes);

            Assert.AreEqual("0 B", fs.ToString(Sushi2.FileSize.Format.Brief, Sushi2.Cultures.English));
            Assert.AreEqual("0 B", fs.ToString(Sushi2.FileSize.Format.Detail, Sushi2.Cultures.English));
        }

        [TestMethod]
        public void NegativeFileSize()
        {
            var total = 1024 * 1024 * 1024 * 1 + 1024 * 1024 * 780 + 1024 * 2 + 0;
            total *= -1;

            var fs = total.ToFileSize();
            Assert.AreEqual(total, fs.Size);

            Assert.AreEqual(0, fs.Exabytes);
            Assert.AreEqual(0, fs.Petabytes);
            Assert.AreEqual(0, fs.Terabytes);
            Assert.AreEqual(1, fs.Gigabytes);
            Assert.AreEqual(780, fs.Megabytes);
            Assert.AreEqual(2, fs.Kilobytes);
            Assert.AreEqual(0, fs.Bytes);

            Assert.AreEqual(0, fs.TotalExabytes);
            Assert.AreEqual(0, fs.TotalPetabytes);
            Assert.AreEqual(0, fs.TotalTerabytes);
            Assert.AreEqual(1.762, Math.Round(fs.TotalGigabytes, 3));
            Assert.AreEqual(0, fs.TotalMegabytes);
            Assert.AreEqual(0, fs.TotalKilobytes);
            Assert.AreEqual(0, fs.TotalBytes);

            Assert.AreEqual("-1.762 GB", fs.ToString(Sushi2.FileSize.Format.Brief, Sushi2.Cultures.English));
            Assert.AreEqual("-1 GB -780 MB -2 KB 0 B", fs.ToString(Sushi2.FileSize.Format.Detail, Sushi2.Cultures.English));
        }

        [TestMethod]
        public void ToShortenedString()
        {
            string x = null;

            Assert.ThrowsException<ArgumentNullException>(() => x.ToShortenedString(10));
            Assert.ThrowsException<ArgumentException>(() => "bar".ToShortenedString(-1));

            Assert.AreEqual("foo", "foo".ToShortenedString(3));
            Assert.AreEqual("foo", "foo".ToShortenedString(30));
            Assert.AreEqual("f...", "foo".ToShortenedString(1));
            Assert.AreEqual("...", "foo".ToShortenedString(0));
            Assert.AreEqual("fo...", "foo".ToShortenedString(2));
            Assert.AreEqual("f?", "foo".ToShortenedString(1, true, "?"));
            Assert.AreEqual("?", "foo".ToShortenedString(0, true, "?"));
            Assert.AreEqual("aloha...", "aloha everyone".ToShortenedString(6));
            Assert.AreEqual("aloha...", "aloha everyone".ToShortenedString(7));
            Assert.AreEqual("aloha...", "aloha everyone".ToShortenedString(8));
            Assert.AreEqual("aloha...", "aloha everyone".ToShortenedString(9));
            Assert.AreEqual("aloha...", " aloha everyone".ToShortenedString(10));
            Assert.AreEqual("aloha ever...", "aloha everyone".ToShortenedString(10, false));
            Assert.AreEqual("aloha everyone", "aloha everyone".ToShortenedString(15));
            Assert.AreEqual("aloha everyone", "aloha everyone".ToShortenedString(15, false));
        }

        [TestMethod]
        public void ToCzechNonBreakingSpacesString()
        {
            Assert.AreEqual("Ferda a&nbsp;mravenec", "Ferda a mravenec".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("Ferda a_mravenec", "Ferda a mravenec".ToCzechNonBreakingSpacesString("_"));
            Assert.AreEqual("Dr.&nbsp;Ferda Mravenec", "Dr. Ferda Mravenec".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("F.&nbsp;Mravenec", "F. Mravenec".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("p.&nbsp;Mravenec", "p. Mravenec".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("30&nbsp;kg", "30 kg".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("15.&nbsp;km", "15. km".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("tab.&nbsp;7, a.&nbsp;s.", "tab. 7, a. s.".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("Ano. Ferda.", "Ano. Ferda.".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("dojel do Prahy. Novák byl", "dojel do Prahy. Novák byl".ToCzechNonBreakingSpacesString());
            Assert.AreEqual("15~%", "15 %".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("5.~díl", "5. díl".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("Bylo jich 5. Další věta.", "Bylo jich 5. Další věta.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("tab.~7", "tab. 7".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("6.~1.", "6. 1.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("30.~12.~2015", "30. 12. 2015".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("foo k~s~v~z~a~i~o~u~bar", "foo k s v z a i o u bar".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("foo K~S~V~Z~A~I~O~U~bar", "foo K S V Z A I O U bar".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("foo~- bar", "foo - bar".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("7~-~3", "7 - 3".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("7~–~3", "7 – 3".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("foo~– bar", "foo – bar".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("602~123~345", "602 123 345".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("Dr.~Ferda mravenec má telefonní číslo 600~111~333.", "Dr. Ferda mravenec má telefonní číslo 600 111 333.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("v~Plzni", "v Plzni".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("1~000~000", "1 000 000".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("25,325~23", "25,325 23".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("§~23", "§ 23".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("#~26", "# 26".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("*~1921", "* 1921".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("†~2000", "† 2000".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("5~str.", "5 str.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("8~hod.", "8 hod.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("s.~53", "s. 53".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("č.~9", "č. 9".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("100~m²", "100 m²".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("19~°C", "19 °C".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("1~000~000~Kč", "1 000 000 Kč".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("250~€", "250~€".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("500~lidí", "500 lidí".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("5.~pluk", "5. pluk".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("8.~kapitola", "8. kapitola".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("II.~patro", "II. patro".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("Karel~IV.", "Karel IV.".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("16.~ledna 1972", "16. ledna 1972".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("1~:~50~000", "1 : 50 000".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("10~:~2~=~5", "10 : 2 = 5".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("+420~800~123~987", "+420 800 123 987".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("T.~G.~M.", "T. G. M.".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("FF~UK", "FF UK".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("ČSN~01~6910", "ČSN 01 6910".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("ISO~9001", "ISO 9001".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("tzv.~klikání", "tzv. klikání".ToCzechNonBreakingSpacesString("~"));
            //Assert.AreEqual("Fr.~Daneš", "Fr. Daneš".ToCzechNonBreakingSpacesString("~"));
            Assert.AreEqual("František Daneš", "František Daneš".ToCzechNonBreakingSpacesString("~"));


        }
    }
}

