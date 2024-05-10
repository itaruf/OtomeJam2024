using UnityEngine;

// Scriptable object that serves as an event dispatcher to handle and dispatch bool messages
[CreateAssetMenu(fileName = "State Event Dispatcher", menuName = "State Event Dispatcher/State Event Dispatcher", order = 0)]
public class StateEventDispatcher : CustomEventDispatcher<EState>
{
}