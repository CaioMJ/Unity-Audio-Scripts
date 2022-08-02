using UnityEngine;

public static class FMODUtility
{
    public static void CreateEventInstanceAndPlay(FMODUnity.EventReference eventPath)
    {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        eventInstance.start();
        eventInstance.release();
    }

    public static void CreateEventInstanceAndPlay(FMODUnity.EventReference eventPath, string parameterName, float parameterValue)
    {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        eventInstance.setParameterByName(parameterName, parameterValue);
        eventInstance.start();
        eventInstance.release();
    }

    public static void CreateEventInstanceAndPlay(FMODUnity.EventReference eventPath, string parameterName, int parameterValue)
    {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        eventInstance.setParameterByName(parameterName, parameterValue);
        eventInstance.start();
        eventInstance.release();
    }

    public static void CreateInstanceAndPlayAttached(FMODUnity.EventReference eventPath, GameObject gObject)
    {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
        eventInstance.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, gObject.transform);
        eventInstance.release();
    }

    public static void PlayOneShot(FMODUnity.EventReference eventPath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath);
    }

    public static void PlayOneShotAttached(FMODUnity.EventReference eventPath, GameObject gObject)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventPath, gObject);
    }
}

