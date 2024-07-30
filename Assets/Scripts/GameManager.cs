using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;set;}

    [SerializeField] private float maxDeadMembers;
    [SerializeField] private float numMembersSuccess;

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
        if(aliveMembers.Count >= numMembersSuccess && AllHaveShotgun())
        {

        }
        
        if(aliveMembers.Count < numMembersSuccess)
        {
            StartCoroutine(UIManager.instance.FadeInAndOutText("Not enough members", 3f));
        }
        if(!AllHaveShotgun())
        {
            StartCoroutine(UIManager.instance.FadeInAndOutText("Not all members are armed", 3f));
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