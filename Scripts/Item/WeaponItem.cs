using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Class", menuName = "Item/Weapon")]
public class WeaponItem : Item
{
    [Header("Weapon")] 
    public float damage;
    public override Item GetItem()
    {
        return this;
        
    }

    public override ConsumableItem GetConsumable()
    {
        return null;
    }

    public override WeaponItem GetWeapon()
    {        
        return this;
    }
}
