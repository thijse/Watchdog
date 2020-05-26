using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// Represents a wildcard running on the RegEx engine.
    /// </summary>
    public class Wildcard : Regex
    {
        /// <summary>
        /// Initializes a wildcard with the given search pattern.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        public Wildcard(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }


        /// <summary>
        /// Initializes a wildcard with the given search pattern and options.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        /// <param name="caseSensitive"></param>
        public Wildcard(string pattern, bool caseSensitive = false)
            : base(WildcardToRegex(pattern), caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase)
        {
        }

                        /// <summary>
        /// Initializes a wildcard with the given search pattern and options.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        /// <param name="options">Regular expression option.</param>
        public Wildcard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {
        }

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to convert.</param>
        /// <returns>A regex equivalent of the given wildcard.</returns>
        public static string WildcardToRegex2(string pattern)
        {
            // match from beginning to end, greedy match
            return "^" + Escape(pattern).
             Replace("\\*", ".*").
             Replace("\\?", ".") + "$";
        }

        public static string WildcardToRegex3(string pattern)
        {
            // match inside of string. Non-greedy. No explicit escaping
            pattern = pattern.Replace(".", @"\."); // escaping
            pattern = pattern.Replace("?", ".");
            pattern = pattern.Replace("*", ".*?");
            pattern = pattern.Replace(@"\", @"\\");
            pattern = pattern.Replace(" ", @"\s");
            return pattern;
        }

        public static string WildcardToRegex(string pattern)
        {
            // match inside of string. Non-greedy. Explicit escaping
            return
                Regex.Escape(pattern).
                    Replace("\\*", ".*?").
                    Replace("\\?", ".");;
        }

        public new static Match Match(string input, string pattern, RegexOptions options)
        {
            string wildcardPattern = WildcardToRegex(pattern);
            return Regex.Match(input, wildcardPattern, options);
        }

        public new static Match Match(string input, string pattern)
        {
            return Match(input, pattern, RegexOptions.None);
        }

        public new static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            string wildcardPattern = WildcardToRegex(pattern);
            return Regex.IsMatch(input, wildcardPattern, options);
        }

        public new static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.None);
        }

        public new static MatchCollection Matches(string input, string pattern, RegexOptions options)
        {
            string wildcardPattern = WildcardToRegex(pattern);
            return Regex.Matches(input, wildcardPattern, options);
        }

        public new static MatchCollection Matches(string input, string pattern)
        {
            return Matches(input, pattern, RegexOptions.None);
        }
    }
}
