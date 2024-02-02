using UnityEngine;

namespace STGames
{
    public class GameView<T> : Singleton<T>, IGameView
    {
        public string ViewId { get => GetType().Name; }
        public virtual GameObject Object => gameObject;
        public virtual void OnViewClose() { }
        public virtual void OnViewOpen() { }
    }
}