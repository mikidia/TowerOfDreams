using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab; // Prefab of the enemy
    public int count; // Number of enemies of this type
    public string tag; // Tag for the enemies of this type
    public bool shouldAssignTag; // Whether the tag should be assigned to the enemy
}

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyType> enemyTypes; // List of enemy types
    public GameObject spawnArea; // GameObject defining the spawn area
    public GameObject player; // The player object
    public float spawnMargin = 1.0f; // Minimum distance from the spawn area's edges
    public LayerMask environmentLayer; // Layer for objects implementing IEnvironment interface

    [Header("Spawn chance")]
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnRegular;
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnElite;
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnBoss;

    [SerializeField] private Transform roomParent; // Room parent
    [SerializeField] private Transform corridorParent; // Corridor parent

    private Bounds spawnBounds;
    [SerializeField] private Transform enemyParent;

    void Start()
    {
        // Check if enemyParent is assigned
        if (enemyParent == null)
        {
            Debug.LogError("EnemyParent is not assigned. Please assign it in the inspector.");
            return;
        }

        // Check if player is assigned
        if (player == null)
        {
            Debug.LogError("Player is not assigned. Please assign it in the inspector.");
            return;
        }

        // Spawn enemies at the start
        SpawnAllEnemies();
    }

    void SpawnAllEnemies()
    {
        // Check for active game objects in roomParent first, then in corridorParent
        if (CheckForActiveObjects(roomParent))
        {
            spawnBounds = GetCombinedBounds(roomParent);
        }
        else if (CheckForActiveObjects(corridorParent))
        {
            spawnBounds = GetCombinedBounds(corridorParent);
        }
        else
        {
            Debug.LogError("No active objects found in roomParent or corridorParent.");
            return;
        }

        foreach (var enemyType in enemyTypes)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                Vector3 spawnPosition;
                do
                {
                    spawnPosition = GetRandomSpawnPosition();
                } while (!IsValidSpawnPosition(spawnPosition));

                SpawnEnemy(spawnPosition, enemyType.enemyPrefab, enemyType.tag, enemyType.shouldAssignTag);
            }
        }
    }

    bool CheckForActiveObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    Bounds GetCombinedBounds(Transform parent)
    {
        Bounds combinedBounds = new Bounds();
        bool hasBounds = false;

        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (!hasBounds)
                    {
                        combinedBounds = renderer.bounds;
                        hasBounds = true;
                    }
                    else
                    {
                        combinedBounds.Encapsulate(renderer.bounds);
                    }
                }
            }
        }
        return combinedBounds;
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position within the spawn bounds, considering the player's height
        return new Vector3(
            Random.Range(spawnBounds.min.x + spawnMargin, spawnBounds.max.x - spawnMargin),
            player.transform.position.y, // Use the player's height
            Random.Range(spawnBounds.min.z + spawnMargin, spawnBounds.max.z - spawnMargin)
        );
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        // Check if the position is not inside another object with the IEnvironment interface
        Collider[] colliders = Physics.OverlapSphere(position, spawnMargin, environmentLayer);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<IEnvironment>() != null)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnEnemy(Vector3 position, GameObject enemyPrefab, string tag, bool shouldAssignTag)
    {
        // Check if enemyPrefab is assigned
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned.");
            return;
        }

        // Spawn the enemy and set enemyParent as its parent
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, enemyParent);
        enemy.SetActive(true);
        enemy.GetComponent<MobType>().SelectType(_chanceToSpawnRegular, _chanceToSpawnElite, _chanceToSpawnBoss);

        // Assign the tag to the enemy if shouldAssignTag is true
        if (shouldAssignTag)
        {
            enemy.tag = tag;
        }
    }
}