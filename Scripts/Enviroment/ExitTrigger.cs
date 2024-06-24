using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField] int type;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            LevelGeneratorManager manager = GameObject.Find("LevelGenerator").GetComponent<LevelGeneratorManager>();
            if (manager != null)
            {
                manager.SetLastExitType(type); // Store the exit type in the manager

                manager.GenerateRoom(false);
                EnemySpawner enemySpawner = GameObject.Find("EnemyManagers").GetComponent<EnemySpawner>();
                enemySpawner.SpawnAllEnemies();
                enemySpawner.RoomCounter++;
                enemySpawner.SpawnNextEnemy(Random.Range(enemySpawner.RoomCounter, enemySpawner.RoomCounter + Random.Range(1, enemySpawner.RoomCounter)));


                // Use Invoke to delay the position setting

                manager.Invoke("DelayedSetPlayerPosition", 0.1f);
            }
        }
    }
}
