using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayCue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioConfigurationSO config;
    [SerializeField] private AudioClipCueSO cue;
    private AudioSource audioSource;

    [Header("Properties")]
    [SerializeField] private PlayOnAwakeMethod playOnAwake = PlayOnAwakeMethod.No;
    [SerializeField] private bool startOnRandomTime;

    private enum PlayOnAwakeMethod
    {
        No,
        PlayWithNoFade,
        PlayWithFade,
    }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnAwake == PlayOnAwakeMethod.PlayWithNoFade)
            Play(false);
        else if (playOnAwake == PlayOnAwakeMethod.PlayWithFade)
            Play(true);
    }

    #region Play and Stop
    private void Initialize()
    {
        config.SetupAudioSource(audioSource);
        cue.Initialize(audioSource);
    }

    public void Play(bool fadeIn)
    {
        if (audioSource.isPlaying) { return; }

        Initialize();
        audioSource.clip = cue.GetNextClip();
        audioSource.Play();

        if (startOnRandomTime)
            SetRandomTime();

        if (fadeIn)
            FadeIn();

        print("PLAY " + gameObject.name + " : " + audioSource.clip);
    }

    public void Stop(bool fadeOut)
    {
        if (!audioSource.isPlaying) { return; }

        if (fadeOut)
            FadeOutAndStop();
        else
            audioSource.Stop();

        print("STOP " + gameObject.name + " : " + audioSource.clip);
    }
    #endregion

    #region Fades
    public void FadeIn()
    {
        audioSource.volume = 0;
        StartCoroutine(AudioUtility.FadeAudioSource(audioSource, cue.fadeInTime, cue.Volume));
    }

    public void FadeOut()
    {
        StartCoroutine(AudioUtility.FadeAudioSource(audioSource, cue.fadeOutTime, 0));
    }

    public void FadeOutAndStop()
    {
        StartCoroutine(AudioUtility.FadeOutAndStopAudioSource(audioSource, cue.fadeOutTime));
    }
    #endregion

    #region Set Audio Source Time
    public void SetRandomTime()
    {
        AudioUtility.SetAudioSourceTimeRandom(audioSource, audioSource.clip);
    }

    public void SetTimeWithPercentage(float audioClipPercentage)
    {
        AudioUtility.SetAudioSourceTimePercentage(audioSource, audioSource.clip, audioClipPercentage);
    }

    public void SetTimeWithSeconds(float audioSourceTime)
    {
        AudioUtility.SetAudioSourceTimeAbsolute(audioSource, audioSourceTime);
    }

    public void CheckLoopPoint()
    {
        AudioUtility.SetAudioSourceTimePercentage(audioSource, audioSource.clip, 90f);
    }
    #endregion
}
