using System.Collections;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public float speedMult = 2;

    public override IEnumerator GetActivateCoroutine(Ball ball)
    {
        // Speed up ball by multiplier
        ball.rb.linearVelocity *= speedMult;
        yield break;
    }
}
