using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CurrencySystem : MonoBehaviour
{
    // Default Currency
    public int defaultMoney;

    // Current Currency
    public int currentMoney;

    public float interval = 5.5f;

    // Methods

    // Set default values
    public void Init()
    {
        currentMoney = defaultMoney;
    }

    public void ResetMoney() 
    { 
        currentMoney = defaultMoney;
    }

    // Gain currency
    public void GainMoney(int val)
    {
        currentMoney += val;
    }

    // Lose currency
    public bool UseMoney(int val)
    {
        if (!EnoughMoney(val))
            return false;
        currentMoney -= val;
        return true;
    }

    // Check emptiness
    public bool EnoughMoney(int val)
    {
        return currentMoney >= val;
    }
}
