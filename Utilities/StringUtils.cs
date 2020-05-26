using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary> String utilities. </summary>
    public class StringUtils
    {
        /// <summary> Convert string from one codepage to another. </summary>
        /// <param name="input">        The string. </param>
        /// <param name="fromEncoding"> input encoding codepage. </param>
        /// <param name="toEncoding">   output encoding codepage. </param>
        /// <returns> the encoded string. </returns>
        public static string ConvertEncoding(string input, Encoding fromEncoding, Encoding toEncoding)
        {
            var byteArray = fromEncoding.GetBytes(input);
            var asciiArray = Encoding.Convert(fromEncoding, toEncoding, byteArray);
            var finalString = toEncoding.GetString(asciiArray);
            return finalString;
        }
        public static string UniqueName(string baseName, string[] names)
        {
            baseName = baseName.TrimEnd();
            baseName = Regex.Replace(baseName, @"\(\d+\)$","");
            baseName = Regex.Replace(baseName, @"\d+\$","");
            baseName = baseName.TrimEnd();
            if (!names.Contains(baseName)) return baseName;
            var count = 1;
            while (true)
            {
                count++;
                var endName = string.Format("{0} ({1})", baseName, count);
                if (!names.Contains(endName)) return endName;
            }
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value)) return source;
            return source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
        }

        public static string TrimStart(this string source, string value)
        {
            if (!source.StartsWith(value)) return source;
            return source.Remove(source.IndexOf(value, StringComparison.Ordinal),value.Length);
        }

        /// <summary>
        /// Returns <paramref name="str"/> with the minimal concatenation of <paramref name="ending"/> (starting from end) that
        /// results in satisfying .EndsWith(ending).
        /// </summary>
        /// <example>"hel".WithEnding("llo") returns "hello", which is the result of "hel" + "lo".</example>
        public static string WithEnding(this string str, string ending)
        {
            if (str == null)
                return ending;

            string result = str;

            // Right() is 1-indexed, so include these cases
            // * Append no characters
            // * Append up to N characters, where N is ending length
            for (int i = 0; i <= ending.Length; i++)
            {
                string tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending))
                    return tmp;
            }

            return result;
        }

        /// <summary>Gets the rightmost <paramref name="length" /> characters from a string.</summary>
        /// <param name="value">The string to retrieve the substring from.</param>
        /// <param name="length">The number of characters to retrieve.</param>
        /// <returns>The substring.</returns>
        public static string Right(this string value, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "Length is less than zero");
            }

            return (length < value.Length) ? value.Substring(value.Length - length) : value;
        }
		
        public static string Remove(this string s, string substring)
        {
            return s.Replace(substring, "");
        }

        public static IEnumerable<string> SplitOnLength(this string s, int partLength) {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static string SplitOnLength(this string s, int partLength,string prepend="", string append="") {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            var result = "";
            for (var i = 0; i < s.Length; i += partLength)
                result += prepend+ s.Substring(i, Math.Min(partLength, s.Length - i)) + append+ ((i < s.Length-partLength)?Environment.NewLine:"");
            return result;
        }

        public static IEnumerable<string> SplitOnNewLine(this string s)
        {
            string[] lines = s.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            return lines;
        }

        public static string ToHex(this string s)
        {
            string hex = "";
            foreach (char c in s)
            {
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(((int)(c)).ToString()));
            }
            return hex;
        }

    }

}
