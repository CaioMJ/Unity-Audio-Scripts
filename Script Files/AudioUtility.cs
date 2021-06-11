using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioUtility
{
	
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

	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip audioClip,
		float minVolume, float maxVolume, float minPitch, float maxPitch)
  	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClip, Random.Range(minVolume, maxVolume));
    	}

	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip[] audioClipArray,
		float minVolume, float maxVolume, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length)], Random.Range(minVolume, maxVolume));
	}
	
	public static void PlayOneShotWithRandomization(AudioSource audioSource, AudioClip[] audioClipArray,
		int index, float minVolume, float maxVolume, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClipArray[index], Random.Range(minVolume, maxVolume));
	}
}
