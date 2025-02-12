using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance { get; private set; }

    public Ball Ball { get; private set; }
    public int LastScoringPlayer { get; private set; }

    public GameObject ballPrefab;
    public Paddle paddle1;
    public Paddle paddle2;

    public GameObject[] powerUpPrefabs;
    public Transform[] powerUpSpawnPoints;

    public TextMeshProUGUI scoreText1;
    public TextMeshProUGUI scoreText2;

    public Vector2 playArea;

    readonly List<GameObject> powerUps = new();

    int score1;
    int score2;

    void Start()
    {
        s_instance = this;
        NewGame();
    }

    void Update()
    {
        // Has ball left play area?
        if (Ball.transform.position.x < playArea.x / -1)
        {
            score2++;
            LastScoringPlayer = 2;
            ScoreChanged();

            NewRound();
        }
        else if (Ball.transform.position.x > playArea.x / 1)
        {
            score1++;
            LastScoringPlayer = 1;
            ScoreChanged();

            NewRound();
        }
    }

    void ScoreChanged()
    {
        scoreText1.text = score1.ToString();
        scoreText2.text = score2.ToString();

        // Lerp color towards red as score increases
        scoreText1.color = Color.Lerp(new Color(1, 1, 1), new Color(1, 0, 0.28f), score1 / 10f);
        scoreText2.color = Color.Lerp(new Color(1, 1, 1), new Color(1, 0, 0.28f), score2 / 10f);

        if (score1 >= 11)
        {
            Debug.Log("Game Over, Left Paddle Win");
            NewGame();
        }
        else if (score2 >= 11)
        {
            Debug.Log("Game Over, Right Paddle Win");
            NewGame();
        }
    }

    void NewGame()
    {
        score1 = 0;
        score2 = 0;
        LastScoringPlayer = 2;
        NewRound();
        ScoreChanged();
    }

    void NewRound()
    {
        ResetBall();
        ResetPaddles();
        ResetPowerUps();
    }

    void ResetPaddles()
    {
        var pos1 = paddle1.transform.position;
        var pos2 = paddle2.transform.position;

        pos1.y = 0;
        pos2.y = 0;

        paddle1.transform.position = pos1;
        paddle2.transform.position = pos2;
    }

    void ResetBall()
    {
        if (Ball != null)
        {
            Destroy(Ball.gameObject);
        }

        // Spawn ball
        Ball = Instantiate(ballPrefab).GetComponent<Ball>();
    }

    void ResetPowerUps()
    {
        // Destroy existing powerups
        foreach (var powerUp in powerUps)
        {
            if (powerUp)
            {
                Destroy(powerUp);
            }
        }
        powerUps.Clear();

        // Create power-ups at random points
        var spawnPoints = new List<Transform>(powerUpSpawnPoints);
        for (int i = 0; i < Random.Range(2, 6); i++)
        {
            int prefabIdx = i % powerUpPrefabs.Length;
            int spawnIdx = Random.Range(0, spawnPoints.Count);

            // Create random powerup
            var powerUp = Instantiate(powerUpPrefabs[prefabIdx]);
            powerUps.Add(powerUp);

            // Choose random spawn point
            powerUp.transform.position = spawnPoints[spawnIdx].transform.position;
            spawnPoints.RemoveAt(spawnIdx);
        }
    }
}
