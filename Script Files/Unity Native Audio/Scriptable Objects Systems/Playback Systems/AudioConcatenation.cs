using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioConcatenation : MonoBehaviour
{
    [Header("AUDIO REFERENCES")]
    [SerializeField] private AudioConfigurationSO config;
    [SerializeField] private AudioClipCueSO cue;
    [SerializeField] private AudioSource[] audioSources; //Array with 2 audio sources
    [Space]
    [Header("CONCATENATION PROPERTIES")]
    [SerializeField] private PlayOnAwakeMethod playOnAwake = PlayOnAwakeMethod.No;
    public ConcatenationInterval concatenationInterval;
    [HideInInspector] public double intervalOnInspector;

    private bool isPlaying;
    private int toggle;
    private double interval, nextStartTime, dspTimeOffset = 0.1;

    public enum ConcatenationInterval { UpdateDynamically, FixedAtIndex0, SetOnInspector }
    private enum PlayOnAwakeMethod { No, PlayWithNoFade, PlayWithFade }


    // Start is called before the first frame update
    void Awake()
    {
        Initialize();

        if (playOnAwake == PlayOnAwakeMethod.PlayWithNoFade)
            Play(false);
        else if (playOnAwake == PlayOnAwakeMethod.PlayWithFade)
            Play(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (AudioSettings.dspTime > nextStartTime - dspTimeOffset)
                Concatenate();
        }
    }

    #region Concatenation

    private void Concatenate()
    {
        ScheduleAudioSource();
        IncrementNextStartTime();
    }

    private void ScheduleAudioSource()
    {
        toggle = 1 - toggle;

        audioSources[toggle].clip = cue.GetNextClip();
        audioSources[toggle].pitch = cue.RandomPitch();
        audioSources[toggle].PlayScheduled(nextStartTime);

        print("CONCATENATE: " + audioSources[toggle].clip + " on " + gameObject.name);
    }

    private void IncrementNextStartTime()
    {
        if (concatenationInterval == ConcatenationInterval.UpdateDynamically)
        {
            interval = ((double)audioSources[toggle].clip.samples / audioSources[toggle].clip.frequency) - cue.reverbTailLength;
        }

        nextStartTime += interval;
    }
    #endregion

    #region Start and Stop 
    private void Initialize()
    {
        foreach (AudioSource aSource in audioSources)
        {
            config.SetupAudioSource(aSource);
            cue.Initialize(aSource);
        }

        if (concatenationInterval == ConcatenationInterval.FixedAtIndex0)
            interval = (double)cue.audioClips[0].samples / cue.audioClips[0].frequency;
        else if (concatenationInterval == ConcatenationInterval.SetOnInspector)
            interval = intervalOnInspector;
    }

    public void Play(bool fadeIn)
    {
        if (isPlaying) { return; }

        Initialize();

        if (fadeIn)
            FadeIn();

        isPlaying = true;
        nextStartTime = AudioSettings.dspTime + dspTimeOffset;

        print("PLAY " + audioSources[toggle].clip);
    }

    public void Stop(bool fadeOut)
    {
        if (!isPlaying) { return; }

        if (fadeOut)
        {
            FadeOut();
        }
        else
        {
            isPlaying = false;
            foreach (AudioSource aSource in audioSources)
                aSource.Stop();
        }

        print("STOP: " + gameObject.name);
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

        print("FADE IN: " + gameObject.name);
    }

    public void FadeOut()
    {
        StartCoroutine(StopLoopAfterFadeOut());
        foreach (AudioSource aSource in audioSources)
            StartCoroutine(AudioUtility.FadeOutAndStopAudioSource(aSource, cue.fadeOutTime));

        print("FADE OUT: " + gameObject.name);
    }

    private IEnumerator StopLoopAfterFadeOut()
    {
        yield return new WaitForSeconds(cue.fadeOutTime);
        isPlaying = false;
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioConcatenation))]
public class AudioConcatenation_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        AudioConcatenation script = (AudioConcatenation)target;

        if (script.concatenationInterval == AudioConcatenation.ConcatenationInterval.SetOnInspector) // if bool is true, show other fields
        {
            script.intervalOnInspector = EditorGUILayout.DoubleField("Interval On Inspector", script.intervalOnInspector);
        }
    }
}
#endif

