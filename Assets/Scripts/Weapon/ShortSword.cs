using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : Weapon
{
    public override Vector3 WeaponPosition()
    {
        return new Vector3(0, 0, 15);
    }
}
