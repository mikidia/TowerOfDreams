using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject slotHolder;
	[SerializeField] private Item ItemToAdd;
	[SerializeField] private Item ItemToRemove;

	[SerializeField] private Slot[] startingItems;
    
	private Slot[] items;

	private GameObject[] slots;

	private Slot movingSlot;

    private Slot tempSlot;

    private Slot originalSlot;

	private bool isMovingItem;

    public void Start(){
		slots = new GameObject[slotHolder.transform.childCount];
		items = new Slot[slots.Length];
		for(int i = 0; i < slotHolder.transform.childCount; i++)
			slots[i] = slotHolder.transform.GetChild(i).gameObject;

        for (int i = 0; i < items.Length; i++)
        {
			items[i] = new Slot();
        }

		for (int i = 0; i < startingItems.Length; i++)
		{
			items[i] = startingItems[i];
		}

        RefreshUI();
		Add(ItemToAdd, 1);
		Remove(ItemToRemove);
	}

	private void Update()
	{
		itemCursor.SetActive(isMovingItem);
		itemCursor.transform.position = Input.mousePosition;
		if (isMovingItem) 
		{
			itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().icon;
		}

        if (Input.GetMouseButtonDown(0)) 
		{ 
			if(isMovingItem) { 
				EndItemMove();
			}
			else
			{
                BeginItemMove();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (isMovingItem)
            {
                EndItemMove_Single();
            }
            else
            {
                BeginItemMove_Half ();
            }
        }
    }

    #region Inventory Utils
    public void RefreshUI(){
		for(int i = 0; i < slots.Length; i++){
			try
			{
				slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
				slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().icon;
				if (items[i].GetItem().isStackable)
					slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
				else
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";

			}
		}
		
	}

	public void Add(Item item, int quantity){
		Slot slot = Contains(item);

		if (slot != null && slot.GetItem().isStackable)
			slot.AddQuantity(1);
		else
		{
			for(int i = 0; i < items.Length; i++)
			{
				if (items[i].GetItem() == null)
				{
					items[i].AddItem(item, quantity);
					break;
				}
			}
		}
		RefreshUI();
	}

	public void Remove(Item item)
	{
		Slot temp = Contains(item);
        if (temp != null)
		{
			if(temp.GetQuantity() > 1)
			{
				temp.RemoveQuantity(1);
			}
			else
			{
				int slotIndex = 0;
				
				for(int i = 0; i < items.Length; i++)
				{
					if (items[i].GetItem() == item)
						{
							slotIndex = i;
							break;
						}
				}
				items[slotIndex].Clear();
			}
		}
		RefreshUI();
	}

	public Slot Contains(Item item){
		for(int i = 0; i < items.Length; i++)
		{
			if (items[i].GetItem() == item)
			{
				return items[i];
			}
		}
		return null;
	}
    #endregion Inventory Utils

    #region Moving Items
	private bool BeginItemMove()
	{
		originalSlot = GetClosestSlot();
		if(originalSlot == null || originalSlot.GetItem() == null)
		{
			return false;
		}

		movingSlot = new Slot(originalSlot.GetItem(), originalSlot.GetQuantity());
		originalSlot.Clear();
		isMovingItem = true;
		RefreshUI();
		return true;
	}

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }
        movingSlot = new Slot(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity()/ 2f));
		originalSlot.RemoveQuantity(movingSlot.GetQuantity());

		if(originalSlot.GetQuantity() == 0)
		{
			originalSlot.Clear();
		}
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {


            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new Slot(originalSlot.GetItem(), originalSlot.GetQuantity());
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();
                    return true;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;

    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return false;
        }

		movingSlot.RemoveQuantity(1);
		if(originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
		{
			originalSlot.AddQuantity(1);
		}
		else
		{
			originalSlot.AddItem(movingSlot.GetItem(), 1);
		}
        originalSlot.AddItem(movingSlot.GetItem(), 1);

		if(movingSlot.GetQuantity() < 1)
		{
			isMovingItem = false;
			movingSlot.Clear();
		}
		else
		{
            isMovingItem = true;
        }
        RefreshUI();
        return true;

    }

	private Slot GetClosestSlot()
	{
        for (int i = 0; i < slots.Length; i++)
		{
			if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
			{
				return items[i];
			}
		}
            return null;
	}
    #endregion Moving Items
}
