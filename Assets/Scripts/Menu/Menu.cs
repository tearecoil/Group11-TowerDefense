using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_InputField playerNameInputField;
    public GameObject mainMenu;
    public GameObject levelMenu;

    private void Start()
    {
    }
    public void Continue() {
        if (playerNameInputField.text != string.Empty)
        {
            PlayerStatus.instance.playerName = playerNameInputField.text;
            int numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers");
            bool alreadyIn = false;
            for (int i = 1; i <= numberOfPlayers; ++i)
            {
                string username = PlayerPrefs.GetString("Name" + i);
                if (username == playerNameInputField.text)
                {
                    alreadyIn = true;
                    break;
                }
            }
            if (alreadyIn)
            {
                PlayerStatus.instance.levelUnlocked = PlayerPrefs.GetInt("Level"+playerNameInputField.text);
            }
            else
            {
                PlayerStatus.instance.levelUnlocked = 0;
                PlayerPrefs.SetString("Name" + (numberOfPlayers + 1), playerNameInputField.text);
                PlayerPrefs.SetInt("Level" + playerNameInputField.text, 0);
                PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers + 1);
                PlayerPrefs.Save();
            }
            mainMenu.SetActive(false);
            levelMenu.SetActive(true);
        }
        else
        {
            // TODO: Set up a dialog here
            Debug.Log("Please Enter your name");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
