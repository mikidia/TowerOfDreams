using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;

    
    public Slot()   
    {
        item = null;
        quantity = 0;
    }
    public Slot(Item _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }

    public Item GetItem()
    {
        return item;
    }

    public int GetQuantity()
    {
        return quantity;
    }



    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
    public void RemoveQuantity(int _quantity)
    {
        quantity -= _quantity;
    }
    
    public void AddItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }
}
