using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string>
{

}

public class GunAnimationEvents : MonoBehaviour
{
    public AnimationEvent GunAnimationEvent = new AnimationEvent();

    public void OnAnimationEvent(string eventName)
    {
        GunAnimationEvent.Invoke(eventName);
    }
}
