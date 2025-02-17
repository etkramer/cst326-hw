using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;

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

    void Start()
    {
        Debug.Assert(s_instance == null);
        s_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft = Mathf.Max(300 - (int)Time.time, 0);
        timerText.text = timeLeft.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                var blockComponent = hit.collider.GetComponent<Block>();
                if (blockComponent != null)
                {
                    blockComponent.OnBlockClicked();
                }
            }
        }
    }
}
