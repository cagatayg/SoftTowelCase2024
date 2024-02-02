using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace STGames
{
    public class BaseBehaviour : MonoBehaviour
    {
        private UINavigation navigation;
        private bool destroyed;

        public UINavigation Navigation
        {
            get
            {
                if (navigation == null) navigation = FindObjectOfType<UINavigation>();
                return navigation;
            }
        }

        protected virtual void Start()
        {
#if UNITY_IOS
            EnterApplication();
#endif
        }


        protected WaitUntil WaitForTask(Task task, float timeoutInSeconds = 0)
        {
            var timeout = timeoutInSeconds == 0 ? DateTime.MaxValue : DateTime.Now.AddSeconds(timeoutInSeconds);
            return new WaitUntil(() => task.IsFaulted || task.IsCanceled || task.IsCompleted || DateTime.Now > timeout || destroyed);
        }
  
        protected WaitUntil WaitForGameLoad(float timeoutInSeconds = 0)
        {
            var timeout = timeoutInSeconds == 0 ? DateTime.MaxValue : DateTime.Now.AddSeconds(timeoutInSeconds);
            return new WaitUntil(() => AS.HasGameLoaded || DateTime.Now > timeout);
        }

        protected virtual void EnterApplication()
        {
        }

        protected virtual void ExitApplication()
        {
        }

#if UNITY_IOS
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                EnterApplication();
            }
            else
            {
                ExitApplication();
            }
        }

#endif

#if UNITY_ANDROID || UNITY_EDITOR
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                ExitApplication();
            }
            else
            {
                EnterApplication();
            }
        }
#endif
        protected void StopRoutine(IEnumerator routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
        }
        
        protected void StopRoutine(Coroutine routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
        }
    }
}

