using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // Update is called once per frame
    void Update()
    {
        int timeLeft = Mathf.Max(300 - (int)Time.time, 0);
        timerText.text = $"{timeLeft}";
    }
}
