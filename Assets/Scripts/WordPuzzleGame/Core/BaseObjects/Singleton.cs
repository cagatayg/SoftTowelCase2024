using System;
using System.Collections.Generic;
using UnityEngine;

namespace STGames
{
    public class Singleton<T> : BaseBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindInstance();
                }

                if (instance == null)
                {
                    Debug.LogWarningFormat("[SingletonComponent] Returning null instance for component of type {0}.", typeof(T));
                }

                return instance;
            }
        }


        protected virtual void Awake()
        {
            SetInstance();
        }

        public static bool Exists()
        {
            return instance != null;
        }

        public bool SetInstance()
        {
            if (instance != null && !EqualityComparer<T>.Default.Equals(instance, GetComponentInstance()))
            {
                Debug.LogWarning("[SingletonComponent] Instance already set for type " + typeof(T));
                return false;
            }

            instance = gameObject.GetComponent<T>();

            return true;
        }

        public void SetInstance(T obj)
        {
            instance = obj;
        }

        private T GetComponentInstance()
        {
            var obj = GetComponent(typeof(T));
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        private static T FindInstance()
        {
            var obj = FindObjectOfType(typeof(T));
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}
