using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorManager : MonoBehaviour
{
    [SerializeField] List<GameObject> roomPrefabs; // List of room prefabs
    [SerializeField] List<GameObject> exitPrefabs; // List of exit prefabs
    [SerializeField] List<GameObject> corridors; // List of corridor rooms 
    [SerializeField] Vector3 exitOffsets; // Offsets for exit positions
    [SerializeField] Transform roomParent; // Parent transform for rooms
    [SerializeField] Transform exitParent; // Parent transform for exits
    [SerializeField] int maxExitCount; // Maximum number of exits
    [SerializeField] float yPosition; // Y position for room placement
    [SerializeField] Transform playerPos; // Player's position transform

    List<int> exitNumber; // List of exit numbers
    Bounds roomBounds; // Bounds of the current room
    private int lastExitType; // Store the last exit type

    public Bounds RoomBounds { get => roomBounds; set => roomBounds = value; }

    private void Start()
    {
        GenerateRoom(true);
        StartCoroutine(CleanExitPrefabsList());
    }

    IEnumerator CleanExitPrefabsList()
    {
        while (true)
        {
            for (int index = exitPrefabs.Count - 1; index >= 0; index--)
            {
                if (exitPrefabs[index] == null)
                {
                    exitPrefabs.RemoveAt(index);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetLastExitType(int type)
    {
        lastExitType = type;
    }

    public void SetPlayerPosition(int type, Bounds bounds)
    {
        Vector3 position = Vector3.zero;

        // Adjust the player position based on the exit type and room bounds
        switch (type)
        {
            case 0: // Bottom center -> Top center
                position = new Vector3(bounds.center.x, yPosition + exitOffsets.y, bounds.max.z - exitOffsets.z);
                break;
            case 1: // Bottom left -> Top left
                position = new Vector3(bounds.min.x + exitOffsets.x, yPosition + exitOffsets.y, bounds.max.z - exitOffsets.z);
                break;
            case 2: // Bottom right -> Top right
                position = new Vector3(bounds.max.x - exitOffsets.x, yPosition + exitOffsets.y, bounds.max.z - exitOffsets.z);
                break;
            case 3: // Top center -> Bottom center
                position = new Vector3(bounds.center.x, yPosition + exitOffsets.y, bounds.min.z + exitOffsets.z);
                break;
            case 4: // Top left -> Bottom left
                position = new Vector3(bounds.min.x + exitOffsets.x, yPosition + exitOffsets.y, bounds.min.z + exitOffsets.z);
                break;
            case 5: // Top right -> Bottom right
                position = new Vector3(bounds.max.x - exitOffsets.x, yPosition + exitOffsets.y, bounds.min.z + exitOffsets.z);
                break;
            case 6: // Center left -> Center right
                position = new Vector3(bounds.max.x - exitOffsets.x, yPosition + exitOffsets.y, bounds.center.z);
                break;
            case 7: // Center right -> Center left
                position = new Vector3(bounds.min.x + exitOffsets.x, yPosition + exitOffsets.y, bounds.center.z);
                break;
            default:
                Debug.LogWarning("Invalid position type specified.");
                return;
        }

        // Ensure the player position is within the bounds
        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.z = Mathf.Clamp(position.z, bounds.min.z, bounds.max.z);

        playerPos.transform.position = position;
    }

    public void DelayedSetPlayerPosition()
    {
        SetPlayerPosition(lastExitType, roomBounds);
    }

    public void GenerateRoom(bool itsFirstRoom)
    {
        DeleteAllRooms();
        DeactivateAllCorridors();

        var roomInstance = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], roomParent);
        var roomCollider = roomInstance.GetComponent<Collider>();
        roomBounds = roomCollider.bounds;

        roomInstance.transform.position = new Vector3(0, yPosition, 0);

        if (!roomInstance.activeSelf)
        {
            roomInstance.SetActive(true);
        }

        if (itsFirstRoom)
        {
            SetPlayerPosition(0, roomBounds);
        }
        else
        {
            SetPlayerPosition(lastExitType, roomBounds);
        }

        GetExitPoints();
        GenerateExits();

        // Ensure at least one exit is active
        if (!CheckExits())
        {
            exitPrefabs[Random.Range(0, exitPrefabs.Count)].SetActive(true);
        }
    }

    void GenerateExits()
    {
        int numberOfExits = Random.Range(1, maxExitCount + 1);
        exitNumber = GenerateUniqueNumbers(numberOfExits, 1, exitPrefabs.Count - 1);
        ActivateExits();
    }

    List<int> GenerateUniqueNumbers(int count, int minValue, int maxValue)
    {
        HashSet<int> uniqueNumbers = new HashSet<int>();

        if (maxValue - minValue + 1 < count)
        {
            Debug.LogError("Range is too small to generate the requested number of unique numbers.");
            return new List<int>();
        }

        while (uniqueNumbers.Count < count)
        {
            int randomNumber = Random.Range(minValue, maxValue + 1);
            uniqueNumbers.Add(randomNumber);
        }

        return new List<int>(uniqueNumbers);
    }

    void GetExitPoints()
    {
        exitPrefabs.Clear();

        foreach (Transform child in roomParent)
        {
            if (child.gameObject.activeSelf)
            {
                foreach (Transform exit in child)
                {
                    exitPrefabs.Add(exit.gameObject);
                }
            }
        }
    }

    void ActivateExits()
    {
        foreach (int index in exitNumber)
        {
            if (index < exitPrefabs.Count)
            {
                exitPrefabs[index].SetActive(true);
            }
        }
    }



    bool CheckExits()
    {
        foreach (var exit in exitPrefabs)
        {
            if (exit.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    void DeleteAllRooms()
    {
        foreach (Transform room in roomParent)
        {
            if (room != roomParent)
            {
                Destroy(room.gameObject);
            }
        }
    }


    void DeactivateAllCorridors()
    {
        foreach (var item in corridors)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
        }
    }
}
