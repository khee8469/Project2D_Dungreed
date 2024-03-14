using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public virtual Vector3 WeaponPosition()
    {
        return Vector3.zero;
    }

    public virtual void AttackMotion()
    {

    }

    public virtual void Skill()
    {

    }
}
