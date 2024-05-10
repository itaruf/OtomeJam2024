using UnityEngine;

// Scriptable object that serves as an event dispatcher to handle and dispatch string messages
[CreateAssetMenu(fileName = "String Event Dispatcher", menuName = "Generic Event Dispatcher/String Event Dispatcher", order = 0)]
public class StringEventDispatcher : CustomEventDispatcher<string>
{
}