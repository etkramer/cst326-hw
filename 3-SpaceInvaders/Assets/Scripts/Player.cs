using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnTransform;

    public float moveSpeed = 1;

    void Update()
    {
        // Firing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Fire a bullet.
            GameObject shot = Instantiate(
                bulletPrefab,
                bulletSpawnTransform.position,
                Quaternion.identity
            );
        }

        // Movement
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only collide with other bullets.
        if (!other.GetComponent<Bullet>())
        {
            return;
        }

        // End game.
        GameManager.Instance.ResetGame();
    }
}
