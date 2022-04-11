using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_Occlusion : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private FMOD_SetParameterValue fmodParameter;
    private GameObject listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 direction = listener.transform.position - transform.position;


        Debug.DrawRay(transform.position, direction);

        if (Physics.Raycast(transform.position, direction, direction.magnitude, layerMask))
        {
            fmodParameter.SetValue(0);
        }
        else
        {
            fmodParameter.SetValue(1);
        }
    }
}
