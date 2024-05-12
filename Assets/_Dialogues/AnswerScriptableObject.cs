using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Answer", menuName = "Dialogues/Answer", order = 0)]
[System.Serializable]
public class AnswerScriptableObject : CustomEventDispatcher<UnityAction>
{
    public EDialogueType dialogueType = EDialogueType.MC_Self;
    public string dialogue;
    public List<CustomEventDispatcher<UnityAction>> events;
}