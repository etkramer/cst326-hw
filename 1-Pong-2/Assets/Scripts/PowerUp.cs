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

        // Destroy self
        Destroy(gameObject);
    }

    public abstract IEnumerator GetActivateCoroutine(Ball ball);
}
