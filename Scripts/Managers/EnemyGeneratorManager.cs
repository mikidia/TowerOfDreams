using UnityEngine;

public class EnemyGeneratorManager : MonoBehaviour
{
    [SerializeField] EnemyScriptableObj[] enemyScrObjests;
    [SerializeField] EnemyMain[] enemys;
    [SerializeField] MobType[] enemysTypes;

    [SerializeField] GameObject enemyParent;
    public static EnemyGeneratorManager _instance;

    private void Awake()
    {
        _instance = this;


    }
    private void Start()
    {
        // Allocate memory for the enemies array based on the number of children under enemyParent

        enemys = new EnemyMain[enemyParent.transform.childCount];
        enemysTypes = new MobType[enemyParent.transform.childCount];

        for (int i = 0; i < enemyParent.transform.childCount; i++)
        {
            enemys[i] = enemyParent.transform.GetChild(i).GetComponent<EnemyMain>();
            enemysTypes[i] = enemyParent.transform.GetChild(i).GetComponent<MobType>();

        }
        GenerateEnemyStats();
    }

    public void GenerateEnemyStats()
    {

        for (int i = 0; i < enemys.Length; i++)
        {
            for (int j = 0; j < enemyScrObjests.Length; j++)
            {
                if (enemysTypes[i].IsRegularMob)
                {
                    enemys[i].Intelect = Random.Range((int)enemyScrObjests[j].Intelect[0], (int)enemyScrObjests[j].Intelect[1] + 1);
                    enemys[i].Stamina = Random.Range((int)enemyScrObjests[j].Stamina[0], (int)enemyScrObjests[j].Stamina[1] + 1);
                    enemys[i].Strength = Random.Range((int)enemyScrObjests[j].Strength[0], (int)enemyScrObjests[j].Strength[1] + 1);
                    enemys[i].Agility = Random.Range((int)enemyScrObjests[j].Agility[0], (int)enemyScrObjests[j].Agility[1] + 1);
                    enemys[i].Vitality = Random.Range((int)enemyScrObjests[j].Vitality[0], (int)enemyScrObjests[j].Vitality[1] + 1);
                    enemys[i].Agressive = Random.Range((int)enemyScrObjests[j].Agressive[0], (int)enemyScrObjests[j].Agressive[1] + 1);


                }
                if (enemysTypes[i].IsEliteMob)
                {

                    enemys[i].Intelect = Random.Range((int)enemyScrObjests[j].EliteIntelect[0], (int)enemyScrObjests[j].EliteIntelect[1] + 1);
                    enemys[i].Stamina = Random.Range((int)enemyScrObjests[j].EliteStamina[0], (int)enemyScrObjests[j].EliteStamina[1] + 1);
                    enemys[i].Strength = Random.Range((int)enemyScrObjests[j].EliteStrength[0], (int)enemyScrObjests[j].EliteStrength[1] + 1);
                    enemys[i].Agility = Random.Range((int)enemyScrObjests[j].EliteAgility[0], (int)enemyScrObjests[j].EliteAgility[1] + 1);
                    enemys[i].Vitality = Random.Range((int)enemyScrObjests[j].EliteVitality[0], (int)enemyScrObjests[j].EliteVitality[1] + 1);
                    enemys[i].Agressive = Random.Range((int)enemyScrObjests[j].EliteAgressive[0], (int)enemyScrObjests[j].EliteAgressive[1] + 1);


                }
                if (enemysTypes[i].IsBossMob)
                {
                    enemys[i].Intelect = Random.Range((int)enemyScrObjests[j].BossIntelect[0], (int)enemyScrObjests[j].BossIntelect[1] + 1);
                    enemys[i].Stamina = Random.Range((int)enemyScrObjests[j].BossStamina[0], (int)enemyScrObjests[j].BossStamina[1] + 1);
                    enemys[i].Strength = Random.Range((int)enemyScrObjests[j].BossStrength[0], (int)enemyScrObjests[j].BossStrength[1] + 1);
                    enemys[i].Agility = Random.Range((int)enemyScrObjests[j].BossAgility[0], (int)enemyScrObjests[j].BossAgility[1] + 1);
                    enemys[i].Vitality = Random.Range((int)enemyScrObjests[j].BossVitality[0], (int)enemyScrObjests[j].BossVitality[1] + 1);
                    enemys[i].Agressive = Random.Range((int)enemyScrObjests[j].BossAgressive[0], (int)enemyScrObjests[j].BossAgressive[1] + 1);



                }


            }
        }
    }
}