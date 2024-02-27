using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public abstract class Item : ScriptableObject
{
    [Header("Item")]
    public int id;

    public string itemName;

    public bool isStackable = true;
    public Sprite icon;


    public abstract Item GetItem();
    public abstract ConsumableItem GetConsumable();

    public abstract WeaponItem GetWeapon();
}