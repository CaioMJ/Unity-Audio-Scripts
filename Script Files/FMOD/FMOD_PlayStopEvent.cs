using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FMOD_PlayStopEvent : MonoBehaviour
{
    [FMODUnity.EventRef][SerializeField] private string eventPath;
    [SerializeField] private bool playOnAwake, stopImmediately;
    public bool is3D;
    [HideInInspector] public GameObject objectToAttach;

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
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach.transform,
                    objectToAttach.GetComponent<Rigidbody>());

            print("PLAY: " + eventPath);
        }
    }

    public void Stop()
    {
        if(!stopImmediately)
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        eventInstance.release();

        print("STOP: " + eventPath);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FMOD_PlayStopEvent))]
public class FMOD_PlayStopEvent_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        FMOD_PlayStopEvent script = (FMOD_PlayStopEvent)target;

        if (script.is3D) // if bool is true, show other fields
        {
            script.objectToAttach = EditorGUILayout.ObjectField("Object To Attach", script.objectToAttach, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif

