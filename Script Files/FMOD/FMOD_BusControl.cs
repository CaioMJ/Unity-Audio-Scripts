using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_BusControl : MonoBehaviour
{
    [SerializeField] private string busPath; //use "bus:/" only for master
    [SerializeField] private bool immediateStop;

    public FMOD.Studio.Bus bus;

    // Start is called before the first frame update
    void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus(busPath);
        string path = "";
        bus.getPath(out path);
        print(path);
    }

    // Update is called once per frame
    public void SetVolume(float volume)
    {
        bus.setVolume(volume);
        print(busPath + " VOLUME: " + volume);
    }

    public void StopAllEvents()
    {
        if (!immediateStop)
            bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else
            bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        print(busPath + " STOP ALL EVENTS");

    }

    public void Pause()
    {
        bus.setPaused(true);

        print(busPath + " PAUSE");
    }

    public void UnPause()
    {
        bus.setPaused(false);

        print(busPath + " UNPAUSE");
    }
}
