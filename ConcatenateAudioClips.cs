using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcatenateAudioClips : MonoBehaviour
{
    //This is current only set up to work using audioClips[0] as reference for length between concatenations, not accounting for 
    //difference in lenghts between clips if you desire a more dynamic, sample accurate concatenation method
    
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource[] audioSource;
    private int randomIndex;
    private int lastRandomIndex;
    private int toggle;
    private double clipLength;
    private double nextStartTime;
    
    // Start is called before the first frame update
    void Start()
    {
        clipLength = (double)audioClips[0].samples / audioClips[0].frequency;
        nextStartTime = AudioSettings.dspTime + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioSettings.dspTime > nextStartTime - 1)
        {
            ConcatenateClipsRandomly();
        }
    }

    private void ConcatenateClipsRandomly()
    {
        toggle = 1 - toggle;

        while (randomIndex == lastRandomIndex)
        {
            randomIndex = Random.Range(0, audioClips.Length);
        }
        lastRandomIndex = randomIndex;

        audioSource[toggle].clip = audioClips[randomIndex];
        audioSource[toggle].PlayScheduled(nextStartTime);

        nextStartTime += clipLength;
        print(audioClips[randomIndex]);
    }
}
