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


}
