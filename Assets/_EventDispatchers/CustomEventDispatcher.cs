using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Custom generic event dispatcher to create child from
public abstract class CustomEventDispatcher<T> : ScriptableObject
{
    protected List<UnityAction<T>> listeners = new();

    public void TriggerEvent(T value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].Invoke(value);
    }

    public void AddListener(UnityAction<T> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(UnityAction<T> listener)
    {
        listeners.Remove(listener);
    }
}