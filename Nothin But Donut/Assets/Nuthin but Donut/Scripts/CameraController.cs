using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public Transform ballTransform;

    // Update is called once per frame
    void Update()
    {
        cam.transform.LookAt(ballTransform.position);
    }
}
