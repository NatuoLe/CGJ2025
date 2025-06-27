using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThGold.Common
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Debug.Log("instance Detesoy");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Set Instance" + name);
                instance = (T)this;
            }
        }
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}