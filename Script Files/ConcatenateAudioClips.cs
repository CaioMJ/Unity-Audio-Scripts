using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script concatenates audio clips from an array in a sample accurate manner. The audio clip array can be played both randomly or sequentially.
public class ConcatenateAudioClips : MonoBehaviour
{
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
    [SerializeField] private bool getLengthDynamically; //If false, the interval between concatenations will always be the exact length of audioClips[0]. If true, the interval between concatenations will be updated dynamically with each clip's length.
    [SerializeField] private bool setLengthOnInspector;
    [SerializeField] private double length;
    [SerializeField] private bool playOnStart;
    
    private bool canGetDspTime, canPlay;
    private int index, lastIndex;
    private int toggle;
    
    private double clipLength, nextStartTime;
    
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
            if (canGetDspTime)
                GetCurrentDspTime();
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
        
        if (getLengthDynamically)
            clipLength = (double)audioClips[index].samples / audioClips[index].frequency;
        else
            clipLength = (double)audioClips[0].samples / audioClips[0].frequency;
    }
    
    public void Play()
    {
        Initialize();
        canPlay = true;
        canGetDspTime = true;
    }
    
    private void GetCurrentDspTime()
    {
        nextStartTime = AudioSettings.dspTime + 0.1f;
        canGetDspTime = false;
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
        if (getLengthDynamically)
        GetNewClipLength();
        
        if (!setLengthOnInspector)
        nextStartTime += clipLength;
        else
        nextStartTime += length;
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
        clipLength = (double)audioClips[index].samples / audioClips[index].frequency;
    }
}
