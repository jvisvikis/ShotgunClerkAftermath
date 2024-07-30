using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private bool groundedPlayer;

    private Camera cam;
    private CharacterController controller;    
    private Vector3 playerVelocity; 

    private InputManager inputManager;

    void Start()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;
    }

    void Update()
    {
        MovePlayer();     
    }

    private void MovePlayer()
    {
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cam.transform.forward * move.z + cam.transform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    
}
