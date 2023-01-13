using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // User interface manager
    public UIManager uiManager;

    // Nymbers of enemy spawners in this level
    private int spawnNumbers;

    // Health system
    public HealthSystem healthSystem;
    
    // Currency system
    public CurrencySystem currencySystem;

    // Singleton
    public static LevelManager instance;
    public int thisLevel;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        instance = this;
        uiManager = FindObjectOfType<UIManager>();
        spawnNumbers = FindObjectsOfType<SpawnPoint>().Length;
        
        if (spawnNumbers <= 0)
        {
            Debug.LogError("Have no spawners");
        }
        Debug.Assert(uiManager && healthSystem && currencySystem, "Wrong initial parameters");
        healthSystem.Init();
        currencySystem.Init();
        uiManager.UpdateHealthUI();
        uiManager.UpdateMoneyUI();
        StartCoroutine(GainMoney());
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Determines if is collision valid for this scene.
    /// </summary>
    /// <returns><c>true</c> if is collision valid the specified myTag otherTag; otherwise, <c>false</c>.</returns>
    /// <param name="myTag">My tag.</param>
    /// <param name="otherTag">Other tag.</param>
    public static bool IsCollisionValid(string myTag, string otherTag)
    {
        bool res = false;
        //Debug.Log("Checking " + myTag + " with " + otherTag);
        switch (myTag)
        {
            case "Tower":
            case "Defender":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        break;
                }
                break;
            case "Enemy":
                switch (otherTag)
                {
                    case "Defender":
                    case "Tower":
                    case "CapturePoint":
                        res = true;
                        //Debug.Log("Enemy is colliding with something");
                        break;
                }
                break;
            case "Bullet":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        break;
                }
                break;
            case "CapturePoint":
                switch (otherTag)
                {
                    case "Enemy":
                        res = true;
                        //Debug.Log("CapturePoint is colliding with Enemy");
                        break;
                }
                break;
            default:
                Debug.Log("Unknown collision tag => " + myTag + " - " + otherTag);
                break;
        }
        return res;
    }

    /// <summary>
    /// Enemy reached capture point.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void Captured(GameObject obj, string param)
    {
        // Lose 1 health
        healthSystem.LoseHealth();
        uiManager.UpdateHealthUI();
        CheckDefeat();
        //Debug.Log("Capture");
    }

    private void CheckDefeat()
    {
        if (healthSystem.healthCount > 0) return;
        uiManager.GoToDefeatMenu();
    }

    /// <summary>
    /// All enemies are dead.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        spawnNumbers--;
        // Enemies dead at all spawners
        if (spawnNumbers <= 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("player");
            Debug.Log(player.gameObject.GetComponent<PlayerStatus>().playerName);
            string playerName = player.gameObject.GetComponent<PlayerStatus>().playerName;
            int levelUnlocked = player.gameObject.GetComponent<PlayerStatus>().levelUnlocked;
            if (thisLevel + 1 >= levelUnlocked && thisLevel < 9)
            {
                PlayerPrefs.SetInt("Level" + playerName, thisLevel + 1);
                player.gameObject.GetComponent<PlayerStatus>().levelUnlocked = thisLevel + 1;
            }
            
            // Victory
            uiManager.GoToVictoryMenu();
        }
    }

    IEnumerator GainMoney()
    {
        yield return new WaitForSeconds(currencySystem.interval);
        currencySystem.GainMoney(1);
        uiManager.UpdateMoneyUI();
        StartCoroutine(GainMoney());
    }
}
