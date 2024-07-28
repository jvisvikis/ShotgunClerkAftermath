using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] private BodyComponents heads;
    [SerializeField] private BodyComponents upperBodies;
    [SerializeField] private BodyComponents lowerBodies;
    [SerializeField] private GameObject blankCharacter;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject upperBody;
    [SerializeField] private GameObject lowerBody;

    public void ChangeHead(bool next)
    {
        heads.partsIdx = next ? ++heads.partsIdx : --heads.partsIdx;
        Debug.Log(heads.partsIdx);
        Destroy(head);
        head = Instantiate(heads.parts[heads.partsIdx%heads.parts.Length], blankCharacter.transform);
        head.transform.localPosition = new Vector3(0,heads.yOffset,0);
    }

    public void ChangeUpperBody(bool next)
    {
        Destroy(upperBody);
        upperBodies.partsIdx = next ? ++upperBodies.partsIdx : --upperBodies.partsIdx;
        upperBody = Instantiate(upperBodies.parts[upperBodies.partsIdx%upperBodies.parts.Length], blankCharacter.transform);
        upperBody.transform.localPosition = new Vector3(0,upperBodies.yOffset,0);
    }

    public void ChangeLowerBody(bool next)
    {
        Destroy(lowerBody);
        lowerBodies.partsIdx = next ? ++lowerBodies.partsIdx : --lowerBodies.partsIdx;
        lowerBody = Instantiate(lowerBodies.parts[lowerBodies.partsIdx%lowerBodies.parts.Length], blankCharacter.transform);
        lowerBody.transform.localPosition = new Vector3(0,lowerBodies.yOffset,0);
    }

    public void CreateCharacter()
    {
        blankCharacter.transform.position = Vector3.zero;
        blankCharacter.transform.parent = null;
        blankCharacter = Instantiate(new GameObject(), gameObject.transform);
        blankCharacter.transform.localPosition = Vector3.zero; 
    }


}
