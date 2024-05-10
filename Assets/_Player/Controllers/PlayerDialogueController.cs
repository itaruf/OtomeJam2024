using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDialogueController : PlayerController 
{
    // Input Action Map that contains all the Input Actions related to the dialogue state
    private InputActionMap _IAM_Dialogue;

    // Move onto the next dialogue
    private InputAction _IA_Next;
    // Open the backlog
    private InputAction _IA_OpenBacklog;
    // Enables or Disables the Auto dialogue mode
    private InputAction _IA_Auto;

    protected override void HandleController(EState state)
    {
        if (state == EState.Dialogue)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _IAM_Dialogue = _inputActionAsset.actionMaps[0];

        // Store the corresponding input action
        _IA_Next = _IAM_Dialogue.actions[0];
        _IA_OpenBacklog = _IAM_Dialogue.actions[1];
        _IA_Auto = _IAM_Dialogue.actions[2];

        // Bind actions to methods or events
        _IA_Next.performed += Next;
        _IA_OpenBacklog.performed += OpenBacklog;
        _IA_Auto.performed += Auto;

        // Enable all input actions
        _IA_Next.Enable();
        _IA_OpenBacklog.Enable();
        _IA_Auto.Enable();
    }

    private void Next(InputAction.CallbackContext context)
    {
        if (_isActive)
        {
            Debug.Log("Next Dialogue");
        }
    }

    private void OpenBacklog(InputAction.CallbackContext context)
    {
        if (_isActive)
        {
            StateManager.Instance.OnBacklogEnter();
            Debug.Log("Open Backlog");
        }
    }

    private void Auto(InputAction.CallbackContext context)
    {
        if (_isActive)
        {
            Debug.Log("Auto Dialogue");
        }
    }
}