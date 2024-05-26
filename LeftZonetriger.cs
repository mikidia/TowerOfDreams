using UnityEngine;

public class LeftZonetriger : MonoBehaviour


{
    [SerializeField] private string[] targetTags; // Массив тегов, которые должны иметь объекты для удержания в зоне
    private BoxCollider triggerZone; // Коллайдер зоны триггера

    private void Start()
    {
        triggerZone = GetComponent<BoxCollider>();
        if (!triggerZone.isTrigger)
        {
            triggerZone.isTrigger = true;
        }

        // Применяем смещения к размерам коллайдера

    }


    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, если объект входит в триггер, можно добавить дополнительную логику здесь
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, если объект имеет один из нужных тегов
        if (HasTargetTag(other))
        {
            // Возвращаем объект внутрь зоны
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

        // Вычисляем ближайшую допустимую позицию с учетом смещений
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

        // Рисуем зону триггера для наглядности в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(triggerZone.bounds.center, triggerZone.bounds.size);




    }
}





