using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThGold.Event;
using System;

namespace ThGold.Event
{
    public class EventTest : MonoBehaviour
    {
        void Awake()
        {
            ThGold.Event.EventHandler.Instance.EventDispatcher.AddEventListener(CustomEvent.Buy, buy);
        }

        private void DropItemEvent(int num)
        {
            Debug.Log("You Drop" + num + "Item");
        }

        private void OnDestroy()
        {
            ThGold.Event.EventHandler.Instance.EventDispatcher.RemoveEventListener(CustomEvent.Buy, buy);
        }
        // Update is called once per frame
        void buy(IEvent aIEvent)
        {
            AppleData a = (AppleData)aIEvent.data;
            Debug.Log("appleName:" + a.name + ";Num:" + a.number + ";Price" + a.price);
        }
        void GetApple(IEvent aIEvent)
        {
            Debug.Log("get" + 3 + "apple");
        }
        public void click()
        {
            AppleData a = new AppleData("�츻ʿ", 2, 10);
            object obj = a;     
            ThGold.Event.EventHandler.Instance.EventDispatcher.DispatchEvent(CustomEvent.Buy, obj);
        }
        public void click2()
        {
        }

    }

    //  public class SampleEvent : Event
    //  {
    //      // GETTER / SETTER
    //      /// <summary>
    //      /// An example of event-specific data you can add in.
    //      /// </summary>
    //      private string _customValue_string;
    //      public string customValue
    //      {
    //          get
    //          {
    //              return _customValue_string;
    //          }
    //          set
    //          {
    //              _customValue_string = value;
    //          }
    //      }

    //      /// <summary>
    //      /// The Event Type Name
    //      /// </summary>
    //      public static string SAMPLE_EVENT = "SAMPLE_EVENT";
    //      /// <summary>
    //      /// Initializes a new instance of the <see cref="com.rmc.projects.event_dispatcher.SampleEvent"/> class.
    //      /// </summary>
    //      /// <param name="aType_str">A type_str.</param>
    //public SampleEvent(string aType_str) : base(aType_str)
    //      {

    //      }
    //      public SampleEvent(string aType_str,object obj) : base(aType_str)
    //      {

    //      }
    // }
}
