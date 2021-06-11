using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartClipAtRandomTime : MonoBehaviour
{
    private AudioClip aClip;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Awake()
    {
        aSource = GetComponent<AudioSource>();
        aClip = aSource.clip;

        aSource.time = Random.Range(0, aClip.length);
        aSource.Play();
    }
}
