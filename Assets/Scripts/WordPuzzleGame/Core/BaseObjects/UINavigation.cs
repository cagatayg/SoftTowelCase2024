using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extensions.DOTweenExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class UINavigation : Singleton<UINavigation>
    {
        public bool IsAnyPopupOpen => openPopups != null && openPopups.Count > 0;
        public IGameView CurrentScreen => currentScreen;
        public IGamePopupView TopMostPopup => topMostPopup;
        [SerializeField] private Transform screensContainer;
        [SerializeField] private Image popupShade;
        private List<IGamePopupView> openPopups;
        private Dictionary<string, IGamePopupView> availablePopups;
        private Dictionary<string, IGameView> availableViews;
        private IGamePopupView topMostPopup;
        private IGameView currentScreen;
        private Tweener shadeTween;

        protected override void Awake()
        {
            base.Awake();
            Initialize(screensContainer);
        }

        private void Initialize(Transform navigationRoot)
        {
            openPopups = new List<IGamePopupView>();
            availablePopups = new Dictionary<string, IGamePopupView>();
            availableViews = new Dictionary<string, IGameView>();
            for (var i = 0; i < navigationRoot.childCount; i++)
            {
                var child = navigationRoot.GetChild(i);
                var childPopupView = child.GetComponent<IGamePopupView>();
                if (childPopupView != null)
                {
                    availablePopups[childPopupView.ViewId] = childPopupView;
                }
                else
                {
                    var childView = child.GetComponent<IGameView>();
                    if (childView != null)
                    {
                        availableViews[childView.ViewId] = childView;
                    }
                }
            }
        }

        public void ClosePopup(string viewId)
        {
            var view = openPopups.FirstOrDefault(a => a.ViewId == viewId);
            if (view == null) return;
            view.Previous = null;
            view.OnPopupClose();
            view.Object.SetActive(false);
            openPopups = openPopups.Where(a => a.ViewId != viewId).ToList();
            ResetState();
            if (topMostPopup != null) topMostPopup.Show();
        }


        public void CloseAllPopups()
        {
            while (openPopups.Count > 0)
            {
                ClosePopup();
            }
        }

        public void ClosePopup()
        {
            if (topMostPopup != null) ClosePopup(topMostPopup.ViewId);
        }

        private void ResetState()
        {
            var zIdx = 0;
            IGamePopupView previous = null;
            topMostPopup = null;
            foreach (var openPopup in openPopups)
            {
                openPopup.ZIndex = zIdx;
                openPopup.Previous = previous;
                previous = openPopup;
                topMostPopup = openPopup;
                zIdx++;
            }

            if (topMostPopup == null) RemoveShade();
            else Shade();
        }

        public T OpenPopup<T>(Action outCallback = null) where T : IGamePopupView
        {
            var viewType = typeof(T);
            var view = availablePopups[viewType.Name];
            OpenPopup(viewType.Name, view, outCallback);
            return GetPopup<T>();
        }

        public void ToScreen<T>() where T : IGameView
        {
            var viewType = typeof(T);
            var view = availableViews[viewType.Name];
            if (currentScreen != null)
            {
                currentScreen.OnViewClose();
                currentScreen.Object.SetActive(false);
            }
            view.Object.SetActive(true);
            view.OnViewOpen();
            currentScreen = view;
        }

        private void OpenPopup(string id, IGamePopupView view, Action outCallback = null)
        {
            if (openPopups != null && openPopups.Any(a => a.ViewId == id)) return;
            view.Object.SetActive(true);
            view.SetOutCallback(outCallback);
            if (topMostPopup != null)
            {
                topMostPopup.Hide();
                view.Previous = topMostPopup;
            }
            openPopups.Add(view);
            ResetState();
        }

        public bool IsOpen<T>() where T : IGamePopupView
        {
            var popup = GetPopup<T>();
            return IsAnyPopupOpen && openPopups.Any(a => a.ViewId == popup.ViewId);
        }

        public bool IsOpen(string viewId)
        {
            return IsAnyPopupOpen && openPopups.Any(a => a.ViewId == viewId);
        }

        public T GetPopup<T>() where T : IGamePopupView
        {
            var viewType = typeof(T);
            var item = availablePopups[viewType.Name];
            if (item is T)
            {
                return (T)item;
            }
            return default;
        }

        private void RemoveShade()
        {
            if (shadeTween != null) shadeTween.Kill();
            shadeTween = popupShade.DOFade(0, 0.3f).OnComplete(() => popupShade.gameObject.SetActive(false));
        }

        private void Shade()
        {
            if (shadeTween != null) shadeTween.Kill();
            popupShade.gameObject.SetActive(true);
            shadeTween = popupShade.DOFade(0.7f, 0.3f);
        }
    }
}