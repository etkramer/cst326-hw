using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField]
    private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new();
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                var waitingRecipeSO = recipeListSO.recipeSOList[
                    UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)
                ];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (var i = 0; i < recipeListSO.recipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (
                waitingRecipeSO.kitchenObjectSOList.Count
                == plateKitchenObject.GetKitchenObjectSOList().Count
            )
            {
                // Has same number of ingredients

                var plateContentsMatchRecipe = true;
                foreach (var recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    var ingredientFound = false;

                    // Cycling through all ingredients in the recipe
                    foreach (
                        var plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()
                    )
                    {
                        // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // Recipe ingredient not found on plate
                        plateContentsMatchRecipe = false;
                    }
                }

                if (plateContentsMatchRecipe)
                {
                    // Player delivered correct recipe
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // No matches found
        // Player not delivering a correct recipe
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
