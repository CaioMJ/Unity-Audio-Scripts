using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayOneShot : MonoBehaviour
{
    public AudioConfigurationSO config;
    public AudioClipCueSO cue;
    [SerializeField] private OneShotMethod playMethod = OneShotMethod.NoRandomization;
    [SerializeField] private bool playOnAwake = false;

    private AudioSource audioSource;

    public enum OneShotMethod
    {
        NoRandomization,
        VolumeRandomization,
        PitchRandomization,
        PitchAndVolumeRandomization,
    }

    #region Setup
    private void OnValidate()
    {
        ConfigureAudioSource();
    }

    private void Awake()
    {
        ConfigureAudioSource();

        if (playOnAwake)
            Play();
    }

    private void ConfigureAudioSource()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        config.SetupAudioSource(audioSource);
        cue.Initialize(audioSource);
    }
    #endregion

    #region Play
    public void Play()
    {
        if (playMethod == OneShotMethod.NoRandomization)
            NoRandomization();
        if (playMethod == OneShotMethod.VolumeRandomization)
            VolumeRandomization();
        if (playMethod == OneShotMethod.PitchRandomization)
            PitchRandomization();
        if (playMethod == OneShotMethod.PitchAndVolumeRandomization)
            PitchAndVolumeRandomization();

        print("PLAY ONE SHOT with " + playMethod + " on " + gameObject.name);
    }

    private void NoRandomization()
    {
        audioSource.PlayOneShot(cue.GetNextClip());
    }

    private void VolumeRandomization()
    {
        audioSource.PlayOneShot(cue.GetNextClip(), cue.RandomVolume());
    }

    private void PitchRandomization()
    {
        audioSource.pitch = cue.RandomPitch();
        audioSource.PlayOneShot(cue.GetNextClip());
    }

    private void PitchAndVolumeRandomization()
    {
        audioSource.pitch = cue.RandomPitch();
        audioSource.PlayOneShot(cue.GetNextClip(), cue.RandomVolume());
    }
    #endregion
}
