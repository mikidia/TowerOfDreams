using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask validLayers; // Слой, который считается допустимым
    public float rayDistance = 1.0f; // Дистанция луча вниз
    public Vector3 rayOffset = Vector3.zero; // Смещение луча

    private Vector3 lastValidPosition; // Последняя допустимая позиция игрока

    void Start()
    {
        // Инициализация последней допустимой позиции
        lastValidPosition = transform.position;
    }
    private void FixedUpdate()
    {
        // Определяем начальную позицию луча с учетом смещения
        Vector3 rayOrigin = transform.position + rayOffset;

        // Создание луча, направленного вниз от смещенной позиции
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        // Выполнение Raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Проверка, принадлежит ли объект под игроком к допустимому слою
            if (((1 << hit.collider.gameObject.layer) & validLayers) != 0)
            {
                // Если слой допустимый, обновляем последнюю допустимую позицию
                lastValidPosition = transform.position;
            }
            else
            {
                // Если слой не допустимый, возвращаем игрока на последнюю допустимую позицию
                transform.position = lastValidPosition;
            }
        }
        else
        {
            // Если луч не находит объекта под игроком, возвращаем игрока на последнюю допустимую позицию
            transform.position = lastValidPosition;
        }
    }

    // Метод для рисования луча в режиме редактирования, чтобы видеть его в редакторе Unity
    void OnDrawGizmos()
    {
        // Определяем начальную позицию луча с учетом смещения
        Vector3 rayOrigin = transform.position + rayOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
    }
}
