using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioUtility
{
	public static IEnumerator FadeInAudioSource(AudioSource audioSource, float duration, float targetVolume)
	{
		audioSource.volume = 0;
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);

			yield return null;
		}
		yield break;
	}

	public static IEnumerator FadeOutAudioSource(AudioSource audioSource, float duration)
	{
		float currentTime = 0;
		float startVolume = audioSource.volume;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);

			yield return null;
		}
		yield break;
	}

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

	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip audioClip,
		float minVolume, float maxVolume, float minPitch, float maxPitch)
    {
		float volume = Random.Range(minVolume, maxVolume);
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClip, volume);
    }

	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip[] audioClipArray,
	float minVolume, float maxVolume, float minPitch, float maxPitch)
	{
		float volume = Random.Range(minVolume, maxVolume);
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length)], volume);
	}
}
