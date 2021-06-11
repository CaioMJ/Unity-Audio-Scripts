using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcatenateAudioClips : MonoBehaviour
{
    [Header("AUDIO REFERENCES")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource[] audioSource;
    [Space]
    [Header("CONCATENATION PROPERTIES")]
    [SerializeField] private bool playClipsRandomly;
    [SerializeField] private bool getLengthDynamically;
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
