using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public int numHits = 0;

    public float speedInitial;
    public float speedIncrement;

    PowerUp currentPowerUp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set initial velocity
        rb.linearVelocity = new Vector3(
            speedInitial * (GameManager.s_instance.LastScoringPlayer == 2 ? -1 : 1),
            0,
            0
        );
    }

    public void ActivatePowerUp(PowerUp powerup)
    {
        // ReferenceEquals to avoid Unity being weird about disposed objects
        if (!ReferenceEquals(currentPowerUp, null))
        {
            StopAllCoroutines();
        }

        // Enable ball trail
        GetComponent<TrailRenderer>().material = powerup.GetComponent<Renderer>().material;
        GetComponent<TrailRenderer>().emitting = true;

        currentPowerUp = powerup;
        StartCoroutine(powerup.GetActivateCoroutine(this));
    }

    public float GetHitSoundPitch()
    {
        // Let powerups set their own pitch
        if (!ReferenceEquals(currentPowerUp, null) && currentPowerUp.hitSoundPitchOverride != -1)
        {
            return currentPowerUp.hitSoundPitchOverride;
        }

        // Compute pitch from num hits
        var pitchRatio = Mathf.Clamp(numHits / 2, 0, 5) / 6f;
        return pitchRatio + 1;
    }
}
