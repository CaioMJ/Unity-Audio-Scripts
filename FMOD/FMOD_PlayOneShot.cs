using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_PlayOneShot : MonoBehaviour
{
    [FMODUnity.EventRef] public string eventPath;
    [SerializeField] private bool is3D;

    public void Play()
    {
        if (!is3D)
            FMODUnity.RuntimeManager.PlayOneShot(eventPath);
        else
            FMODUnity.RuntimeManager.PlayOneShotAttached(eventPath, gameObject);

        print("PLAY : " + eventPath);
    }
}
