using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class MainMenuView : GameView<MainMenuView>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button languageButton;
        [SerializeField] private TextMeshProUGUI playButtonText;

        protected override void Awake()
        {
            base.Awake();
            playButton.onClick.AddListener(Play);
            languageButton.onClick.AddListener(Language);
        }

        public override void OnViewOpen()
        {
            playButtonText.SetText($"Level {LSh.CurrentLevel}");
        }

        private void Language()
        {
            Navigation.OpenPopup<LanguagePopupView>();
        }

        private void Play()
        {
            Navigation.CloseAllPopups();
            Navigation.ToScreen<GameplayView>();
        }
    }
}