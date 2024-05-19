using UnityEngine;

public class EnemyMain : MonoBehaviour, IEnemy
{
    [SerializeField] private float intelect;
    [SerializeField] private float stamina;
    [SerializeField] private float strength;
    [SerializeField] private float agility;
    [SerializeField] private float vitality;
    [SerializeField] private float agresive;

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




}
