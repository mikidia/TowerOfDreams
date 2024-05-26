using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    Rigidbody rb;

    [Header("Player Movement Settings")]
    [SerializeField] private float _playerMoveSpeed;
    [Header("Dash Settings")]
    [SerializeField] Image dashCooldownImage;
    [SerializeField] private bool _isDashAnvaliable;
    [SerializeField] float _returnBasicSpeed;
    [SerializeField] Player player;

    [SerializeField] float _energy;
    [SerializeField] float _maxEnergy;
    [SerializeField] float staminaRegeneration;
    public static PlayerMovementScript instance;

    [SerializeField] float _dashSpeed;
    [SerializeField] float dashUseStamina;
    [SerializeField] float _dashCd;

    private Vector3 dashDirection;

    public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
    public bool IsDashAnvaliable { get => _isDashAnvaliable; set => _isDashAnvaliable = value; }
    public float ReturnBasicSpeed { get => _returnBasicSpeed; set => _returnBasicSpeed = value; }
    public Player Player { get => player; set => player = value; }
    public float Energy { get => _energy; set => _energy = value; }
    public float MaxEnergy { get => _maxEnergy; set => _maxEnergy = value; }
    public float StaminaRegeneration { get => staminaRegeneration; set => staminaRegeneration = value; }

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        AddStamina();
    }

    public void Movement(Vector3 input)
    {
        Vector3 newPos = (input.normalized * _playerMoveSpeed * Time.deltaTime);
        newPos = new Vector3(rb.position.x + newPos.x, rb.position.y, rb.position.z + newPos.z);
        rb.MovePosition(newPos);
    }

    void AddStamina()
    {
        if (_energy < _maxEnergy)
        {
            _energy += staminaRegeneration * Time.deltaTime;
        }
    }

    private void DetermineDashDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            dashDirection = (targetPosition - transform.position).normalized;
            dashDirection.y = 0;
        }
    }

    public void UseDash()
    {
        if (_isDashAnvaliable && _energy - dashUseStamina >= 0)
        {
            DetermineDashDirection();
            _energy -= dashUseStamina;
            StartCoroutine(DashAddSpeed());
        }
    }

    IEnumerator DashAddSpeed()
    {
        _isDashAnvaliable = false;
        _playerMoveSpeed += _dashSpeed;
        rb.AddForce(dashDirection * _dashSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(_returnBasicSpeed);
        _playerMoveSpeed -= _dashSpeed;
        rb.velocity = Vector3.zero;
        StartCoroutine(DashCd());
    }

    IEnumerator DashCd()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _dashCd)
        {
            elapsedTime += Time.deltaTime;
            if (dashCooldownImage != null)
            {
                dashCooldownImage.fillAmount = elapsedTime / _dashCd;
            }
            yield return null;
        }


        _isDashAnvaliable = true;
    }
}

