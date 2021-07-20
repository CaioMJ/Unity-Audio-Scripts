using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Markov chain probabilities")]
[System.Serializable]
public class TwoDimensionalFloatArray : ScriptableObject
{
    [System.Serializable]
    public struct rowData
    {
        public float[] output;
    }
    public rowData[] input = new rowData[10];
}
