using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float bulletLifetime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        // Set velocity at spawn.
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * speed;

        // Schedule destruction.
        StartCoroutine(MainCoroutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy self
        Destroy(gameObject);
    }

    IEnumerator MainCoroutine()
    {
        // Bullets have limited lifetime.
        yield return new WaitForSeconds(bulletLifetime);
        if (this.gameObject)
        {
            Destroy(this.gameObject);
        }
    }
}
