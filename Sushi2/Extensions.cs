using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;

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
            return text.ToDbString().ToStringWithoutDiacritics().ToLower();
        }

        /// <summary>
        /// Convert string to sort string. Can be sorted by classic binary sorting.
        /// </summary>	
        /// <param name="text">Text.</param>	
        /// <returns>Sort text.</returns>
        public static string ToSortedString(this string text)
        {
            var t = text.ToDbString().ToLower();

            // ch is special case
            t = t.Replace("ch", "i*");

            var result = string.Empty;

            foreach (var c in t)
            {
                var s = c.ToString();
                var norm = s.ToStringWithoutDiacritics();

                if (s.Equals(norm))
                {
                    result += s;
                }
                else
                {
                    // special case for ž and ě and ů
                    // not 100% correct for ž but unable to find char to be order "after z"
                    // on the other hand not a problem from any existing czech word
                    switch (s)
                    {
                        case "ž":
                            result += "zz";
                            break;
                        case "e":
                            result += "f0";
                            break;
                        case "ě":
                            result += "f1";
                            break;
                        case "ú":
                            result += "v0";
                            break;
                        case "ů":
                            result += "v1";
                            break;
                        default:
                            result += (char)(Convert.ToChar(norm) + 1);
                            result += "*";
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
        /// <param name="s">String.</param>
        /// <returns>Text without any diacritics.</returns>
        public static string ToStringWithoutDiacritics(this string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            const string textWithDiacritic = "ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïñòóôõöøùúûüýÿĀāĂăĄąĆćČčĎďĐđĒēĖėĘęĚěĞğĢģĪīĮįİıĶķĹĺĻļĽľŁłŃńŅņŇňŌōŐőŔŕŖŗŘřŚśŞşŠšŢţŤťŪūŮůŰűŲųŸŹźŻżŽžƠơƯư";
            const string simplifiedText = "AAAAAACEEEEIIIIDNOOOOOOUUUUYaaaaaaceeeeiiiinoooooouuuuyyAaAaAaCcCcDdDdEeEeEeEeGgGgIiIiIiKkLlLlLlLlNnNnNnOoOoRrRrRrSsSsSsTtTtUuUuUuUuYZzZzZzOoUu";

            var result = new StringBuilder(s.Length);

            for (var i = 0; i < s.Length; i++)
            {
                var pos = textWithDiacritic.IndexOf(s[i]);

                result.Append(pos >= 0 ? simplifiedText[pos] : s[i]);
            }

            s =  result.ToString();

            var newStringBuilder = new StringBuilder();
                newStringBuilder.Append(s.Normalize(NormalizationForm.FormKD)
                                                .Where(x => x < 128)
                                                .ToArray());

           return newStringBuilder.ToString();                        
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

            var attribute = (T)memberInfo?.GetCustomAttributes(typeof(T), false).FirstOrDefault();

            return attribute;
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
    }
}
