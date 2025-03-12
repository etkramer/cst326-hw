using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float durationSeconds = 5;

    void Start()
    {
        StartCoroutine(MainCoroutine());
    }

    IEnumerator MainCoroutine()
    {
        // Return to menu after (durationSeconds) seconds.
        yield return new WaitForSeconds(durationSeconds);
        SceneManager.LoadScene("MenuScene");
    }
}
