using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// User interface and events manager.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Next level scene name
    public string nextLevel;

    // Pause menu canvas
    public GameObject pauseMenu;

    // Defeat menu canvas
    public GameObject defeatMenu;

    // Victory menu canvas
    public GameObject victoryMenu;

    // Level interface
    public GameObject levelUI;


    // Is game paused?
    private bool paused;

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        Debug.Assert(pauseMenu && defeatMenu && victoryMenu && levelUI, "Wrong initial parameters");
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        GoToLevel();
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (paused == false)
        {
            // Pause on escape button
            if (Input.GetButtonDown("Cancel") == true)
            {
                PauseGame(true);
                GoToPauseMenu();
            }
            // User press mouse button
            if (Input.GetMouseButtonDown(0) == true)
            {
                // Check if pointer over UI components
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
                foreach (RaycastResult res in results)
                {
                    if (res.gameObject.CompareTag("ActionIcon"))
                    {
                        Debug.Log("Go Go power ranger");
                        hittedObj = res.gameObject;
                        break;
                    }
                }
                if (results.Count <= 0) // No UI components on pointer
                {
                    // Check if pointer over colliders
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        // If this is tower collider
                        if (hit.collider.gameObject.CompareTag("Home") || hit.collider.gameObject.CompareTag("Tower"))
                        {
                            hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
                }
                // Send message with user click data
                EventManager.TriggerEvent("UserClick", hittedObj, null);
            }
        }

        else
        {
            if (Input.GetButtonDown("Cancel") == true)
            {
                GoToLevel();
                PauseGame(false);
            }
        }
    }

    /// <summary>
    /// Stop current scene and load new scene
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    private void LoadScene(string sceneName)
    {
        EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Start new game.
    /// </summary>
    public void NewGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    /// <summary>
    /// Gos to main menu.
    /// </summary>
    public void GoToMainMenu()
    {
        LoadScene("Menu");
    }

    /// <summary>
    /// Closes all UI canvases.
    /// </summary>
    private void CloseAllUI()
    {
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
        levelUI.SetActive(false);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    private void PauseGame(bool pause)
    {
        paused = pause;
        // Stop the time on pause
        Time.timeScale = pause ? 0f : 1f;
        EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }

    /// <summary>
    /// Gos to pause menu.
    /// </summary>
    private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Gos to level.
    /// </summary>
    private void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);    
        PauseGame(false);
    }

    /// <summary>
    /// Gos to defeat menu.
    /// </summary>
    public void GoToDefeatMenu()
    {
        PauseGame(true);
        CloseAllUI();
        defeatMenu.SetActive(true);
    }

    /// <summary>
    /// Gos to victory menu.
    /// </summary>
    public void GoToVictoryMenu()
    {
        PauseGame(true);
        CloseAllUI();
        victoryMenu.SetActive(true);
    }

    /// <summary>
    /// Start next level.
    /// </summary>
    public void GoToNextLevel()
    {
        LoadScene(nextLevel);
    }

    /// <summary>
    /// Restarts current level.
    /// </summary>
    public void RestartLevel()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        LoadScene(activeScene);
    }
    
    public void UpdateHealthUI()
    {
        int currentHealth = LevelManager.instance.healthSystem.healthCount;
        Transform healthUI = levelUI.transform.Find("Health");
        //Debug.Log(healthUI);
        Transform healthText = healthUI.Find("Health Text");
        //Debug.Log(healthText.GetComponent<TMP_Text>());
        healthText.GetComponent<TMP_Text>().text =  currentHealth.ToString();
    }

    public void UpdateMoneyUI()
    {
        int currentMoney = LevelManager.instance.currencySystem.currentMoney;
        Transform moneyUI = levelUI.transform.Find("Money");
        //Debug.Log(healthUI);
        Transform moneyText = moneyUI.Find("Money Text");
        //Debug.Log(healthText.GetComponent<TMP_Text>());
        moneyText.GetComponent<TMP_Text>().text = currentMoney.ToString();
    }

    /// <summary>
    /// On unit die.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void UnitDie(GameObject obj, string param)
    {
        /*// If this is enemy
        if (obj.CompareTag("Enemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price != null)
            {
                // Add gold for enemy kill
                AddGold(price.price);
            }
        }*/
    }

    
}
