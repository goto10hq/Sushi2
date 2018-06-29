using System;
using System.Linq;

namespace Sushi2
{
    public static class RandomWordGenerator
    {
        private const int _firstSyllableHasStartConsonant = 60; // % 1st syllables which start with a consonant
        private const int _syllableHasStartConsonant = 50; // % syllables which start with a consonant
        private const int _syllableHasEndConsonant = 70; // % syllables which end with a consonant
        private const int _startConsonantComplex = 40; // % end consonants which are complex
        private const int _endConsonantComplex = 50; // % end consonants which are complex
        private const int _finalConsonantsModified = 30; // % modifiable end consonants which are modified
        private const int _vowelComplex = 30; // % vowels which are complex

        // Used to determine the syllable count randomly. Higher numbers mean a syllable is more likely to be chosen.
        // Index 0 is 1 syllable, index 1 is 2 syllables, etc.
        private static readonly int[] _syllableWeights =
            {
                2, // 1 syllable words
				5, // 2 syllable words
				3 // 3 syllable words
			};

        private static readonly int _combinedSyllableWeights;

        static RandomWordGenerator()
        {
            _combinedSyllableWeights = 0;

            foreach (var weight in _syllableWeights)
            {
                _combinedSyllableWeights += weight;
            }
        }

        static readonly char[] _vowels =
            {
                'a', 'e', 'i', 'o', 'u', 'y'
            };

        static readonly string[] _complexVowels =
            {
                "ai", "au",
                "ea", "ee",
                "ie",
                "oo", "oa", "oi", "ou",
                "ua"
            };

        private static readonly string[] _simpleConsonants =
            {
                "b", "c", "d", "g", "l", "m", "n", "p", "s", "t", "w", "z", "v"
            };

        // complex consonant sounds
        private static readonly string[] _startConsonants =
            {
                "bl", "br",
                "ch", "cl", "cr",
                "dr",
                "f", "fl", "fr",
                "gl", "gr",
                "h",
                "j",
                "k", "kl", "kn", "kr",
                "pe", "ph", "pl", "pr",
                "qu",
                "rh",
                "sc", "sh", "sk", "sl", "sm", "sn", "sp", "st", "str",
                "th", "tr", "tw",
                "v",
                "wh", "wr",
                "y"
            };

        private static readonly string[] _endConsonants =
            {
                "ch", "ck",
                "dge",
                "fe", "ff", "ft",
                "ght", "gh",
                "ke",
                "ld", "ll",
                "nd", "ng", "nk", "nt",
                "mg", "mp",
                "re", "rp", "rt",
                "sh", "sk", "sp", "ss", "st",
                "tch", "th",
                "x"
            };

        // consonants which can be modified (softening the consonant or changing the vowel) by adding an "e" to the end
        private static readonly char[] _endConsonantsModifiable =
            {
                'b', 'd', 'g', 'l', 'm', 'n', 'r', 't'
            };

        // consonants which must be modified if they occur at the end of a word
        private static readonly char[] _endConsonantsMustBeModified =
            {
                'c', 'l', 'v', 's'
            };

        // consonants where another consonant never follows mid-word
        private static readonly char[] _consonantNeverFollows =
            {
                'b'
            };

        private static readonly Random _rand = new Random();

        public static string GetWord()
        {
            // determine no of syllables
            var syllables = 0;
            var selector = _rand.Next(_combinedSyllableWeights - 1);

            for (var i = 0; i < _syllableWeights.Length; i++)
            {
                var weight = _syllableWeights[i];
                if (selector < weight)
                {
                    syllables = i + 1;
                    break;
                }

                selector -= weight;
            }

            return GetWord(syllables);
        }

        public static string GetWord(int syllables)
        {
            if (syllables < 1)
                throw new ArgumentException("Word must have at least 1 syllable.");

            var word = "";
            string lastSyllable = null;

            for (var i = 0; i < syllables; i++)
            {
                var makeItShort = i > 0 && syllables > 2;
                var last = i == syllables - 1;
                var syllable = Syllable(lastSyllable, makeItShort, last);

                // single syllable words must be more than one character
                while (syllables == 1 &&
                       syllable.Length == 1)
                {
                    syllable = Syllable(lastSyllable, makeItShort, last);
                }

                //if(i > 0) word += "-";
                word += syllable;
                lastSyllable = syllable;
            }

            return word;
        }

        private static string Syllable(string previous, bool makeItShort, bool isLast)
        {
            var isFirst = previous == null;
            var lastSyllableSimple = previous != null && previous.Length < 3;
            var lastSyllableComplexEnd = false;
            var lastLetterIsNeverFollowedByConsonant = false;

            if (previous != null)
            {
                var count = 0;

                for (var i = previous.Length - 1; i > 0; i--)
                {
                    var lookAt = previous[i];

                    if (!_vowels.Contains(lookAt))
                        count++;
                    else
                        break;
                }

                lastSyllableComplexEnd = count > 1;

                if (_consonantNeverFollows.Contains(previous[previous.Length - 1]))
                    lastLetterIsNeverFollowedByConsonant = true;
            }

            var startConsonant = isFirst
                                      ? _rand.Next(100) < _firstSyllableHasStartConsonant
                                      : _rand.Next(100) < _syllableHasStartConsonant;

            startConsonant = startConsonant && (isFirst || lastLetterIsNeverFollowedByConsonant);

            // always start with consonant if previous syllable ends in vowel
            if (!startConsonant && previous != null &&
                !lastLetterIsNeverFollowedByConsonant)
            {
                var lastChar = previous[previous.Length - 1];
                if (_vowels.Contains(lastChar))
                    startConsonant = true;
            }

            var endConsonant = !(makeItShort && startConsonant) && _rand.Next(100) < _syllableHasEndConsonant;

            // always end in consonant if the last syllable was short
            if (lastSyllableSimple && !endConsonant)
            {
                endConsonant = true;
            }

            var syllable = "";

            string start = null;
            var complexStartConsonant = false;

            if (startConsonant)
            {
                complexStartConsonant = !lastSyllableComplexEnd && _rand.Next(100) < _startConsonantComplex;
                start = complexStartConsonant ? _startConsonants.Random() : _simpleConsonants.Random();
                syllable += start;
            }

            var complexVowel = startConsonant && !complexStartConsonant && _rand.Next(100) < _vowelComplex;

            if (complexVowel)
                syllable += _complexVowels.Random();
            else
                syllable += _vowels.Random().ToString();

            if (endConsonant)
            {
                var complex = _rand.Next(100) < _endConsonantComplex;
                var startEndsWithR = start != null && start.EndsWith("r");
                string end;

                do
                {
                    end = complex ? _endConsonants.Random() : _simpleConsonants.Random();
                }
                while (startEndsWithR && end.StartsWith("r"));

                var modify = isLast && (_endConsonantsMustBeModified.Contains(end[end.Length - 1]) || (
                                                                                                          (end == "ch" || _endConsonantsModifiable.Contains(end[end.Length - 1])) &&
                                                                                                          _rand.Next(100) < _finalConsonantsModified));
                if (modify)
                    end += "e";

                syllable += end;
            }

            return syllable;
        }
    }

    internal static class RandomWordExtensions
    {
        private static readonly Random _rand = new Random();

        public static string Random(this string[] source)
        {
            return source[_rand.Next(source.Length - 1)];
        }

        public static char Random(this char[] source)
        {
            return source[_rand.Next(source.Length - 1)];
        }
    }
}