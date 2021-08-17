using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioConfigurationApplier : MonoBehaviour
{
	public AudioConfigurationSO config;
	public AudioClipCueSO cue;

	private void OnValidate()
	{
		ConfigureAudioSource();
	}

	private void Start()
	{
		ConfigureAudioSource();
	}

	private void ConfigureAudioSource()
	{
		if (config != null)
		{
			AudioSource audioSource = GetComponent<AudioSource>();
			config.SetupAudioSource(audioSource);
		}

		if(cue != null)
        {
			AudioSource audioSource = GetComponent<AudioSource>();
			cue.Initialize(audioSource);
        }
	}
}
