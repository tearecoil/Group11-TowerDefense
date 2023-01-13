using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EffectGainMoney : MonoBehaviour, IEffect
{
    public int profit = 5;
    public float cooldown = 3.0f;

    private Animator animator;
    
    void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        RunEffect();
    }

    public void RunEffect()
    {
        StartCoroutine(GainMoney());
    }

    IEnumerator GainMoney()
    {
        yield return new WaitForSeconds(cooldown);
        animator.SetTrigger("Effect");
        LevelManager.instance.currencySystem.GainMoney(profit);
        LevelManager.instance.uiManager.UpdateMoneyUI();
        StartCoroutine(GainMoney());
    }
}
