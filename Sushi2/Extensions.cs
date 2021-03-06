﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Globalization;

namespace Sushi2
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Convert string to db string. Never null and trimmed.
        /// </summary>
        public static string ToDbString(this string text)
        {
            text = text ?? "";

            return text.Trim();
        }

        /// <summary>
        /// Convert string to db string. Never null and trimmed, diacritics removed and lowered.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>Normalized text.</returns>
        public static string ToNormalizedString(this string text)
        {
            return text.ToDbString().ToStringWithoutDiacritics().ToLower(Cultures.Invariant);
        }

        /// <summary>
        /// Convert string to sort string. Can be sorted by classic binary sorting.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>Sort text.</returns>
        public static string ToCzechSortedString(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            text = text.ToLower(Cultures.Czech);

            // ch is special case
            text = text.Replace("ch", "i*");

            var result = string.Empty;

            foreach (var c in text)
            {
                var s = c.ToString(Cultures.Current);
                var norm = s.ToStringWithoutDiacritics();

                if (s.Equals(norm, StringComparison.Ordinal))
                {
                    if (s.Equals("e", StringComparison.Ordinal))
                        result += "ea";
                    if (s.Equals("z", StringComparison.Ordinal))
                        result += "za";
                    else if (s.Equals("u", StringComparison.Ordinal))
                        result += "ua";
                    else
                        result += s;
                }
                else
                {
                    switch (s)
                    {
                        // https://cs.wikipedia.org/wiki/Abecedn%C3%AD_%C5%99azen%C3%AD
                        // basically it's almost impossible to make make it 100% without comparing existing strings :(
                        case "ž":
                            result += "zb";
                            break;

                        case "é":
                            result += "eb";
                            break;

                        case "ě":
                            result += "ec";
                            break;

                        case "ú":
                            result += "ub";
                            break;

                        case "ů":
                            result += "uc";
                            break;

                        default:
                            if (string.IsNullOrEmpty(norm))
                                break;

                            if (norm.Length != 1)
                            {
                                result += norm;
                            }
                            else
                            {
                                result += (char)(Convert.ToChar(norm, Cultures.Czech) + 1);
                                result += "*";
                            }
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to convert a given object to nullable DateTime.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>DateTime? value or null.</returns>
        public static DateTime? ToDateTime(this object o)
        {
            if (o == null)
                return null;

            var time = o as DateTime?;

            if (time != null)
                return time;

            DateTime result;
            return (DateTime.TryParse(o.ToString(), out result) ? (DateTime?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to nullable usigned byte.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>byte? or null.</returns>
        public static byte? ToByte(this object o)
        {
            if (o == null)
                return null;

            var b = o as byte?;

            if (b != null)
                return b;

            byte result;
            return (byte.TryParse(o.ToString(), out result) ? (byte?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to nullable int 16.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>Int16? or null.</returns>
        public static short? ToInt16(this object o)
        {
            if (o == null)
                return null;

            var s = o as short?;
            if (s != null)
                return s;

            short result;
            return (short.TryParse(o.ToString(), out result) ? (short?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to nullable int 32.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>Int32? or null.</returns>
        public static int? ToInt32(this object o)
        {
            if (o == null)
                return null;

            var i = o as int?;
            if (i != null)
                return i;

            int result;
            return (int.TryParse(o.ToString(), out result) ? (int?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to nullable double.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>Double? or null.</returns>
        public static double? ToDouble(this object o)
        {
            if (o == null)
                return null;

            var d = o as double?;
            if (d != null)
                return d;

            double result;
            return (double.TryParse(o.ToString(), out result) ? (double?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to nullable int 64.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>Int64? or null.</returns>
        public static long? ToInt64(this object o)
        {
            if (o == null)
                return null;

            var l = o as long?;
            if (l != null)
                return l;

            long result;
            return (long.TryParse(o.ToString(), out result) ? (long?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to float.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>float? or null.</returns>
        public static float? ToFloat(this object o)
        {
            if (o == null)
                return null;

            var f = o as float?;
            if (f != null)
                return f;

            var i = o as int?;
            if (i != null)
                return i;

            var l = o as long?;
            if (l != null)
                return l;

            var @decimal = o as decimal?;
            if (@decimal != null)
                return (float?)@decimal;

            var f1 = o as ulong?;
            if (f1 != null)
                return f1;

            var u = o as uint?;
            if (u != null)
                return u;

            var d = o as double?;
            if (d != null)
                return (float?)d;

            float result;
            return (float.TryParse(o.ToString(), out result) ? (float?)result : null);
        }

        /// <summary>
        /// Tries to convert a given object to decimal.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>decimal? or null.</returns>
        public static decimal? ToDecimal(this object o)
        {
            if (o == null)
                return null;

            var @decimal = o as decimal?;
            if (@decimal != null)
                return @decimal;

            var i = o as int?;
            if (i != null)
                return i;

            var l = o as long?;
            if (l != null)
                return l;

            var decimal1 = o as ulong?;
            if (decimal1 != null)
                return decimal1;

            var u = o as uint?;
            if (u != null)
                return u;

            var f = o as float?;
            if (f != null)
                return (decimal?)f;

            var d = o as double?;
            if (d != null)
                return (decimal?)d;

            decimal result;
            return (decimal.TryParse(o.ToString(), out result) ? (decimal?)result : null);
        }

        /// <summary>
        /// Detects whether the given object is (or can be converted to) a Guid.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>True if the Object is (or can be converted to) a Guid. False otherwise.</returns>
        public static bool IsGuid(this object o)
        {
            if (o == null)
                return false;

            if (o is Guid)
                return true;

            var rgxGuid = new Regex(RegexPatterns.Guid, RegexOptions.Compiled);
            return rgxGuid.IsMatch(o.ToString());
        }

        /// <summary>
        /// Tries to convert a given object to Guid.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>Guid? or null.</returns>
        public static Guid? ToGuid(this object o)
        {
            if (o == null)
                return null;

            var guid = o as Guid?;
            if (guid != null)
                return guid;

            return (IsGuid(o) ? ((Guid?)new Guid(o.ToString())) : null);
        }

        /// <summary>
        /// Tries to convert a given value to bool?.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>bool? or null.</returns>
        public static bool? ToBoolean(this object o)
        {
            if (o == null)
                return null;

            var b = o as bool?;
            if (b != null)
                return b;

            var s = o.ToString();

            s = s.Trim();

            if (s.Length == 0)
                return null;

            if (string.Compare(s, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            if (string.Compare(s, bool.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
                return false;

            int i;
            return (int.TryParse(s, out i) ? (bool?)(i != 0) : null);
        }

        /// <summary>
        /// Remove all the diacritivs signs with simplified ones.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Text without any diacritics.</returns>
        public static string ToStringWithoutDiacritics(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrEmpty(input))
                return "";

            if (input.All(x => x < 0x80))
                return input;

            // result often can be at least two times longer than input string.
            var sb = new StringBuilder(input.Length * 2);

            foreach (char c in input)
            {
                if (c < 0x80)
                {
                    sb.Append(c);
                }
                else
                {
                    int high = c >> 8;
                    int low = c & 0xff;

                    if (Characters.AllCharacters.TryGetValue(high, out string[] transliterations))
                    {
                        sb.Append(transliterations[low]);
                    }
                }
            }

            // special case for russian ь
            return sb.ToString().Replace("'", "");
        }

        /// <summary>
        /// Create accent insensitive string with a special care for 'CH' sign.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>String suitable for SQL.</returns>
        public static string ToAccentInsensitiveSqlString(this string s)
        {
            if (s == null)
                return null;

            var defs = new List<string>
            {
                "ÁAaá",
                "ČčCc",
                "ĎDďd",
                "ĚÉËěéëEe",
                "ÍIíi",
                "ŇňNn",
                "ÓÖOoóö",
                "ŘřRr",
                "ŠSšs",
                "ŤťTt",
                "ŮÚÜůúüUu",
                "YÝýy",
                "ŽžZz",
            };

            var result = "";

            for (var i = 0; i < s.Length; i++)
            {
                // special situation solved for 'ch'
                if (s[i] == 'c' || s[i] == 'C')
                {
                    if (i + 1 < s.Length &&
                        (s[i + 1] == 'h' || s[i + 1] == 'H'))
                    {
                        result += s[i];
                        continue;
                    }
                }

                var replaced = false;

                foreach (var d in defs)
                {
                    if (d.Contains(s[i].ToString()))
                    {
                        result += "[" + d + "]";
                        replaced = true;
                        break;
                    }
                }

                if (!replaced)
                    result += s[i];
            }

            return result;
        }

        /// <summary>
        /// Get mime type according to filename.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>Mime type.</returns>
        public static string ToMimeType(this string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            var ext = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(ext))
                return _defaultMimeType;

            if (ext.StartsWith(".", StringComparison.Ordinal))
                ext = ext.Substring(1);

            return !_mimeTypes.ContainsKey(ext) ? _defaultMimeType : _mimeTypes[ext];
        }

        /// <summary>
        /// Get attribute of enum.
        /// </summary>
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var memberInfo = enumValue.GetType().GetTypeInfo().GetMember(enumValue.ToString()).FirstOrDefault();

            var attribute = (T)memberInfo?.GetCustomAttributes(typeof(T), true).FirstOrDefault();

            return attribute;
        }

        /// <summary>
        /// Get attribute of enum.
        /// </summary>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            if (type.GetCustomAttributes(typeof(T), true).FirstOrDefault() is T att)
                return att;

            return default(T);
        }

        /// <summary>
        /// Get attribute of any object.
        /// </summary>
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
                return valueSelector(att);

            return default(TValue);
        }

        /// <summary>
        /// Convert date time to epoch.
        /// </summary>
        public static int ToEpoch(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1);
            var epochTimeSpan = date - epoch;

            return (int)epochTimeSpan.TotalSeconds;
        }

        /// <summary>
        /// Get property value using reflection.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="name">Name of property.</param>
        /// <returns>Value.</returns>
        public static object GetPropertyValue(this object obj, string name)
        {
            if (obj == null)
                return null;

            foreach (var part in name.Split('.'))
            {
                var type = obj.GetType();

                var info = type.GetTypeInfo().GetProperty(part);

                if (info == null)
                    return null;

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        /// <summary>
        /// Get property value using reflection.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="name">Name of property.</param>
        /// <returns>Value.</returns>
        public static T GetPropertyValue<T>(this object obj, string name)
        {
            var retval = GetPropertyValue(obj, name);

            if (retval == null)
                return default(T);

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        /// <summary>
        /// Get week of year based on ISO 8601.
        /// </summary>
        /// <param name="dateTime">Datetime.</param>
        /// <returns>The week number.</returns>
        public static int ToIso8601WeekOfYear(this DateTime dateTime)
        {
            while (true)
            {
                var startOfYear = dateTime.AddDays(-dateTime.Day + 1).AddMonths(-dateTime.Month + 1);

                // get dec 31st of the year
                var endOfYear = startOfYear.AddYears(1).AddDays(-1);

                // ISO 8601 weeks start with Monday
                // The first week of a year includes the first Thursday
                // returns 0 for sunday up to 6 for saturday
                int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
                var nds = dateTime.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
                var wk = nds / 7;

                switch (wk)
                {
                    case 0:
                        // return weeknumber of dec 31st of the previous year
                        dateTime = startOfYear.AddDays(-1);
                        continue;
                    case 53:
                        // if dec 31st falls before thursday it is week 01 of next year
                        return endOfYear.DayOfWeek < DayOfWeek.Thursday ? 1 : wk;

                    default:
                        return wk;
                }
            }
        }

        /// <summary>
        /// Get shuffled collection.
        /// </summary>
        /// <param name="collection">Original collection.</param>
        /// <returns>Shuffled collection.</returns>
        public static IEnumerable<T> ToShuffledCollection<T>(this IEnumerable<T> collection)
        {
            var provider = RandomNumberGenerator.Create();
            var result = collection.ToList();
            var n = result.Count;

            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = result[k];
                result[k] = result[n];
                result[n] = value;
            }

            return result;
        }

        // public static IEnumerable<string> ToCommands(this string input)
        // {
        //     var result = input.Split('"')
        //         .Select((element, index) => index % 2 == 0  // If even index
        //                     ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
        //                     : new[] { element })  // Keep the entire item
        //         .SelectMany(element => element).ToList();

        //     return result;
        // }

        /// <summary>
        /// Gets string with replaced values.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="original">Original.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="comparison">Comparison.</param>
        public static string ToReplacedString(this string original, string oldValue, string newValue, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            if (original == null)
                throw new ArgumentNullException(nameof(original));

            var sb = new StringBuilder();

            var previousIndex = 0;
            var index = original.IndexOf(oldValue, comparison);

            while (index != -1)
            {
                sb.Append(original.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);

                index += oldValue.Length;
                previousIndex = index;
                index = original.IndexOf(oldValue, index, comparison);
            }

            sb.Append(original.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
	    /// Implode a collection of object to string.
	    /// </summary>
	    /// <param name="list">A list of objects.</param>
	    /// <param name="split">Split string.</param>
	    /// <param name="lastSplit">Last split string.</param>
	    /// <returns>One big string of all the objects.</returns>
	    public static string ToImplodedString<T>(this IEnumerable<T> list, string split = ", ", string lastSplit = null)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (split == null)
                throw new ArgumentNullException(nameof(split));

            int count = list.Count();

            var vals = new List<string>(count);

            foreach (T o in list)
                vals.Add(o.ToString());

            if (count == 1)
                return vals[0];

            if (count == 2)
            {
                if (lastSplit != null)
                    return vals[0] + lastSplit + vals[1];

                return vals[0] + split + vals[1];
            }

            var res = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                var s = split;

                if (i == count - 1 && lastSplit != null)
                    s = lastSplit;

                if (res.Length != 0)
                    res.Append(s);

                res.Append(vals[i]);
            }

            return res.ToString();
        }

        /// <summary>
        /// Convert string to safe filename.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <param name="safePart">Safe part with which to replace unsafe chars.</param>
        /// <param name="removeAdditionalDots">Remove additional dots.</param>
        /// <param name="removeDoubledSafeParts">Remove doubled safe parts.</param>
        /// <returns>Safe filename.</returns>
        public static string ToFilename(this string fileName, string safePart = "_", bool removeAdditionalDots = true, bool removeDoubledSafeParts = true)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (safePart == null)
                throw new ArgumentNullException(nameof(safePart));

            if (safePart != null &&
                removeAdditionalDots &&
                safePart.IndexOf(".", StringComparison.Ordinal) >= 0)
            {
                throw new ArgumentException("Safe part cannot contains '.' (dot) if removing additional dots mode is on.");
            }

            var sb = new StringBuilder();
            var newFilename = fileName.ToNormalizedString();

            foreach (var t in newFilename)
            {
                var r = new Regex("[qwertyuiopasdfghjklzxcvbnm0123456789_\\-\\.]", RegexOptions.IgnoreCase);

                if (!r.IsMatch(t.ToString(Cultures.Invariant)))
                    sb.Append(safePart);
                else
                    sb.Append(t);
            }

            newFilename = sb.ToString();

            if (removeAdditionalDots)
            {
                while (newFilename.ToCharArray().Count(c => c == '.') > 1)
                {
                    int idx = newFilename.IndexOf('.');
                    newFilename = newFilename.Remove(idx, 1);
                    newFilename = newFilename.Insert(idx, safePart);
                }
            }

            if (removeDoubledSafeParts)
            {
                while (newFilename.IndexOf(safePart + safePart, StringComparison.CurrentCulture) >= 0)
                {
                    newFilename = newFilename.ToReplacedString(safePart + safePart, safePart, StringComparison.CurrentCulture);
                }
            }

            return newFilename;
        }

        /// <summary>
		/// Convert string to slug.
		/// </summary>
		/// <param name="url">Any string.</param>
		/// <param name="safePart">Replace unsafe char with this string.</param>
		/// <param name="maxLength">Max url length.</param>
        /// <param name="removeDoubledSafeParts">Remove doubled safe parts.</param>
        /// <param name="smartCutting">Smart cutting.</param>
		/// <returns>Url friendly string.</returns>
		public static string ToSlug(this string url, string safePart = "-", int maxLength = 80, bool removeDoubledSafeParts = true, bool smartCutting = true)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (maxLength < 1)
                throw new ArgumentException("Max length must be greater than 0.", nameof(maxLength));

            var def = "[a-z0-9\\-]";

            url = url.ToNormalizedString();

            var result = new StringBuilder();
            var r = new Regex(def);
            var cutted = false;
            var lastSafeIndex = -1;

            for (int i = 0; i < url.Length; i++)
            {
                if (i >= maxLength)
                {
                    cutted = true;
                    break;
                }

                var now = url[i].ToString(CultureInfo.InvariantCulture);

                if (!r.IsMatch(now))
                {
                    if (result.Length == 0)
                        continue;

                    lastSafeIndex = i;
                    result.Append(safePart);
                }
                else
                {
                    result.Append(now);
                }
            }

            var rx = result.ToString();

            // remove end chars
            while (rx.Length > 0 &&
                rx.EndsWith(safePart, StringComparison.OrdinalIgnoreCase))
            {
                if (rx.Length - safePart.Length <= 0)
                    break;

                rx = rx.Substring(0, rx.Length - safePart.Length);
            }

            // smart cutting
            if (smartCutting &&
                cutted &&
                rx.Length >= maxLength &&
                lastSafeIndex != -1)
            {
                rx = rx.Substring(0, lastSafeIndex);
            }

            if (removeDoubledSafeParts)
            {
                while (rx.IndexOf(safePart + safePart, StringComparison.CurrentCulture) >= 0)
                {
                    rx = rx.ToReplacedString(safePart + safePart, safePart, StringComparison.CurrentCulture);
                }
            }

            if (string.IsNullOrEmpty(rx))
                return safePart;

            return rx;
        }

        /// <summary>
        /// Converts guid to short guid string.
        /// </summary>
        /// <param name="guid">Guid.</param>
        /// <returns>Short guid.</returns>
        public static string ToShortGuid(this Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-").Replace("=", "");
            return encoded;
        }

        /// <summary>
        /// Converts base64 string to guid.
        /// </summary>
        /// <param name="value">The base64 encoded string of a guid.</param>
        /// <returns>A guid or null.</returns>
        public static Guid? FromShortGuid(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            value = value.Replace("_", "/").Replace("-", "+");

            try
            {
                byte[] buffer = Convert.FromBase64String(value + "==");
                return new Guid(buffer);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
		/// Convert to human readable file size.
		/// </summary>
		/// <param name="size">Size in bytes.</param>
		/// <returns>Human readable string.</returns>
		public static FileSize ToFileSize(this int size) => ((long)size).ToFileSize();

        /// <summary>
        /// Convert to human readable file size.
        /// </summary>
        /// <param name="size">Size in bytes.</param>
        /// <returns>FileSize object.</returns>
        public static FileSize ToFileSize(this long size) => new FileSize(size);

        /// <summary>
        /// Shorten string and add ... if needed.
        /// </summary>
        /// <param name="text">Original text.</param>
        /// <param name="maxSize">Max size of the text.</param>
        /// <param name="wholeWordsOnly">Whole words are returned only.</param>
        /// <param name="postfix">Postfix.</param>
        /// <returns>Shortened string.</returns>
        public static string ToShortenedString(this string text, int maxSize, bool wholeWordsOnly = true, string postfix = "...")
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (maxSize < 0)
                throw new ArgumentException("Max size must be 0 or greater.", nameof(maxSize));

            if (text?.Length == 0 ||
                maxSize == 0)
            {
                return postfix;
            }

            if (text.Length > maxSize)
            {
                if (wholeWordsOnly)
                {
                    int idx = text.Substring(0, maxSize - 1).LastIndexOf(" ", StringComparison.Ordinal);

                    if (idx == -1)
                        idx = maxSize;

                    text = text.Substring(0, idx).Trim() + postfix;
                }
                else
                {
                    text = text.Substring(0, maxSize).Trim() + postfix;
                }
            }

            return text;
        }

        /// <summary>
        /// Get text with non-breaking spaces where needed.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="nbsp">Non-breaking space character.</param>
        /// <returns>Text with non-breaking spaces where needed.</returns>
        /// <remarks>Based on https://github.com/Mikulas/vlna</remarks>
        public static string ToCzechNonBreakingSpacesString(this string text, string nbsp = "&nbsp;")
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var prepositions = new Regex(@"\b(k|s|v|z|a|i|o|u|K|S|V|Z|A|I|O|U)(\s)");
            text = prepositions.Replace(text, "$1" + nbsp);

            var degrees = new Regex(@"(akad|Bc|BcA|CSc|doc|Dr|DrSc|DSc|ICDr|Ing|JUDr|MDDr|MgA|Mgr|MSDr|MUDr|MVDr|PaedDr|Ph\.D|PharmDr|PhDr|PhMr|prof|RCDr|RNDr|RSDr|RTDr|Th\.D|ThDr|ThLic|ThMgr|DiS)\.(\s)");
            text = degrees.Replace(text, "$1." + nbsp);

            var names = new Regex(@"(\p{Lu}\.)\s(\p{Lu})");
            text = names.Replace(text, "$1" + nbsp + "$2");

            var dears = new Regex(@"\b(\p{Ll}\.)\s(\p{Lu})");
            text = dears.Replace(text, "$1" + nbsp + "$2");

            var numbers = new Regex(@"(\d)\s");
            text = numbers.Replace(text, "$1" + nbsp);

            var numbersPlus = new Regex(@"(\w\.)\s(\d|\p{Ll})");
            text = numbersPlus.Replace(text, "$1" + nbsp + "$2");

            var hyphensAndDashes = new Regex(@"(\s)(-|–)");
            text = hyphensAndDashes.Replace(text, nbsp + "$2");

            var hyphensAndDashesPlus = new Regex(@"(-|–)\s(\d)");
            text = hyphensAndDashesPlus.Replace(text, "$1" + nbsp + "$2");            

            return text;
        }
    }
}