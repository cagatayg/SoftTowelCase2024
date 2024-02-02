using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class PrevLevel : Singleton<PrevLevel>
    {
        [SerializeField] private Button button;
        protected override void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var level = LSh.CurrentLevel;
            if(level == 1) return;
            LS.Set(SK.Level, level - 1);
            GameplayView.Instance.OnViewOpen();
        }
    }
}