using UnityEngine;

[CreateAssetMenu(fileName = "newAudioClipCue", menuName = "Audio/Audio Clip Cue")]
public class AudioClipCueSO : ScriptableObject
{
	[Header("Clip Properties")]
    public AudioClip[] audioClips;
	public bool loop = false;
	[Range(0f, 1f)] public float Volume = 1f;
	[Range(-3f, 3f)] public float Pitch = 1f;

	[Header("Randomization")]
	public SequenceMode sequenceMode = SequenceMode.Sequential;
	[Range(-3f, -0f)] public float NegativePitchVariation = 0f;
	[Range(0, 3f)] public float PositivePitchVariation = 0f;
	[Range(-1f, 0f)] public float NegativeVolumeVariation = 0f;
	[Range(0f, 1f)] public float PositiveVolumeVariation = 0f;
	[HideInInspector] public int lastIndex, nextIndex;

	[Header("Loop Properties")]
	public float fadeInTime;
	public float fadeOutTime;
	public double reverbTailLength;
	public double barLength;
	public double bpm;
	public int timeSigUpper;
	public int timeSigLower;

	[Header("Markov Chain")]
	public TwoDimensionalFloatArraySO markovChainProbabilities;

	public enum SequenceMode
	{
		Random,
		RandomNoImmediateRepeat,
		Sequential,
		MarkovChain
	}

	public void Initialize(AudioSource audioSource)
	{
		audioSource.volume = Volume;
		audioSource.pitch = Pitch;
		audioSource.loop = loop;
	}

	public AudioClip GetNextClip()
	{
		// Fast out if there is only one clip to play
		if (audioClips.Length == 1)
			return audioClips[0];

		if (nextIndex == -1)
		{
			// Index needs to be initialised: 0 if Sequential, random if otherwise
			nextIndex = (sequenceMode == SequenceMode.Sequential) ? 0 : Random.Range(0, audioClips.Length);
		}
		else
		{
			// Select next clip index based on the appropriate SequenceMode
			switch (sequenceMode)
			{
				case SequenceMode.Random:
					nextIndex = Random.Range(0, audioClips.Length);
					break;

				case SequenceMode.RandomNoImmediateRepeat:
					do
					{
						nextIndex = Random.Range(0, audioClips.Length);
					} while (nextIndex == lastIndex);
					break;

				case SequenceMode.Sequential:
					nextIndex = (int)Mathf.Repeat(++nextIndex, audioClips.Length);
					break;

				case SequenceMode.MarkovChain:
					nextIndex = MarkovChainIndex(lastIndex);
					break;
			}
		}

		lastIndex = nextIndex;

		return audioClips[nextIndex];
	}

	private int MarkovChainIndex(int previousIndex)
	{
		float probability = Random.Range(0f, 1f);
		int nextIndex = 0;
		float accumulator = markovChainProbabilities.input[previousIndex].output[nextIndex];

		while (accumulator < probability)
		{
			nextIndex++;
			accumulator += markovChainProbabilities.input[previousIndex].output[nextIndex];
		}

		return nextIndex;
	}
	public float RandomPitch()
    {
		float randomPitch = Random.Range(Pitch + NegativePitchVariation, Pitch + PositivePitchVariation);
		return randomPitch;
    }

	public float RandomVolume()
    {
		float randomVolume = Random.Range(Volume + NegativeVolumeVariation, Volume + PositiveVolumeVariation);
		return randomVolume;
	}   
}