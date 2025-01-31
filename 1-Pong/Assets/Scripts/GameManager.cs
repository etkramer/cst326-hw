using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public GameObject ballPrefab;
    public GameObject paddle1;
    public GameObject paddle2;

    public float ballSpeedInitial;
    public float ballSpeedIncrement;

    public Vector2 playArea;

    GameObject ball;

    int score1;
    int score2;
    int lastScored;

    void Start()
    {
        s_instance = this;
        NewGame();
    }

    void Update()
    {
        // Has ball left play area?
        if (ball.transform.position.x < playArea.x / -1)
        {
            score2++;
            lastScored = 2;
            Debug.Log($"Player 2 scored. New score is {score1}:{score2}");
            NewRound();
        }
        else if (ball.transform.position.x > playArea.x / 1)
        {
            score1++;
            lastScored = 1;
            Debug.Log($"Player 1 scored. New score is {score1}:{score2}");
            NewRound();
        }

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
        lastScored = 2;
        NewRound();
    }

    void NewRound()
    {
        ResetBall();
        ResetPaddles();
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
        if (ball != null)
        {
            Destroy(ball);
        }

        // Spawn ball
        ball = Instantiate(ballPrefab);

        // Set ball initial velocity
        var rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(ballSpeedInitial * (lastScored == 2 ? -1 : 1), 0, 0);
    }
}
