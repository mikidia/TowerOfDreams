using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyAbility
{
    // Здесь можно добавить способности врага
}

public class EnemyMain : MonoBehaviour, IEnemy
{
    [Header("Base Stats")]

    [SerializeField] private float baseHp = 10f;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseEnergy = 50f;
    [Header("Final  Stats")]

    [SerializeField] private float intelect;
    [SerializeField] private float stamina;
    [SerializeField] private float strength;
    [SerializeField] private float agility;
    [SerializeField] private float vitality;
    [SerializeField] private float agresive;
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float energy;
    [SerializeField] private bool isDeath = false;

    private MobType mobType;

    public static EnemyMain _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        SetupCharacteristics();
    }

    void SetupCharacteristics()
    {
        // Устанавливаем базовые значения
        hp = baseHp;
        damage = baseDamage;
        moveSpeed = baseMoveSpeed;
        energy = baseEnergy;

        mobType = GetComponent<MobType>();

        if (mobType != null)
        {

            if (mobType.IsRegularMob)
            {
                hp += vitality * 1.5f;
                damage += strength * 1.5f;
                moveSpeed += agility * 1.5f;
                energy += agility * 3.0f;
            }
            else if (mobType.IsEliteMob)
            {
                hp += vitality * 6f;
                damage += strength * 2.0f;
                moveSpeed += agility * 1.3f;
                energy += agility * 3.0f;
            }
            else if (mobType.IsBossMob)
            {
                hp += vitality * 10.0f;
                damage += strength * 3.0f;
                moveSpeed += agility * 1.2f;
                energy += agility * 3.0f;
            }
        }
    }

    // Свойства для доступа к характеристикам
    public float Intelect
    {
        get { return intelect; }
        set { intelect = value; }
    }

    public float Stamina
    {
        get { return stamina; }
        set { stamina = value; }
    }

    public float Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public float Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    public float Vitality
    {
        get { return vitality; }
        set { vitality = value; }
    }

    public float Agressive
    {
        get { return agresive; }
        set { agresive = value; }
    }

    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float Energy
    {
        get { return energy; }
        set { energy = value; }
    }




    public void GetDamage(int damage)
    {
        if (hp - damage <= 0)
        {

            Destroy(gameObject);
            isDeath = true;

        }
        else
        {
            hp -= damage;
            StartCoroutine("DamageEffect");



        }


    }
    IEnumerator DamageEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.black;



    }
}
