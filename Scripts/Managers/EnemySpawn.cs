using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawn : MonoBehaviour
{
    // The enemy prefab to be spawned.
    [SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField] float spawnTime = 3f;
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform bossParent;

    [SerializeField] float maxEnemy = 4;// How long between each spawn.
    [SerializeField] Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    GameManager gameManager;
    LevelGenerator levelGenerator;
    [SerializeField]GameObject boss;
    bool spawnIsActive = true;

    void Start ()
    {


        levelGenerator =  GameObject.Find("LevelGeneratorManager").GetComponent<LevelGenerator>(); 
        spawnPoints = GetComponentsInChildren<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        setSpawnPoints();
    }


    void setSpawnPoints () 
    {
        foreach (Transform i in spawnPoints) 
        {
            i.position = new Vector3(Random.Range(levelGenerator.FloorSize[0].x, levelGenerator.FloorSize[1].x) ,i.position.y, Random.Range(levelGenerator.FloorSize[0].y, levelGenerator.FloorSize[1].y));
        
        }

        Spawn();
    }
    private void Update ()
    {
        if (gameManager.enemyIsdeath > 50) 
        {
            bool spawnIsActive = false;
        }
    }


    void Spawn ()
    {
        if ((enemyParent.childCount<maxEnemy+ enemies.Count)&&spawnIsActive) 
        {
            spawnPoints = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++) spawnPoints[i] = transform.GetChild(i);
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            if (spawnPoints.Length > 0) Invoke("Spawn", spawnTime);
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int selectEnemy = Random.Range(0, 3);
           GameObject enemyprefab =  Instantiate(enemies[selectEnemy], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation, enemyParent);
            enemyprefab.SetActive(true);
        }else if (!spawnIsActive) 
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            GameObject enemyprefab =  Instantiate(boss,spawnPoints[spawnPointIndex].position,spawnPoints[spawnPointIndex].rotation, bossParent);


        }



    }

}
