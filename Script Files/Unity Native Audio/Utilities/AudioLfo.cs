using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLfo : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Lfo Setup")]
    [SerializeField] private LfoTarget lfoTarget = LfoTarget.Volume;
    [SerializeField] private float rate; //frequency of the lfo
    [SerializeField] private float depth; //lfo will oscillate between the set value and its negative
    [SerializeField] private float depthOffset;
    [HideInInspector] public float currentDepth, currentOffset, currentRate;
    private float lfoIndex;
    private float defaultValue;

    [Header("Behaviour")]
    [SerializeField] private bool playOnStart;
    [SerializeField] private FadeInBehaviour fadeIn = FadeInBehaviour.FadeBoth;
    [SerializeField] private float fadeInTime;
    [SerializeField] private FadeOutBehaviour fadeOut = FadeOutBehaviour.FadeBoth;
    [SerializeField] private float fadeOutTime;
    private float fadeTimeShort = 0.1f;

    private bool run = false;

    private enum LfoTarget { Volume, Pitch, Pan }
    private enum FadeInBehaviour { NoFade, FadeDepth, FadeRate, FadeBoth }
    private enum FadeOutBehaviour { NoFade, FadeDepth, FadeBoth }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playOnStart)
            Run();
    }

    #region LFO
    private void Update()
    {
        if (run)
        {
            switch (lfoTarget)
            {
                case LfoTarget.Volume:
                    audioSource.volume = Lfo();
                    break;
                case LfoTarget.Pitch:
                    audioSource.pitch = Lfo();
                    break;
                case LfoTarget.Pan:
                    audioSource.panStereo = Lfo();
                    break;
            }
        }
    }

    private float Lfo()
    {
        lfoIndex += Time.deltaTime * currentRate;
        float lfo = (Mathf.Sin(lfoIndex) * currentDepth) + currentOffset;
        return lfo + defaultValue;

        //if (lfoTarget != LfoTarget.Volume)
    }
    #endregion

    #region Play And Stop
    public void Run()
    {
        if(run) { return; }

        SetDefaultValue();

        switch (fadeIn)
        {
            case FadeInBehaviour.FadeBoth:
                FadeInDepth(fadeInTime);
                FadeInRate(fadeInTime);
                break;
            case FadeInBehaviour.FadeDepth:
                FadeInDepth(fadeInTime);
                FadeInRate(fadeTimeShort);
                break;
            case FadeInBehaviour.FadeRate:
                FadeInRate(fadeInTime);
                FadeInDepth(fadeTimeShort);
                break;
            case FadeInBehaviour.NoFade:
                FadeInDepth(fadeTimeShort);
                FadeInRate(fadeTimeShort);
                break;
        }

        run = true;
    }

    public void Stop()
    {
        if(!run) { return; }

        switch (fadeOut)
        {
            case FadeOutBehaviour.FadeBoth:
                FadeOutDepth(fadeOutTime);
                FadeOutRate(fadeOutTime);
                StartCoroutine(StopAfterFading(fadeOutTime));
                break;
            case FadeOutBehaviour.FadeDepth:
                FadeOutDepth(fadeOutTime);
                StartCoroutine(StopAfterFading(fadeOutTime));
                break;
            case FadeOutBehaviour.NoFade:
                FadeOutRate(fadeTimeShort);
                FadeOutDepth(fadeTimeShort);
                StartCoroutine(StopAfterFading(fadeTimeShort));
                break;
        }
    }

    private void SetDefaultValue()
    {
        switch (lfoTarget)
        {
            case LfoTarget.Volume:
                defaultValue = audioSource.volume;
                break;
            case LfoTarget.Pitch:
                defaultValue = audioSource.pitch;
                break;
            case LfoTarget.Pan:
                defaultValue = audioSource.panStereo;
                break;
        }
    }
    #endregion

    #region Fades
    private void FadeInDepth(float time)
    {
        StartCoroutine(InterpolateDepth(time, 0, depth, 0, depthOffset));
    }

    private void FadeOutDepth(float time)
    {
        StartCoroutine(InterpolateDepth(time, currentDepth, 0, depthOffset, 0));
    }

    private void FadeInRate(float time)
    {
        StartCoroutine(InterpolateRate(time, 0, rate));
    }

    private void FadeOutRate(float time)
    {
        StartCoroutine(InterpolateRate(time, currentRate, 0));
    }

    private IEnumerator StopAfterFading(float time)
    {
        yield return new WaitForSeconds(time);
        run = false;
    }
    #endregion

    #region Interpolation
    private IEnumerator InterpolateRate(float duration, float startingRate, float targetRate) 
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            currentRate = Mathf.Lerp(startingRate, targetRate, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator InterpolateDepth(float duration,
        float startingDepth, float targetDepth, float startingOffset, float targetOffset)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            currentDepth = Mathf.Lerp(startingDepth, targetDepth, currentTime / duration);
            currentOffset = Mathf.Lerp(startingOffset, targetOffset, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    #endregion
}