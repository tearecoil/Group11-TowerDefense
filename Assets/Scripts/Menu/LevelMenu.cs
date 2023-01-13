using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public TMP_Text welcomeText;
    public int maxLevel = 12;
    // Start is called before the first frame update
    void OnEnable()
    {
        welcomeText.text = "Chào mừng " + PlayerStatus.instance.playerName + "!\nVui lòng chọn màn chơi.";
        GameObject levelButtons = GameObject.Find("Levels");
        int level = PlayerPrefs.GetInt("Level" + PlayerStatus.instance.playerName);
        
        for (int i = 0; i <= level; i++)
        {
            Debug.Log("Level " + i);
            GameObject buttonObject = levelButtons.transform.GetChild(i).gameObject;
            buttonObject.GetComponent<Button>().interactable = true;
            //buttonObject.GetComponent<Button>().onClick.AddListener(() => { GoToLevel(i - 1); });
        }

    }

    public void GoToLevel(string mapName)
    {
        Debug.Log(mapName);
        SceneManager.LoadScene(mapName);
    }
}
