using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private void Awake()
    {
        _instance = this;
        ConfineCursor(); // Ensuring cursor is confined to the game window on awake
    }

    private void Start()
    {
        HideCursor();
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Ensuring cursor is not locked
    }

    public void HideCursor()
    {
        Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Locked; Ensuring cursor is locked and hidden
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void ReturnTimeScale()
    {
        Time.timeScale = 1;
    }

    public void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}