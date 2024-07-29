using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : Interactable
{
    
    public override void Interact()
    {
        player.StartUsingWhiteBoard();
        UIManager.instance.ToggleCrosshairVisibility();
    }
}
