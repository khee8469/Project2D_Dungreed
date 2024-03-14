using Unity.VisualScripting;
using UnityEngine;

public class Spear : Weapon
{
    [SerializeField] Transform leftFlip;



    public override Vector3 WeaponPosition()
    {
        return new Vector3(0, 0, -90);
    }

    public override void AttackMotion()
    {
        
    }

    public override void Skill()
    {

    }
}
