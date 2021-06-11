using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetMasterVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("AudioVolume");
    }

    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("AudioVolume", sliderValue);
    }
}
