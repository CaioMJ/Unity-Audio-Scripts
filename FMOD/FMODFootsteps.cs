using UnityEngine;

public class FMODFootsteps : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _footstepsEventPath;
    [SerializeField] private LayerMask _layerMask;
    private float _distance = 1f;
    private float _material;

    //TESTING
    private float _timer;
    private float _footstepLoopTime = 1;

    private void Update()
    {
        //TESTING
        _timer += Time.deltaTime;
        if(_timer > _footstepLoopTime)
        {
            PlayFootstep();
            _timer = 0;
        }
    }

    private void CheckTerrain()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _distance, _layerMask);

        if(hit.collider != null)
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Terrain1":
                    _material = 0;
                    break;
                case "Terrain2":
                    _material = 1;
                    break;
                case "Terrain3":
                    _material = 2;
                    break;
                case "Terrain4":
                    _material = 3;
                    break;
                case "Terrain5":
                    _material = 4;
                    break;
            }
            Debug.Log(hit.collider.tag);
        }
    }

    public void PlayFootstep()
    {
        CheckTerrain();
        FMOD.Studio.EventInstance footstepsEventInstance = FMODUnity.RuntimeManager.CreateInstance(_footstepsEventPath);
        footstepsEventInstance.setParameterByName("Terrain", _material);
        footstepsEventInstance.start();
        footstepsEventInstance.release();
    }
}
