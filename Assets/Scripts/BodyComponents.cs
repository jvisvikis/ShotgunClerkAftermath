using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyComponents", menuName = "ScriptableObjects/BodyComponents")]
public class BodyComponents : ScriptableObject
{
    public float yOffset;
    public GameObject [] parts;
    public int partsIdx;
}
