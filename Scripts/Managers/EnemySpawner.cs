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
    public GameObject player; // The player object
    public float spawnMargin = 1.0f; // Minimum distance from the spawn area's edges
    public LayerMask environmentLayer; // Layer for objects implementing IEnvironment interface
    public Vector3 spawnOffset; // Offset for spawn position adjustment

    [Header("Spawn chance")]
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnRegular;
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnElite;
    [Range(0, 1000)]
    [SerializeField] private int _chanceToSpawnBoss;

    [SerializeField] private Transform roomParent; // Room parent
    [SerializeField] private int roomCounter = 1;

    private Bounds spawnBounds;
    [SerializeField] private Transform enemyParent;

    // List to store all spawned enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Index for the next enemy to spawn
    private int nextEnemyIndex = 0;

    public int RoomCounter { get => roomCounter; set => roomCounter = value; }

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

        // Spawn all enemies initially but keep them inactive
        SpawnAllEnemies();
        SpawnNextEnemy(Random.Range(roomCounter, roomCounter + Random.Range(1, roomCounter)));
    }

    private void FixedUpdate()
    {
    }

    public void SpawnAllEnemies()
    {
        spawnBounds = new Bounds(Vector3.zero, Vector3.zero);
        // Check for active game objects in roomParent first
        if (CheckForActiveObjects(roomParent))
        {
            spawnBounds = GetCombinedBounds(roomParent);
        }
        else
        {
            Debug.LogError("No active objects found in roomParent.");
            return;
        }
        foreach (var enemyType in enemyTypes)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                Vector3 spawnPosition;
                int attempts = 0;
                const int maxAttempts = 100;
                do
                {
                    spawnPosition = GetRandomSpawnPosition() + spawnOffset;
                    attempts++;
                } while (!IsValidSpawnPosition(spawnPosition) && attempts < maxAttempts);

                if (attempts >= maxAttempts)
                {
                    Debug.LogWarning("Failed to find a valid spawn position after " + maxAttempts + " attempts.");
                    return;
                }
                else
                {
                    // Spawn enemy and add to the list
                    GameObject enemy = SpawnEnemy(spawnPosition, enemyType.enemyPrefab, enemyType.tag, enemyType.shouldAssignTag);
                    if (enemy != null)
                    {
                        enemy.SetActive(false); // Keep the enemy inactive
                        spawnedEnemies.Add(enemy);
                    }
                }
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
        // Adjust bounds to include spawnMargin
        return combinedBounds;
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Calculate screen bounds in world space
        Camera mainCamera = Camera.main;
        Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.transform.position.z));

        // Randomly decide which side to spawn on (left, right, top, bottom)
        int side = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Left
                spawnPosition = new Vector3(screenBottomLeft.x - 10, player.transform.position.y, Random.Range(screenBottomLeft.z, screenTopRight.z));
                break;
            case 1: // Right
                spawnPosition = new Vector3(screenTopRight.x + 10, player.transform.position.y, Random.Range(screenBottomLeft.z, screenTopRight.z));
                break;
            case 2: // Top
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), player.transform.position.y, screenTopRight.z + 10);
                break;
            case 3: // Bottom
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), player.transform.position.y, screenBottomLeft.z - 10);
                break;
        }

        return spawnPosition;
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        // Cast a ray downward from the position to check for collisions with the environment layer
        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hit;

        // Adjust the ray distance as needed; here, we assume 100 units is sufficient
        float rayDistance = 10f;

        if (Physics.Raycast(ray, out hit, rayDistance, environmentLayer))
        {
            return true;
        }

        return false;
    }

    public void SetupCharacteristics()
    {
        // Implementation for setting up characteristics if needed
    }

    GameObject SpawnEnemy(Vector3 position, GameObject enemyPrefab, string tag, bool shouldAssignTag)
    {
        // Check if enemyPrefab is assigned
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned.");
            return null;
        }

        // Spawn the enemy and set enemyParent as its parent
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, enemyParent);
        enemy.GetComponent<MobType>().SelectType(_chanceToSpawnRegular, _chanceToSpawnElite, _chanceToSpawnBoss);
        RecheckEnemyPosition(enemy, spawnBounds);

        // Assign the tag to the enemy if shouldAssignTag is true
        if (shouldAssignTag)
        {
            enemy.tag = tag;
        }

        return enemy;
    }

    void RecheckEnemyPosition(GameObject enemyObj, Bounds boundsOfRoom)
    {
        bool rightPosition = false;

        do
        {
            if (enemyObj.transform.position.x < boundsOfRoom.min.x - 2 ||
                enemyObj.transform.position.z < boundsOfRoom.min.z - 2 ||
                enemyObj.transform.position.x > boundsOfRoom.max.x + 2 ||
                enemyObj.transform.position.z > boundsOfRoom.max.z + 2)
            {
                enemyObj.transform.position = new Vector3(
                    Random.Range(boundsOfRoom.min.x + 2, boundsOfRoom.max.x - 2),
                    player.transform.position.y,
                    Random.Range(boundsOfRoom.min.z + 2, boundsOfRoom.max.z - 2)
                );
            }
            else
            {
                rightPosition = true;
            }
        } while (!rightPosition);
    }

    // Method to spawn the next enemy from the list
    public void SpawnNextEnemy(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (nextEnemyIndex < spawnedEnemies.Count)
            {
                spawnedEnemies[nextEnemyIndex].SetActive(true);
                nextEnemyIndex++;
            }
            else
            {
                Debug.LogWarning("No more enemies to spawn.");
            }
        }
    }
}
