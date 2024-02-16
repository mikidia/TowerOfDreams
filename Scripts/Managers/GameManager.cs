using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declarations
    [SerializeField]Player player;
    [SerializeField]UiManager uiManager;
    bool cursorVisible=false;

    #endregion
    private void Start ()
    {
        uiManager= GameObject.Find("UiManager").GetComponent<UiManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Cursor.visible = cursorVisible;
    }
    private void FixedUpdate ()
    {
        ShowStaminaBar();
        print("fixed");
    }
    void ShowStaminaBar () 
    {
        if (player.Stamina>=player.MaxStamina) 
        {
              uiManager.StaminaBarHide();
            print("hide");
        }
        else 
        {
        uiManager.StaminaBarShow();
        }
    }
    
}
