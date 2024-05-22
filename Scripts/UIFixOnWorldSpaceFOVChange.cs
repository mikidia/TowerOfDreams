using Cinemachine;
using UnityEngine;

public class UIFOVController : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float walkFOV = 55f;
    [SerializeField] private float runFOV = 65f;
    [SerializeField] private float idleFOV = 45f;
    [SerializeField] private float zoomSpeed = 1f;

    [Header("UI Elements")]
    [SerializeField] private RectTransform[] uiElements;

    private Vector3 _input;
    private float _initialFOV;
    private Vector2[] _initialAnchoredPositions;

    void Start()
    {
        _initialFOV = virtualCamera.m_Lens.FieldOfView;
        _initialAnchoredPositions = new Vector2[uiElements.Length];

        for (int i = 0; i < uiElements.Length; i++)
        {
            _initialAnchoredPositions[i] = uiElements[i].anchoredPosition;
        }
    }

    void Update()
    {
        UpdateCameraFOV();
        AdjustUIPositions();
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

    private void AdjustUIPositions()
    {
        float currentFOV = virtualCamera.m_Lens.FieldOfView;
        float fovRatio = currentFOV / _initialFOV;

        for (int i = 0; i < uiElements.Length; i++)
        {
            Vector2 initialPos = _initialAnchoredPositions[i];
            Vector2 adjustedPos = initialPos / fovRatio;
            uiElements[i].anchoredPosition = adjustedPos;
        }
    }
}
