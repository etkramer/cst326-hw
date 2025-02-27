using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionBlock : Block
{
    public override void OnBlockUse()
    {
        GameManager.s_instance.NumCoins++;
        GameManager.s_instance.NumPoints += 100;
    }
}
