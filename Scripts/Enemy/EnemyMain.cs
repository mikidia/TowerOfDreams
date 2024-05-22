using UnityEngine;

[System.Serializable]
public class EnemyAbility
{
    // Здесь можно добавить способности врага
}

public class EnemyMain : MonoBehaviour, IEnemy
{
    [SerializeField] private float baseIntelect = 1f;
    [SerializeField] private float baseStamina = 1f;
    [SerializeField] private float baseStrength = 1f;
    [SerializeField] private float baseAgility = 1f;
    [SerializeField] private float baseVitality = 1f;
    [SerializeField] private float baseAgressive = 1f;

    [SerializeField] private float baseHp = 10f;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseEnergy = 50f;

    private float intelect;
    private float stamina;
    private float strength;
    private float agility;
    private float vitality;
    private float agresive;
    private float hp;
    private float damage;
    private float moveSpeed;
    private float energy;

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
        intelect = baseIntelect;
        stamina = baseStamina;
        strength = baseStrength;
        agility = baseAgility;
        vitality = baseVitality;
        agresive = baseAgressive;
        hp = baseHp;
        damage = baseDamage;
        moveSpeed = baseMoveSpeed;
        energy = baseEnergy;

        mobType = GetComponent<MobType>();

        if (mobType != null)
        {
            // Добавляем модификаторы в зависимости от типа моба
            if (mobType.IsRegularMob)
            {
                hp += vitality * 1.5f;
                damage += strength * 1.5f;
                moveSpeed += agility * 1.5f;
                energy += agility * 3.0f;
            }
            else if (mobType.IsEliteMob)
            {
                hp += vitality * 3f;
                damage += strength * 2.0f;
                moveSpeed += agility * 2.0f;
                energy += agility * 3.0f;
            }
            else if (mobType.IsBossMob)
            {
                hp += vitality * 6.0f;
                damage += strength * 3.0f;
                moveSpeed += agility * 1.5f;
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
}
