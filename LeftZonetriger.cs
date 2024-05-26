using UnityEngine;

public class LeftZonetriger : MonoBehaviour


{
    [SerializeField] private string[] targetTags; // ������ �����, ������� ������ ����� ������� ��� ��������� � ����
    private BoxCollider triggerZone; // ��������� ���� ��������

    private void Start()
    {
        triggerZone = GetComponent<BoxCollider>();
        if (!triggerZone.isTrigger)
        {
            triggerZone.isTrigger = true;
        }

        // ��������� �������� � �������� ����������

    }


    private void OnTriggerEnter(Collider other)
    {
        // ���������, ���� ������ ������ � �������, ����� �������� �������������� ������ �����
    }

    private void OnTriggerExit(Collider other)
    {
        // ���������, ���� ������ ����� ���� �� ������ �����
        if (HasTargetTag(other))
        {
            // ���������� ������ ������ ����
            ReturnToZone(other);
        }
    }

    private bool HasTargetTag(Collider other)
    {
        foreach (string tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    private void ReturnToZone(Collider other)
    {
        Bounds bounds = triggerZone.bounds;
        Vector3 otherPosition = other.transform.position;

        // ��������� ��������� ���������� ������� � ������ ��������
        float clampedX = Mathf.Clamp(otherPosition.x, bounds.min.x, bounds.max.x);
        float clampedZ = Mathf.Clamp(otherPosition.z, bounds.min.z, bounds.max.z);

        other.transform.position = new Vector3(clampedX, otherPosition.y, clampedZ);
    }



    private void OnDrawGizmosSelected()
    {
        if (triggerZone == null)
        {
            triggerZone = GetComponent<BoxCollider>();
        }

        // ������ ���� �������� ��� ����������� � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(triggerZone.bounds.center, triggerZone.bounds.size);




    }
}





