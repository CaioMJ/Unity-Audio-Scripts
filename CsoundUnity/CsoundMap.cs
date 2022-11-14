using UnityEngine;

public static class CsoundMap
{
    //Scales a float between a minimum and a maximum value.
    public static float ScaleFloat(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    //Scales the ChannelRange minValue and maxValue and passes it into Csound, mapping them to a defined range.
    public static void MapValueToChannelRange(CsoundChannelRangeSO csoundChannels, float minRange, float maxRange, float incomingData, CsoundUnity csoundUnity)
    {
        //Cycles through every channel defined in the ChannelRange asset.
        foreach (CsoundChannelRangeSO.CsoundChannelData data in csoundChannels.channelData)
        {
            //Scales the defined minValue and maxValue variables to a range.
            float value =
                Mathf.Clamp(ScaleFloat(minRange, maxRange, data.minValue, data.maxValue, incomingData), data.minValue, data.maxValue);

            //Passes the vlaue to Csound
            if (!data.returnAbsoluteValue)
            {
                csoundUnity.SetChannel(data.name, value);
            }
            //Passes the absolute value to Csound if the bool is checked for the channel.
            else
            {
                csoundUnity.SetChannel(data.name, Mathf.Abs(value));
            }
        }
    }

}
