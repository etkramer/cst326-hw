using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI pointText;

    public int timeLimit = 100;
    public int timeReachedGoal = -1;

    public bool hasHitTimeLimit = false;

    private int _numCoins;
    public int NumCoins
    {
        get => _numCoins;
        set
        {
            _numCoins = value;
            coinText.text = _numCoins.ToString();
        }
    }

    private int _numPoints;
    public int NumPoints
    {
        get => _numPoints;
        set
        {
            _numPoints = value;
            pointText.text = _numPoints.ToString().PadLeft(6, '0');
        }
    }

    void Start()
    {
        Debug.Assert(s_instance == null);
        s_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft = Mathf.Max(
            timeLimit
                - (
                    timeReachedGoal == -1
                        ? (int)Time.time
                        : Math.Min((int)Time.time, timeReachedGoal)
                ),
            0
        );
        timerText.text = timeLeft.ToString();

        if (timeLeft <= 0 && !hasHitTimeLimit)
        {
            Debug.Log("Player failed");
            hasHitTimeLimit = true;
        }
    }
}
