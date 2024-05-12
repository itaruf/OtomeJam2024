using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Scriptable object that serves as an event dispatcher to handle and dispatch bool messages
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogues/Dialogue", order = 0)]
public abstract class DialogueScriptableObject : CustomEventDispatcher<UnityAction>
{
    public string characterName;
    public string dialogue;
    public Sprite characterSprite;
    public List<CustomEventDispatcher<UnityAction>> events;
}