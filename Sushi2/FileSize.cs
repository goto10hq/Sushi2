using System;
using System.Globalization;
using System.Text;

namespace Sushi2
{
    public sealed class FileSize
    {
        public enum Format
        {
            Brief,
            Detail
        }

        const int OneKilo = 1024;

        int _numberOfUnits => _units.Length;
        readonly string[] _units = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        public long Bytes { get; private set; }
        public long Kilobytes { get; private set; }
        public long Megabytes { get; private set; }
        public long Gigabytes { get; private set; }
        public long Terabytes { get; private set; }
        public long Petabytes { get; private set; }
        public long Exabytes { get; private set; }

        public double TotalBytes { get; private set; }
        public double TotalKilobytes { get; private set; }
        public double TotalMegabytes { get; private set; }
        public double TotalGigabytes { get; private set; }
        public double TotalTerabytes { get; private set; }
        public double TotalPetabytes { get; private set; }
        public double TotalExabytes { get; private set; }

        public readonly long Size;

        public FileSize(long totalBytes)
        {
            Size = totalBytes;

            long start = Math.Abs(Size);
            int i = _numberOfUnits - 1;
            var totalSet = false;

            while (i >= 0)
            {
                var x = start;
                long min = 1;

                for (var n = 0; n < i; n++)
                {
                    min *= 1024;
                }

                if (x > min)
                {
                    if (!totalSet)
                    {
                        SetTotal(i, (double)start / min);
                        totalSet = true;
                    }

                    var d = start / min;
                    this[i] = d;

                    var minus = d;

                    for (var n = 0; n < i; n++)
                    {
                        minus *= 1024;
                    }

                    start -= minus;
                }

                i--;
            }
        }

        void SetTotal(int index, double value)
        {
            switch (index)
            {
                case 0: TotalBytes = value; break;
                case 1: TotalKilobytes = value; break;
                case 2: TotalMegabytes = value; break;
                case 3: TotalGigabytes = value; break;
                case 4: TotalTerabytes = value; break;
                case 5: TotalPetabytes = value; break;
                case 6: TotalExabytes = value; break;
                default:
                    throw new IndexOutOfRangeException("Index of file size part is out of range.");
            }
        }

        double GetTotal(int index)
        {
            switch (index)
            {
                case 0: return TotalBytes;
                case 1: return TotalKilobytes;
                case 2: return TotalMegabytes;
                case 3: return TotalGigabytes;
                case 4: return TotalTerabytes;
                case 5: return TotalPetabytes;
                case 6: return TotalExabytes;
                default:
                    throw new IndexOutOfRangeException("Index of file size part is out of range.");
            }
        }

        long this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Bytes;
                    case 1: return Kilobytes;
                    case 2: return Megabytes;
                    case 3: return Gigabytes;
                    case 4: return Terabytes;
                    case 5: return Petabytes;
                    case 6: return Exabytes;
                    default:
                        throw new IndexOutOfRangeException("Index of file size part is out of range.");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Bytes = value; break;
                    case 1: Kilobytes = value; break;
                    case 2: Megabytes = value; break;
                    case 3: Gigabytes = value; break;
                    case 4: Terabytes = value; break;
                    case 5: Petabytes = value; break;
                    case 6: Exabytes = value; break;
                    default:
                        throw new IndexOutOfRangeException("Index of file size part is out of range.");
                }
            }
        }

        public override string ToString()
        {
            return ToString(Format.Brief, Cultures.Current);
        }

        public string ToString(Format format, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
                cultureInfo = Cultures.Current;

            if (format == Format.Brief)
            {
                for (var n = _numberOfUnits - 1; n >= 0; n--)
                {
                    var l = GetTotal(n);

                    if (l > 0)
                        return l.ToString((Size < 0 ? "-" : "") + "0.### ", cultureInfo) + _units[n];
                }

                return 0.ToString("0.### ", cultureInfo) + _units[0];
            }

            if (format == Format.Detail)
            {
                var sb = new StringBuilder();
                var started = false;

                for (var n = _numberOfUnits - 1; n >= 0; n--)
                {
                    var l = this[n];

                    if (l > 0 ||
                        started)
                    {
                        started = true;

                        if (l != 0 &&
                            Size < 0)
                            sb.Append("-");

                        sb.Append(l.ToString(cultureInfo));
                        sb.Append(" ");
                        sb.Append(_units[n]);

                        if (n > 0)
                            sb.Append(" ");
                    }
                }

                if (sb.Length == 0)
                    return 0.ToString(cultureInfo) + " " + _units[0];

                return sb.ToString();
            }

            throw new ArgumentException("Formatting is not supported.", nameof(format));
        }
    }
}