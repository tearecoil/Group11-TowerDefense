using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    [HideInInspector]
    public Tower myTower;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        Debug.Assert(myTower, "Wrong initial parameters");
    }

    /// <summary>
    /// Build the tower.
    /// </summary>
    /// <param name="prefab">Prefab.</param>
    public void Build(GameObject prefab)
    {
        myTower.BuildTower(prefab);
    }
}
