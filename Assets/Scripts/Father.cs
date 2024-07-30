using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Father : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private float rotSpeed = 3f;
    [SerializeField] private string [] voiceLines;
    [SerializeField] private string [] afterCreationLines;
    [SerializeField] private AudioClip [] audioClips;
    [SerializeField] private AudioClip [] afterCreationAudioClips;

    private AudioManager audioManager;
    private GameManager gameManager;

    private bool startedTalking;
    private bool stoppedTalking;
    private bool stopCondition;
    private TextDisplay textDisplay;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        gameManager = GameManager.instance;
        textDisplay = UIManager.instance.textDisplay;
        player = FindObjectOfType<PlayerController>().gameObject;

        animator.SetBool("Talking", true); 
        StartCoroutine(StartTalking(voiceLines, audioClips, 0, audioClips.Length));
    }

    void Update()
    {
        FacePlayer();
    }

    private void FacePlayer()
    {
        var lookPos = player.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles =new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
    }

    public void AfterCreationLines()
    {
        StartCoroutine(StartTalking(afterCreationLines, afterCreationAudioClips,0, afterCreationAudioClips.Length));
    }

    public IEnumerator StartTalking(string [] lines, AudioClip[] audioClips, int idx, int stopIdx)
    {
        animator.SetBool("Talking", true);
        audioManager.Play2DAudio(audioClips[idx], 1f, false, false);
        textDisplay.TypeLine(lines[idx]);
        yield return new WaitForSeconds(audioClips[idx].length);
        idx++;
        if(idx < stopIdx || (stopCondition && idx < audioClips.Length))
        {
            StartCoroutine(StartTalking(lines, audioClips, idx, stopIdx));
        }
        else if(idx < audioClips.Length )
        {
            animator.SetBool("Talking", false);
            textDisplay.TypeLine(" ");
            while(!stopCondition)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            stopCondition = true;
            animator.SetBool("Talking", true);
            StartCoroutine(StartTalking(lines, audioClips, idx, stopIdx));
        }
        else
        {
            stoppedTalking = true;
            animator.SetBool("Talking", false);
            textDisplay.TypeLine(" ");
        }      
    }

}
