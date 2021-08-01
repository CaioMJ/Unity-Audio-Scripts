using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "newAudioConfiguration", menuName = "Audio/Audio Configuration")]
public class AudioConfigurationSO : ScriptableObject
{
	public AudioMixerGroup OutputAudioMixerGroup = null;

	// Simplified management of priority levels (values are counterintuitive, see enum below)
	[SerializeField] private PriorityLevel _priorityLevel = PriorityLevel.Standard;
	[HideInInspector]
	public int Priority
	{
		get { return (int)_priorityLevel; }
		set { _priorityLevel = (PriorityLevel)value; }
	}

	[Header("Sound properties")]
	public bool Mute = false;
	public bool PlayOnAwake = false;
	[Range(-1f, 1f)] public float PanStereo = 0f;
	[Range(0f, 1.1f)] public float ReverbZoneMix = 1f;

	[Header("Spatialisation")]
	[Range(0f, 1f)] public float SpatialBlend = 0f;
	public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
	[Range(0.1f, 10f)] public float MinDistance = 0.1f;
	[Range(5f, 100f)] public float MaxDistance = 50f;
	[Range(0, 360)] public int Spread = 0;
	[Range(0f, 5f)] public float DopplerLevel = 1f;

	[Header("Ignores")]
	public bool BypassEffects = false;
	public bool BypassListenerEffects = false;
	public bool BypassReverbZones = false;
	public bool IgnoreListenerVolume = false;
	public bool IgnoreListenerPause = false;

	private enum PriorityLevel
	{
		Highest = 0,
		High = 64,
		Standard = 128,
		Low = 194,
		VeryLow = 256,
	}

	public void SetupAudioSource(AudioSource audioSource)
	{
		audioSource.outputAudioMixerGroup = OutputAudioMixerGroup;
		audioSource.mute = Mute;
		audioSource.playOnAwake = PlayOnAwake;
		audioSource.bypassEffects = BypassEffects;
		audioSource.bypassListenerEffects = BypassListenerEffects;
		audioSource.bypassReverbZones = BypassReverbZones;
		audioSource.priority = Priority;
		audioSource.panStereo = PanStereo;
		audioSource.spatialBlend = SpatialBlend;
		audioSource.reverbZoneMix = ReverbZoneMix;
		audioSource.dopplerLevel = DopplerLevel;
		audioSource.spread = Spread;
		audioSource.rolloffMode = RolloffMode;
		audioSource.minDistance = MinDistance;
		audioSource.maxDistance = MaxDistance;
		audioSource.ignoreListenerVolume = IgnoreListenerVolume;
		audioSource.ignoreListenerPause = IgnoreListenerPause;
	}
}