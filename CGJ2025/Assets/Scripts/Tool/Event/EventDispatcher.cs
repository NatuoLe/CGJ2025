// C# unity事件管理器 在哈希表中使用字符串，对委托和事件管理 
// 不知道他们何时何地declared/defined 也允许使用事件。

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ThGold.Event {
    /// <summary>
    /// Event delegateN事件委托
    /// </summary>
    public delegate void EventDelegate(IEvent iEvent);

    public delegate void EventDelegate0();

    /// <summary>
    /// Event listening mode.
    /// </summary>
    public enum EventDispatcherAddMode {
        DEFAULT,
        SINGLE_SHOT
    }

    public class EventDispatcher : IEventDispatcher {
        /// <summary>
        /// The mom_serialized object.
        /// </summary>
        private Hashtable _eventListenerDatas_hashtable = new Hashtable();

        private Hashtable _eventObjectDatas_hashtable = new Hashtable();

        private Hashtable _hashCodeMapToListener_hashtable = new Hashtable();
        /// <summary>
        /// The _target_object.
        /// </summary>
        private object _target_object;

        ///<summary>
        ///	 Constructor
        ///</summary>
        public EventDispatcher(object aTarget_object) {
            //
            _target_object = aTarget_object;
        }

        /// <summary>
        /// Adds the event listener.
        /// </summary>
        /// <returns>
        /// The event listener.
        /// </returns>
        /// <param name='aEventName_string'>
        /// If set to <c>true</c> a event name_string.
        /// </param>
        /// <param name='aEventDelegate'>
        /// If set to <c>true</c> a event delegate.
        /// </param>
        public bool AddEventListener(string aEventName_string, EventDelegate aEventDelegate) {
            return AddEventListener(aEventName_string, aEventDelegate, EventDispatcherAddMode.DEFAULT);
        }

        /// <summary>
        /// Adds the event listener.
        /// </summary>
        /// <returns>
        /// The event listener.
        /// </returns>
        /// <param name='aEventName_string'>
        /// If set to <c>true</c> a event type_string.
        /// </param>
        /// <param name='aEventDelegate'>
        /// If set to <c>true</c> a event delegate.
        /// </param>
        /// <param name='aEventDispatcherAddMode'>
        /// If set to <c>true</c> event listening mode.
        /// </param>
        public bool AddEventListener(string aEventName_string, EventDelegate aEventDelegate, EventDispatcherAddMode aEventDispatcherAddMode) {
            //
            bool wasSuccessful_boolean = false;

            //
            object aIEventListener = _getArgumentsCallee(aEventDelegate);

            //
            if (aIEventListener != null && aEventName_string != null) {
                //	OUTER
                string keyForOuterHashTable_string = _getKeyForOuterHashTable(aEventName_string);
                if (!_eventListenerDatas_hashtable.ContainsKey(keyForOuterHashTable_string)) {
                    _eventListenerDatas_hashtable.Add(keyForOuterHashTable_string, new Hashtable());
                }

                //	INNER
                Hashtable inner_hashtable = _eventListenerDatas_hashtable[keyForOuterHashTable_string] as Hashtable;
                EventListenerData eventListenerData = new EventListenerData(aIEventListener, aEventName_string, aEventDelegate, aEventDispatcherAddMode);
                //
                string keyForInnerHashTable_string = _getKeyForInnerHashTable(eventListenerData);
                if (inner_hashtable.Contains(keyForInnerHashTable_string)) {
                    //THIS SHOULD *NEVER* HAPPEN - REMOVE AFTER TESTED WELL
                    Debug.Log("TODO (FIX THIS): Event Manager: Listener: " + keyForInnerHashTable_string + " is already in list for event: " +
                              keyForOuterHashTable_string);
                }
                else {
                    //	ADD
                    inner_hashtable.Add(keyForInnerHashTable_string, eventListenerData);
                    _addObjectHashKey(aIEventListener, keyForInnerHashTable_string);
                    wasSuccessful_boolean = true;

                    //Debug.Log ("	ADDED AT: " + keyForInnerHashTable_string + " = " +  eventListenerData);
                }
            }

            return wasSuccessful_boolean;
        }


        /// <summary>
        /// Hases the event listener.
        /// </summary>
        /// <returns>
        /// The event listener.
        /// </returns>
        /// <param name='aIEventListener'>
        /// If set to <c>true</c> a I event listener.
        /// </param>
        /// <param name='aEventName_string'>
        /// If set to <c>true</c> a event name_string.
        /// </param>
        /// <param name='aEventDelegate'>
        /// If set to <c>true</c> a event delegate.
        /// </param>
        public bool hasEventListener(string aEventName_string, EventDelegate aEventDelegate) {
            //
            bool hasEventListener_boolean = false;

            //
            object aIEventListener = _getArgumentsCallee(aEventDelegate);

            //	OUTER
            string keyForOuterHashTable_string = _getKeyForOuterHashTable(aEventName_string);
            if (_eventListenerDatas_hashtable.ContainsKey(keyForOuterHashTable_string)) {
                //	INNER
                Hashtable inner_hashtable = _eventListenerDatas_hashtable[keyForOuterHashTable_string] as Hashtable;
                string keyForInnerHashTable_string =
                    _getKeyForInnerHashTable(new EventListenerData(aIEventListener, aEventName_string, aEventDelegate, EventDispatcherAddMode.DEFAULT));
                //
                if (inner_hashtable.Contains(keyForInnerHashTable_string)) {
                    hasEventListener_boolean = true;
                }
            }

            return hasEventListener_boolean;
        }

        /// <summary>
        /// Removes the event listener.
        /// </summary>
        /// <returns>
        /// The event listener.
        /// </returns>
        /// <param name='aIEventListener'>
        /// If set to <c>true</c> a I event listener.
        /// </param>
        /// <param name='aEventName_string'>
        /// If set to <c>true</c> a event name_string.
        /// </param>
        /// <param name='aEventDelegate'>
        /// If set to <c>true</c> a event delegate.
        /// </param>
        public bool RemoveEventListener(string aEventName_string, EventDelegate aEventDelegate) {
            //
            bool wasSuccessful_boolean = false;

            //
            if (hasEventListener(aEventName_string, aEventDelegate)) {
                //	OUTER
                string keyForOuterHashTable_string = _getKeyForOuterHashTable(aEventName_string);
                Hashtable inner_hashtable = _eventListenerDatas_hashtable[keyForOuterHashTable_string] as Hashtable;
                //
                object aIEventListener = _getArgumentsCallee(aEventDelegate);
                //  INNER
                string keyForInnerHashTable_string = 
                    _getKeyForInnerHashTable(new EventListenerData(aIEventListener, aEventName_string, aEventDelegate, EventDispatcherAddMode.DEFAULT));
                /*string KeyForInnerhash_string = EventListenerData.TryGetHashCode(aIEventListener, aEventName_string,
                    aEventDelegate, EventDispatcherAddMode.DEFAULT).ToString();*/
                inner_hashtable.Remove(keyForInnerHashTable_string);
                _removeObjectHashKey(aIEventListener, keyForInnerHashTable_string);
                wasSuccessful_boolean = true;
            }

            return wasSuccessful_boolean;
        }

        /// <summary>
        /// Removes all event listeners.
        /// </summary>
        /// <returns>
        /// The all event listeners.
        /// </returns>
        public bool RemoveAllEventListeners() {
            //
            bool wasSuccessful_boolean = false;

            //TODO, IS IT A MEMORY LEAK TO JUST RE-CREATE THE TABLE? ARE THE INNER HASHTABLES LEAKING?
            _eventListenerDatas_hashtable = new Hashtable();

            return wasSuccessful_boolean;
        }

        public void DispatchEvent(string EventName, object data) {
            Event ev = new Event(EventName, data);
            DispatchEvent(ev);
        }

        public void DispatchEvent(string EventName) {
            Event ev = new Event(EventName);
            DispatchEvent(ev);
        }
        

        /// <summary>
        /// Dispatchs the event.
        /// </summary>
        /// <returns>
        /// The event.
        /// </returns>
        /// <param name='aIEvent'>
        /// If set to <c>true</c> a I event.
        /// </param>
        public bool DispatchEvent(IEvent aIEvent) {
            //
            bool wasSuccessful_boolean = false;

            //
            _doAddTargetValueToIEvent(aIEvent);

            //	OUTER
            string keyForOuterHashTable_string = _getKeyForOuterHashTable(aIEvent.type);
            int dispatchedCount_int = 0;
            if (_eventListenerDatas_hashtable.ContainsKey(keyForOuterHashTable_string)) {
                //	INNER
                Hashtable inner_hashtable = _eventListenerDatas_hashtable[keyForOuterHashTable_string] as Hashtable;
                IEnumerator innerHashTable_ienumerator = inner_hashtable.GetEnumerator();
                DictionaryEntry dictionaryEntry;
                EventListenerData eventListenerData;
                ArrayList toBeRemoved_arraylist = new ArrayList();
                //
                while (innerHashTable_ienumerator.MoveNext()) {
                    dictionaryEntry = (DictionaryEntry) innerHashTable_ienumerator.Current;
                    eventListenerData = dictionaryEntry.Value as EventListenerData;

                    //***DO THE DISPATCH***
                    //Debug.Log ("DISPATCH : ");
                    //Debug.Log ("	n    : " + eventListenerData.eventName );
                    //Debug.Log ("	from : " + aIEvent.target );
                    //Debug.Log ("	to   : " + eventListenerData.eventListener );
                    //Debug.Log ("	del  : " + eventListenerData.eventDelegate + " " + (eventListenerData.eventDelegate as System.Delegate).Method.DeclaringType.Name + " " + (eventListenerData.eventDelegate as System.Delegate).Method.Name.ToString());
                    eventListenerData.eventDelegate(aIEvent);


                    //TODO - THIS IS PROBABLY FUNCTIONAL BUT NOT OPTIMIZED, MY APPROACH TO HOW/WHY SINGLE SHOTS ARE REMOVED
                    //REMOVE IF ONESHOT
                    if (eventListenerData.eventListeningMode == EventDispatcherAddMode.SINGLE_SHOT) {
                        toBeRemoved_arraylist.Add(eventListenerData);
                    }


                    //MARK SUCCESS, BUT ALSO CONTINUE LOOPING TOO
                    wasSuccessful_boolean = true;
                    dispatchedCount_int++;
                }


                //CLEANUP ANY ONE-SHOT, SINGLE-USE 
                EventListenerData toBeRemoved_eventlistenerdata;
                for (int count_int = toBeRemoved_arraylist.Count - 1; count_int >= 0; count_int--) {
                    toBeRemoved_eventlistenerdata = toBeRemoved_arraylist[count_int] as EventListenerData;
                    RemoveEventListener(toBeRemoved_eventlistenerdata.eventName, toBeRemoved_eventlistenerdata.eventDelegate);
                }
            }

            return wasSuccessful_boolean;
        }
        

        /// <summary>
        /// _dos the add target value to I event.
        /// </summary>
        /// <param name='aIEvent'>
        /// A I event.
        /// </param>
        /// <exception cref='System.NotImplementedException'>
        /// Is thrown when a requested operation is not implemented for a given type.
        /// </exception>
        public void _doAddTargetValueToIEvent(IEvent aIEvent) {
            aIEvent.target = _target_object;
        }

        public void OnApplicationQuit() {
            //TODO, DO THIS CLEANUP HERE, OR OBLIGATE API-USER TO DO IT??
            _eventListenerDatas_hashtable.Clear();
        }


        private string _getKeyForOuterHashTable(string aEventName_string) {
            //SIMPLY USING THE EVENT NAME - METHOD USED HERE, IN CASE I WANT TO TWEAK THIS MORE...
            return aEventName_string;
        }

        private string _getKeyForInnerHashTable(EventListenerData aEventListenerData) {
            //VERY UNIQUE - NICE!
            return aEventListenerData.eventListener.GetType().FullName + "_" + aEventListenerData.eventName + "_" +
                   (aEventListenerData.eventDelegate as System.Delegate).Method.Name.ToString() + aEventListenerData.GetHashCode();
        }

        public object _getArgumentsCallee(EventDelegate aEventDelegate) {
            return (aEventDelegate as System.Delegate).Target;
        }

        public void printAllEvents() {
            Debug.Log("printAllEvents");
            // 遍历Hashtable的所有键
            foreach (DictionaryEntry entry in _eventListenerDatas_hashtable) {
                // 获取键和值
                string key = (string) entry.Key;
                object value = entry.Value;
                foreach (DictionaryEntry table in (Hashtable) value) {
                    string childKey = (string) table.Key;
                    object chilValue = table.Value;
                    // 输出键和值
                    Debug.Log($"Key:  {childKey}");
                }
            }
        }

        private void _addObjectHashKey(object obj, string keyforEventObject) {
            if (!_eventObjectDatas_hashtable.ContainsKey(obj)) {
                _eventObjectDatas_hashtable.Add(obj, new List<string>());
            }

            //	INNER
            List<string> inner_hashtable = _eventObjectDatas_hashtable[obj] as List<string>;
            if (!inner_hashtable.Contains(keyforEventObject)) {
                //	ADD
                inner_hashtable.Add(keyforEventObject);
            }
        }

        private void _removeObjectHashKey(object obj, string keyforEventObject) {
            if (_eventObjectDatas_hashtable.ContainsKey(obj)) {
                List<string> inner_hashtable = _eventObjectDatas_hashtable[obj] as List<string>;
                if (!inner_hashtable.Contains(keyforEventObject)) {
                    //	ADD
                    inner_hashtable.Remove(keyforEventObject);
                }
            }
        }
    }
}