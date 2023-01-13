using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{    
    void SetDamage(int damage);
    void Fire(Transform target, GameObject from = null);
}
