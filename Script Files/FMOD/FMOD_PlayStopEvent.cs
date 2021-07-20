using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_PlayStopEvent : MonoBehaviour
{
    [FMODUnity.EventRef][SerializeField] private string eventPath;
    [SerializeField] private bool is3D, stopImmediately, playOnAwake;

    public FMOD.Studio.EventInstance eventInstance;
    private FMOD.Studio.PLAYBACK_STATE state;

    private void Awake()
    {
        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        eventInstance.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            eventInstance.start();

            if (is3D)
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, GetComponent<Transform>(),
                    GetComponent<Rigidbody>());

            print("PLAY: " + eventInstance);
        }
    }

    public void Stop()
    {
        if(!stopImmediately)
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        eventInstance.release();

        print("STOP: " + eventInstance);
    }
}
