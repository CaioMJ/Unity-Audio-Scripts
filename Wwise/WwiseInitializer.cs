using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseInitializer : MonoBehaviour
{
    public static WwiseInitializer instance { get; private set; }

    [SerializeField] private AK.Wwise.Bank[] banksToLoadOnAwake;
    [SerializeField] private AK.Wwise.Event[] eventsToPostOnStart;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        AkSoundEngine.RegisterGameObj(gameObject);

        foreach (AK.Wwise.Bank wwiseBank in banksToLoadOnAwake)
        {
            wwiseBank.Load();
            Debug.Log("WWISE BANK LOADED: " + wwiseBank);
        }
    }

    private void Start()
    {
        foreach(AK.Wwise.Event wwiseEvent in eventsToPostOnStart)
        {
            wwiseEvent.Post(gameObject);
            Debug.Log("WWISE EVENT POSTED: " + wwiseEvent);
        }
    }
}
