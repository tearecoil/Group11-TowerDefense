using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamageTaker : MonoBehaviour
{
    // Start hitpoints
    public Stats stats;
    public int hitpoints = 1;
    // Remaining hitpoints
    public int currentHitpoints;
    // Hit visual effect duration
    public float hitDisplayTime = 0.2f;

    // Image of this object
    private SpriteRenderer sprite;
    // Visualisation of hit
    private bool hitCoroutine;

    private bool isTower;

    public GameObject floorPrefab;

    private EffectReflectDamage effectReflectDamage;

    Animator animator;


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        stats = GetComponent<Stats>();
        currentHitpoints = stats.health;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        effectReflectDamage= GetComponentInChildren<EffectReflectDamage>();
        if (tag == "Tower") isTower = true;
        Debug.Assert(sprite, "Wrong initial parameters");
    }

    /// <summary>
    /// Take damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void TakeDamage(int damage, GameObject from = null)
    {
        if (effectReflectDamage != null)
        {
            Debug.Log("Reflect Damage");
            if (from != null) {
                DamageTaker enemyDamageTaker = from.GetComponent<DamageTaker>();
                if (enemyDamageTaker != null)
                {
                    animator.SetTrigger("Attack");
                    enemyDamageTaker.TakeDamage(stats.damage, from);
                }
            }
            
        }
        if (currentHitpoints > damage)
        {
            // Still alive
            currentHitpoints -= damage;
            animator.SetTrigger("Hurt");
        }
        else
        {
            // Die
            currentHitpoints = 0;
            Die();
        }
    }

    /// <summary>
    /// Die this instance.
    /// </summary>
    public void Die()
    {
        EventManager.TriggerEvent("UnitDie", gameObject, null);
        //EventManager.TriggerEvent("AllEnemiesAreDead", gameObject, null);
        Transform old = transform;
        Transform parent = transform.parent;
        Destroy(gameObject);
        if (isTower)
        {
            GameObject newTower = Instantiate(floorPrefab, parent);
            newTower.transform.position = old.position;
            newTower.transform.rotation = old.rotation;
        }
    }
}
