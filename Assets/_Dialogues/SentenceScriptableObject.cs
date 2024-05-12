using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Sentence", menuName = "Dialogues/Sentence", order = 0)]
[System.Serializable]
public class SentenceScriptableObject : CustomEventDispatcher<UnityAction>
{
    public EDialogueType dialogueType = EDialogueType.Normal;
    public string dialogue = "";
    public List<CustomEventDispatcher<UnityAction>> events;
}