using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField]
    private FryingRecipeSO[] fryingRecipeSOArray;

    [SerializeField]
    private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                        }
                    );

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        burningRecipeSO = GetBurningRecipeSOWithInput(
                            GetKitchenObject().GetKitchenObjectSO()
                        );
                        burningTimer = 0f;

                        OnStateChanged?.Invoke(
                            this,
                            new OnStateChangedEventArgs() { state = state }
                        );

                        OnProgressChanged.Invoke(
                            this,
                            new IHasProgress.OnProgressChangedEventArgs()
                            {
                                progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                            }
                        );
                    }

                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax,
                        }
                    );

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(
                            this,
                            new OnStateChangedEventArgs() { state = state }
                        );

                        OnProgressChanged.Invoke(
                            this,
                            new IHasProgress.OnProgressChangedEventArgs()
                            {
                                progressNormalized = 0f,
                            }
                        );
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(ICharacter player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(
                        GetKitchenObject().GetKitchenObjectSO()
                    );

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = state });
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    if (
                        plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())
                    )
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(
                            this,
                            new OnStateChangedEventArgs() { state = state }
                        );

                        OnProgressChanged.Invoke(
                            this,
                            new IHasProgress.OnProgressChangedEventArgs()
                            {
                                progressNormalized = 0f,
                            }
                        );
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = state });

                OnProgressChanged.Invoke(
                    this,
                    new IHasProgress.OnProgressChangedEventArgs() { progressNormalized = 0f }
                );
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
