using UnityEngine;
[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy/Create New Enemy")]
public class EnemyScriptableObj : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] bool isBoss;
    [SerializeField] bool isEliteMob;
    [SerializeField] bool isRegularMob;





    [Tooltip("enemy name")]

    [SerializeField] string _name;
    [Tooltip("Open some new skills  ")]
    [Min(1)]
    [SerializeField] float[] _intelect;
    [Tooltip("Enemy can use more times ability")]
    [Min(1)]

    [SerializeField] float[] _stamina;
    [Tooltip("Enemy has more damage and damage resist")]
    [Min(1)]

    [SerializeField] float[] _strength;
    [Tooltip("Enemy can move and attack faster ")]
    [Min(1)]

    [SerializeField] float[] _agility;
    [Tooltip("Enemy has more hp and")]
    [Min(1)]

    [SerializeField] float[] _vitality;
    [Tooltip("The more agressive is enemy the more them try to attack player and less dodge attacks combine with intelect")]
    [Min(1)]

    [SerializeField] float[] _agressive;
    [SerializeField] float[] _expAfterDeath;






    [Tooltip("elite enemy name")]
    [SerializeField] string _eliteName;
    [Tooltip("elite Open some new skills  ")]
    [SerializeField] float[] _eliteIntelect;
    [Tooltip("elite Enemy can use more times ability")]
    [SerializeField] float[] _eliteStamina;
    [Tooltip("elite Enemy has more damage and damage resist")]
    [SerializeField] float[] _eliteStrength;
    [Tooltip("elite Enemy can move and attack faster ")]
    [SerializeField] float[] _eliteAgility;
    [Tooltip("elite Enemy has more hp and")]
    [SerializeField] float[] _eliteVitality;
    [Tooltip("The more agressive is enemy the more them try to attack player and less dodge attacks combine with intelect")]
    [SerializeField] float[] _eliteAgressive;
    [SerializeField] float[] _eliteExpAfterDeath;




    [Tooltip("Boss enemy name")]
    [SerializeField] string _bossName;
    [Tooltip("Boss Open some new skills  ")]
    [SerializeField] float[] _bossIntelect;
    [Tooltip("Boss Enemy can use more times ability")]
    [SerializeField] float[] _bossStamina;
    [Tooltip("Boss Enemy has more damage and damage resist")]
    [SerializeField] float[] _bossStrength;
    [Tooltip("Boss Enemy can move and attack faster ")]
    [SerializeField] float[] _bossAgility;
    [Tooltip("Boss Enemy has more hp and")]
    [SerializeField] float[] _bossVitality;
    [Tooltip("The more agressive is enemy the more them try to attack player and less dodge attacks combine with intelect")]
    [SerializeField] float[] _bossAgressive;
    [SerializeField] float[] _bossExpAfterDeath;


    public string Name { get => _name; set => _name = value; }
    public float[] Intelect { get => _intelect; set => _intelect = value; }
    public float[] Stamina { get => _stamina; set => _stamina = value; }
    public float[] Strength { get => _strength; set => _strength = value; }
    public float[] Agility { get => _agility; set => _agility = value; }
    public float[] Vitality { get => _vitality; set => _vitality = value; }
    public float[] Agressive { get => _agressive; set => _agressive = value; }
    public string EliteName { get => _eliteName; set => _eliteName = value; }

    public float[] EliteIntelect { get => _eliteIntelect; set => _eliteIntelect = value; }
    public float[] EliteStamina { get => _eliteStamina; set => _eliteStamina = value; }
    public float[] EliteStrength { get => _eliteStrength; set => _eliteStrength = value; }
    public float[] EliteAgility { get => _eliteAgility; set => _eliteAgility = value; }
    public float[] EliteVitality { get => _eliteVitality; set => _eliteVitality = value; }
    public float[] EliteAgressive { get => _eliteAgressive; set => _eliteAgressive = value; }
    public string BossName { get => _bossName; set => _bossName = value; }
    public float[] BossIntelect { get => _bossIntelect; set => _bossIntelect = value; }
    public float[] BossStamina { get => _bossStamina; set => _bossStamina = value; }
    public float[] BossStrength { get => _bossStrength; set => _bossStrength = value; }
    public float[] BossAgility { get => _bossAgility; set => _bossAgility = value; }
    public float[] BossVitality { get => _bossVitality; set => _bossVitality = value; }
    public float[] BossAgressive { get => _bossAgressive; set => _bossAgressive = value; }
    public float[] ExpAfterDeath { get => _expAfterDeath; set => _expAfterDeath = value; }
    public float[] EliteExpAfterDeath { get => _eliteExpAfterDeath; set => _eliteExpAfterDeath = value; }
    public float[] BossExpAfterDeath { get => _bossExpAfterDeath; set => _bossExpAfterDeath = value; }
}
