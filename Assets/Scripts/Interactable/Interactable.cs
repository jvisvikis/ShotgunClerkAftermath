using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public PlayerInteract player {get;private set;}
    private string interactableUIText = "Use";

    void Start()
    {
        player = FindObjectOfType<PlayerInteract>().GetComponent<PlayerInteract>();
    }

    public abstract void Interact();

    public void ShowInteractableText()
    {
        UIManager.instance.SetUseText(interactableUIText);
    }
}
