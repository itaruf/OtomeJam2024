using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSettingsManager : MonoBehaviour
{
    private GameSettingsManager _instance;

    public GameSettingsManager Instance { get => _instance; set => _instance = value; }

    [SerializeField] private CameraShakeEventDispatcher _EDI_CameraShake;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        if (_EDI_CameraShake != null)
        {
            _EDI_CameraShake.AddListener(Shake);
        }
    }

    private void Shake(UnityAction unityAction)
    {
        Debug.Log("Shake");
    }
}