using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODOcclusion : MonoBehaviour
{
    [SerializeField] private LayerMask occlusioObstacleLayerMask;
    [SerializeField] private FMODUnity.StudioEventEmitter fmodEventEmitter;
    [SerializeField] private bool debugRay;
    private GameObject _listener;

    // Start is called before the first frame update
    void Start()
    {
        _listener = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //Calculates the direction from the emitter to the listener
        Vector3 direction = _listener.transform.position - transform.position;

        //Checks to see if there's an obstruction between emitter and listener...
        if (Physics.Raycast(transform.position, direction, direction.magnitude, occlusioObstacleLayerMask))
        {
            //...if there is an obstruction, set the occlusion parameter value to 1...
            fmodEventEmitter.SetParameter("Occlusion", 1);
        }
        else
        {
            //...if there isn't an obstruction, set occlusion parameter value to 0.
            fmodEventEmitter.SetParameter("Occlusion", 0);
        }

        if (debugRay)
        {
            Debug.DrawRay(transform.position, direction);
        }

    }
}
