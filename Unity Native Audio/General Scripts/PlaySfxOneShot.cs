using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySfxOneShot : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;
    [SerializeField] [Range(0,2)] private float minPitch, maxPitch;
    [SerializeField] [Range(0, 1)] private float minVol, maxVol;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySfxWithRandomization()
    {
        AudioUtility.PlayOneShotWithRandomization(audioSource, sfx, minVol, maxVol, minPitch, maxPitch);
    }
}
