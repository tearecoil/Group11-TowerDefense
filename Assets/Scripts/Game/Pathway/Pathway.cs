using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pathway for enemies to move
/// </summary>
[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length > 1)
        {
            int idx;
            for (idx = 1; idx < waypoints.Length; ++idx)
            {
                Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, Color.blue);
            }
        }
    }

    /// <summary>
    /// Get the nearest waypoint to a given position
    /// </summary>
    /// <param name="position">Position</param>
    /// <returns>The nearest waypoint</returns>
    public Waypoint GetNearestWaypoint(Vector3 position)
    {
        Waypoint nearestWaypoint = null;
        float minDistance = float.MaxValue;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            if (waypoint.GetHashCode() != GetHashCode())
            {
                Vector3 vect = position - waypoint.transform.position;
                float distance = vect.magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }
        }
        return nearestWaypoint;
    }

    /// <summary>
    /// Get the next waypoint in this pathway
    /// </summary>
    /// <param name="currentWaypoint">Current waypoint</param>
    /// <param name="loop">If set to <c>true</c>, loop</param>
    /// <returns>The next waypoint</returns>
    public Waypoint GetNextWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint next = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
            idx += 1;
        else
            idx = 0;
        if (loop == true || idx != 0)
            next = transform.GetChild(idx).GetComponent<Waypoint>();
        return next;
    }

    /// <summary>
    /// Get the distance of the path from a waypoint to the end
    /// </summary>
    /// <param name="fromWaypoint">Starting waypoint</param>
    /// <returns>Path distance</returns>
    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
        for (idx = 0; idx < waypoints.Length; ++idx)
        {
            if (hitted == true)
            {
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx] == fromWaypoint)
            {
                hitted = true;
            }
        }
        return pathDistance;
    }
}
