using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Scriptable object that serves as an event dispatcher to handle and dispatch bool messages
[CreateAssetMenu(fileName = "Answer", menuName = "Dialogues/Answer", order = 0)]
public class AnswerScriptableObject : CustomEventDispatcher<UnityAction>
{
    public string dialogue;
    public List<CustomEventDispatcher<UnityAction>> triggerEvent;
}