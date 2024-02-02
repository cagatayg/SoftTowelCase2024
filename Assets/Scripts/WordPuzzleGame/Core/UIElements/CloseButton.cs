using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class CloseButton : BaseBehaviour
    {
        [SerializeField]
        private Button button;

        private void OnEnable()
        {
            button.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            Navigation.ClosePopup();
        }
    }
}
