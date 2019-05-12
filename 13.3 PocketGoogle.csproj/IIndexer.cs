using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly char[] getsplit = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        private readonly Dictionary<string, Dictionary<int, List<int>>> words;

        public Indexer()
        {
            words = new Dictionary<string, Dictionary<int, List<int>>>();
        }

        public void Add(int id, string documentText)
        {
            var doc = documentText.Split(getsplit);
            int pos = 0;
            foreach (var word in doc)
            {
                if (!words.ContainsKey(word))
                {
                    words.Add(word, new Dictionary<int, List<int>>());
                }
                if (!words[word].ContainsKey(id))
                {
                    words[word].Add(id, new List<int>());
                }
                words[word][id].Add(pos);
                pos += word.Length + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            if (words.ContainsKey(word))
                return new List<int>(words[word].Keys);
            return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (words.ContainsKey(word))
                if (words[word].ContainsKey(id))
                    return words[word][id];
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var word in words.Keys)
            {
                words[word].Remove(id);
            }
        }
    }
}
