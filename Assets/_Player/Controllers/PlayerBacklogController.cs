using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBacklogController : PlayerController
{
    // Input Action Map that contains all the Input Actions related to the backlog state
    private InputActionMap _IAM_Backlog;

    // Close the backlog
    private InputAction _IA_CloseBacklog;

    protected override void HandleController(EState state)
    {
        if (state == EState.Backlog)
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

        _IAM_Backlog = _inputActionAsset.actionMaps[1];

        // Store the corresponding input action
        _IA_CloseBacklog = _IAM_Backlog.actions[0];

        // Bind actions to methods/events
        _IA_CloseBacklog.performed += CloseBacklog;

        // Enable all input actions
        _IA_CloseBacklog.Enable();
    }

    private void CloseBacklog(InputAction.CallbackContext context)
    {
        if (_isActive)
        {
            StateManager.Instance.OnBacklogExit();
            Debug.Log("Close Backlog");
        }
    }
}