using UnityEngine;

public class Slot : MonoBehaviour
{
    public int slotNumber;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} entered slot {slotNumber}");
    }
}
