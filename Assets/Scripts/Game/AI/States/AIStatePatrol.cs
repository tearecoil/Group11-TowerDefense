using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatePatrol : MonoBehaviour, IAIState
{
    public Pathway path;
    public bool loop = false;
    public string aggressiveAIState;
    public string passiveAIState;
    public Animator animator;

    //private Animation anim;
    private Waypoint destination;
    NavAgent navAgent;
    AIBehavior aIBehavior;

    /// <summary>
    /// Awake this instance
    /// </summary>
    void Awake() 
    {
        //anim = GetComponent<Animation>();
        navAgent= GetComponent<NavAgent>();
        aIBehavior= GetComponent<AIBehavior>();
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(aIBehavior && navAgent, "Wrong initial parameters");
    }
    public void OnStateEnter(string previousState, string newState)
    {
        if (path == null)
        {
            path = FindObjectOfType<Pathway>();
        }
        if (destination == null) 
        {
            destination = path.GetNearestWaypoint(transform.position);
        }

        // Set destination for navigation agent
        navAgent.destination = destination.transform.position;
        if (animator != null)
        {
            // Start moving
            navAgent.move = true;
            // Play animation
            animator.SetBool ("IsMoving", navAgent.move);
        }
    }

    public void OnStateExit(string previousState, string newState)
    {
        if (animator != null)
        {
            // Stop moving
            navAgent.move = false;
            // Stop animation
            animator.SetBool("IsMoving", navAgent.move);
        }
    }

    public void TriggerEnter(Collider2D my, Collider2D other)
    {

    }

    public void TriggerStay(Collider2D my, Collider2D other)
    {
        aIBehavior.ChangeState(aggressiveAIState);
    }

    public void TriggerExit(Collider2D my, Collider2D other) 
    { 
    }

    /// <summary>
    /// Fixed update for this instance
    /// </summary>
    void FixedUpdate()
    {
        if (destination != null)
        {
            // If destination reached
            if ((Vector2)destination.transform.position == (Vector2)transform.position)
            {
                // Get next waypoint from my path
                destination = path.GetNextWaypoint(destination, loop);
                if (destination != null)
                {
                    // Set destination for navigation agent
                    navAgent.destination = destination.transform.position;
                }
            }
        }
    }

    public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }
}
