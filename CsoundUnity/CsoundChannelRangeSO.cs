using UnityEngine;

[CreateAssetMenu(fileName = "CsoundChannelData", menuName = "Csound/ChannelRange")]
public class CsoundChannelRangeSO : ScriptableObject
{
    [System.Serializable]
    public struct CsoundChannelData
    {
        public string name;
        public float minValue, maxValue;
        public bool returnAbsoluteValue;
    }

    public CsoundChannelData[] channelData;

    public float GetRandomValue(int index, bool debug)
    {
        float randomValue = Random.Range(channelData[index].minValue, channelData[index].maxValue);

        if (debug)
            Debug.Log("CSOUND set random value: " + channelData[index].name + " set RANDOM value " + randomValue);

        return randomValue;
    }
}


