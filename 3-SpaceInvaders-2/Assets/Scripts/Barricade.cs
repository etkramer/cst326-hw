using UnityEngine;

public class Barricade : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only collide with bullets.
        if (!other.GetComponent<Bullet>())
        {
            return;
        }

        // Destroy self
        Destroy(gameObject);
    }
}
