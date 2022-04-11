using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePlayEvent : MonoBehaviour
{
    public AK.Wwise.Event eventToPlay;
        
    public void Play()
    {
        if (eventToPlay != null)
            eventToPlay.Post(gameObject);
    }

    public void Play(string eventOverride)
    {
        AkSoundEngine.PostEvent(eventOverride, gameObject);
    }

    public void PlayAnimationEvent(string animationEvent)
    {
        AkSoundEngine.PostEvent(animationEvent, gameObject);
    }
}
