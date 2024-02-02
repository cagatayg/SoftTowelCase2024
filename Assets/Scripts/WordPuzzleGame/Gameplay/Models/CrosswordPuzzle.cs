using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace STGames
{
    public enum SolveWordResult : byte
    {
        Added,
        NotExist,
        Duplicate,
        Bonusword,
        BonuswordDuplicate,
    }

    public class CrosswordPuzzle
    {
        public List<string> SolvedWordsInOrder = new List<string>();
        public int2 Dimensions { get; set; }
        public char[] Letters { get; set; }
        public Dictionary<string, int2[]> Lookup { get; internal set; }
        public Dictionary<int2, char> PositionToLetter { get; internal set; }
        public List<string> BonusWords { get; set; }
        public List<string> SolvedBonusWords { get; set; } = new List<string>();
        public HashSet<int2> SolvedPositions { get; set; } = new HashSet<int2>();
        
        public Level Level { get; internal set; }
        

        public bool IsPuzzleCompleted()
        {
            return IsSolved(GetAllPositionsAsSet().ToArray());
        }

        public int PositionCount
        {
            get { return GetAllPositionsAsSet().ToArray().Length; }
        }

        public string[] BoardWords => Lookup.Keys.ToArray();

        public bool HasBonusWords => BonusWords.Count != 0;

        public string[] AllBonusWords => BonusWords.ToArray();
        

        public HashSet<string> AllWords;
        
        public int NumberOfSolvedBonusWords => SolvedBonusWords.Count;

        public int2[] UnsolvedPositions => GetAllPositionsAsSet().Where(p => !SolvedPositions.Contains(p)).ToArray();

        public HashSet<int2> GetAllPositionsAsSet()
        {
            return new HashSet<int2>(GetAllPositions());
        }

        private IEnumerable<int2> GetAllPositions()
        {
            var positionSet = new HashSet<int2>();
            foreach (var values in Lookup.Values)
            {
                foreach (var value in values)
                {
                    positionSet.Add(value);
                }
            }

            return positionSet.ToArray();
        }

        public char GetLetterAtPosition(int2 position)
        {
            if (PositionToLetter.ContainsKey(position) == false)
            {
                return ' ';
            }

            return PositionToLetter[position];
        }

        public int2[] GetPositionsForWord(string word)
        {
            return Lookup.ContainsKey(word) ? Lookup[word] : null;
        }

        public List<string> GetWordFromPosition(int2 pos)
        {
            return Lookup.Where(l => l.Value.Contains(pos)).Select(l => l.Key).ToList();
        }

        public SolveWordResult GetPossibleSolution(string word)
        {
            var positions = GetPositionsForWord(word);

            if (positions == null || !BoardWords.Contains(word))
            {
                if (BonusWords.Contains(word))
                {
                    if (SolvedBonusWords.Contains(word))
                    {
                        return SolveWordResult.BonuswordDuplicate;
                    }

                    return SolveWordResult.Bonusword;
                }

                return SolveWordResult.NotExist;
            }

            return IsSolved(positions) ? SolveWordResult.Duplicate : SolveWordResult.Added;
        }


        public SolveWordResult Solve(string word)
        {

            var positions = GetPositionsForWord(word);
            if (positions == null || !BoardWords.Contains(word))
            {
                if (BonusWords.Contains(word))
                {
                    if (SolvedBonusWords.Contains(word))
                    {
                        return SolveWordResult.BonuswordDuplicate;
                    }
                    else
                    {
                        SolvedBonusWords.Add(word);
                        return SolveWordResult.Bonusword;
                    }
                }

                return SolveWordResult.NotExist;
            }
            if(!SolvedWordsInOrder.Contains(word)) SolvedWordsInOrder.Add(word);
            return IsSolved(positions) ? SolveWordResult.Duplicate : SolvePositions(positions);
        }

        public int UnsolvedWordCount
        {
            get
            {
                var count = 0;
                foreach (var word in BoardWords)
                {
                    var positions = GetPositionsForWord(word);
                    if (IsSolved(positions) == false)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public List<string> UnsolvedWords
        {
            get
            {
                var result = new List<string>();
                foreach (var word in BoardWords)
                {
                    var positions = GetPositionsForWord(word);
                    if (IsSolved(positions) == false)
                    {
                        result.Add(word);
                    }
                }

                return result;
            }
        }

        public string NextUnsolvedWord
        {
            get
            {
                foreach (var word in BoardWords)
                {
                    var positions = GetPositionsForWord(word);
                    if (IsSolved(positions) == false)
                    {
                        return word;
                    }
                }

                return null;
            }
        }

        public string NextUnsolvedWordRandom
        {
            get
            {
                foreach (var word in BoardWords.OrderBy((s) => Common.Random.Next()))
                {
                    var positions = GetPositionsForWord(word);
                    if (IsSolved(positions) == false)
                    {
                        return word;
                    }
                }

                return null;
            }
        }

        public SolveWordResult SolvePositions(int2[] positions)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                if (SolvedPositions.Contains(positions[i])) continue;
                SolvedPositions.Add(positions[i]);
            }
            
            return SolveWordResult.Added;
        }

        public bool IsSolved(int2[] positions)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                if (SolvedPositions.Contains(positions[i])) continue;
                return false;
            }

            return true;
        }

        public bool IsSolved(string word)
        {
            var positions = GetPositionsForWord(word);
            if (positions == null) return false;
            return IsSolved(positions);
        }

        public void SolvePosition(int2 position)
        {
            if (!SolvedPositions.Contains(position))
            {
                SolvedPositions.Add(position);
            }
        }
    }
}