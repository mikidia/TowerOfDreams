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
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject looseMenu;

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
        Cursor.visible = cursorVisible;
        pauseMenu.SetActive(true);
        audio.StopMusicAndSound();
    }
    public void UpPause () 
    {
        pauseMenu.SetActive(false);

        cursorVisible = false;
        Cursor.visible = cursorVisible;
        Time.timeScale = 1.0f;
        audio.StartMusicAfterPause();
    }
    public void win ()
    {   cursorVisible = true;
        Cursor.visible = cursorVisible;

        Time.timeScale = 0f;
        
        winMenu.SetActive(true);
        audio.StopMusicAndSound();
    }
    public void loose ()
    {   cursorVisible = true;
        Cursor.visible = cursorVisible;
        Time.timeScale = 0f;
        
        looseMenu.SetActive(true);
        audio.StopMusicAndSound();
    }



}
