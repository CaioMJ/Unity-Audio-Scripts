using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
///Static class with general audio functionalities
/// </summary>
public static class AudioUtility
{
    #region Fades
    /// <summary>
    /// Changes volume of an audio mixer group over time
    /// </summary>
    public static IEnumerator FadeMixerGroup(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
	{
		float currentTime = 0;
		float currentVol;

		audioMixer.GetFloat(exposedParam, out currentVol);
		currentVol = Mathf.Pow(10, currentVol / 20);
		float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
			audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
			yield return null;
		}
		yield break;
	}

	/// <summary>
    /// Sets audio source volume to 0, plays audio source, and fades it in over time
    /// </summary>
	public static IEnumerator PlayAndFadeInAudioSource(AudioSource audioSource, float duration, float targetVolume)
	{
		audioSource.volume = 0;
		float currentTime = 0;
		audioSource.Play();
		
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);

			yield return null;
		}
		yield break;
	}	

	/// <summary>
    /// Changes an audio source volume over time
    /// </summary>
	public static IEnumerator FadeAudioSource(AudioSource audioSource, float duration, float targetVolume)
	{
		float startingVolume = audioSource.volume;
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / duration);

			yield return null;
		}
		yield break;
	}

	/// <summary>
    /// Fades audio source volume to 0 and stops the source once done
    /// </summary>
	public static IEnumerator FadeOutAndStopAudioSource(AudioSource audioSource, float duration)
	{
		float currentTime = 0;
		float startVolume = audioSource.volume;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);

			yield return null;
		}
		audioSource.Stop();
		yield break;
	}

	/// <summary>
    /// Crosfades two audio sources, setting the audioSourceIn volume to 0 at the start
    /// </summary>
	public static IEnumerator CrossfadeAudioSources(AudioSource audioSourceIn, float targetVolumeIn, AudioSource audioSourceOut, float duration)
	{
		float currentTime = 0;
		float startVolumeOut = audioSourceOut.volume;
		audioSourceIn.volume = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSourceOut.volume = Mathf.Lerp(startVolumeOut, 0, currentTime / duration);
			audioSourceIn.volume = Mathf.Lerp(0, targetVolumeIn, currentTime / duration);

			yield return null;
		}
		yield break;
	}

    /// <summary>
    /// Crosfades two audio sources, setting the audioSourceIn volume to 0 at the start, also playing the audioSourceIn at the start and stopping audioSourceOut at the end
    /// </summary>
    public static IEnumerator CrossfadeAudioSourcesWithPlayAndStop(AudioSource audioSourceIn, float targetVolumeIn, AudioSource audioSourceOut, float duration)
	{
		float currentTime = 0;
		float startVolumeOut = audioSourceOut.volume;
		audioSourceIn.volume = 0;
		audioSourceIn.Play();

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSourceOut.volume = Mathf.Lerp(startVolumeOut, 0, currentTime / duration);
			audioSourceIn.volume = Mathf.Lerp(0, targetVolumeIn, currentTime / duration);

			yield return null;
		}
		audioSourceOut.Stop();
		yield break;
	}
    #endregion

    #region Play One Shots
    /// <summary>
    /// Plays a one shot audio clip with pitch and volume randomization
    /// </summary>
    public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip audioClip,
		float minVolume, float maxVolume, float minPitch, float maxPitch)
  	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClip, Random.Range(minVolume, maxVolume));
    	}

	/// <summary>
	/// Plays a random audio clip from an array as a one shot with pitch and volume randomization
	/// </summary>
	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip[] audioClipArray,
		float minVolume, float maxVolume, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length)], Random.Range(minVolume, maxVolume));
	}

	/// <summary>
	/// Plays an indexed audio clip from an array as a one shot with pitch and volume randomization
	/// </summary>
	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip[] audioClipArray,
		int index, float minVolume, float maxVolume, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClipArray[index], Random.Range(minVolume, maxVolume));
	}
	#endregion


	#region Set Audio Source Time
	/// <summary>
	/// Set audio source time to a random time between 0 and the clip's length
	/// </summary>
	public static void SetAudioSourceTimeRandom(AudioSource audioSource, AudioClip audioClip)
    {
		audioSource.time = Random.Range(0, audioClip.length);
    }

	/// <summary>
	/// Set audio source time to the specified float as a percentage of the total clip's length
	/// </summary>
	public static void SetAudioSourceTimePercentage(AudioSource audioSource, AudioClip audioClip, float percentage)
    {
		audioSource.time = (audioClip.length * percentage) / 100;
	}

	/// <summary>
	/// Set audio source time to the specified float as an absolute time value
	/// </summary>
	public static void SetAudioSourceTimeAbsolute(AudioSource audioSource, float time)
	{
		audioSource.time = time;
	}
	#endregion
}
