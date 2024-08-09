using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;set;}

    [SerializeField] private float maxDeadMembers;
    [SerializeField] private float numMembersSuccess;

    public bool fullTutPlayed {get; set;}
    private bool playedAudio;
    private bool tutorialAudioPlayed;
    private int successfulHeists;
    private List<CrewMember> deadMembers;
    private List<CrewMember> aliveMembers;
    
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            successfulHeists = PlayerPrefs.GetInt("successfulHeists", 0);
            deadMembers = new List<CrewMember>();
            aliveMembers = new List<CrewMember>();
            DontDestroyOnLoad(this);
        }
    }

    public void AddDeadMember(CrewMember member)
    {
        if(!playedAudio)
        {
            playedAudio = true;
            FindObjectOfType<Father>().AfterCreationLines();
        }
        if(deadMembers.Count >= maxDeadMembers) 
        {   
            Destroy(deadMembers[0].gameObject);
            deadMembers.RemoveAt(0);
        }
        aliveMembers.Remove(member);
        deadMembers.Add(member);
    }

    public void AddAliveMember(CrewMember member)
    {
        aliveMembers.Add(member);
    }

    public void GoHeist()
    {
        if(!AllHaveShotgun())
        {
            StartCoroutine(UIManager.instance.FadeInAndOutText("Not all members are armed", 3f));
            return;
        }
        Debug.Log(aliveMembers.Count);
        if(aliveMembers.Count >= numMembersSuccess)
        {
            UIManager.instance.PlayHeistCutscene(true);
        }
        
        if(aliveMembers.Count < numMembersSuccess)
        {
            UIManager.instance.PlayHeistCutscene(false);
        }

    }

    public void OnSceneLoad()
    {
        aliveMembers.Clear();
        deadMembers.Clear();
        if(FindObjectOfType<Father>() != null && !fullTutPlayed) 
        {
            FindObjectOfType<Father>().StartIntro();
        }
       
    }

    private bool AllHaveShotgun()
    {
        foreach(CrewMember member in aliveMembers)
        {
            if(!member.hasShotgun)
            {
                return false;
            }
        }
        return true;
    }
}
