using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var resultSentences = new List<List<string>>();
            var sentences = text.Split('.', '!', '?', ';', ':', '(', ')');
            foreach (var sentence in sentences)
            {
                char[] divider = DeleteNotTender(sentence);
                string[] constructor = sentence.Split(divider);
                List<string> words = new List<string>();
                foreach (var speech in constructor)
                {
                    string word = Regex.Replace(speech, @"\p{C}+", "");
                    word = word.ToLower();
                    if (word.Length > 0)
                        if (char.IsLetter(word[0]) || word.StartsWith("\'"))
                            words.Add(word);
                }
                if (words.Count > 0) resultSentences.Add(words);
            }
            return resultSentences;
        }

        public static char[] DeleteNotTender(string sentence)
        {
            char[] divider = new char[sentence.Length];
            int i = 0;
            foreach (char symbol in sentence)
            {
                if (!char.IsLetter(symbol) && symbol != '\'') divider[i] = symbol;
                i++;
            }
            return divider;
        }
    }
}