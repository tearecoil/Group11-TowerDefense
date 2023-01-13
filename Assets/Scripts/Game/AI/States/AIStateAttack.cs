using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAttack : MonoBehaviour, IAIState
{
    // Attack target closest to the capture point
    public bool useTargetPriority = false;
    // Go to this state if agressive event occures
    public string agressiveAiState;
    // Go to this state if passive event occures
    public string passiveAiState;


    // AI behavior of this object
    private AIBehavior aiBehavior;
    // Target for attack
    private GameObject target;
    // List with potential targets finded during this frame
    private List<GameObject> targetsList = new List<GameObject>();
    // My melee attack type if it is
    private IAttack meleeAttack;
    // My ranged attack type if it is
    private IAttack rangedAttack;
    // Type of last attack is made
    private IAttack myLastAttack;
    // My navigation agent if it is
    NavAgent nav;
    // Allows to await new target for one frame before exit from this state
    private bool targetless;

    Animator animator;

    /// <summary>
    /// Awake this instance
    /// </summary>
    void Awake()
    {
        aiBehavior= GetComponent<AIBehavior>();
        meleeAttack = GetComponentInChildren<AttackMelee>();
        rangedAttack = GetComponentInChildren<AttackRanged>();
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavAgent>();
        Debug.Assert((aiBehavior != null) && ((meleeAttack != null) || (rangedAttack != null)), 
            "Wrong initial parameters");
    }

    void FixedUpdate()
    {
        if ((target == null) && (targetsList.Count > 0)) 
        {
            target = GetTopMostTarget();
            if ((target != null) && (nav != null)) 
            {
                // Look at the target
                nav.LookAt(target.transform);
            }
        }
        if (target == null)
        {
            if (targetless == false)
            {
                targetless = true;
            }
            else
            {
                aiBehavior.ChangeState(passiveAiState);
            }
        }
    }

    public GameObject GetTarget() { return target; }

    private void LoseTarget()
    {
        target = null;
        targetless = false;
        myLastAttack = null;
    }

    public void OnStateEnter(string previousState, string newState)
    {
        //Debug.Log(gameObject + " Enter Attack State");
    }

    public void OnStateExit(string previousState, string newState)
    {
        LoseTarget();
        if (animator != null)
        {
            // Stop animation
            animator.SetTrigger("Attack");
        }
    }

    public void TriggerEnter(Collider2D my, Collider2D other)
    {
        //Debug.Log("Trigger Enter Attack State");
    }

    public void TriggerStay(Collider2D my, Collider2D other)
    {
        if (target == null)
        {
            targetsList.Add(other.gameObject);
        }
        else
        {
            if (target == other.gameObject)
            {
                if (my.name == "MeleeAttack")
                {
                    if (meleeAttack != null)
                    {
                        myLastAttack = meleeAttack;
                        meleeAttack.Attack(other.transform);
                    }
                }
                else if (my.name == "RangedAttack")
                {
                    //Debug.Log("They are in");
                    if (rangedAttack != null)
                    {
                        if ((meleeAttack == null) 
                            || ((meleeAttack != null) && (myLastAttack != meleeAttack)))
                        {
                            // Remember my last attack type
                            myLastAttack = rangedAttack;
                            // Try to make ranged attack
                            rangedAttack.Attack(other.transform);
                        }
                    }
                }
            }
        }
    }

    public void TriggerExit(Collider2D my, Collider2D other)
    {
        if (other.gameObject == target) 
        {
            // Lose my target if it quit attack range
            LoseTarget();
        }
    }

    private GameObject GetTopMostTarget()
    {
        GameObject res = null;
        if (useTargetPriority == true)
        {
            float minPathDistance = float.MaxValue;
            foreach (GameObject ai in targetsList)
            {
                if (ai != null)
                {
                    AIStatePatrol aIStatePatrol = ai.GetComponent<AIStatePatrol>();
                    float distance = aIStatePatrol.GetRemainingPath();
                    if (distance < minPathDistance)
                    {
                        minPathDistance = distance;
                        res = ai;
                    }
                }
            }
        }
        else // Get the first target from the list
            res = targetsList[0];

        // Clear the list of potential targets
        targetsList.Clear();
        return res;
    }
}
