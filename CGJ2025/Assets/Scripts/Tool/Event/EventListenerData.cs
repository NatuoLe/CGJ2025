using System;
using UnityEngine;

namespace ThGold.Event
{
    /// <summary>
    /// EventListenerData
    /// </summary>
    public class EventListenerData
    {
        /// <summary>
        /// The _event listener.
        /// </summary>
        private object _eventListener;
        public object eventListener
        {
            get
            {
                return _eventListener;
            }
            set
            {
                _eventListener = value;

            }
        }

        /// <summary>
        /// 事件名
        /// </summary>
        private string _eventName_string;
        public string eventName
        {
            get
            {
                return _eventName_string;
            }
            set
            {
                _eventName_string = value;

            }
        }



        /// <summary>
        /// 事件委托
        /// </summary>
        private EventDelegate _eventDelegate;
        public EventDelegate eventDelegate
        {
            get
            {
                return _eventDelegate;
            }
            set
            {
                _eventDelegate = value;

            }
        }

        private EventDispatcherAddMode _eventListeningMode;
        public EventDispatcherAddMode eventListeningMode
        {
            get
            {
                return _eventListeningMode;
            }
            set
            {
                _eventListeningMode = value;

            }
        }
        public Guid Guid{ get; private set; }

        
        private int _cachedHashCode; // 缓存哈希值s
        ///<summary>
        ///	 Constructor
        ///</summary>
        public EventListenerData(object aEventListener, string aEventName_string, EventDelegate aEventDelegate, EventDispatcherAddMode aEventListeningMode)
        {
            _eventListener = aEventListener;
            _eventName_string = aEventName_string;
            _eventDelegate = aEventDelegate;
            _eventListeningMode = aEventListeningMode;
            _cachedHashCode = CalculateHashCode(); // 初始化时计算并缓存哈希值
        }

        // 计算哈希值
        private int CalculateHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (eventListener?.GetHashCode() ?? 0);
            hash = hash * 23 + (eventName?.GetHashCode() ?? 0);
            hash = hash * 23 + (eventDelegate?.GetHashCode() ?? 0);
            hash = hash * 23 + eventListeningMode.GetHashCode();
            return hash;
        }

        public static int TryGetHashCode(object aEventListener, string aEventName_string, EventDelegate aEventDelegate,
            EventDispatcherAddMode aEventListeningMode)
        {
            int hash = 17;
            hash = hash * 23 + (aEventListener?.GetHashCode() ?? 0);
            hash = hash * 23 + (aEventName_string?.GetHashCode() ?? 0);
            hash = hash * 23 + (aEventDelegate?.GetHashCode() ?? 0);
            hash = hash * 23 + aEventListeningMode.GetHashCode();
            return hash;
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            return _cachedHashCode;
        }

        // 重写 Equals 方法
        public override bool Equals(object obj)
        {
            if (obj is EventListenerData other)
            {
                return eventListener == other.eventListener &&
                       eventName == other.eventName &&
                       eventDelegate == other.eventDelegate &&
                       eventListeningMode == other.eventListeningMode;
            }
            return false;
        }
    }
}