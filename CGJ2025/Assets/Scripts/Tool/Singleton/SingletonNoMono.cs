using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThGold.Common
{
    public class SingletonNoNomo<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstance());

        public static T Instance => _instance.Value;

        private static T CreateInstance()
        {
            if (typeof(T).IsClass)
            {
                return Activator.CreateInstance<T>();
            }
            else
            {
                Debug.LogError("Singleton type must be a class.");
                return null;
            }
        }

        protected SingletonNoNomo()
        {
            if (_instance.IsValueCreated)
            {
                Debug.LogError("Singleton instance already exists.");
            }
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}