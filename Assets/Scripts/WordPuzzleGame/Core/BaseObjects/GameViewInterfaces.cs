using System;
using UnityEngine;

namespace STGames
{
    public interface IGameView
    {
        public GameObject Object { get; }
        string ViewId { get; }
        void OnViewOpen();
        void OnViewClose();
    }

    public interface IGamePopupView
    {
        public GameObject Object { get; }
        string ViewId { get; }
        public int ZIndex { get; set; }
        public IGamePopupView Previous { get; set; }
        void SetOutCallback(Action outCallback); 
        void OnPopupOpen();
        void OnPopupClose();
        void Show();
        void Hide();
    }
}