using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private static StateManager _instance;

    // Keep track of the current state
    private EState _currentState;

    // State Events
    public event StateEvents.OnStateChange onStateChanged;
    // Dialogue State Events
    public event StateEvents.OnDialogueStateEnter onDialogueEnter;
    public event StateEvents.OnDialogueStateExit onDialogueExit;
    // Backlog State Events
    public event StateEvents.OnBacklogStateEnter onBacklogEnter;
    public event StateEvents.OnBacklogStateExit onBacklogExit;

    // State Event Dispatchers
    public StateEventDispatcher EDI_onStateChange;
    public StateEventDispatcher EDI_onStateEnter;
    public StateEventDispatcher EDI_onStateExit;

    public StateEventDispatcher EDI_onDialogueEnter;
    public StateEventDispatcher EDI_onDialogueExit;

    public StateEventDispatcher EDI_onBacklogEnter;
    public StateEventDispatcher EDI_onBacklogExit;

    // Property accessors
    public static StateManager Instance { get => _instance; set => _instance = value; }
    public EState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        EDI_onStateChange.AddListener(UpdateState);
    }

    private void UpdateState(EState state)
    {
        _currentState = state;
    }

    public void OnDialogueEnter()
    {
        EDI_onStateChange.TriggerEvent(EState.Dialogue);
        EDI_onDialogueEnter.TriggerEvent(EState.Dialogue);
        onDialogueEnter?.Invoke();
    }

    public void OnDialogueExit()
    {
        EDI_onStateExit.TriggerEvent(EState.Dialogue);
        EDI_onDialogueExit.TriggerEvent(EState.Dialogue);
        onDialogueExit?.Invoke();
    }

    public void OnBacklogEnter()
    {
        EDI_onStateChange.TriggerEvent(EState.Backlog);
        EDI_onBacklogEnter.TriggerEvent(EState.Backlog);
        onBacklogEnter?.Invoke();
    }

    public void OnBacklogExit()
    {
        EDI_onStateExit.TriggerEvent(EState.Backlog);
        EDI_onBacklogExit.TriggerEvent(EState.Backlog);
        onBacklogExit?.Invoke();
    }
}