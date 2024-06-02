using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private int roomIndex;
    private LevelGeneratorManager levelGeneratorManager;

    public void Initialize(int index, LevelGeneratorManager generator)
    {
        roomIndex = index;
        levelGeneratorManager = generator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelGeneratorManager != null)
            {
                levelGeneratorManager.SwitchRoom(roomIndex);
            }
            else
            {
                Debug.LogError("LevelGeneratorManager �� ��������������� ��� ExitTrigger �� ������� " + gameObject.name);
            }
        }
    }
}
