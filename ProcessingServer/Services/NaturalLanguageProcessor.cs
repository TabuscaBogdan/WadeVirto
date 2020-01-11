using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ProcessingServer.Services
{
    public static class NaturalLanguageProcessor
    {

        private static readonly string MusicGenerePath = Directory.GetCurrentDirectory()+@"\Resources\MusicGenres.txt";
        private static string[] MusicGenres = System.IO.File.ReadAllLines(MusicGenerePath);
        private static string[] DislikeWords = new string[] { "not", "unlike", "don't", "shit", "crap", "no", "dislike", "disagree" };
        private static string[] Punctuation = new string[] {".",",","!","?"};
        private static string RemovePunctuation(string word)
        {
            foreach (var punct in Punctuation)
            {
                word=word.Replace(punct, "");
            }

            return word;
        }

        private static bool CheckDislike(int index,string[] words)
        {
            int negationDistance = 3;
            int distance = 1;
            while (index-distance>=0 &&distance<=negationDistance)
            {
                if (DislikeWords.Contains(words[index - distance].ToLower()))
                    return true;
                distance++;
            }

            return false;
        }

        private static bool CheckDislike(int index, string text)
        {
            var preIndexText = text.Substring(0, index);
            var preWordText = preIndexText.Split(" ");
            int len = preWordText.Length;
            int i = 1;
            while (i<=3 && len-i>=0)
            {
                if (DislikeWords.Contains(preWordText[len - i]))
                    return true;
                i++;
            }
            return false;
        }

        private static string NameChecks(int index, string[] words, ref int wordNameCounts)
        {
            string word = words[index];

            if (Char.IsUpper(word[0]))
            {
                if (index != 0)
                    if (!words[index - 1].Contains(".") || !words[index - 1].Contains("!") || !words[index - 1].Contains("?"))
                        if (index + 1 < words.Length)
                        {
                            int nameWCount = 1;
                            string name = word;
                            while (Char.IsUpper(words[index + nameWCount][0]))
                            {
                                bool punctuation = false;
                                foreach (var pct in Punctuation)
                                {
                                    if (words[index + nameWCount].Contains(pct))
                                        punctuation = true;
                                }
                                
                                name = name + " " + RemovePunctuation(words[index + nameWCount]);
                                wordNameCounts = nameWCount;
                                nameWCount += 1;
                                if (punctuation == true)
                                    return name;
                            }

                            return name;
                        }
            }

            return null;
        }
        public static async Task<Dictionary<string, List<string>>> InterpretPreferences(string text)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var preferences = new Dictionary<string, List<string>>();
            var words = text.Split(" ");

            preferences.Add("!LikeMusicTypes",new List<string>());
            preferences.Add("LikeMusicTypes",new List<string>());
            preferences.Add("!LikeArtist",new List<string>());
            preferences.Add("LikeArtist",new List<string>());
            preferences.Add("LikeSong",new List<string>());

            for (int index = 0; index < words.Length; index++)
            {
                int nameLen = 0;
                var word = NameChecks(index, words, ref nameLen);
                if (word != null)
                {
                    if (CheckDislike(index, words))
                        preferences["!LikeArtist"].Add(word);
                    else
                        preferences["LikeArtist"].Add(word);
                    index += nameLen;
                }
            }

            foreach (var genre in MusicGenres)
            {
                if (text.Contains(genre))
                {
                    int finding = text.IndexOf(genre);
                    if(CheckDislike(finding, text))
                        preferences["!LikeMusicTypes"].Add(genre);
                    else
                        preferences["LikeMusicTypes"].Add(genre);
                }
            }


            return preferences;
        }
    }
}
