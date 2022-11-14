using UnityEngine;

[CreateAssetMenu(fileName = "CsoundChannelData", menuName = "Csound/ChannelValue")]
public class CsoundChannelValueSO : ScriptableObject
{
    [System.Serializable]
    public struct CsoundChannelData
    {
        public string name;
        public float value;
    }

    public CsoundChannelData[] channelData;
}
