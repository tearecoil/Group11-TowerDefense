using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour, IAttack
{
    // Damage amount
    public int damage = 1;
    // Cooldown between attacks
    public float cooldown = 1f;

    // Animation controller for this AI
    private Animator animator;
    // Counter for cooldown calculation
    private float cooldownCounter;
    public Stats stats;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        cooldownCounter = cooldown;
        stats = transform.parent.GetComponent<Stats>();
        damage = stats.damage;
        Debug.Assert(animator != null);
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (cooldownCounter < cooldown)
        {
            cooldownCounter += Time.deltaTime;
            
        }
    }

    /// <summary>
    /// Attack the specified target if cooldown expired
    /// </summary>
    /// <param name="target">Target.</param>
    public void Attack(Transform target)
    {
        if (cooldownCounter >= cooldown)
        {
            cooldownCounter = 0f;
            Smash(target);
        }
    }

    /// <summary>
    /// Make melee attack
    /// </summary>
    /// <param name="target">Target.</param>
    private void Smash(Transform target)
    {
        if (target != null)
        {
            // If target can receive damage
            animator.SetTrigger("Attack");
        }
    }
}
