using UnityEngine;

// Scriptable object that serves as an event dispatcher to handle and dispatch bool messages
[CreateAssetMenu(fileName = "Bool Event Dispatcher", menuName = "Generic Event Dispatcher/Bool Event Dispatcher", order = 0)]
public class BoolEventDispatcher : CustomEventDispatcher<bool>
{
}