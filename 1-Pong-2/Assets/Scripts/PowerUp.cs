using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected Ball ball;

    void OnTriggerEnter(Collider other)
    {
        ball = other.GetComponent<Ball>();
        if (ball == null)
        {
            return;
        }

        ball.ActivatePowerUp(this);

        // Enable ball trail
        other.GetComponent<TrailRenderer>().emitting = true;
        other.GetComponent<TrailRenderer>().material = GetComponent<Renderer>().material;

        // Destroy self
        Destroy(gameObject);
    }

    public abstract IEnumerator GetCoroutine(Ball ball);
}
