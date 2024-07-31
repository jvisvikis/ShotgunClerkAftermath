using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] private AudioClip fatherCreationLine;
    [SerializeField] private AudioClip afterCreationAudioLine;
    [SerializeField] private string afterCreationLineString;
    [SerializeField] private BodyComponents heads;
    [SerializeField] private BodyComponents upperBodies;
    [SerializeField] private BodyComponents lowerBodies;
    [SerializeField] private GameObject blankCharacterPrefab;
    private bool intro;
    private GameObject blankCharacter;
    private GameObject head;
    private GameObject upperBody;
    private GameObject lowerBody;
    private PlayerInteract player;

    void Awake()
    {
        player = FindObjectOfType<PlayerInteract>().GetComponent<PlayerInteract>();
        ResetComponents();
    }
    public void ChangeHead(bool next)
    {
        Vector3 oldPos = Vector3.zero;
        if(head != null ) oldPos = head.transform.localPosition;
        Destroy(head);
        heads.partsIdx = next ? ++heads.partsIdx : --heads.partsIdx;
        head = Instantiate(heads.parts[heads.partsIdx%heads.parts.Length], blankCharacter.transform);
        if(upperBody != null) head.transform.parent = upperBody.transform;
        head.transform.localPosition = oldPos;        
    }

    public void ChangeUpperBody(bool next)
    {
        Vector3 oldPos = Vector3.zero;
        if(head != null) head.transform.parent = blankCharacter.transform;
        if(upperBody != null) oldPos = upperBody.transform.localPosition;
        Destroy(upperBody);
        upperBodies.partsIdx = next ? ++upperBodies.partsIdx : --upperBodies.partsIdx;
        upperBody = Instantiate(upperBodies.parts[upperBodies.partsIdx%upperBodies.parts.Length], blankCharacter.transform);
        upperBody.transform.localPosition = oldPos;
        if(lowerBody != null) upperBody.transform.parent = lowerBody.transform;
        if(head == null) return;
        head.transform.parent = upperBody.transform;
        head.transform.localPosition = Vector3.zero;
        if(upperBody.name.Contains("long")) head.transform.localPosition = new Vector3(0,upperBodies.longYOffset,0);
        if(upperBody.name.Contains("short")) head.transform.localPosition = new Vector3(0,upperBodies.shortYOffset,0);
        
    }

    public void ChangeLowerBody(bool next)
    {
        if(upperBody != null) upperBody.transform.parent = blankCharacter.transform;
        Destroy(lowerBody);
        lowerBodies.partsIdx = next ? ++lowerBodies.partsIdx : --lowerBodies.partsIdx;
        lowerBody = Instantiate(lowerBodies.parts[lowerBodies.partsIdx%lowerBodies.parts.Length], blankCharacter.transform);
        lowerBody.transform.localPosition = Vector3.zero;
        if(upperBody == null) return;
        upperBody.transform.parent = lowerBody.transform;
        upperBody.transform.localPosition = Vector3.zero;
        if(lowerBody.name.Contains("long")) upperBody.transform.localPosition = new Vector3(0,lowerBodies.longYOffset,0);
        if(lowerBody.name.Contains("short")) upperBody.transform.localPosition = new Vector3(0,lowerBodies.shortYOffset,0);

    }

    public void CreateCharacter()
    {
        if(head == null || upperBody == null || lowerBody == null)
        {
            return;
        }
        blankCharacter.transform.position = Vector3.zero;
        blankCharacter.AddComponent<CrewMember>();
        blankCharacter.GetComponent<CrewMember>().upperBody = this.upperBody.transform;
        blankCharacter.layer = 9;
        GameManager.instance.AddAliveMember(blankCharacter.GetComponent<CrewMember>());
        ResetComponents();
        player.StopUsingWhiteBoard();
        AudioManager.instance.Play2DAudio(fatherCreationLine,1f,false,false);
        
        
        if(!intro)
        {
            intro = true;
            StartCoroutine(ContinueAudio());
        }
    }

    public IEnumerator ContinueAudio()
    {
        yield return new WaitForSeconds(fatherCreationLine.length + 1f);
        AudioClip [] audio = {afterCreationAudioLine};
        string [] line = {afterCreationLineString};
        StartCoroutine(FindObjectOfType<Father>().StartTalking(line, audio, 0, 1));
    }

    private void ResetComponents()
    {
        head = null;
        upperBody = null;
        lowerBody = null;
        blankCharacter = Instantiate(blankCharacterPrefab, gameObject.transform);
        blankCharacter.transform.localPosition = Vector3.zero; 
    }
}
