using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera deathCamera;
    public Camera playerCamera;




    public Transform target;
    public Vector3 offset;   // The offset between the camera and the player

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position - target.forward * offset.z + target.up * offset.y;

            deathCamera.transform.position = desiredPosition;
            deathCamera.transform.rotation = target.rotation;
        }
    }
  
    public void Start(){
        firstPerson();
    }
    public void thirdPerson(){
        playerCamera.enabled = false;
        deathCamera.enabled = true;
    }

    public void firstPerson(){
        playerCamera.enabled = true;
        deathCamera.enabled = false;
    }

    public void AssignTarget(GameObject player){
        target = player.transform;
    }
    public void AssignPlayerCamera(Camera playerCameras)
    {
        playerCamera = playerCameras;
        deathCamera.enabled = false;
        playerCamera.enabled = true;     }
}
