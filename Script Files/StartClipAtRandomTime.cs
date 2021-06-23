using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartClipAtRandomTime : MonoBehaviour
{
    private AudioClip aClip;
    private AudioSource aSource;
    [SerializeField] private bool playOnAwake;

    void Awake()
    {
        aSource = GetComponent<AudioSource>();
        
        if(playOnAwake)
            Play();
    }
    
    public void Play()
    {
        aClip = aSource.clip;
        aSource.time = Random.Range(0, aClip.length);
        aSource.Play();
    }
}
