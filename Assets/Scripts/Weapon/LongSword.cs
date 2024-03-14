using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    [SerializeField] Transform leftFlip;

    private void OnEnable()
    {
        
    }

    public override Vector3 WeaponPosition()
    {
        return new Vector3(0, 0, 15);
    }

    public override void AttackMotion()
    {
        if (!Manager.Game.player.WeaponWield)
        {
            leftFlip.localScale = new Vector3(1, -1, 1);
            Manager.Game.player.WeaponWield = true;
        }
        else if (!Manager.Game.player.WeaponWield)
        {
            leftFlip.localScale = new Vector3(1, 1, 1);
            Manager.Game.player.WeaponWield = false;
        }
    }
}
