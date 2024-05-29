using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private string[] targetTags; // Массив тегов, которые должны иметь объекты для удержания в зоне
    private List<Collider> collidersInZone = new List<Collider>(); // Список коллайдеров в зоне
    private BoxCollider triggerZone; // Коллайдер зоны триггера
    private Bounds zoneBounds; // Границы зоны

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

        // Включаем все коллайдеры текущего объекта и его дочерних объектов
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider col in colliders)
        {
            zoneBounds.Encapsulate(col.bounds);
        }

        // Устанавливаем размеры и центр основного коллайдера
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
                // Возвращаем объект внутрь зоны, если он уходит за пределы
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

        // Вычисляем ближайшую допустимую позицию
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

        // Рисуем зону триггера для наглядности в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(triggerZone.bounds.center, triggerZone.bounds.size);

        // Рисуем границы зоны
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

        // Объединяем границы текущей зоны с границами другой зоны
        zoneBounds.Encapsulate(otherCollider.bounds);

        // Обновляем размеры и центр текущего коллайдера
        triggerZone.size = zoneBounds.size;
        triggerZone.center = zoneBounds.center - transform.position;

        // Удаляем другой триггер, так как он теперь объединен с текущим
        Destroy(otherZone.gameObject);
    }
}
