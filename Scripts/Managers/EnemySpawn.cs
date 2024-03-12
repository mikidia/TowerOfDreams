using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;                // The enemy prefab to be spawned.
    public List<GameObject> enemies = new List<GameObject>();
    public float spawnTime = 3f;            // How long between each spawn.

    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.


    void Start ()
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) spawnPoints[i] = transform.GetChild(i);

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        if (spawnPoints.Length > 0) InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int selectEnemy = Random.Range(0, 3);
        Instantiate(enemies[selectEnemy], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);


    }

}
