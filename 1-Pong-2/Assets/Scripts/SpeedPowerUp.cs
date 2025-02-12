using System.Collections;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public override IEnumerator GetActivateCoroutine(Ball ball)
    {
        // Speed up ball 2x
        ball.rb.linearVelocity *= 2f;
        yield break;
    }
}
