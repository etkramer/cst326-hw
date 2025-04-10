using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour, ICharacter
{
    enum RobotState
    {
        Idle,
        FetchingPlate,
        FetchingIngredient,
        DeliveringPlate,
    }

    [SerializeField]
    private Transform kitchenObjectHoldPoint;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private RobotState state;
    private KitchenObject kitchenObject;

    // RobotState.FetchingIngredient
    private BaseCounter fetchIngredientTargetCounter;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        var nonEmptyCounters = BaseCounter.All.Where(counter => counter.HasKitchenObject());

        // Fetch various game state info
        var hasPlate = GetKitchenObject() is PlateKitchenObject;
        var isPlateAvailable = PlatesCounter.Instance.HasPlate();

        // Do we have any complete recipes ready to assemble?
        var nextIngredientCounter = default(BaseCounter);
        var canDeliverPlate = false;
        foreach (var recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            var counters = new List<BaseCounter>();
            var isRecipeReady = true;
            foreach (var neededItemSO in recipeSO.kitchenObjectSOList)
            {
                // Is there a counter with this item sitting on it?
                var counterWithItem = nonEmptyCounters.FirstOrDefault(counter =>
                    counter.GetKitchenObject().GetKitchenObjectSO() == neededItemSO
                );

                // Couldn't find a counter with this ingredient.
                if (counterWithItem == null)
                {
                    // Unless we already have this item on a plate, the recipe isn't ready.
                    if (
                        GetKitchenObject() is not PlateKitchenObject plate
                        || !plate
                            .GetKitchenObjectSOList()
                            .Any(plateItem => plateItem == neededItemSO)
                    )
                    {
                        isRecipeReady = false;
                        break;
                    }
                }
                else
                {
                    counters.Add(counterWithItem);
                }
            }

            if (isRecipeReady)
            {
                // Still have more counters to fetch from
                if (counters.Any())
                {
                    // Choose closest counter as target
                    nextIngredientCounter = counters
                        .OrderBy(counter =>
                            (counter.transform.position - transform.position).magnitude
                        )
                        .First();

                    break;
                }
                // We already have the complete recipe on our plate
                else
                {
                    canDeliverPlate = true;
                    nextIngredientCounter = null;

                    break;
                }
            }
        }

        // Main state machine
        const float interactRange = 0.1f;
        switch (state)
        {
            case RobotState.Idle:
                // Make sure we're stopped
                navMeshAgent.isStopped = true;

                // Should we fetch a plate?
                if (!hasPlate && isPlateAvailable)
                {
                    // Start fetching the plate
                    state = RobotState.FetchingPlate;
                    MoveTo(PlatesCounter.Instance.transform.position);
                    break;
                }

                // Do we have a plate, *and* a recipe to target?
                if (hasPlate && nextIngredientCounter != null)
                {
                    // Start fetching the first ingredient
                    state = RobotState.FetchingIngredient;
                    fetchIngredientTargetCounter = nextIngredientCounter;
                    MoveTo(fetchIngredientTargetCounter.transform.position);
                }

                // Do we have a plate containing a complete requested recipe?
                if (hasPlate && canDeliverPlate)
                {
                    state = RobotState.DeliveringPlate;
                    MoveTo(DeliveryCounter.Instance.transform.position);
                }

                break;

            case RobotState.FetchingPlate:
                // Reached plate counter?
                if (navMeshAgent.remainingDistance <= interactRange)
                {
                    // Pick up plate
                    PlatesCounter.Instance.Interact(this);

                    // Return to idle
                    state = RobotState.Idle;
                }

                break;

            case RobotState.FetchingIngredient:
                // Reached target counter?
                if (navMeshAgent.remainingDistance <= interactRange)
                {
                    // Pick up ingredient
                    fetchIngredientTargetCounter.Interact(this);

                    // Reset target counter
                    fetchIngredientTargetCounter = null;

                    // Return to idle
                    state = RobotState.Idle;
                }

                break;

            case RobotState.DeliveringPlate:
                // Reached delivery counter?
                if (navMeshAgent.remainingDistance <= interactRange)
                {
                    // Deliver plate
                    DeliveryCounter.Instance.Interact(this);

                    // Return to idle
                    state = RobotState.Idle;
                }

                break;
        }

        // Update animation
        animator.SetBool("IsMoving", !navMeshAgent.isStopped);

        // Debug.Log(state);
    }

    void MoveTo(Vector3 location)
    {
        // Set destination
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(location);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
