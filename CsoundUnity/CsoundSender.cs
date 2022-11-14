using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides methods to send data in the CsoundUnityPreset and CsoundScoreEventSO assets through a CsoundUnity component.
/// </summary>
[RequireComponent(typeof(CsoundUnity))]
public class CsoundSender : MonoBehaviour
{
    [Tooltip("Reference to the CsoundUnity component to send values to. Will automatically get the component attached to the same object if left empty.")]
    public CsoundUnity csoundUnity;
    [Space]
    public CsoundSenderPresets InstrumentPresets;
    [Space]
    public CsoundSenderScoreEvents ScoreEvents;
    [Space]
    public CsoundSenderTrigger ChannelTrigger;
    [Space]
    public CsoundSenderChannelValues ChannelValues;
    [Space]
    public CsoundSenderRandomValues RandomChannelValues;

    #region UNITY LIFE CYCLE
    private void Awake()
    {
        //Gets the CsoundUnity component attached to the object if the inspector field is empty.
        if (csoundUnity == null)
        {
            csoundUnity = GetComponent<CsoundUnity>();

            if (csoundUnity == null)
                Debug.LogError("No CsoundUnity component attached to " + gameObject.name);
        }
    }

    void Start()
    {
        StartCoroutine(Initialization());
    }

    private IEnumerator Initialization()
    {
        while (!csoundUnity.IsInitialized)
        {
            Debug.Log("CSOUND NOT INITIALIZED");
            yield return null;
        }

        Debug.Log("CSOUND INITIALIZED");

        ////Calls SetPreset if setPresetOnStart is true.
        if (InstrumentPresets.setPresetOnStart)
            SetPreset(InstrumentPresets.presetIndexOnStart);

        //Call SendCoreEvent is scoreEventIndexOnStart istrue.
        if (ScoreEvents.sendScoreEventOnStart)
            SendScoreEvent(ScoreEvents.scoreEventIndexOnStart);

        //Set defined trigger channel to 1 if triggerOnStart is true.
        if (ChannelTrigger.triggerOnStart)
            SetTrigger(1);

        //Calls SetChannelsToRandomValue if setChannelRandomValuesOnStart is true.
        if (RandomChannelValues.setChannelRandomValuesOnStart)
            SetChannelsToRandomValue(RandomChannelValues.randomValueIndexOnStart);

        //Calls SetChannelValue if setChannelValueOnStart is true.
        if (ChannelValues.setChannelValueOnStart)
            SetChannelValue(ChannelValues.channelValueIndexOnStart);
    }
    #endregion

    //#region PRESETS
    /// <summary>
    /// Reset values for the currently indexed CsoundUnityPreset preset.
    /// </summary>
    /// <param name="index"></param>
    public void ResetPreset()
    {
        //Reset preset to the current preset list index.
        csoundUnity.SetPreset(InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex]);

        if (InstrumentPresets.debugPresets)
            Debug.Log("CSOUND " + gameObject.name + " set preset: " + InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex]);
    }

    /// <summary>
    /// Uses the indexed CsoundUnityPreset asset to set the instrument's preset.
    /// </summary>
    /// <param name="index"></param>
    public void SetPreset(int index)
    {
        //Set current preset index to the passed index.
        InstrumentPresets.presetCurrentIndex = index;
        //Set preset.
        csoundUnity.SetPreset(InstrumentPresets.presetList[index]);

        if (InstrumentPresets.debugPresets)
            Debug.Log("CSOUND " + gameObject.name + " set preset: " + InstrumentPresets.presetList[InstrumentPresets.presetCurrentIndex]);
    }

    /// <summary>
    /// Adds a CsoundUnityPreset asset to the preset list and sets it as a preset.
    /// </summary>
    /// <param name = "preset" ></ param >
    public void SetPreset(CsoundUnityPreset preset)
    {
        //Adds new item to the preset list as the last item.
        InstrumentPresets.presetList.Add(preset);
        //Calls SetPreset passing in the last item as the index.
        SetPreset(InstrumentPresets.presetList.Count - 1);
    }

    /// <summary>
	/// Sets a random preset from the inspector defined list.
	/// </summary>
    public void SetRandomPreset()
    {
        //Set preset index to a random number within the range of the lsit.
        InstrumentPresets.presetCurrentIndex = Random.Range(0, InstrumentPresets.presetList.Count);
        //Sets the random preset.
        SetPreset(InstrumentPresets.presetCurrentIndex);
    }

    /// <summary>
	/// Set the currently indexed preset and increments the index, cycling back to 0 if it reaches the end of the list.
	/// </summary>
    public void SetNextPreset()
    {
        //Increments preset index.
        InstrumentPresets.presetCurrentIndex++;
        //Restes index to 0 if it goes above the list count
        if (InstrumentPresets.presetCurrentIndex > InstrumentPresets.presetList.Count - 1)
        {
            InstrumentPresets.presetCurrentIndex = 0;
        }
        //Sets the currently indexed preset.
        ResetPreset();
    }

    /// <summary>
    /// Set the currently indexed preset and decreases the index, cycling back to the end of the list if it reaches 0.
    /// </summary>
    public void SetPreviousPreset()
    {
        //Decreases preset index.
        InstrumentPresets.presetCurrentIndex--;
        //Rests the index to the top of the list if it reaches 0.
        if (InstrumentPresets.presetCurrentIndex < 0)
        {
            InstrumentPresets.presetCurrentIndex = InstrumentPresets.presetList.Count - 1;
        }
        //Sets the currently indexed preset.
        ResetPreset();
    }

    // #endregion

    #region TRIGGER
    /// <summary>
    /// Toggles the defined trigger channel value between 0 and 1.
    /// </summary>
    public void ToggleTrigger()
    {
        //Toggles the value of the trigger chanel between 0 and 1.
        ChannelTrigger.triggerValue = 1 - ChannelTrigger.triggerValue;
        //Passes value to Csound.
        csoundUnity.SetChannel(ChannelTrigger.triggerChannelName, ChannelTrigger.triggerValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " trigger: " + ChannelTrigger.triggerValue);
    }

    /// <summary>
    /// Passes in a trigger channel and its current value to toggle it between 0 and 1.
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="value"></param>
    public void ToggleTrigger(string channelName, int value)
    {
        //Toggle the passed in value between 1 and 0.
        int toggledValue = 1 - value;
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, toggledValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " trigger: " + channelName + " , " + toggledValue);
    }

    /// <summary>
    /// Passes a value to the defined Csound trigger channel.
    /// </summary>
    public void SetTrigger(int value)
    {
        //Sets the trigger channel value to the value passed as an argument.
        ChannelTrigger.triggerValue = value;
        //Passes value to Csound.
        csoundUnity.SetChannel(ChannelTrigger.triggerChannelName, ChannelTrigger.triggerValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " csound trigger: " + ChannelTrigger.triggerValue);
    }

    /// <summary>
    /// Passes a value to the a Csound channel.
    /// </summary>
    public void SetTrigger(string channelName, int value)
    {
        //Sets the trigger channel value to the value passed as an argument.
        ChannelTrigger.triggerValue = value;
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, ChannelTrigger.triggerValue);

        if (ChannelTrigger.debugTrigger)
            Debug.Log("CSOUND " + gameObject.name + " csound trigger: " + ChannelTrigger.triggerValue);
    }
    #endregion

    #region SCORE EVENTS
    /// <summary>
    /// Uses the ScoreEvent asset currently indexed to send a score event.
    /// </summary>
    public void SendScoreEvent()
    {
        csoundUnity.SendScoreEvent(ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " +
                ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex] + " " + ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());
    }

    /// <summary>
    /// Uses the indexed ScoreEvent asset to send a score event.
    /// </summary>
    /// <param name="index"></param>
    public void SendScoreEvent(int index)
    {
        ScoreEvents.scoreEventCurrentIndex = index;
        csoundUnity.SendScoreEvent(ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " +
                ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex] + " " + ScoreEvents.scoreEventsList[ScoreEvents.scoreEventCurrentIndex].ConcatenateScoreEventString());
    }

    /// <summary>
    /// Adds a ScoreEvent asset to the list and sends it as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEvent(CsoundScoreEventSO scoreEvent)
    {
        csoundUnity.SendScoreEvent(scoreEvent.ConcatenateScoreEventString());

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " + scoreEvent + " " + scoreEvent.ConcatenateScoreEventString());
    }

    /// <summary>
    /// Adds a ScoreEvent asset to the list and sends it as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEventAndAddToList(CsoundScoreEventSO scoreEvent)
    {
        //Adds new ScoreEvent asset to the list as the last item.
        ScoreEvents.scoreEventsList.Add(scoreEvent);
        //Calls SendScoreEvent passing in the last item as the index.
        SendScoreEvent(ScoreEvents.scoreEventsList.Count - 1);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND " + gameObject.name + " score event: " + scoreEvent + " " + scoreEvent.ConcatenateScoreEventString());
    }

    /// <summary>
    /// Passes in a string to be sent as a score event.
    /// </summary>
    /// <param name="scoreEvent"></param>
    public void SendScoreEvent(string scoreEvent)
    {
        csoundUnity.SendScoreEvent(scoreEvent);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scoreEvent);
    }

    /// <summary>
    /// Passes in separate values for each p field to be sent as a score event.
    /// </summary>
    /// <param name="scorechar"></param>
    /// <param name="instrument"></param>
    /// <param name="delay"></param>
    /// <param name="duration"></param>
    public void SendScoreEvent(string scorechar, string instrument, float delay, float duration)
    {
        csoundUnity.SendScoreEvent(scorechar + " " + instrument + " " + delay + " " + duration);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scorechar + " " + instrument + " " + delay + " " + duration);
    }

    /// <summary>
    /// Passes in separate values for each p field to be sent as a score event.
    /// </summary>
    public void SendScoreEvent(string scorechar, string instrument, float delay, float duration, float[] extraPFields)
    {
        string concatenatedPFields = string.Join(" ", extraPFields);
        csoundUnity.SendScoreEvent(scorechar + " " + instrument + " " + delay + " " + duration + " " + concatenatedPFields);

        if (ScoreEvents.debugScoreEvents)
            Debug.Log("CSOUND" + gameObject.name + " score event: " + scorechar + " " + instrument + " " + delay + " " + duration + " " + concatenatedPFields);

    }

    /// <summary>
	/// Sends the currently indexed score event and increments the index, cycling back to the beginning of the list when it reaches the end.
	/// </summary>
    public void SendNextScoreEvent()
    {
        //Increments the index.
        ScoreEvents.scoreEventCurrentIndex++;
        //Resets index to 0 if it goes above the list count.
        if (ScoreEvents.scoreEventCurrentIndex > ScoreEvents.scoreEventsList.Count - 1)
        {
            ScoreEvents.scoreEventCurrentIndex = 0;
        }
        //Sends currently indexed score event.
        SendScoreEvent();
    }

    /// <summary>
    /// Sends the currently indexed score event and decreases the index, cycling back to the end of the list when it reaches 0.
    /// </summary>
    public void SendPreviousScoreEvent()
    {
        //Decreases the index.
        ScoreEvents.scoreEventCurrentIndex--;
        //Rests the index to the top of the list if it reaches 0.
        if (ScoreEvents.scoreEventCurrentIndex < 0)
        {
            ScoreEvents.scoreEventCurrentIndex = ScoreEvents.scoreEventsList.Count - 1;
        }
        //Sends currently indexed score event.
        SendScoreEvent();
    }

    /// <summary>
	/// Sends a random score event from the list.
	/// </summary>
    public void SendRandomScoreEvent()
    {
        //Generates a random score event index.
        ScoreEvents.scoreEventCurrentIndex = Random.Range(0, ScoreEvents.scoreEventsList.Count - 1);
        //Sends the random score event.
        SendScoreEvent();
    }

    #endregion

    #region SET CHANNELS TO RANDOM VALUE
    /// <summary>
    /// Uses the currently indexed ChannelData asset in the randomValueChannels array to randomize values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue()
    {
        for (int i = 0; i < RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData.Length; i++)
        {
            //Get the random value from the scriptable object and passes it to Csound.
            csoundUnity.SetChannel(RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData[i].name,
                RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
        }

    }

    /// <summary>
    /// Changes the indexed ChannelData asset in the randomValueChannels array and randomizes values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue(int index)
    {
        RandomChannelValues.randomValueCurrentIndex = index;

        for (int i = 0; i < RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData.Length; i++)
        {
            //Get the random value from the scriptable object and passes it to Csound.
            csoundUnity.SetChannel(RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].channelData[i].name,
                RandomChannelValues.randomValueChannelsList[RandomChannelValues.randomValueCurrentIndex].GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
        }
    }

    /// <summary>
    /// Passses in a ChannelData asset to randomize channel values between the defined minValue and maxValue.
    /// </summary>
    public void SetChannelsToRandomValue(CsoundChannelRangeSO newChannelRange)
    {
        for (int i = 0; i < newChannelRange.channelData.Length; i++)
        {
            //Get the random value from the scriptable object and passes it to Csound.
            csoundUnity.SetChannel(newChannelRange.channelData[i].name, newChannelRange.GetRandomValue(i, RandomChannelValues.debugRandomChannelsValues));
        }
    }

    /// <summary>
    /// Generates a random value between a minimum and maximum range and assigns that to a Csound channel.
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    public void SetChannelsToRandomValue(string channelName, float minValue, float maxValue)
    {
        //Generates random value.
        float randomValue = Random.Range(minValue, maxValue);
        //Passes value to Csound.
        csoundUnity.SetChannel(channelName, randomValue);

        if (RandomChannelValues.debugRandomChannelsValues)
            Debug.Log("CSOUND " + gameObject.name + " channel random value: " + channelName + " , " + randomValue);
    }

    /// <summary>
    /// Passes an array of string as channel names and generates an unique rnadom value for each channel within the same min and max range.
    /// </summary>
    /// <param name="channelNames"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    public void SetChannelsToRandomValue(string[] channelNames, float minValue, float maxValue)
    {
        //Passes value to Csound and generates a random value for each individual channel.
        foreach (string name in channelNames)
        {
            float randomValue = Random.Range(minValue, maxValue);
            csoundUnity.SetChannel(name, randomValue);

            if (RandomChannelValues.debugRandomChannelsValues)
                Debug.Log("CSOUND " + gameObject.name + " channel random value: " + name + " , " + randomValue);
        }
    }
    #endregion

    #region SET CHANNEL VALUES
    /// <summary>
	/// Sends the currently indexed Channel Values to Csound.
	/// </summary>
    public void SetChannelValue()
    {
        foreach (CsoundChannelValueSO.CsoundChannelData data in ChannelValues.setChannelValuesList[ChannelValues.channelValueCurrentIndex].channelData)
        {
            csoundUnity.SetChannel(data.name, data.value);
        }

        if (ChannelValues.debugChannelsValues)
            Debug.Log("CSOUND " + gameObject.name + " channel values: " + ChannelValues.setChannelValuesList[ChannelValues.channelValueCurrentIndex]);
    }

    /// <summary>
	/// Sets channel values according to the passed in index.
	/// </summary>
	/// <param name="index"></param>
    public void SetChannelValue(int index)
    {
        ChannelValues.channelValueCurrentIndex = index;

        foreach (CsoundChannelValueSO.CsoundChannelData data in ChannelValues.setChannelValuesList[ChannelValues.channelValueCurrentIndex].channelData)
        {
            csoundUnity.SetChannel(data.name, data.value);
        }

        if (ChannelValues.debugChannelsValues)
            Debug.Log("CSOUND " + gameObject.name + " channel values: " + ChannelValues.setChannelValuesList[ChannelValues.channelValueCurrentIndex]);
    }

    /// <summary>
	/// Sends the passsed in channel values to Csound.
	/// </summary>
	/// <param name="channelValue"></param>
    public void SetChannelValue(CsoundChannelValueSO channelValue)
    {
        foreach (CsoundChannelValueSO.CsoundChannelData data in channelValue.channelData)
        {
            csoundUnity.SetChannel(data.name, data.value);
        }

        if (ChannelValues.debugChannelsValues)
            Debug.Log("CSOUND " + gameObject.name + " channel values: " + channelValue);
    }

    /// <summary>
    /// Adds the Channel Value asset ot the lsit and sets its values.
    /// </summary>
    public void SetChannelValueAndAddToList(CsoundChannelValueSO channelValue)
    {
        //Adds new item to the channel valuelist as the last item.
        ChannelValues.setChannelValuesList.Add(channelValue);
        //Calls SetPreset passing in the last item as the index.
        SetChannelValue(ChannelValues.setChannelValuesList.Count - 1);
    }

    /// <summary>
	/// Sets the currently index channel value and increments the index, cycling back to the beginning if it reaches the end of the list.
	/// </summary>
    public void SetNextChannelValue()
    {
        //Increments the index.
        ChannelValues.channelValueCurrentIndex++;
        //Resets index to 0 if it reaches the end of the list.
        if (ChannelValues.channelValueCurrentIndex > ChannelValues.setChannelValuesList.Count - 1)
        {
            ChannelValues.channelValueCurrentIndex = 0;
        }
        //Sets currently indexed channel value.
        SetChannelValue();
    }

    /// <summary>
    /// Sets the currently index channel value and decreases the index the index, cycling back to the end of the list if it reaches 0.
    /// </summary>
    public void SetPreviousChannelValue()
    {
        //Decreases the index.
        ChannelValues.channelValueCurrentIndex--;
        //Resets index to the end of the list if it reaches 0.
        if (ChannelValues.channelValueCurrentIndex < 0)
        {
            ChannelValues.channelValueCurrentIndex = ChannelValues.setChannelValuesList.Count - 1;
        }
        //Sets currently indexed channel value.
        SetChannelValue();
    }
    #endregion
}

[System.Serializable]
public class CsoundSenderPresets
{
    //[Tooltip("Array containing ChannelData asssets to be used as instrument presets")]
    public List<CsoundUnityPreset> presetList = new List<CsoundUnityPreset>();
    [Tooltip("Defined which preset to be set on start")]
    public int presetIndexOnStart;
    [Tooltip("If true, sets the defined preset value on start")]
    public bool setPresetOnStart;
    [Tooltip("Prints preset name changing presets.")]
    public bool debugPresets;

    [HideInInspector] public int presetCurrentIndex = 0;
}

[System.Serializable]
public class CsoundSenderScoreEvents
{
    [Tooltip("Array containing ScoreEvent asssets")]
    public List<CsoundScoreEventSO> scoreEventsList = new List<CsoundScoreEventSO>();
    [Tooltip("Defined which score event to send on start")]
    public bool sendScoreEventOnStart;
    [Tooltip("If true, sends the defined score event on start")]
    public int scoreEventIndexOnStart;
    [Tooltip("Prints score events.")]
    public bool debugScoreEvents;

    [HideInInspector] public int scoreEventCurrentIndex = 0;
}

[System.Serializable]
public class CsoundSenderTrigger
{
    [Tooltip("Defines the channel name that is used to start and stop the Csound instrument")]
    public string triggerChannelName;
    [Tooltip("If true, sets the trigger channel to a value of 1")]
    public bool triggerOnStart = false;
    [Tooltip("Prints trigger channel values.")]
    public bool debugTrigger;

    [HideInInspector] public int triggerValue = 0;
}

[System.Serializable]
public class CsoundSenderChannelValues
{
    [Tooltip("List of channels and values to set them to")]
    public List<CsoundChannelValueSO> setChannelValuesList = new List<CsoundChannelValueSO>();
    public int channelValueIndexOnStart;
    [Tooltip("If true, sets channel values on start.")]
    public bool setChannelValueOnStart = false;
    [Tooltip("Prints channel names and value when setting them.")]
    public bool debugChannelsValues;

    [HideInInspector] public int channelValueCurrentIndex = 0;
}

[System.Serializable]
public class CsoundSenderRandomValues
{
    [Tooltip("List of ChannelRange assets to be used to randomize channel values.")]
    public List<CsoundChannelRangeSO> randomValueChannelsList = new List<CsoundChannelRangeSO>();
    public int randomValueIndexOnStart;
    [Tooltip("If true, sets channels to random values on start.")]
    public bool setChannelRandomValuesOnStart = false;
    [Tooltip("Prints channel names and values when randomizing values.")]
    public bool debugRandomChannelsValues;

    [HideInInspector] public int randomValueCurrentIndex = 0;
}