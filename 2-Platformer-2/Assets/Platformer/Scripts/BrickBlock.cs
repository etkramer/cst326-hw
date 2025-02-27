using UnityEngine;
using UnityEngine.EventSystems;

public class BrickBlock : Block
{
    public override void OnBlockUse()
    {
        GameManager.s_instance.NumPoints += 100;
        Destroy(gameObject);
    }
}
