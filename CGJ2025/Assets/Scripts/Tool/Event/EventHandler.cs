using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThGold.Common;
using System;

namespace ThGold.Event
{
    public class EventHandler : Singleton<EventHandler>
    {
        public EventDispatcher EventDispatcher;
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("EventDispatcher new");
            EventDispatcher = new EventDispatcher(this);
            //DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {  Debug.Log("EventDispatcher OnDestroy");
            EventDispatcher.RemoveAllEventListeners();
        }
    }
}
