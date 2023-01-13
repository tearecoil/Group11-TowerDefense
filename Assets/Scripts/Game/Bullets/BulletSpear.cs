using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class represents a spear to be thrown with no penetration
/// </summary>
public class BulletSpear : MonoBehaviour, IBullet
{
    public int damage = 1;

    public float lifeTime = 3f;

    public float speed = 3f;

    public float speedUpOverTime = 0.5f;

    public float hitDistance = 0.2f;

    public float ballisticOffset = 0.5f;

    public bool freezeRotation = false;

    private Vector2 originPoint;

    private Transform target;

    private Vector2 aimPoint;

    private Vector2 myVirtualPosition;

    private Vector2 myPreviousPosition;

    private float counter;

    private SpriteRenderer sprite;

    private GameObject from;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void Fire(Transform target, GameObject from)
    {
        sprite = GetComponent<SpriteRenderer>(); 
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint= target.position;
        this.from = from;
        Destroy(gameObject, lifeTime);
    }    

    void Update()
    {
        counter += Time.deltaTime;
        speed += Time.deltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint= target.position;
        }

        // Calculate the distance from the firepoint to aim
        Vector2 originDistance = aimPoint - originPoint;

        // Calculate the remaining distance
        Vector2 distanceToAim = aimPoint - myVirtualPosition;

        // Move towards aim
        myVirtualPosition = Vector2.Lerp(originPoint, 
            aimPoint, 
            counter * speed / originDistance.magnitude);

        // Add ballistic offset to the trajectory
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);

        // Rotate the bullet towards the trajectory
        LookAtDirection2D((Vector2)transform.position - myVirtualPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;

        // Close enough to hit
        if (distanceToAim.magnitude <= hitDistance) 
        {
            Debug.Log(target);
            if (target != null)
            {
                DamageTaker damageTaker = target.GetComponent<DamageTaker>();
                if (damageTaker != null)
                {
                    damageTaker.TakeDamage(damage, from);
                }
                
            }
            Destroy(gameObject);
        }
    }

    private void LookAtDirection2D(Vector2 direction)
    {
        if (freezeRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            return myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else return myVirtualPosition;
    }
}
