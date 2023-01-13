using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReflectDamage : MonoBehaviour, IEffect
{
    private Animator animator;

    void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        RunEffect();
    }

    public void RunEffect()
    {
        
    }
}
