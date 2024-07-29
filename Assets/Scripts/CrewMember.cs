using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CrewMember : MonoBehaviour
{
    [SerializeField] private float distToFacePlayer = 3f;
    [SerializeField] private float rotSpeed = 7f;
    [SerializeField] private float idleDuration = 5f;
    [SerializeField] private float wanderRadius = 5f;

    private float idleTimer;

    private NavMeshAgent agent;
    private AgentState state;
    private Transform playerTransform;
    private Rigidbody rigidbody;
    private List<Rigidbody> rbs;
    // Start is called before the first frame update
    void Awake()
    {
        state = AgentState.Wandering;
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        rbs = new List<Rigidbody>();
        rigidbody.isKinematic = true;
        rbs.Add(rigidbody);
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
            {
                case AgentState.Idle:

                    if(Vector3.Distance(transform.position, playerTransform.position) <= distToFacePlayer)
                    {
                        state = AgentState.FacingPlayer;
                    }   

                    idleTimer += Time.deltaTime;
                    
                    if(idleTimer >= idleDuration)
                    {
                        idleTimer = 0;
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        state = AgentState.Wandering;
                    }
                    break;
                case AgentState.Wandering:

                    if(agent.destination == transform.position)
                    {
                        state = AgentState.Idle;
                    }

                    if(Vector3.Distance(transform.position, playerTransform.position) <= distToFacePlayer)
                    {
                        state = AgentState.FacingPlayer;
                    }                    

                    break;
                case AgentState.FacingPlayer:

                    agent.SetDestination(transform.position);
                    FaceTransform(playerTransform);
                    if(Vector3.Distance(transform.position, playerTransform.position) > distToFacePlayer)
                    {
                        state = AgentState.Idle;
                    }
                    break;
            }
    }

    public void Die()
    {
        if(state == AgentState.Dead)
        {
            return;
        }
        state = AgentState.Dead;
        agent.enabled = false;
        DisableKinematics();   
        rigidbody.AddForce(transform.forward * -500f);
        GameManager.instance.AddDeadMember(this);
    }

    private void DisableKinematics()
    {
        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
    }

    private void FaceTransform(Transform target)
    {
        var lookPos = target.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles =new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }

    private enum AgentState{
        Idle,
        Wandering,
        FacingPlayer,
        Dead
    }
}
