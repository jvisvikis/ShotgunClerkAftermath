using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunRack : Interactable
{
    [SerializeField] private GameObject shotgunPrefab;

    public override void Interact()
    {
        player.TakeShotgun(shotgunPrefab);
    }
}
