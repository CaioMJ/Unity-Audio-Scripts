using System.Collections;
using UnityEngine;

public class AudioLoopReverbTail : MonoBehaviour
{
    [SerializeField] private AudioConfigurationSO config;
    [SerializeField] private AudioClipCueSO cue;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private AudioSource[] audioSources; //Array with 2 audio sources

    private double dspTimeOffset = 0.1f;
    private double nextLoopStart;
    private bool isLooping = false;
    private int toggle;

    private void OnValidate()
    {
        ConfigureAudioSources();
    }

    void Awake()
    {
        if (playOnAwake)
            Play(true);
    }

    void Update()
    {
        if (isLooping)
        {
            if(AudioSettings.dspTime > nextLoopStart - dspTimeOffset)
                ScheduleAudioSource();
        }
    }

    #region Loop Methods
    private void ScheduleAudioSource()
    {
        toggle = 1 - toggle;

        audioSources[toggle].PlayScheduled(nextLoopStart);
        IncrementNextLoopStart();

        print("LOOP POINT FOR CLIP " + audioSources[toggle].clip + " on " + gameObject.name);
    }

    private void IncrementNextLoopStart()
    {
        nextLoopStart += ((double)audioSources[toggle].clip.samples / audioSources[toggle].clip.frequency) - cue.reverbTailLength;
    }
    #endregion

    #region Start and Stop Methods
    private void ConfigureAudioSources()
    {
        foreach (AudioSource aSource in audioSources)
        {
            config.SetupAudioSource(aSource);
            cue.Initialize(aSource);
            aSource.clip = cue.GetNextClip();
        }
    }

    public void Play(bool fadeIn)
    {
        if (isLooping) { return; }

        ConfigureAudioSources();

        if (fadeIn)
            FadeIn();
        else
        {
            foreach (AudioSource aSource in audioSources)
                aSource.volume = cue.Volume;
        }

        isLooping = true;
        nextLoopStart = AudioSettings.dspTime + dspTimeOffset;

        print("PLAY " + gameObject.name + " : " + audioSources[toggle].clip);
    }

    public void Stop(bool fadeOut)
    {
        if(!isLooping) { return; }

        if (fadeOut)
        {
            StartCoroutine(StopLoopAfterFadeOut());
            FadeOut();
        }
        else
        {
            isLooping = false;
            foreach (AudioSource aSource in audioSources)
                aSource.Stop();
        }

        print("STOP: " + audioSources[toggle].clip + "on: " + gameObject.name);
    }

    private IEnumerator StopLoopAfterFadeOut()
    {
        yield return new WaitForSeconds(cue.fadeOutTime);
        isLooping = false;
    }
    #endregion

    #region Fades
    public void FadeIn()
    {
        foreach (AudioSource aSource in audioSources)
        {
            aSource.volume = 0;
            StartCoroutine(AudioUtility.FadeAudioSource(aSource, cue.fadeInTime, cue.Volume));
        }

        print("FADE IN: " + audioSources[toggle].clip + " on: " + gameObject.name);
    }

    public void FadeOut()
    {
        foreach (AudioSource aSource in audioSources)
            StartCoroutine(AudioUtility.FadeOutAndStopAudioSource(aSource, cue.fadeOutTime));

        print("FADE OUT: " + audioSources[toggle].clip + " on: " + gameObject.name);
    }
    #endregion
}