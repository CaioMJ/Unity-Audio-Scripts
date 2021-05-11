using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetMasterVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        StartCoroutine(DelayedFadeIn());
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("AudioVolume");
    }

    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("AudioVolume", sliderValue);
    }

    private IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSeconds(1);

        StartCoroutine(AudioUtility.FadeMixerGroup(audioMixer, "SubMixVolume", 1, 1));
    }
}
