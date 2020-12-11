using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScheduledLoop : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource; //array with 2 audio sources

    private double loopDuration;
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

    private void StartLoop()
    {
        isLooping = true;
        print(isLooping);
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
        print("LOOP");
    }
}
