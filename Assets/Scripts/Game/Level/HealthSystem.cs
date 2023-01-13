using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public int healthCount;
    public int defaultHealthCount;
    public UIManager uIManager;
    // Initiate the Health system (reset health count)
    public void Init()
    {
        healthCount = defaultHealthCount;
    }

    public void ResetHealthCount() { 
        healthCount = defaultHealthCount;
    }

    // Lose health count
    public void LoseHealth(int value = 1)
    {
        if (healthCount == 0) return;
        healthCount -= value;
    }

}
