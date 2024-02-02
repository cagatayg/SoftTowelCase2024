using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

namespace STGames
{
    public static class CrosswordPuzzleData
    {
        private static Dictionary<Language, Dictionary<int, Level>> _levelsCache;

        public static Level LoadLevel(int levelNo)
        {
            var lang = AS.Language;
            if (_levelsCache == default) _levelsCache = new Dictionary<Language, Dictionary<int, Level>>();
            if (!_levelsCache.ContainsKey(lang))
            {
                var textAsset = Resources.Load<TextAsset>($"LevelData/{lang}");
                if (textAsset != null && textAsset.text != null)
                {
                    _levelsCache[lang] = JsonConvert.DeserializeObject<Dictionary<int, Level>>(textAsset.text);
                }
            }
            return _levelsCache[lang][levelNo];
        }

        public static CrosswordPuzzle CreateCrosswordPuzzle(Level level)
        {
            var lookup = GetLookup(level.BoardWords);
            var positionToLetter = GetPositionToLetter(lookup);
            var bonusWords = level.BonusWords;
            var allWords = new HashSet<string>(lookup.Keys);
            allWords.UnionWith(bonusWords);
            var puzzle = new CrosswordPuzzle
            {
                Letters = level.WheelLetters.ToCharArray(),
                Dimensions = new int2(level.Width, level.Height),
                Lookup = lookup,
                PositionToLetter = positionToLetter,
                BonusWords = bonusWords,
                Level = level,
                AllWords = allWords
            };
            return puzzle;
        }


        private static Dictionary<int2, char> GetPositionToLetter(Dictionary<string, int2[]> lookup)
        {
            var positionToLetter = new Dictionary<int2, char>();

            foreach (var lookupKeyValue in lookup)
            {
                var word = lookupKeyValue.Key;
                var wordLength = word.Length;

                for (var i = 0; i < wordLength; i++)
                {
                    var letterPosition = lookupKeyValue.Value[i];
                    positionToLetter[letterPosition] = word[i];
                }
            }
            return positionToLetter;
        }


        private static Dictionary<string, int2[]> GetLookup(IEnumerable<BoardWord> levelWords)
        {
            var lookup = new Dictionary<string, int2[]>();
            foreach (var boardWord in levelWords)
            {
                var positions = new int2[boardWord.Word.Length];

                for (var i = 0; i < boardWord.Word.Length; i++)
                {
                    if (boardWord.IsVertical)
                    {
                        positions[i] = new int2(boardWord.X, boardWord.Y + i);
                    }
                    else
                    {
                        positions[i] = new int2(boardWord.X + i, boardWord.Y);
                    }
                }

                lookup[boardWord.Word] = positions;
            }
            return lookup;
        }
    }
}