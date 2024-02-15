using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField]GameObject inventory;
    public void ShowInventory () 
    {
        inventory.SetActive(true);
    } 
    public void HideInvemtory () 
    {
        inventory.SetActive(false);
    }
}
