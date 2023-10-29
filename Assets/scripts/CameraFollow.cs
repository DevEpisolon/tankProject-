using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform tankArm;

    private void Update()
    {
        // Match the camera's position and rotation to the tank's arm
        // transform.position = tankArm.position;
        transform.rotation = tankArm.rotation;
    }
}
