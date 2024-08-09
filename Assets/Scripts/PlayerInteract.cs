using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float grabDistance = 5f;
    [SerializeField] private float recoilForce = -400f;

    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask crewLayer;
    [SerializeField] private Vector3 shotgunPosOffset = new Vector3(0.7f,-0.3f,0.6f);
    [SerializeField] private Vector3 shotgunPlacementPos = new Vector3(0.05f, 1f, -0.19f); //Placement for crew member
    [SerializeField] private Vector3 shotgunPlacementRot = new Vector3(35f, -88.726f, -100.713f); //Placement for crew member
    [SerializeField] private AudioClip shotgunPickup;
    [SerializeField] private string [] handOverLines;
    [SerializeField] private AudioClip [] handOverAudioLines;

    public bool usingWhiteboard {get; set;}
    private bool shotOnce;
    private bool handOverOnce;
    private bool playedAudio;

    public GameObject itemEquipped {get;private set;}
    private ShotgunFire shotgun;
    private Camera cam;
    private SwitchCamera switchCamera; 

    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        switchCamera = GetComponent<SwitchCamera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;    
        inputManager = InputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.PlayerFired() && !usingWhiteboard) Interact();
        if(inputManager.PlayerExited() && usingWhiteboard) StopUsingWhiteBoard();
        if(inputManager.PlayerHandedOver() && itemEquipped != null) HandOver();  
    }

    void FixedUpdate()
    {
        UIManager.instance.SetUseText("");
        if(usingWhiteboard) return;
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, grabDistance))
        {
            if(interactableLayer == (interactableLayer | (1 << hit.collider.gameObject.layer)))
            {
                hit.collider.gameObject.GetComponent<Interactable>().ShowInteractableText();
                return;
            }
            if(itemLayer == (itemLayer | (1 << hit.collider.gameObject.layer)) && itemEquipped == null)
            {
                UIManager.instance.SetUseText("Grab");
                return;
            } 
            if(crewLayer == (crewLayer | (1 << hit.collider.gameObject.layer)) && itemEquipped != null)
            {
                UIManager.instance.SetUseText("Give");
                if(shotOnce) UIManager.instance.SetUseText("H to give");
                return;
            } 
        } 
        
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, grabDistance))
        {
            if(interactableLayer == (interactableLayer | (1 << hit.collider.gameObject.layer)))
            {
                hit.collider.gameObject.GetComponent<Interactable>().Interact();
                return;
            }
            if(itemLayer == (itemLayer | (1 << hit.collider.gameObject.layer)) && itemEquipped == null)
            {
                GrabItem(hit.collider.gameObject.transform.root.gameObject);
                return;
            } 
        }

        if(shotgun != null)
            {
                shotOnce = true;
                shotgun.Shoot();
                DropItem();
            }
    }

    private void GrabItem(GameObject item)
    {
        if(shotgunPickup != null)
            AudioManager.instance.Play3DAudio(shotgunPickup, transform);

        itemEquipped = item;
        itemEquipped.GetComponent<Rigidbody>().isKinematic = true;
        itemEquipped.transform.parent = cam.transform;
        itemEquipped.transform.localPosition = shotgunPosOffset;
        itemEquipped.transform.localRotation = Quaternion.Euler(0,-5,0);
        shotgun = itemEquipped.GetComponent<ShotgunFire>();
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

    private void HandOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, grabDistance))
        {
            if(crewLayer == (crewLayer | (1 << hit.collider.gameObject.layer)))
            {
                CrewMember member = hit.collider.gameObject.GetComponent<CrewMember>();
                member.hasShotgun = true;
                itemEquipped.transform.parent = member.transform;
                itemEquipped.transform.localPosition = shotgunPlacementPos;
                itemEquipped.transform.localRotation = Quaternion.Euler(shotgunPlacementRot);
                itemEquipped.GetComponent<Recoil>().enabled = false;
                itemEquipped = null;
                shotgun = null;
                if(!handOverOnce && !GameManager.instance.fullTutPlayed)
                {
                    handOverOnce = true;
                    StartCoroutine(FindObjectOfType<Father>().StartTalking(handOverLines,handOverAudioLines,0,2));
                    GameManager.instance.fullTutPlayed = true;
                }

            }
        }
    }

    public void StartUsingWhiteBoard()
    {
        usingWhiteboard = true;
        if(itemEquipped != null)itemEquipped.SetActive(false);
        switchCamera.SwitchPriority();        
    }

    public void StopUsingWhiteBoard()
    {
        usingWhiteboard = false;
        if(itemEquipped != null)itemEquipped.SetActive(true);
        switchCamera.SwitchPriority();
        UIManager.instance.ToggleCrosshairVisibility();
    }

    public void TakeShotgun(GameObject shotgun)
    {
        shotgun = Instantiate(shotgun);
        GrabItem(shotgun);
    }
}
