using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script uses AudioSource.PlayScheduled() in order to loop audio with a reverb tail
public class PlayScheduledLoop : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources; //array with 2 audio sources
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private double loopDuration; //This variable should hold the exact value in seconds (to the smallest possible decimal) for the loop's duration
    [SerializeField] [Range(0f, 1f)] private float volume;
    [SerializeField] private bool playOnAwake;
   
    private double nextLoopStart;
    private bool canPlay;
    private int arrayToggle;

    void Awake()
    {
        if (playOnAwake)
            Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            if (AudioSettings.dspTime > nextLoopStart - 0.1)
                ConcatenateLoop();
        }
    }

    private void Initialize()
    {
        foreach(AudioSource aSource in audioSources)
        {
            aSource.clip = audioClip;
            aSource.volume = volume; //OR SWAP THIS LINE FOR A FADE IN METHOD
        }
    }

    public void Play() //Call this function to play audio
    {
        Initialize();
        canPlay = true;
        nextLoopStart = AudioSettings.dspTime + 0.1;
        print("START LOOP FOR " + audioClip);
    }
    
    public void StopLoop() //This will only stop subsequent loops from hapenning, this won't immediately stop audio
    {
        canPlay = false;

        foreach(AudioSource aSource in audioSources)
        {
            //CALL A FADE OUT OR AUDIO SOURCE STOP METHODD HERE IF NEEDED
        }
    }
    
    private void ConcatenateLoop()
    {
        arrayToggle = 1 - arrayToggle;

        audioSources[arrayToggle].PlayScheduled(nextLoopStart);
        nextLoopStart += loopDuration;
        print("LOOP POINT FOR " + audioClip);
    }
}
