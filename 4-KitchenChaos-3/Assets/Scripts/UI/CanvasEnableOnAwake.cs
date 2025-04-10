using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasEnableOnAwake : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Canvas>().enabled = true;
    }
}
