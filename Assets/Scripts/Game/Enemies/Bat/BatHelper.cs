using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHelper : MonoBehaviour
{
    public void FinishAttack()
    {
        AudioSource audioSource = transform.parent.GetComponent<AudioSource>();
        audioSource.Play();
        GameObject target = transform.parent.GetComponent<AIStateAttack>().GetTarget();
        if (target != null)
        {
            int damage = transform.parent.GetComponentInChildren<AttackMelee>().damage;
            DamageTaker damageTaker = target.GetComponent<DamageTaker>();
            if (damageTaker != null)
            {
                damageTaker.TakeDamage(damage, transform.parent.gameObject);
            }
            else if (target.CompareTag("CapturePoint") == true)
            {
                EventManager.TriggerEvent("Captured", null, null);
                EventManager.TriggerEvent("UnitDie", transform.parent.gameObject, null);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
