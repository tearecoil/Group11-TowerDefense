using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerIcon : MonoBehaviour
{
    // Tower prefab for this icon
    public GameObject towerPrefab;

    // Text field for tower price
    private TMP_Text priceText;
    
    // Parent building tree
    private TowerSelection myTree;

    private void OnEnable()
    {
        EventManager.StartListening("UserClick", UserClick);
    }

    void OnDisable()
    {
        EventManager.StopListening("UserClick", UserClick);
    }

    void Awake()
    {
        // Get building tree from parent object
        myTree = transform.GetComponentInParent<TowerSelection>();
        priceText = GetComponentInChildren<TMP_Text>();
        Debug.Assert(priceText && myTree, "Wrong initial parameters");
        if (towerPrefab == null)
        {
            // If this icon have no tower prefab - hide icon
            gameObject.SetActive(false);
        }
        else
        {
            // Display tower price
            priceText.text = towerPrefab.GetComponent<Stats>().price.ToString();
        }
    }

    private void UserClick(GameObject obj, string param)
    {
        // If clicked on this icon
        if (obj == gameObject)
        {
            // Build the tower
            myTree.Build(towerPrefab);
            Debug.Log("Lets go");
        }
    }
}
