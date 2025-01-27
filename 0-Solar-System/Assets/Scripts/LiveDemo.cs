using UnityEngine;

public class LiveDemo : MonoBehaviour
{
    public float yawDegreesPerSecond = 45f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("hello unity world!");
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = GetComponent<Transform>();
        myTransform.Rotate(0, yawDegreesPerSecond * Time.deltaTime, 0);

        Debug.Log("hello unity world update!");
    }
}
