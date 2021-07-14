using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Music Properties")]
public class MusicProperties : ScriptableObject
{
    public AudioClip clip;
    public bool useArray;
    public AudioClip[] clips;
    public double reverbTail;
    [Range(0f, 1f)] public float volume;
    public float fadeOutTime;
    public float fadeInTime;
}
