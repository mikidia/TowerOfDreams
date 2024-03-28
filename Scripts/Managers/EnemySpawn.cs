using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // The enemy prefab to be spawned.
    [SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField] float spawnTime = 3f;            // How long between each spawn.
    [SerializeField] Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    LevelGenerator levelGenerator;

    void Start ()
    {


        levelGenerator = LevelGenerator.instance;
        spawnPoints = GetComponentsInChildren<Transform>();
        setSpawnPoints();
    }


    void setSpawnPoints () 
    {
        foreach (Transform i in spawnPoints) 
        {
            i.position = new Vector3(Random.RandomRange(levelGenerator.FloorSize[0].x, levelGenerator.FloorSize[1].x) ,i.position.y, Random.RandomRange(levelGenerator.FloorSize[0].y, levelGenerator.FloorSize[1].y));
        
        }

        Spawn();
    }


    void Spawn ()
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) spawnPoints[i] = transform.GetChild(i);
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        if (spawnPoints.Length > 0) Invoke("Spawn", spawnTime);
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int selectEnemy = Random.Range(0, 3);
        Instantiate(enemies[selectEnemy], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);


    }

}
