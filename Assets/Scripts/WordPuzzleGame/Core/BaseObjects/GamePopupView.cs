using DG.Tweening;
using System;
using UnityEngine;

namespace STGames
{
    public class GamePopupView<T> : Singleton<T>, IGamePopupView
    {
        protected bool IsOpen => Navigation.IsOpen(ViewId);
        public virtual int ZIndex { get; set; }
        public virtual IGamePopupView Previous { get; set; }
        public virtual GameObject Object => gameObject;
        public string ViewId => GetType().Name;

        private Action _outCallback;

        protected new virtual void Awake()
        {
            base.Awake();
        }

        public virtual void SetOutCallback(Action outCallback = null)
        {
            _outCallback = outCallback;
        }

        public virtual void OnPopupOpen()
        {
        }

        public virtual void OnPopupClose()
        {
        }

        private void OnEnable()
        {
            OnPopupOpen();
        }

        protected void ScaleUp()
        {
            transform.localScale = Vector3.one * 0.9f;
            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

  
        private void OnDisable()
        {
            _outCallback?.Invoke();
            _outCallback = null;
        }


        public void Show()
        {
            transform.localScale = Vector3.one;
        }

        public void Hide()
        {
            transform.localScale = Vector3.zero;
        }
    }
}