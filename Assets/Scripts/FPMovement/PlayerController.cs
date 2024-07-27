using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float grabDistance = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float recoilForce = -400f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask whiteBoardLayer;
    [SerializeField] private Vector3 shotgunPosOffset;
    [SerializeField] private AudioClip shotgunPickup;

    private bool groundedPlayer;
    

    public GameObject itemEquipped {get;private set;}
    private ShotgunFire shotgun;
    private Camera cam;
    private CharacterController controller;    
    private Vector3 playerVelocity;
    private SwitchCamera switchCamera; 

    //Managers
    private InputManager inputManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        switchCamera = GetComponent<SwitchCamera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;    
        inputManager = InputManager.instance;
    }

    void Update()
    {
        MovePlayer();

        if(inputManager.PlayerFired())
        {
            Interact();
        }
        // if(inputManager.PlayerFired() && itemEquipped == null)
        // {
        //     GrabItem();
        // }
        // else if(inputManager.PlayerFired())
        // {
        //     shotgun.Shoot();
        //     DropItem();
        // }
        
        
        
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

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, grabDistance))
        {
            if(whiteBoardLayer == (whiteBoardLayer | (1 << hit.collider.gameObject.layer)))
            {
                switchCamera.SwitchPriority();
                return;
            }
            if(itemLayer == (itemLayer | (1 << hit.collider.gameObject.layer)) && itemEquipped == null)
            {
                GrabItem(hit);
                return;
            } 
        }

        if(shotgun != null)
            {
                shotgun.Shoot();
                DropItem();
            }
    }

    private void GrabItem(RaycastHit hit)
    {
        if(itemLayer == (itemLayer | (1 << hit.collider.gameObject.layer)))
        {
            if(shotgunPickup != null)
                AudioManager.instance.Play3DAudio(shotgunPickup, transform);

            itemEquipped = hit.collider.gameObject.transform.root.gameObject;
            itemEquipped.GetComponent<Rigidbody>().isKinematic = true;
            itemEquipped.transform.parent = cam.transform;
            itemEquipped.transform.localPosition = shotgunPosOffset;
            itemEquipped.transform.localRotation = Quaternion.Euler(0,-5,0);
            shotgun = itemEquipped.GetComponent<ShotgunFire>();
        }
    }


    private void DropItem()
    {
        Rigidbody rb = itemEquipped.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(itemEquipped.transform.forward * recoilForce);
        
        itemEquipped.transform.parent = null;
        itemEquipped = null;
        shotgun = null;
    }
}
