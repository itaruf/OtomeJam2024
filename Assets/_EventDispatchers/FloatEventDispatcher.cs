using UnityEngine;

// Scriptable object that serves as an event dispatcher to handle and dispatch float messages
[CreateAssetMenu(fileName = "Float Event Dispatcher", menuName = "Generic Event Dispatcher/Float Event Dispatcher", order = 0)]
public class FloatEventDispatcher : CustomEventDispatcher<float>
{
}