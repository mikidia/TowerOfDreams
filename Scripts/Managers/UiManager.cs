using TMPro;
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
    [Header("LvlUp Menu")]
    [SerializeField] GameObject[] placeHolderForImages;
    [SerializeField] GameObject[] buttons;
    [SerializeField] Image[] skillImagesforLvlmenu;
    [SerializeField] GameObject levelUpMenu;
    [SerializeField] GameObject[] reloadObjects;
    [SerializeField] Image[] skillImages;
    [SerializeField] SkillPrefab skillPrefab;
    [SerializeField] TextMeshProUGUI[] playerSkillNumbers;
    [SerializeField] TextMeshProUGUI[] playerAfterLvlUpSkillNumbers;

    Reload[] reloadScripts;
    [SerializeField] GameObject reloadParent;

    [SerializeField] Player player;

    public static UImanager _instance;

    public GameObject[] ReloadObjects { get => reloadObjects; set => reloadObjects = value; }
    public Reload[] ReloadScripts { get => reloadScripts; set => reloadScripts = value; }

    void Start()
    {
        reloadScripts = reloadParent.GetComponentsInChildren<Reload>(true);
        player = GameObject.Find("Player").GetComponent<Player>();
        _instance = this;
        ChangeInventoryIcons();
    }

    // Update is called once per frame
    void Update()
    {
        StaminaBarFill();
        HideStamina();
        ExpBar();
        HPBarFill();
        HideHP();
    }

    void HideStamina()
    {
        staminaBarParent.SetActive(PlayerMovementScript.instance.Energy < PlayerMovementScript.instance.MaxEnergy);
    }

    public void StaminaBarFill()
    {
        staminaBar.fillAmount = PlayerMovementScript.instance.Energy / PlayerMovementScript.instance.MaxEnergy;
    }

    public void ExpBar()
    {
        float xpProgress = LevelingScr._instance.Exp / LevelingScr._instance.MaxExp;
        expBar.fillAmount = xpProgress;
    }

    public void HPBarFill()
    {
        hpBar.fillAmount = player.Hp / player.MaxHp;
    }

    void HideHP()
    {
        hpBarParent.SetActive(player.Hp < player.MaxHp);
    }

    private void FixedUpdate()
    {
        ChangeInventoryIcons();
    }

    void ChangeInventoryIcons()
    {
        for (int i = 0; i < placeHolderForImages.Length && i < player.SkillPrefabs.Length; i++)
        {
            Image inventoryImage = placeHolderForImages[i].GetComponent<Image>();
            if (inventoryImage != null)
            {
                inventoryImage.sprite = player.SkillPrefabs[i].sprite;
                inventoryImage.color = new Color(255, 255, 255);
            }
            else
            {
                Debug.LogError($"Inventory slot at index {i} does not have an Image component!");
            }
        }
    }

    public void SelectNextSlot()
    {
        if (player.SellectedSlot == 0)
        {
            inventory[3].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
        }
        else
        {
            inventory[player.SellectedSlot - 1].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
        }
    }

    public void SelectPreviouslySlot()
    {
        if (player.SellectedSlot == 3)
        {
            inventory[0].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
        }
        else
        {
            inventory[player.SellectedSlot + 1].SetActive(false);
            inventory[player.SellectedSlot].SetActive(true);
        }
    }

    public void ShowLevelUpMenu(int skillsNumber, Skills[] skillsToLvLUp)
    {
        ChangePlayerSkillsNumbers();

        levelUpMenu.SetActive(true);
        for (int i = 0; i < skillsNumber; i++)
        {
            skillImagesforLvlmenu[i].gameObject.SetActive(true);
            buttons[i].SetActive(true);
            if (!CheckPlayerStatsLvl(skillsToLvLUp, player, i))
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }

            skillImagesforLvlmenu[i].sprite = skillsToLvLUp[i].sprite;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = skillsToLvLUp[i].name;
        }

        GameManager._instance.ShowCursor();
        GameManager._instance.PauseTime();
    }

    bool CheckPlayerStatsLvl(Skills[] SkilsMinLvl, Player playerSkils, int i)
    {
        if (SkilsMinLvl[i]._stamina > playerSkils.Stamina)
        {
            return false;
        }
        return true;
    }

    public void HideLevelUpMenu()
    {
        GameManager._instance.ReturnTimeScale();
        levelUpMenu.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            skillImagesforLvlmenu[i].gameObject.SetActive(false);
            buttons[i].SetActive(false);
        }
        GameManager._instance.HideCursor();
    }

    public void ChangePlayerSkillsNumbers()
    {
        playerSkillNumbers[0].text = player.Intelect.ToString();
        playerSkillNumbers[1].text = player.Stamina.ToString();
        playerSkillNumbers[2].text = player.Strength.ToString();
        playerSkillNumbers[3].text = player.Agility.ToString();
        playerSkillNumbers[4].text = player.Vitality.ToString();

        playerAfterLvlUpSkillNumbers[0].text = (player.Intelect + 2).ToString();
        playerAfterLvlUpSkillNumbers[1].text = (player.Stamina + 2).ToString();
        playerAfterLvlUpSkillNumbers[2].text = (player.Strength + 2).ToString();
        playerAfterLvlUpSkillNumbers[3].text = (player.Agility + 2).ToString();
        playerAfterLvlUpSkillNumbers[4].text = (player.Vitality + 2).ToString();
    }
}