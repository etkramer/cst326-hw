using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
