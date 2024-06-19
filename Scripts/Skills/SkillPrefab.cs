using System.Collections;
using UnityEngine;

public class SkillPrefab : MonoBehaviour
{
    [SerializeField] float skillTime;
    [SerializeField] float skillCd;
    [SerializeField] int slot;
    [SerializeField] float castTime;



    UImanager ui;
    [SerializeField] Player player;

    public float SkillTime { get => skillTime; set => skillTime = value; }
    public float SkillCd1 { get => skillCd; set => skillCd = value; }
    public float CastTime { get => castTime; set => castTime = value; }

    private void Start()
    {
        StartCoroutine("skillDuration");

    }
    public void ApplySlot(int numberOfSlot)
    {
        slot = numberOfSlot;

    }

    IEnumerator skillDuration()
    {

        yield return new WaitForSeconds(skillTime);

        //skillManager.AddSkill(this.gameObject);
        //ui.ReloadObjects[slot].SetActive(true);



        GameObject.Destroy(gameObject);

    }
    IEnumerator SkillCd()
    {

        yield return new WaitForSeconds((float)skillCd);

    }
}
