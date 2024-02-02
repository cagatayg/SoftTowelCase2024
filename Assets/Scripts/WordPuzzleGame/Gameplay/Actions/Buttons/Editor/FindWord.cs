using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class FindWord : Singleton<FindWord>
    {
        [SerializeField] private Button button;
        protected override void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            var word = puzzle.NextUnsolvedWordRandom;
            GameManager.Instance.SolveWord(word);
        }
    }
}