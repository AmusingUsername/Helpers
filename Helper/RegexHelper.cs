using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Helper
{
    public static class RegexHelper
    {
        /// <summary>
        /// Return all matches to the specified regex pattern in a list of strings.
        /// </summary>
        /// <param name="input">String to search for matches.</param>
        /// <param name="pattern">Pattern to match against.</param>
        /// <param name="matchMarker">If regex pattern contains a matching group, returns the values matching this marker.</param>
        /// <returns>List of strings matching the specified regex pattern.</returns>
        public static List<string> getAllMatches(string input, string pattern, string matchMarker = "")
        {
            List<string> matches = new List<string>();
            if(matchMarker.StartsWith("<") && matchMarker.EndsWith(">"))
            {
                matchMarker = matchMarker.Remove(matchMarker.Length - 1, 1).Remove(0, 1);
            }
            Match m = Regex.Match(input, pattern);
            while(m.Success)
            {
                if (string.IsNullOrEmpty(matchMarker))
                {
                    matches.Add(m.Value);
                }
                else
                {
                    matches.Add(m.Result("${" + matchMarker + "}"));
                }
                m = m.NextMatch();
            }
            return matches;
        }
    }
}
