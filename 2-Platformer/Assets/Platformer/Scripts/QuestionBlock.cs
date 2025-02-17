using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionBlock : Block
{
    public override void OnBlockClicked()
    {
        GameManager.s_instance.NumCoins += 1;
    }
}
