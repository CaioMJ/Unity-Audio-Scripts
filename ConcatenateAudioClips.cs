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
    [Header("CONCATENATION PROPERTIES")]
    [SerializeField] private bool playClipsRandomly;
    [SerializeField] private bool getLengthDynamically; //If true, the next clip will play back immediately at the end of the previous clip. If false, the length index 0 of the audio clip array will serve as a fixed interval between playbacks
    private int index, lastIndex;
    private int toggle;

    private double clipLength, nextStartTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playClipsRandomly)
            index = Random.Range(0, audioClips.Length);
        else
            index = 0;

        if(getLengthDynamically)
            clipLength = (double)audioClips[index].samples / audioClips[index].frequency;
        else
            clipLength = (double)audioClips[0].samples / audioClips[0].frequency;

        nextStartTime = AudioSettings.dspTime + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioSettings.dspTime > nextStartTime - 1)
        {
            ConcatenateClips();
        }
    }

    private void ConcatenateClips()
    {
        toggle = 1 - toggle;
        audioSource[toggle].clip = audioClips[index];
        audioSource[toggle].PlayScheduled(nextStartTime);

        if (getLengthDynamically)
        {
            GetNewClipLength();
        }
        nextStartTime += clipLength;

        if (playClipsRandomly)
            RandomIndex();
        else
            SequentialIndex();
    }

    private void RandomIndex()
    {
        while (index == lastIndex)
        {
            index = Random.Range(0, audioClips.Length);
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
