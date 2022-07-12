using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class AudioIntermittentEmitter : MonoBehaviour
{
    [Header("Audio References")]
    [SerializeField] private AudioConfigurationSO config;
    [SerializeField] private AudioClipCueSO cue;
    private AudioSource audioSource;

    [Header("Intermittent Properties")]
    [Range(0f, 30f)]
    public float minTime;
    [Range(0f, 30f)]
    public float maxTime;
    [SerializeField] private bool playOnAwake;

    public bool randomPosition;
    [HideInInspector] public Transform listenerPosition;
    [HideInInspector] public Vector3 maxDistanceDivisor;
    private bool enablePlay;

    #region Setup
    private void OnValidate()
    {
        ConfigureAudioSource();
    }

    private void Awake()
    {
        if (playOnAwake)
            Play();
    }

    private void ConfigureAudioSource()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        config.SetupAudioSource(audioSource);
        cue.Initialize(audioSource);
    }
    #endregion

    #region Play and Stop
    public void Play()
    {
        if (enablePlay) { return; }

        enablePlay = true;
        ConfigureAudioSource();
        StartCoroutine("IntermittentSound");

        print("PLAY INTERMITTENT SOURCE: " + gameObject.name);
    }

    public void Stop()
    {
        if(!enablePlay) { return; }

        enablePlay = false;

        print("STOP INTERMITTENT SOURCE: " + gameObject.name);
    }
    #endregion

    #region Intermittent Coroutine
    private IEnumerator IntermittentSound()
    {
        float waitTime = Random.Range(minTime, maxTime);

        if (audioSource.clip == null)
        {
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(audioSource.clip.length + waitTime);
        }
        if (enablePlay)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (randomPosition)
            RandomizePosition();

        audioSource.volume = cue.RandomVolume(); //Random.Range(cue.MinVolume, cue.MaxVolume);
        audioSource.pitch = cue.RandomPitch(); //Random.Range(cue.MinPitch, cue.MaxPitch);
        audioSource.clip = cue.GetNextClip();
        audioSource.Play();
        StartCoroutine("IntermittentSound");

        print("INTERMITTENT SOUND " + gameObject.name + " : " + audioSource.clip);
    }

    private void RandomizePosition()
    {
        if (maxDistanceDivisor.x == 0)
            maxDistanceDivisor.x = config.MaxDistance;
        if (maxDistanceDivisor.y == 0)
            maxDistanceDivisor.y = config.MaxDistance;
        if (maxDistanceDivisor.z == 0)
            maxDistanceDivisor.z = config.MaxDistance;

        Vector3 randomPositionOffset =new Vector3(
            Random.Range(audioSource.maxDistance, -audioSource.maxDistance) / maxDistanceDivisor.x,
            Random.Range(audioSource.maxDistance, -audioSource.maxDistance) / maxDistanceDivisor.y,
            Random.Range(audioSource.maxDistance, -audioSource.maxDistance) / maxDistanceDivisor.z);

        gameObject.transform.position = listenerPosition.position + randomPositionOffset;        
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioIntermittentEmitter))]
public class AudioIntermitternEmiter_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        AudioIntermittentEmitter script = (AudioIntermittentEmitter)target;

        if (script.randomPosition) // if bool is true, show other fields
        {
            script.listenerPosition = EditorGUILayout.ObjectField("Listener Position", script.listenerPosition, typeof(Transform), true) as Transform;
            script.maxDistanceDivisor = EditorGUILayout.Vector3Field("Max Distance Divisor", script.maxDistanceDivisor);
        }
    }
}
#endif
