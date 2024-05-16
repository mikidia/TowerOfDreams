using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("Bars")]
    [SerializeField] Image staminaBar;
    [SerializeField] GameObject staminaBarParent;

    [SerializeField] Image hpBar;
    [SerializeField] GameObject hpBarParent;

    [SerializeField] Image expBar;
    [SerializeField] GameObject expBarParent;


    [SerializeField] GameObject[] inventory;
    [SerializeField] GameObject[] reloadObjects;
    [SerializeField] Image[] skillImages;
    [SerializeField] SkillPrefab skillPrefab;
    Reload[] reloadScripts;
    [SerializeField] GameObject reloadParent;




    [SerializeField] Player player;


    public static UImanager _instance;

    public GameObject[] ReloadObjects { get => reloadObjects; set => reloadObjects = value; }
    public Reload[] ReloadScripts { get => reloadScripts; set => reloadScripts = value; }

    void Start()
    {
        reloadScripts = reloadParent.GetComponentsInChildren<Reload>(true);
        player = Player._instance;
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        StaminaBarFill();
        HideStamina();
        ExpBar();



    }
    void HideStamina()
    {

        if (player.Stamina == player.MaxStamina)
        {

            staminaBarParent.SetActive(false);
        }
        if (player.Stamina < player.MaxStamina)
        {

            staminaBarParent.SetActive(true);

        }


    }
    public void StaminaBarFill()
    {
        staminaBar.fillAmount = player.Stamina / player.MaxStamina;
    }
    public void ExpBar()
    {

        print("asdsad");
        expBar.fillAmount = LevelingScr._instance.Exp / LevelingScr._instance.MaxExp;

    }
    public void SelectNextSlot()
    {





        if (player.SellectedSlot == 0)
        {
            inventory[3].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
            return;

        }
        else
        {
            inventory[player.SellectedSlot - 1].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
            return;


        }




    }
    public void SelectPreviouslySlot()
    {
        if (player.SellectedSlot == 3)
        {
            inventory[0].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
            return;

        }
        else
        {
            inventory[player.SellectedSlot + 1].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
            return;


        }




    }

}
