using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;

    PowerUp currentPowerUp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ActivatePowerUp(PowerUp powerup)
    {
        if (currentPowerUp != null)
        {
            StopAllCoroutines();
        }

        currentPowerUp = powerup;
        StartCoroutine(powerup.GetCoroutine(this));
    }
}
