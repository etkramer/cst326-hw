using System.Collections;
using UnityEngine;

public class ZigZagPowerUp : PowerUp
{
    public float speed = 2f;
    public float interval = 0.5f;

    float timeNextFlip = 0;

    public override IEnumerator GetCoroutine(Ball ball)
    {
        while (ball)
        {
            var rb = ball.GetComponent<Rigidbody>();

            // Flip direction every (interval) seconds
            if (Time.time >= timeNextFlip)
            {
                speed *= -1;
                timeNextFlip = Time.time + interval;
            }

            var moveDelta = new Vector3(0, speed * Time.deltaTime, 0);

            // Don't move the ball through surfaces
            if (!ball.rb.SweepTest(moveDelta, out _, Mathf.Abs(moveDelta.y)))
            {
                // Move ball vertically
                var pos = ball.transform.position;
                ball.transform.position = pos + moveDelta;
            }

            yield return null;
        }
    }
}
