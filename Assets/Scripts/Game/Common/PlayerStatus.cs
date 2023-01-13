using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public string playerName;
    public int levelUnlocked;
    public static PlayerStatus instance;
    // Start is called before the first frame update
    void Awake()
    {  
        instance = this;
        //playerName = PlayerPrefs.GetString("Name");
        levelUnlocked = PlayerPrefs.GetInt("Level" + playerName);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadStats()
    {
        levelUnlocked = PlayerPrefs.GetInt("Level" + playerName);
    }

    public void SaveStats()
    {
        Debug.Log(playerName);
        //PlayerPrefs.SetString("Name", playerName);
        PlayerPrefs.SetInt("Level" + playerName, levelUnlocked + 1);
        PlayerPrefs.Save();
    }
}
