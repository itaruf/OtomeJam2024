using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : MonoBehaviour
{
    // Fields
    [SerializeField] protected InputActionAsset _inputActionAsset;

    protected bool _isActive = false;

    virtual protected void Awake()
    {
        if (StateManager.Instance != null)
        {
            StateManager.Instance.EDI_onStateChange.AddListener(HandleController);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StateManager.Instance.EDI_onStateChange.TriggerEvent(EState.Dialogue);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StateManager.Instance.EDI_onStateChange.TriggerEvent(EState.Backlog);
        }
    }

    abstract protected void HandleController(EState state);
}