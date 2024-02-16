using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    [SerializeField]GameObject inventory;
    [SerializeField]GameObject staminaBar;
    [SerializeField]Player player;
    Slider staminaSlider;


    private void FixedUpdate ()
    {
        StaminaBarFill();
    }
    private void Start ()
    {
        staminaSlider = staminaBar.GetComponent<Slider>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void ShowInventory () 
    {
        inventory.SetActive(true);
    } 
    public void HideInvemtory () 
    {
        inventory.SetActive(false);
    }
    public void StaminaBarFill () 
    {
        staminaSlider.maxValue = player.MaxStamina;
        staminaSlider.value = player.Stamina;
    }
    public void StaminaBarShow () 
    {
        staminaBar.SetActive(true);


    }
    public void StaminaBarHide()
    {
        staminaBar.SetActive(false);


    }
}
