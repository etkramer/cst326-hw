using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static List<BaseCounter> All { get; } = new();

    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        All.Clear();
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField]
    private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Awake()
    {
        All.Add(this);
    }

    public virtual void Interact(ICharacter player) { }

    public virtual void InteractAlternate(ICharacter player) { }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
