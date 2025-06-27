using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThGold.Event;
public class EventPlayer : MonoBehaviour
{
    public EventDispatcher EventDispatcher;
    public EventPlayer()
    {
        EventDispatcher = new EventDispatcher(this);
    }
}