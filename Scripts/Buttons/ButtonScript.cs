using TMPro;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public void Button1()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;
        UImanager._instance.HideLevelUpMenu();


    }
    public void Button2()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;

        UImanager._instance.HideLevelUpMenu();



    }
    public void Button3()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;
        UImanager._instance.HideLevelUpMenu();



    }
    public void Button4()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;
        UImanager._instance.HideLevelUpMenu();



    }
    public void AddIntelect()
    {

        Player._instance.Intelect += 2;
        UImanager._instance.HideLevelUpMenu();


    }
    public void AddStamina()
    {

        Player._instance.Stamina += 2;
        UImanager._instance.HideLevelUpMenu();


    }
    public void AddStrenght()
    {

        Player._instance.Strength += 2;
        UImanager._instance.HideLevelUpMenu();


    }
    public void AddAgility()
    {

        Player._instance.Agility += 2;
        UImanager._instance.HideLevelUpMenu();


    }
    public void AddViatlity()
    {

        Player._instance.Vitality += 2;
        UImanager._instance.HideLevelUpMenu();


    }
}
