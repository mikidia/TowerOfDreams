using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "new Skill", menuName = "skills/Create New Skill")]
public class Skills : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public float damage;
    [SerializeField] public float duration;
    [SerializeField] public float cd;
    [Tooltip("0 not damageable skill,1 splashing,2 fire,3 ice,4 crushing,5 bullet,6 magical")]
    [SerializeField] public float typeOfDamage;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject skillEffect;


    [Tooltip("Minimum lvl of characteristic to open skill")]
    [SerializeField] public float _intelect;
    [Tooltip("Minimum lvl of characteristic to open skill")]
    [SerializeField] public float _stamina;
    [Tooltip("Minimum lvl of characteristic to open skill")]
    [SerializeField] public float _strength;
    [Tooltip("Minimum lvl of characteristic to open skill")]
    [SerializeField] public float _agility;
    [Tooltip("Minimum lvl of characteristic to open skill")]
    [SerializeField] public float _vitality;

    [Tooltip("Skill Script left empty if dont use or assigt script from ASSETS FOLDER not from HIERARCHY ")]

    [SerializeField] MonoScript scripts;

    public virtual void UseSkill() { }

}
