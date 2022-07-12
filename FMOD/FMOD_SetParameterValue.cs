using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FMOD_SetParameterValue : MonoBehaviour
{
    [SerializeField] private string parameterName;
    public bool isLocal;
    [HideInInspector] public FMOD_PlayStopEvent instance;

    public void SetValue(float value, bool ignoreSeekSpeed)
    {
        if (!isLocal)
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, value, ignoreSeekSpeed);
        else
            instance.eventInstance.setParameterByName(parameterName, value, ignoreSeekSpeed);

        print("PARAMETER: " + parameterName + " VALUE: " + value);
    }

    public void SetValue(float value)
    {
        if (!isLocal)
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
        else
            instance.eventInstance.setParameterByName(parameterName, value);

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


#if UNITY_EDITOR
[CustomEditor(typeof(FMOD_SetParameterValue))]
public class FMOD_SetParameterValue_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        FMOD_SetParameterValue script = (FMOD_SetParameterValue)target;

        if (script.isLocal) // if bool is true, show other fields
        {
            script.instance = EditorGUILayout.ObjectField("Event Instance", script.instance, typeof(FMOD_PlayStopEvent), true) as FMOD_PlayStopEvent;
        }
    }
}
#endif

