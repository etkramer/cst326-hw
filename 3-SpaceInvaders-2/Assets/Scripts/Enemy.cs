using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int scoreValue = 0;
    public float minTimeToFire = 1f;
    public float maxTimeToFire = 60f;

    public AudioClip deathClip;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameManager.OnGameSpeedChanged += OnGameSpeedChanged;
        OnGameSpeedChanged(GameManager.Instance.GameSpeed);

        // Schedule firing.
        StartCoroutine(MainCoroutine());
    }

    void Update()
    {
        if (!GameManager.Instance.isPlayerAlive)
        {
            animator.speed = 0;
        }
    }

    IEnumerator MainCoroutine()
    {
        // Enemies fire back sometimes (random, varying interval).
        yield return new WaitForSeconds(
            Random.Range(minTimeToFire, maxTimeToFire) / (GameManager.Instance.GameSpeed * 2)
        );

        if (gameObject && GameManager.Instance.isPlayerAlive)
        {
            // Fire a bullet.
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnGameSpeedChanged(float gameSpeed)
    {
        if (animator)
        {
            animator.speed = gameSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only collide with other bullets.
        if (!other.GetComponent<Bullet>())
        {
            return;
        }

        // Update score and game speed
        GameManager.Instance.Score += scoreValue;
        GameManager.Instance.GameSpeed += GameManager.Instance.gameSpeedIncrement;

        // Destroy self
        audioSource.PlayOneShot(deathClip, 0.5f);
        animator.SetBool("IsDead", true);
        animator.speed = 0.2f;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.75f);
    }
}
