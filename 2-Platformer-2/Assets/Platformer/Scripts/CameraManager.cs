using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public float speed = 1;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
