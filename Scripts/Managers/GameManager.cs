using UnityEngine;

public class GameManager : MonoBehaviour
{


    public static GameManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        HideCursor();
    }

    public void ShowCursor()
    {

        Cursor.visible = true;

    }
    public void HideCursor()
    {

        Cursor.visible = false;

    }
    public void PauseTime()
    {

        Time.timeScale = 0;

    }
    public void ReturnTimeScale()
    {

        Time.timeScale = 1;


    }
}
