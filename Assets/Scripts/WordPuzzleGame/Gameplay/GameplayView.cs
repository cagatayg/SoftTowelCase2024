using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace STGames
{
    public class GameplayView : GameView<GameplayView>
    {
        [SerializeField] private WheelView wheelView;
        [SerializeField] private CrosswordView crosswordView;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private GameObject editorActions;

        protected override void Awake()
        {
            base.Awake();
            backButton.onClick.AddListener(OnBackButton);
        }

        public override void OnViewOpen()
        {
            editorActions.SetActive(Application.isEditor);
            StartSinglePlayerGame();
        }

        private void StartSinglePlayerGame()
        {
            GameManager.Instance.LoadLevel();
            StartCoroutine(DrawSinglePlayerGame());
        }

        private IEnumerator DrawSinglePlayerGame()
        {
            levelNameText.SetText($"Level {LSh.CurrentLevel}");
            crosswordView.Create();
            crosswordView.Draw();
            wheelView.Create();
            yield return new WaitForSeconds(0.5f);
            LogLevelInfo();
        }


        private void LogLevelInfo()
        {
#if UNITY_EDITOR
            Debug.Log($"WORDS: {string.Join(", ", GameBootstrap.CurrentPuzzle.BoardWords)}");
            Debug.Log($"BONUS WORDS: {string.Join(", ", GameBootstrap.CurrentPuzzle.BonusWords)}");
#endif
        }

        private void OnBackButton()
        {
            GameManager.Instance.Remove();
            Navigation.CloseAllPopups();
            Navigation.ToScreen<MainMenuView>();
        }
    }
}