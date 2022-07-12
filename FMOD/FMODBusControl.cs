using System.Collections;
using UnityEngine;

public class FMODBusControl : MonoBehaviour
{
    [SerializeField] private string _busPath; //use "bus:/" only for master
    [SerializeField] private bool _immediateStop;

    public FMOD.Studio.Bus bus;

    // Start is called before the first frame update
    void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus(_busPath);
        string path = "";
        bus.getPath(out path);
        Debug.Log("FMOD bus path: " + path);
    }

    // Update is called once per frame
    public void SetVolume(float volume)
    {
        bus.setVolume(volume);
        Debug.Log("FMOD" + _busPath + " VOLUME: " + volume);
    }

    public void StopAllEvents()
    {
        if (!_immediateStop)
            bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else
            bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        Debug.Log("FMOD" + _busPath + " STOP ALL EVENTS");

    }

    public void Pause()
    {
        bus.setPaused(true);

        Debug.Log("FMOD" + _busPath + " PAUSE");
    }

    public void UnPause()
    {
        bus.setPaused(false);

        Debug.Log("FMOD" + _busPath + " UNPAUSE");
    }
}
