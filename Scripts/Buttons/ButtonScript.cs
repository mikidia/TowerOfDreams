using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{

    public void Button1()
    {
        try
        {
            LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;

        }
        catch
        {
            LevelingScr._instance.AddSkill(this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        UImanager._instance.HideLevelUpMenu();



    }
    public void Button2()
    {
        try
        {
            LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;

        }
        catch
        {
            LevelingScr._instance.AddSkill(this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        UImanager._instance.HideLevelUpMenu();



    }
    public void Button3()
    {
        try
        {
            LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;

        }
        catch
        {
            LevelingScr._instance.AddSkill(this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        UImanager._instance.HideLevelUpMenu();



    }
    public void Button4()
    {
        try
        {
            LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;

        }
        catch
        {
            LevelingScr._instance.AddSkill(this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        }
        UImanager._instance.HideLevelUpMenu();



    }
    public void AddIntelect()
    {

        Player._instance.Intelect += 2;
        UImanager._instance.HideLevelUpMenu();
        LevelingScr._instance.UpdatePlayerStats();

    }
    public void AddStamina()
    {

        Player._instance.Stamina += 2;
        UImanager._instance.HideLevelUpMenu();
        LevelingScr._instance.UpdatePlayerStats();

    }
    public void AddStrenght()
    {

        Player._instance.Strength += 2;
        UImanager._instance.HideLevelUpMenu();
        LevelingScr._instance.UpdatePlayerStats();

    }
    public void AddAgility()
    {

        Player._instance.Agility += 2;
        UImanager._instance.HideLevelUpMenu();
        LevelingScr._instance.UpdatePlayerStats();

    }
    public void AddViatlity()
    {

        Player._instance.Vitality += 2;
        UImanager._instance.HideLevelUpMenu();
        LevelingScr._instance.UpdatePlayerStats();

    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainSchene", LoadSceneMode.Single);


    }
}
