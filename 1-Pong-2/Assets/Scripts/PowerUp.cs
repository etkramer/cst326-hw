using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float hitSoundPitchOverride = -1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Ball>(out var ball))
        {
            return;
        }

        ball.ActivatePowerUp(this);

        // Destroy self
        Destroy(gameObject);
    }

    public abstract IEnumerator GetActivateCoroutine(Ball ball);
}
