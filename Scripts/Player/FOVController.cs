using Cinemachine;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float runFOV = 80f;
    [SerializeField] private float idleFOV = 70f;
    [SerializeField] private float zoomSpeed = 1f;

    private Vector3 _input;

    void Update()
    {
        UpdateCameraFOV();
    }

    public void SetInput(Vector3 input)
    {
        _input = input;
    }

    private void UpdateCameraFOV()
    {
        float targetFOV;

        if (_input.magnitude > 0.1f)
        {
            targetFOV = _input.magnitude > 1f ? runFOV : walkFOV; // Consider running if magnitude > 1
        }
        else
        {
            targetFOV = idleFOV;
        }

        float currentFOV = virtualCamera.m_Lens.FieldOfView;
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
