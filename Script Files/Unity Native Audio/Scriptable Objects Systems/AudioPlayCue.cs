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
    [SerializeField] private bool randomStartTime;
    [SerializeField] private PlayOnAwakeMethod playOnAwake = PlayOnAwakeMethod.No;

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

        if (randomStartTime)
            AudioUtility.SetAudioSourceTimeRandom(audioSource, audioSource.clip);

        if (fadeIn)
            FadeIn();

        print("PLAY " + gameObject.name + " : " + audioSource.clip);
    }

    public void Stop(bool fadeOut)
    {
        if (!audioSource.isPlaying) { return; }

        if (fadeOut)
            FadeOut();
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
        StartCoroutine(StopAfterFadeOut());
    }

    private IEnumerator StopAfterFadeOut()
    {
        yield return new WaitForSeconds(cue.fadeOutTime);
        audioSource.Stop();
    }
    #endregion
}
