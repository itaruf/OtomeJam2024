using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Scriptable object that serves as an event dispatcher to handle and dispatch bool messages
[CreateAssetMenu(fileName = "Camera Shake", menuName = "Camera/Camera Shake", order = 0)]
public class CameraShakeEventDispatcher : CustomEventDispatcher<UnityAction>
{
   
}
