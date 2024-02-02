using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace STGames
{
    public class LanguagePopupView : GamePopupView<LanguagePopupView>
    {
        [SerializeField] private GameObject languagePrefab;
        [SerializeField] private Transform languageLayout;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private CloseButton closeButton;

        private bool isLanguageLayoutCreated;
        private Language? currentLanguage;
        private bool isFirstTimeLanguageSelection;

        protected override void Awake()
        {
            base.Awake();
            isFirstTimeLanguageSelection = !LS.HasMilestoneReached(Milestone.FirstLanguageSelected);
            confirmButton.onClick.AddListener(OnConfirmClick);
        }

        public override void OnPopupOpen()
        {
            ScaleUp();
            title.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            closeButton.gameObject.SetActive(!isFirstTimeLanguageSelection);
            CreateLanguageLayout();
        }


        private void ToggleLanguage(Language language)
        {
            for (var i = 0; i < languageLayout.childCount; i++)
            {
                var languageButtonView = languageLayout.GetChild(i).GetComponent<LanguageButtonView>();
                var toggle = languageButtonView.GetComponentInChildren<Toggle>();
                toggle.isOn = languageButtonView.buttonLanguage == language;
            }
        }

        private void SetCurrentLanguage(Language language)
        {
            currentLanguage = language;
            title.SetText(LanguageUtil.GetNativeLanguageName(language));
            confirmButton.gameObject.SetActive(true);
            if (confirmButton.transform.localScale.x == 0)
            {
                confirmButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
            }
            else
            {
                confirmButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1);
            }
        }

        private void CreateLanguageLayout()
        {
            if (isLanguageLayoutCreated) return;

            var languages = Enum.GetValues(typeof(Language)).Cast<Language>().OrderBy(s => s.ToString());

            foreach (var language in languages)
            {
                var languageGameObject = Instantiate(languagePrefab, languageLayout);
                var languageButtonView = languageGameObject.GetComponent<LanguageButtonView>();
                languageButtonView.LanguageChanged += SetCurrentLanguage;
                languageButtonView.SetLanguage(language);
            }

            var toggleGroup = languageLayout.gameObject.AddComponent<ToggleGroup>();
            toggleGroup.allowSwitchOff = true;

            for (var i = 0; i < languageLayout.childCount; i++)
            {
                var toggle = languageLayout.GetChild(i).GetComponentInChildren<Toggle>();
                toggle.group = toggleGroup;
            }

            if (isFirstTimeLanguageSelection)
            {
                confirmButton.gameObject.SetActive(false);
                toggleGroup.SetAllTogglesOff(false);
            }
            else
            {
                ToggleLanguage(AS.Language);
            }

            isLanguageLayoutCreated = true;
        }

        private void OnConfirmClick()
        {
            if (currentLanguage == null) return;
            AS.Language = currentLanguage.Value;
            LSh.SetLanguage(currentLanguage.Value);
            Navigation.ClosePopup();
        }
    }
}