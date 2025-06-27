using System;
using UnityEngine;

namespace ThGold.Common {
    public class ScriptObjectSingleton<T> : ScriptableObject where T : ScriptableObject {
        private static T _instance;

        public static T Instance {
            get { return _instance; }
        }
        
    }
}