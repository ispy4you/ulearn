using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var actualCount = Math.Min(count, phrases.Count - leftBorder);
            var result = new List<string>();
            var nextPhrases = 0;
            if (leftBorder == phrases.Count)
                return new string[0];
            for (var i = 0; i < actualCount; i++)
            {
                nextPhrases = leftBorder + i;
                if (!phrases[nextPhrases].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    break;
                result.Add(phrases[nextPhrases]);
            }
            return result.ToArray();
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            int left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            int right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            return right - left - 1;
        }
    }
}