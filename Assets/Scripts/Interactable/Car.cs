using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Interactable
{
    public override void Interact()
    {
        GameManager.instance.GoHeist();
    }
}
