using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script concatenates audio clips from an array in a sample accurate manner. The audio clip array can be played both randomly or sequentially.
public class ConcatenateAudioClips : MonoBehaviour
{
    private enum ConcatenationInterval { UpdateDynamically, FixedAtIndex0, SetOnInspector};

    [Header("AUDIO REFERENCES")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource[] audioSource; //Array with 2 audio sources
    [Space]
    [Header("AUDIO SOURCE PROPERTIES")]
    [Range(0f, 3f)] [SerializeField] private float minPitch;
    [Range(0f, 3f)] [SerializeField] private float maxPitch;
    [Range(0f, 1f)] [SerializeField] private float volume;
    [Header("CONCATENATION PROPERTIES")]
    [SerializeField] private bool playClipsRandomly;
    [SerializeField] private ConcatenationInterval concatenationInterval;
    [SerializeField] private double intervalOnInspector;
    [SerializeField] private bool playOnStart;
    
    private bool canGetDspTime, canPlay;
    private int index, lastIndex;
    private int toggle;
    
    private double interval, nextStartTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            Play();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            if (AudioSettings.dspTime > nextStartTime - 0.1f)
                ConcatenateClips();
        }
        
        //TEST INPUT
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canPlay)
                Stop();
            else
                Play();
        }
    }
    
    private void Initialize()
    {
        foreach (AudioSource source in audioSource)
            source.volume = volume;
        
        if (playClipsRandomly)
            index = UnityEngine.Random.Range(0, audioClips.Length);
        else
            index = 0;

        if (concatenationInterval == ConcatenationInterval.UpdateDynamically)
            interval = (double)audioClips[index].samples / audioClips[index].frequency;
        else if (concatenationInterval == ConcatenationInterval.FixedAtIndex0)
            interval = (double)audioClips[0].samples / audioClips[0].frequency;
        else if (concatenationInterval == ConcatenationInterval.SetOnInspector)
            interval = intervalOnInspector;
    }
    
    public void Play()
    {
        Initialize();
        nextStartTime = AudioSettings.dspTime + 0.1f;
        canPlay = true;
    }
    
    public void Stop()
    {
        canPlay = false;
    }
    
    private void ConcatenateClips()
    {
        ScheduleAudioSource();
        UpdateNextStartTime();
        GenerateNewIndex();
    }
    
    private void ScheduleAudioSource()
    {
        toggle = 1 - toggle;
        audioSource[toggle].clip = audioClips[index];
        audioSource[toggle].pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        audioSource[toggle].PlayScheduled(nextStartTime);
        print(audioClips[index]);
    }
    
    private void UpdateNextStartTime()
    {
        if (concatenationInterval == ConcatenationInterval.UpdateDynamically)
            GetNewClipLength();

        nextStartTime += interval;
    }
    
    private void GenerateNewIndex()
    {
        if (playClipsRandomly)
            RandomIndex();
        else
            SequentialIndex();
    }
    
    private void RandomIndex()
    {
        while (index == lastIndex)
        {
            index = UnityEngine.Random.Range(0, audioClips.Length);
        }
        lastIndex = index;
    }
    
    private void SequentialIndex()
    {
        if (index != audioClips.Length - 1)
            index++;
        else
            index = 0;
    }
    
    private void GetNewClipLength()
    {
        interval = (double)audioClips[index].samples / audioClips[index].frequency;
    }
}
