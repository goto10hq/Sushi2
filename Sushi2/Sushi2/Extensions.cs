using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

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

            if (ext.StartsWith("."))
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
    }
}
