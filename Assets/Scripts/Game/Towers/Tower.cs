using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Prefab for building tree
    public GameObject towerSelectionPrefab;
    // Attack range of this tower
    public GameObject range;

    // User interface manager
    private UIManager uiManager;
    // Level UI canvas for building tree display
    private Canvas canvas;
    // Collider of this tower
    private Collider2D bodyCollider;
    // Displayed building tree
    private TowerSelection activeTowerSelection;

    private void OnEnable()
    {
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canv in canvases)
        {
            if (canv.CompareTag("LevelUI"))
            {
                canvas = canv;
                break;
            }
        }
        bodyCollider = GetComponent<Collider2D>();
        Debug.Assert(uiManager && canvas && bodyCollider, "Wrong initial parameters");
    }

    private void OpenTowerSelection()
    {
        if (towerSelectionPrefab != null)
        {
            Debug.Log("Activate my trap card");
            // Create building tree
            activeTowerSelection = Instantiate(towerSelectionPrefab, canvas.transform).GetComponent<TowerSelection>();
            // Set it over the tower
            activeTowerSelection.transform.position = transform.position + new Vector3(1.5f, 0, 0);
            //Debug.Log(activeTowerSelection.transform.position);
            activeTowerSelection.myTower = this;
            // Disable tower raycast
            bodyCollider.enabled = false;
        }
    }

    private void CloseBuildingTree()
    {
        if (activeTowerSelection != null)
        {
            Destroy(activeTowerSelection.gameObject);
            // Enable tower raycast
            bodyCollider.enabled = true;
        }
    }

    public void BuildTower(GameObject towerPrefab)
    {
        // Close active building tree
        CloseBuildingTree();
        Stats stats = towerPrefab.GetComponent<Stats>();
        // If enough gold
        if (LevelManager.instance.currencySystem.EnoughMoney(stats.price))
        {
            LevelManager.instance.currencySystem.UseMoney(stats.price);
            uiManager.UpdateMoneyUI();
            // Create new tower and place it on same position
            GameObject newTower = Instantiate(towerPrefab, transform.parent);
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            // Destroy old tower
            Destroy(gameObject);
        }
    }

    private void UserClick(GameObject obj, string param)
    {
        //Debug.Log("Click on placeable tile");
        if (obj == gameObject) // This tower is clicked
        {
            // Show attack range
            ShowRange(true);
            if (obj.CompareTag("Home") && activeTowerSelection == null)
            {
                // Open building tree if it is not
                OpenTowerSelection();
            }
        }
        else // Other click
        {
            // Hide attack range
            ShowRange(false);
            // Close active building tree
            CloseBuildingTree();
        }
    }

    private void GamePaused(GameObject obj, string param)
    {
        if (param == bool.TrueString) // Paused
        {
            CloseBuildingTree();
            bodyCollider.enabled = false;
        }
        else // Unpaused
        {
            bodyCollider.enabled = true;
        }
    }

    /// <summary>
    /// Display tower's attack range.
    /// </summary>
    /// <param name="condition">If set to <c>true</c> condition.</param>
    private void ShowRange(bool condition)
    {
        if (range != null)
        {
            range.SetActive(condition);
        }
    }
}
