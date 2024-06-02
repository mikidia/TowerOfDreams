using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask validLayers; // ����, ������� ��������� ����������
    public float rayDistance = 1.0f; // ��������� ���� ����
    public Vector3 rayOffset = Vector3.zero; // �������� ����

    private Vector3 lastValidPosition; // ��������� ���������� ������� ������

    void Start()
    {
        // ������������� ��������� ���������� �������
        lastValidPosition = transform.position;
    }
    private void FixedUpdate()
    {
        // ���������� ��������� ������� ���� � ������ ��������
        Vector3 rayOrigin = transform.position + rayOffset;

        // �������� ����, ������������� ���� �� ��������� �������
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        // ���������� Raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // ��������, ����������� �� ������ ��� ������� � ����������� ����
            if (((1 << hit.collider.gameObject.layer) & validLayers) != 0)
            {
                // ���� ���� ����������, ��������� ��������� ���������� �������
                lastValidPosition = transform.position;
            }
            else
            {
                // ���� ���� �� ����������, ���������� ������ �� ��������� ���������� �������
                transform.position = lastValidPosition;
            }
        }
        else
        {
            // ���� ��� �� ������� ������� ��� �������, ���������� ������ �� ��������� ���������� �������
            transform.position = lastValidPosition;
        }
    }

    // ����� ��� ��������� ���� � ������ ��������������, ����� ������ ��� � ��������� Unity
    void OnDrawGizmos()
    {
        // ���������� ��������� ������� ���� � ������ ��������
        Vector3 rayOrigin = transform.position + rayOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
    }
}
