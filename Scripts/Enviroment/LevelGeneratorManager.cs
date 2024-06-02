using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorManager : MonoBehaviour
{
    public List<GameObject> roomPrefabs; // ������ �������� ������
    public GameObject exitPrefab; // ������ ������
    public int numberOfRooms = 10; // ���������� ������ ��� ���������
    public float roomSpacing = 10f; // ���������� ����� ���������
    public Transform player; // ������ �� ������

    public Vector3 roomOffset = Vector3.zero; // ������ ��� ������� ������
    public Vector3 exitOffset = Vector3.zero; // ������ ��� ������� �������

    private Vector3 nextRoomPosition; // ������� ��� ��������� �������
    private Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
    private List<GameObject> generatedRooms = new List<GameObject>();
    private Dictionary<Vector3, GameObject> roomPositions = new Dictionary<Vector3, GameObject>();
    private Dictionary<GameObject, Vector3> roomOriginalPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        GenerateLevel();
    }

    void Update()
    {
        UpdateRoomPositions();
    }

    void GenerateLevel()
    {
        Vector3 startPosition = player.position + roomOffset; // ��������� ������� � ������ �������
        nextRoomPosition = startPosition;

        for (int i = 0; i < numberOfRooms; i++)
        {
            // �������� ��������� ������ �������
            GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];

            // ������� ��������� ������� � �������� ������� � ������ �������
            GameObject roomInstance = Instantiate(roomPrefab, nextRoomPosition, Quaternion.identity);
            roomInstance.SetActive(i == 0); // �������� ������ ������ �������

            // ���������� ������������ ������� �������
            roomOriginalPositions[roomInstance] = nextRoomPosition;

            // ������� �������� ������
            CreateExits(roomInstance, i);

            // ��������� ������� � ������ � �������
            generatedRooms.Add(roomInstance);
            roomPositions[nextRoomPosition] = roomInstance;

            // ��������� ������� ��� ��������� �������
            nextRoomPosition = GetNextRoomPosition();
        }
    }

    void CreateExits(GameObject room, int roomIndex)
    {
        List<Transform> exitPoints = new List<Transform>();
        foreach (Transform child in room.transform)
        {
            if (child.CompareTag("ExitPoint"))
            {
                exitPoints.Add(child);
            }
        }

        if (exitPoints.Count > 0)
        {
            foreach (var exitPoint in exitPoints)
            {
                Vector3 exitPosition = exitPoint.position + exitOffset;
                GameObject exitInstance = Instantiate(exitPrefab, exitPosition, exitPoint.rotation);
                exitInstance.SetActive(roomIndex == 0); // ������ �������� ������ ����� ��� ������ �������

                ExitTrigger exitTrigger = exitInstance.AddComponent<ExitTrigger>();
                exitTrigger.Initialize(roomIndex, this);

                exitInstance.transform.parent = room.transform;
            }
        }
        else
        {
            Debug.LogWarning("ExitPoint �� ������ � ������� " + room.name);
        }
    }

    Vector3 GetNextRoomPosition()
    {
        Vector3 candidatePosition;
        do
        {
            Vector3 direction = directions[Random.Range(0, directions.Length)];
            candidatePosition = nextRoomPosition + direction * roomSpacing + roomOffset;
        } while (roomPositions.ContainsKey(candidatePosition));

        return candidatePosition;
    }

    void UpdateRoomPositions()
    {
        foreach (var kvp in roomOriginalPositions)
        {
            GameObject room = kvp.Key;
            Vector3 originalPosition = kvp.Value;
            room.transform.position = originalPosition + roomOffset;
        }
    }

    public void SwitchRoom(int currentRoomIndex)
    {
        if (currentRoomIndex >= 0 && currentRoomIndex < generatedRooms.Count)
        {
            generatedRooms[currentRoomIndex].SetActive(false);

            int nextRoomIndex = currentRoomIndex + 1;
            if (nextRoomIndex < generatedRooms.Count)
            {
                generatedRooms[nextRoomIndex].SetActive(true);

                foreach (Transform child in generatedRooms[nextRoomIndex].transform)
                {
                    if (child.CompareTag("ExitPoint"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
