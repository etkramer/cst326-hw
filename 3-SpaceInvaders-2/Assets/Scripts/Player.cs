using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnTransform;

    public float moveSpeed = 1;

    public AudioClip deathClip;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameManager.Instance.isPlayerAlive)
        {
            // No input during death animation.
            return;
        }

        // Firing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Fire a bullet.
            Instantiate(
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

        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        // Destroy self
        GameManager.Instance.isPlayerAlive = false;
        audioSource.PlayOneShot(deathClip);
        animator.SetBool("IsDead", true);
        animator.speed = 0.2f;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.75f);

        // End game.
        GameManager.Instance.EndGame();
    }
}
