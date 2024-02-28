using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]

public class ConsumableItem : Item
{
    [Header("Consumable")] 
    public float healthAdded;
    
    public override Item GetItem()
    {
        return this;
        
    }

    public override ConsumableItem GetConsumable()
    {
        return this;
    }

    public override WeaponItem GetWeapon()
    {        
        return null;
    }
}
