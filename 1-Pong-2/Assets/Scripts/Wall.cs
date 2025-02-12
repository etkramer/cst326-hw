using UnityEngine;

public class Wall : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        var speed = rb.linearVelocity.magnitude;

        // Compute new direction
        var newDir = rb.linearVelocity.normalized;
        newDir.y *= -1;

        // Compute final velocity
        rb.linearVelocity = newDir * speed;
    }
}
