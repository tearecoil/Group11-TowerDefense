using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIColliderTrigger : MonoBehaviour
{
    // Allowed objects tags for collision detection
    public List<string> tags = new List<string>();

    // My collider
    private Collider2D col;
    // AI behaviour component in parent object
    private AIBehavior aiBehavior;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponentInParent<AIBehavior>();
        Debug.Assert(col && aiBehavior, "Wrong initial parameters");
    }

    /// <summary>
    /// Determines whether this instance is tag allowed the specified tag.
    /// </summary>
    /// <returns><c>true</c> if this instance is tag allowed the specified tag; otherwise, <c>false</c>.</returns>
    /// <param name="tag">Tag.</param>
    private bool IsTagAllowed(string tag)
    {
        bool res = false;
        if (tags.Count > 0)
        {
            foreach (string str in tags)
            {
                if (str == tag)
                {
                    res = true;
                    break;
                }
            }
        }
        else
        {
            res = true;
        }
        return res;
    }

    /// <summary>
    /// Raises the trigger enter2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collision Detected!");
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
            aiBehavior.TriggerEnter2D(col, other);
            //Debug.Log("Collide");
        }
    }

    /// <summary>
    /// Raises the trigger stay2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Stay Detected!");
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
            aiBehavior.TriggerStay2D(col, other);
            //Debug.Log("Stay");
        }
    }

    /// <summary>
    /// Raises the trigger exit2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Exit Detected!");
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
            aiBehavior.TriggerExit2D(col, other);
            //Debug.Log("Exit");
        }
    }
}
