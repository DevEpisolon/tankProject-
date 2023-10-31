using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform tankArm;

    private void Update()
    {
        transform.rotation = tankArm.rotation;
    }
}
