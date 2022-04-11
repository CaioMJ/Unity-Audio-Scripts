using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseOcclusion : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask; //Obstacles

    private GameObject listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.FindGameObjectWithTag("AudioListener");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = listener.transform.position - transform.position;

        Debug.DrawRay(transform.position, direction);

        if (Physics.Raycast(transform.position, direction, direction.magnitude, layerMask))
        {
            AkSoundEngine.PostEvent("Occlusion_On", this.gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Occlusion_Off", this.gameObject);
        }
    }
}