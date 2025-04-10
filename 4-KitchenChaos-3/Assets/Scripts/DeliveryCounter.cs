public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    public override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    public override void Interact(ICharacter player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
