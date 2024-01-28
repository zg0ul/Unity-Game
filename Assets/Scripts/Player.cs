using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter; 
    }

    // SerializeField and private accessor makes the variable visible in the inspector, but not in other scripts
    [SerializeField] private float moveSpeed = 7f; // The speed at which the player moves
    [SerializeField] private GameInput gameInput; // The GameInput script that we created earlier
    [SerializeField] private LayerMask countersLayerMask;
    
    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        // in the Raycast function, the direction vector (moveDir) would be zero if we stop moving, which would shoot a Raycast nowhere, therefore if we stop moving we won't be able to detect the object anymore, a fix for that is to save the last direction before we stop moving and use that to detect an object.
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                // Has ClearCounter
                if (clearCounter != selectedCounter) {
                    // to keep track of the selected counter
                    SetSelectedCounter(clearCounter);
                }
            } else {
                // if the player interacts with an object that isn't a counter or doesn't have the clearCounter script then reset selectedCounter to null
                SetSelectedCounter(null);
            }
        } else {
            // if the player isn't interacting with a clearCounter then we reset selectedCounter to being null
            SetSelectedCounter(null);
        }

        Debug.Log(selectedCounter);
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); // Gets the input vector from the GameInput script

        // make sure that the player moves on the x axis and z axis (instead of y axis)
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            // Time.deltaTime is the time between each frame, multiplying it by the moveDir vector makes sure that the player moves at the same speed on all computers and doesn't move faster with higher fps
            transform.position += moveDir * moveDistance; // moves the player
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f; // the speed at which the player rotates
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // rotates the player to face the direction it's moving in
    }

    private void SetSelectedCounter(ClearCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
                    {
                        selectedCounter = this.selectedCounter
                    });
    }
}
