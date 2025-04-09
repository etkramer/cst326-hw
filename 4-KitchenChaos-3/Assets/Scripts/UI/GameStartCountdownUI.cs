using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField]
    private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    void Update()
    {
        int countdownNumber = Mathf.CeilToInt(
            KitchenGameManager.Instance.GetCountdownToStartTimer()
        );
        countdownText.text = countdownNumber.ToString();

        if (countdownNumber != previousCountdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
