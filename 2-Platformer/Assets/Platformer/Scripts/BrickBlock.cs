using UnityEngine;
using UnityEngine.EventSystems;

public class BrickBlock : Block
{
    public override void OnBlockClicked()
    {
        Destroy(gameObject);
    }
}
