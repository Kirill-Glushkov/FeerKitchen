using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO _plateKitchenOnjectSO;

    private float _spawnPlateTimerMax = 4f;
    private float _spawnPlateTimer;
    private int _platesSpawnedAmount;
    private int _platesSpawnedAmountMax = 4;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if(_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if(KitchenGameManager.Instance.IsGamePlaying() && _platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                _platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {

        if(_platesSpawnedAmount > 0)
        {
            _platesSpawnedAmount--;

            KitchenObject.SpawnKitchenObject(_plateKitchenOnjectSO, player);

            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
        }
    }
}
