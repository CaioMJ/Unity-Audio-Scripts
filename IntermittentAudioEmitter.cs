using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermittentAudioEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [Range(0f, 1f)]
    public float minVolume, maxVolume;
    [Range(0f, 30f)]
    public float minTime, maxTime;
    [Range(0f, 50f)]
    public int randomDistance, maxDistance;
    [Range(0f, 1.1f)]
    public float spatialBlend;
    [Space]
    [SerializeField] private AudioClip[] audioClips;
    public bool enablePlayMode;
    private int randomIndex;
    private int lastRandomIndex;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 0.1f;

        StartCoroutine("IntermittentSound");
    }

    private void SetAudioSourceProperties()
    {
        audioSource.maxDistance = maxDistance - Random.Range(0f, randomDistance);
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = Random.Range(minVolume, maxVolume);

        while (randomIndex == lastRandomIndex)
        {
            randomIndex = Random.Range(0, audioClips.Length);
        }
        lastRandomIndex = randomIndex;
        audioSource.clip = audioClips[randomIndex];
    }

    private void PlaySound()
    {
        SetAudioSourceProperties();
        audioSource.Play();
        print(audioSource.clip);
        StartCoroutine("IntermittentSound");
    }

    private IEnumerator IntermittentSound()
    {
        float waitTime = Random.Range(minTime, maxTime);

        if(audioSource.clip == null)
        {
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(audioSource.clip.length + waitTime);
        }
        if (enablePlayMode)
        {
            PlaySound();
        }
    }
}
