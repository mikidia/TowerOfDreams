using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private string[] targetTags; // ������ �����, ������� ������ ����� ������� ��� ��������� � ����
    private List<Collider> collidersInZone = new List<Collider>(); // ������ ����������� � ����
    private BoxCollider triggerZone; // ��������� ���� ��������
    private Bounds zoneBounds; // ������� ����

    private void Awake()
    {
        triggerZone = GetComponent<BoxCollider>();
        if (!triggerZone.isTrigger)
        {
            triggerZone.isTrigger = true;
        }
        CalculateCombinedBounds();
    }

    private void CalculateCombinedBounds()
    {
        zoneBounds = new Bounds(transform.position, Vector3.zero);

        // �������� ��� ���������� �������� ������� � ��� �������� ��������
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider col in colliders)
        {
            zoneBounds.Encapsulate(col.bounds);
        }

        // ������������� ������� � ����� ��������� ����������
        triggerZone.size = zoneBounds.size;
        triggerZone.center = zoneBounds.center - transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasTargetTag(other))
        {
            if (!collidersInZone.Contains(other))
            {
                collidersInZone.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HasTargetTag(other))
        {
            if (collidersInZone.Contains(other))
            {
                collidersInZone.Remove(other);
                // ���������� ������ ������ ����, ���� �� ������ �� �������
                ReturnToZone(other);
            }
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
        Vector3 otherPosition = other.transform.position;

        // ��������� ��������� ���������� �������
        float clampedX = Mathf.Clamp(otherPosition.x, zoneBounds.min.x, zoneBounds.max.x);
        float clampedZ = Mathf.Clamp(otherPosition.z, zoneBounds.min.z, zoneBounds.max.z);

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

        // ������ ������� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(zoneBounds.center, zoneBounds.size);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerZone otherZone = other.GetComponent<TriggerZone>();
        if (otherZone != null)
        {
            CombineZones(otherZone);
        }
    }

    private void CombineZones(TriggerZone otherZone)
    {
        BoxCollider otherCollider = otherZone.GetComponent<BoxCollider>();

        // ���������� ������� ������� ���� � ��������� ������ ����
        zoneBounds.Encapsulate(otherCollider.bounds);

        // ��������� ������� � ����� �������� ����������
        triggerZone.size = zoneBounds.size;
        triggerZone.center = zoneBounds.center - transform.position;

        // ������� ������ �������, ��� ��� �� ������ ��������� � �������
        Destroy(otherZone.gameObject);
    }
}
