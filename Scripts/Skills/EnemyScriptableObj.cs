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

    public string Name { get => _name; set => _name = value; }
    public float[] Intelect { get => _intelect; set => _intelect = value; }
    public float[] Stamina { get => _stamina; set => _stamina = value; }
    public float[] Strength { get => _strength; set => _strength = value; }
    public float[] Agility { get => _agility; set => _agility = value; }
    public float[] Vitality { get => _vitality; set => _vitality = value; }
    public float[] Agressive { get => _agressive; set => _agressive = value; }
}
