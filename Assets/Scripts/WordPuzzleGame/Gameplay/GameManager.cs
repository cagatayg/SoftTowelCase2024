using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace STGames
{
    public class GameManager : Singleton<GameManager>
    {
        public void LoadLevel()
        {
            var levelId = LSh.CurrentLevel;
            var level = CrosswordPuzzleData.LoadLevel(levelId);
            GameBootstrap.IsLevelFinished = false;
            GameBootstrap.CurrentPuzzle = CrosswordPuzzleData.CreateCrosswordPuzzle(level);
        }

        public void Remove()
        {
            WheelView.Instance.Destroy();
            CrosswordView.Instance.Destroy();
        }

        public void SolveWord(string word)
        {
            if (GameBootstrap.IsLevelFinished) return;
            var puzzle = GameBootstrap.CurrentPuzzle;
            var solution = puzzle.Solve(word);
            switch (solution)
            {
                case SolveWordResult.Added:
                    OnWordFound(word);
                    break;
                case SolveWordResult.NotExist:
                    // To be implemented
                    break;
                case SolveWordResult.Duplicate:
                    // To be implemented
                    break;
                case SolveWordResult.Bonusword:
                    // To be implemented
                    break;
                case SolveWordResult.BonuswordDuplicate:
                    // To be implemented
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnWordFound(string word)
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            var positions = puzzle.GetPositionsForWord(word);
            var letters = GetLetterViewsAtPositions(positions);
            foreach (var letter in letters)
            {
                letter.Found();
            }

            CheckCompleted();
        }

        private List<BaseLetterBox> GetLetterViewsAtPositions(int2[] positions)
        {
            return GameBootstrap.LetterBoxes.Where(lb => positions.Contains(lb.Position)).ToList();
        }

        private void CheckCompleted()
        {
            if (GameBootstrap.IsLevelFinished) return;
            if (GameBootstrap.CurrentPuzzle.IsPuzzleCompleted())
            {
                GameBootstrap.IsLevelFinished = true;
                var level = LSh.CurrentLevel;
                var nextLevel = level + 1;
                if (nextLevel > 20)
                {
                    Debug.LogError("Game Finished");
                    return;
                }

                LS.Set(SK.Level, nextLevel);
                StartCoroutine(FinishLevel());
            }
        }

        private IEnumerator FinishLevel()
        {
            yield return new WaitForSeconds(0.5f);
            GameplayView.Instance.OnViewOpen();
        }
    }
}