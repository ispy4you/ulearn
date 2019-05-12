using System;
using System.Collections.Generic;


namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase
            (Dictionary<string,
            string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            string extraword = null;
            string resultword = phraseBeginning;
            for (int i = 0; i < wordsCount; i++)
            {
                if (nextWords.ContainsKey(phraseBeginning))
                    extraword = nextWords[phraseBeginning];
                else if (phraseBeginning.Split().Length > 1)
                {
                    if (nextWords.ContainsKey(SplitWord(2, phraseBeginning) + " " + SplitWord(1, phraseBeginning)))
                        extraword = nextWords[SplitWord(2, phraseBeginning) + " " + SplitWord(1, phraseBeginning)];
                    else if (nextWords.ContainsKey(SplitWord(1, phraseBeginning)))
                        extraword = nextWords[SplitWord(1, phraseBeginning)];
                    else break;
                }
                else
                    break;
                if (phraseBeginning.Split().Length > 1)
                    phraseBeginning = SplitWord(1, phraseBeginning) + " " + extraword;
                else phraseBeginning = phraseBeginning + " " + extraword;
                resultword += (" " + extraword);
            }
            return resultword;
        }

        public static string SplitWord(int i, string text)
        {
            return text.Split()[text.Split().Length - i];
        }
    }
}