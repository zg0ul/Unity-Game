using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // SerializeField and private accessor makes the variable visible in the inspector, but not in other scripts
    [SerializeField] private float moveSpeed = 7f; // The speed at which the player moves

    private bool isWalking;

    private void Update() {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x += 1;
        } 

        inputVector = inputVector.normalized; // Normalize the vector so that the player doesn't move faster diagonally
        
        // make sure that the player moves on the x axis and z axis (instead of y axis)
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        // Time.deltaTime is the time between each frame, multiplying it by the moveDir vector makes sure that the player moves at the same speed on all computers and doesn't move faster with higher fps
        transform.position += moveDir * moveSpeed * Time.deltaTime; // moves the player

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f; // the speed at which the player rotates
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // rotates the player to face the direction it's moving in
    }

    public bool IsWalking() {
        return isWalking;
    }
}
