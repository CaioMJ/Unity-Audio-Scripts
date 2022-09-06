using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseRTPC : MonoBehaviour
{
    [SerializeField] AK.Wwise.RTPC rtpc;

    public void SetGlobalValue(float value)
    {
        rtpc.SetGlobalValue(value);
    }

    public void SetLocalValue(float value)
    {
        rtpc.SetValue(gameObject, value);
    }
}
