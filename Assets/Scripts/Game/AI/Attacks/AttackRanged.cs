using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : MonoBehaviour, IAttack
{
    // Damage amount
    public int damage = 1;
    // Cooldown between attacks
    public float cooldown = 1f;
    // Prefab for arrows
    public GameObject arrowPrefab;
    // From this position arrows will fired
    public Transform firePoint;

    // Animation controller for this AI
    private Animator animator;
    // Counter for cooldown calculation
    private float cooldownCounter;

    public Stats stats;

    void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        cooldownCounter = cooldown;
        stats = transform.parent.GetComponent<Stats>();
        damage = stats.damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownCounter < cooldown)
        {
            cooldownCounter += Time.deltaTime;
        }
    }

    public void Attack(Transform target)
    {
        if (cooldownCounter >= cooldown)
        {
            cooldownCounter = 0f;
            Fire(target);
        }
    }

    private void Fire(Transform target)
    {
        if (target != null)
        {
            // Create arrow
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            IBullet bullet = arrow.GetComponent<IBullet>();
            bullet.SetDamage(damage);
            bullet.Fire(target);
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}
