using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class LanguageButtonView : MonoBehaviour
    {
        [SerializeField] private Toggle selectButton;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color textInitialColor;
        public Language buttonLanguage;
        public Action<Language> LanguageChanged { get; set; }

        private void OnEnable()
        {
            selectButton.onValueChanged.AddListener(OnSelectButtonValueChanged);
        }

        private void OnDisable()
        {
            selectButton.onValueChanged.RemoveListener(OnSelectButtonValueChanged);
        }

        public void SetLanguage(Language language)
        {
            buttonLanguage = language;
            text.SetText($"{LanguageUtil.GetNativeLanguageName(language)}");
        }

        private void OnSelectButtonValueChanged(bool isSelected)
        {
            if (!isSelected)
            {
                text.color = textInitialColor;
                return;
            }
            text.color = Color.white;
            LanguageChanged.Invoke(buttonLanguage);
        }
    }
}