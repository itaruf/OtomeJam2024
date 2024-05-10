public static class StateEvents
{
    public delegate void OnStateChange(EState state);

    public delegate void OnDialogueStateEnter();
    public delegate void OnDialogueStateExit();

    public delegate void OnBacklogStateEnter();
    public delegate void OnBacklogStateExit();
}