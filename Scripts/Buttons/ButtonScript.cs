using TMPro;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public void Button1()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;


    }
    public void Button2()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;


    }
    public void Button3()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;


    }
    public void Button4()
    {
        LevelingScr._instance.SkillLevels[this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text] += 1;


    }
}
