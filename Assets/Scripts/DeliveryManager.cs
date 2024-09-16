using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance;

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    [SerializeField] private RecipeListSO _recipeListSO;

    private List<RecipeSO> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipesMax = 4;
	private int _successfulRecipesAmount;
	

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new List<RecipeSO>();    
    }

    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if(_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && _waitingRecipeSOList.Count < _waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = _recipeListSO._resipeSOList[UnityEngine.Random.Range(0, _recipeListSO._resipeSOList.Count)];
                _waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }

        } 
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeSOList[i];

            if(waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateCountentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateCountentsMatchesRecipe = false;
                    }
                }

                if (plateCountentsMatchesRecipe)
                {
					_successfulRecipesAmount++;
					
                    _waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeSOList;
    }
	
	public int GetSuccessfulRecipesAmount()
	{
		return _successfulRecipesAmount;
	}
}
