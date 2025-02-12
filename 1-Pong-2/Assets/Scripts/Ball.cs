using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    public float speedInitial;
    public float speedIncrement;

    PowerUp currentPowerUp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set initial velocity
        rb.linearVelocity = new Vector3(
            speedInitial * (GameManager.s_instance.lastScored == 2 ? -1 : 1),
            0,
            0
        );
    }

    public void ActivatePowerUp(PowerUp powerup)
    {
        if (currentPowerUp != null)
        {
            StopAllCoroutines();
        }

        // Enable ball trail
        GetComponent<TrailRenderer>().material = powerup.GetComponent<Renderer>().material;
        GetComponent<TrailRenderer>().emitting = true;

        currentPowerUp = powerup;
        StartCoroutine(powerup.GetActivateCoroutine(this));
    }
}
