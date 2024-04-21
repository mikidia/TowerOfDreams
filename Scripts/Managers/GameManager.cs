using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declarations
    [SerializeField]Player player;
    [SerializeField]UiManager uiManager;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject pauseMenu;
    bool inventoryIsOpen =false;
    bool cursorVisible=false;
    public int enemyIsdeath = 0;
    SoundManager audio;

    

    #endregion
    private void Start ()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Cursor.visible = cursorVisible;
        audio = GameObject.Find("Sound manager").GetComponent<SoundManager>();

    }
    public void addDeathForEnemy () 
    {

        enemyIsdeath++;


    }


    public void showInventory () 
    {
        inventoryIsOpen = !inventoryIsOpen;
        if (inventoryIsOpen) 
        {
            inventory.SetActive(true);



        }
        else { 
            inventory.SetActive(false);
        }
    
    
    }
    public void Pause () 
    {
        Time.timeScale = 0f;
        cursorVisible = true;
        pauseMenu.SetActive(true);
        audio.StopMusicAndSound();
    }
    public void UpPause () 
    {
        pauseMenu.SetActive(false);
        cursorVisible = false;
        Time.timeScale = 1.0f;
        audio.StartMusicAfterPause();
    }



}
