using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class NextLevel : Singleton<NextLevel>
    {
        [SerializeField] private Button button;
        protected override void Awake()
        {
            button.onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            var level = LSh.CurrentLevel;
            if(level == 20) return;
            LS.Set(SK.Level, level + 1);
            GameplayView.Instance.OnViewOpen();
        }
    }
}