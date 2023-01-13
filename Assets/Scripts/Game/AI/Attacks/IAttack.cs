using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all attack types
/// </summary>
public interface IAttack
{
    void Attack(Transform target);
}
