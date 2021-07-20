using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_SetParameterValue : MonoBehaviour
{
    [SerializeField] private string parameterName;
    [SerializeField] private bool isLocal;
    [SerializeField] private FMOD_PlayStopEvent instance;

    public void SetValue(float value, bool ignoreSeekSpeed)
    {
        if (!isLocal)
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, value, ignoreSeekSpeed);
        else
            instance.eventInstance.setParameterByName(parameterName, value, ignoreSeekSpeed);

        print("PARAMETER: " + parameterName + " VALUE: " + value);
    }

    public void SetValue(int value)
    {
        if (!isLocal)
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
        else
            instance.eventInstance.setParameterByName(parameterName, value);

        print("PARAMETER: " + parameterName + " VALUE: " + value);
    }
}
