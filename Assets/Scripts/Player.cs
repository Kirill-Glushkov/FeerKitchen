using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance
    {
        get; private set;
    }
	
	public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter _selectedCounter;
    }

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _moveRotation = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private KitchenObject _kitchenObject;

    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more Player");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
		
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
		if(!KitchenGameManager.Instance.IsGamePlaying()) return;
		
        Debug.Log("Alternative Player");
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractorAlternate(this);
            Debug.Log("InteractorRull");
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
		if(!KitchenGameManager.Instance.IsGamePlaying()) return;
		
        if(_selectedCounter != null)
        {
            _selectedCounter.Interact(this);

        }

    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }  

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }
        float interactDistance = 2f;

        if(Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, _counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                 SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
     
    }

    private void HandleMovement()
    {
            Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            float moveDistance = _moveSpeed * Time.deltaTime;
            float playerRadius = 0.5f;
            float playerHeight = 2f;
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                    else
                    {

                    }

                }
            }

            if (canMove)
            {
                transform.position += moveDir * moveDistance;
            }

            _isWalking = moveDir != Vector3.zero;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _moveRotation);    
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this._selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            _selectedCounter = _selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;
		
		if(kitchenObject != null)
		{
			OnPickedSomething?.Invoke(this, EventArgs.Empty);
		}
	}
    public KitchenObject GetKitchenObject() { return _kitchenObject; }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
