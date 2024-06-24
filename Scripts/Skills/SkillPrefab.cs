using System.Collections;
using UnityEngine;

public class SkillPrefab : MonoBehaviour
{
    [SerializeField] float skillTime;
    [SerializeField] float skillCd;
    [SerializeField] int skillDamage;
    [SerializeField] float slowdowneffect;
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

        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void FixedUpdate()
    {
        UpdateScale(player);
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
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyMain>();
        if (enemy != null)
        {
            enemy.GetDamage(skillDamage);
            enemy.MoveSpeed = enemy.MoveSpeed / 2;




        }
    }


    public void UpdateScale(Player player)
    {
        if (gameObject.transform.localScale.x != 1 + player.Intelect / 10)
            gameObject.transform.localScale = new Vector3(1 + player.Intelect / 10, 1, 1 + player.Intelect / 10);

    }
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyMain>();
        if (enemy != null)
        {
            enemy.MoveSpeed = enemy.baseMoveSpeed;




        }
    }

}
