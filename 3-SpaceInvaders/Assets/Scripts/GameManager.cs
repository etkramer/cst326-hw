using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<float> OnGameSpeedChanged;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public GameObject playerPrefab;
    public GameObject enemyGroupPrefab;
    public GameObject barricadesPrefab;
    public float gameSpeedIncrement = 0.1f;

    public Bounds Bounds => GetComponent<BoxCollider2D>().bounds;

    public float GameSpeed
    {
        get => _gameSpeed;
        set
        {
            _gameSpeed = value;
            OnGameSpeedChanged?.Invoke(_gameSpeed);
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = _score.ToString("0000");

            // If this will be our new high score, store it in session state but don't update until next round.
            if (_score > HighScore)
            {
                HighScore = _score;
            }
        }
    }

    public int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            highScoreText.text = _highScore.ToString("0000");

            // Store in editor session state.
            SessionState.SetInt(nameof(HighScore), _highScore);
        }
    }

    private float _gameSpeed = 1f;
    private int _score = 0;
    private int _highScore = 0;
    private GameObject _player;
    private GameObject _enemyGroup;
    private GameObject _barricades;

    void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    void Start()
    {
        // Load high score.
        HighScore = SessionState.GetInt(nameof(HighScore), 0);

        ResetGame();
    }

    public void ResetGame()
    {
        Debug.Log("New game");

        Score = 0;
        GameSpeed = 1f;

        // (Re)spawn player
        if (_player)
        {
            Destroy(_player);
        }
        _player = Instantiate(playerPrefab);
        _player.transform.position = new Vector3(-4.5f, -4, 0);

        // (Re)spawn enemies
        if (_enemyGroup)
        {
            Destroy(_enemyGroup);
        }
        _enemyGroup = Instantiate(enemyGroupPrefab);
        _enemyGroup.transform.position = new Vector3(0, 2.5f, 0);

        // (Re)spawn barricades
        if (_barricades)
        {
            Destroy(_barricades);
        }
        _barricades = Instantiate(barricadesPrefab);
        _barricades.transform.position = new Vector3(0, -2.75f, 0);
    }
}
