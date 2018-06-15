using System;
using System.Globalization;

namespace Sushi2
{
    public static class Cultures
    {
        static readonly Lazy<CultureInfo> _en = new Lazy<CultureInfo>(() => new CultureInfo("en-US"));
        static readonly Lazy<CultureInfo> _cz = new Lazy<CultureInfo>(() => new CultureInfo("cs-CZ"));

        /// <summary>
        /// Get culture info for EN.
        /// </summary>
        public static CultureInfo English => _en.Value;

        /// <summary>
        /// Get culture info for CZ.
        /// </summary>
        public static CultureInfo Czech => _cz.Value;

        /// <summary>
        /// Get invariant culture.
        /// </summary>
        public static CultureInfo Invariant => CultureInfo.InvariantCulture;
    }
}