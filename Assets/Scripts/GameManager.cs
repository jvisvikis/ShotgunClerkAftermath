using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;set;}

    [SerializeField] private float maxDeadMembers;

    private List<CrewMember> deadMembers;
    
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            deadMembers = new List<CrewMember>();
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
        deadMembers.Add(member);
    }
}
