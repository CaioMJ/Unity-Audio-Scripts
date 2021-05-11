using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScheduledLoop : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource; //array with 2 audio sources

    private double loopDuration; //This variable should hold the exact value in seconds (to the smallest possible decimal)for the loop's duration
    private double nextLoopStart;

    private bool canGetDspTime = true;
    private bool isLooping;
    private int arrayToggle;

    // Update is called once per frame
    void Update()
    {
        if (isLooping)
        {
            GetCurrentDspTime();

            if (AudioSettings.dspTime > nextLoopStart - 0.1)
            {
                ConcatenateLoop();
            }
        }
    }

    public void StartLoop() //Call this function to play audio
    {
        isLooping = true;
        print("START PLAY SCHEDULED LOOP");
    }
    
    public void StopLoop() //This will only stop subsequent loops from hapenning, this won't immediately stop audio
    {
        isLooping = false;
        canGetDspTime = true;
        //INSERT FADE OUT OR AUDIO SOURCE STOP METHODD HERE IF NEEDED
    }
    
    private void GetCurrentDspTime()
    {
        if (canGetDspTime)
        {
            nextLoopStart = AudioSettings.dspTime + 0.1;
            canGetDspTime = false;
        }
    }

    private void ConcatenateLoop()
    {
        arrayToggle = 1 - arrayToggle;

        audioSource[arrayToggle].PlayScheduled(nextLoopStart);
        nextLoopStart += loopDuration;
        print("LOOP POINT");
    }
}
