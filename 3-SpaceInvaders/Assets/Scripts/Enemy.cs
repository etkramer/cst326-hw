using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int scoreValue = 0;
    public float minTimeToFire = 1f;
    public float maxTimeToFire = 60f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        GameManager.OnGameSpeedChanged += OnGameSpeedChanged;
        OnGameSpeedChanged(GameManager.Instance.GameSpeed);

        // Schedule firing.
        StartCoroutine(MainCoroutine());
    }

    IEnumerator MainCoroutine()
    {
        // Enemies fire back sometimes (random, varying interval).
        yield return new WaitForSeconds(
            Random.Range(minTimeToFire, maxTimeToFire) / (GameManager.Instance.GameSpeed * 2)
        );

        if (this.gameObject)
        {
            // Fire a bullet.
            GameObject shot = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnGameSpeedChanged(float gameSpeed)
    {
        if (animator)
        {
            animator.speed = 0.1f * GameManager.Instance.GameSpeed;
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
        Destroy(gameObject);
    }
}
