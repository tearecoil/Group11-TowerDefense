using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    public float speed = 1.0f;
    public bool move = true;
    public bool turn = true;
    public Vector2 destination;
    public Vector2 velocity;
    private Vector2 prevPosition;

    private void OnEnable()
    {
        prevPosition= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If moving is allowed
        if (move == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                destination, speed * Time.deltaTime);
        }

        // Calculate the velocity
        velocity = (Vector2)transform.position - prevPosition;
        velocity /= Time.deltaTime;

        // If turning is allowed
        if (turn == true) 
        {
            SetSpriteDirection(destination - (Vector2)transform.position);
        }
        prevPosition = transform.position;
    }

    /// <summary>
    /// Looks at direction
    /// </summary>
    /// <param name="direction">Direction</param>
    private void SetSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0f && transform.localScale.x > 0f) // To the right
        {
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0f && transform.localScale.x < 0f) // To the left
        {
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Looks at target
    /// </summary>
    /// <param name="target">Target</param>
    public void LookAt(Transform target)
    {
        SetSpriteDirection(target.position - transform.position);
    }

    /// <summary>
    /// Looks at the direction
    /// </summary>
    /// <param name="direction"></param>
    public void LookAt(Vector2 direction)
    {
        SetSpriteDirection(direction);
    }    
}
