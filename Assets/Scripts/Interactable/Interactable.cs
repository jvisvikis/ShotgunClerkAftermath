using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public PlayerController player {get;private set;}

    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }

    public abstract void Interact();
}
