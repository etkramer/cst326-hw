using System.Collections;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public override IEnumerator GetCoroutine(Ball ball)
    {
        // Speed up ball 2x
        ball.GetComponent<Rigidbody>().linearVelocity *= 2f;

        yield return null;
    }
}
