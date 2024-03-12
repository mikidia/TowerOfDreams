using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region Declarations

    [Header("Player settings")]
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerDamage;
    public HealthBar healthBar;

    [SerializeField] int _maxHp;
    [SerializeField] int _playerHp;
    [SerializeField] bool _playerControlIsEnable;

    [Header("Player attack settings")]
    [SerializeField] float _attackDistance;
    [SerializeField] float _attackSpeed;
    [SerializeField] Vector3 attackOffset;



    [Header("Roll settings")]
    [SerializeField] float _rollCd;
    [SerializeField] float _rollDistance;
    [SerializeField] float _rollSpeed;
    [SerializeField] bool _rollIsPossible;
    [SerializeField]float _rollSpendStamina;

    [Header("Stamina settings")]
    [SerializeField] float _stamina=100;
    [SerializeField]float _maxStamina;
    [SerializeField]float _staminaRegeneration;

    #region Nonserialized

    [NonSerialized] Vector3 facingDirection;
    [NonSerialized] Vector3 offset;
    [NonSerialized] Vector3 input;
    [NonSerialized] bool _isAttackAvailable = true;
    [NonSerialized] bool _playerDamageable;
    [NonSerialized] Animator animator;
    [NonSerialized] Rigidbody rb;
    [SerializeField]PlayerAttackTriger attackTriger;
    [SerializeField]Vector3 attackDirection;
    [NonSerialized]UiManager uiManager;

    #endregion


    #region Input Buttons
    KeyCode interractButton = KeyCode.E;
    KeyCode attackButton = KeyCode.Mouse0;
    KeyCode rollButton = KeyCode.LeftShift;
    KeyCode inventoryButton = KeyCode.I;







    #endregion

    #region Getters
    public float Stamina { get => _stamina; }
    public float MaxStamina { get => _maxStamina; }
    #endregion



    #endregion


    #region MonoBehaviour

    private void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        _playerControlIsEnable = true;
        _rollIsPossible = true;
        attackTriger = GetComponentInChildren<PlayerAttackTriger>();
        animator = GetComponent<Animator>();
        //uiManager = UiManager.instance;


    }
    private void Start ()
    {
        _playerHp = _maxHp;
        healthBar.SetMaxHealth(_maxHp);
    }

    #endregion


    void Update ()
    {


        if (_playerControlIsEnable)
        {
            GetInputs();
            SetDirection();
            Movement();


        }

        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(20);

        UpdateStamina();
        Debugs();
    }


    void GetInputs ()
    {

        if (Input.GetKeyDown(rollButton) &&  && (math.abs(input.x) >= 0.1 || math.abs(input.z) >= 0.1))
        {
            StartCoroutine("Roll");
            //animator.SetTrigger("Roll");

        }
        if (Input.GetKeyDown(interractButton))
        {
            //Interract
            //animator.SetTrigger("Interract");


        }
        if (Input.GetKeyDown(attackButton) && _isAttackAvailable == true)
        {
            StartCoroutine("AttackCd");
            animator.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(inventoryButton) )
        {
            
        }
    }

    void TakeDamage (int damage)
    {
        _playerHp -= damage;
        healthBar.SetHealth(_playerHp);
        //animator.SetTrigger("GetDamage");

    }
    #region Movement,animations
    void Movement ()
    {


        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 newPos =rb.position+(input.normalized*_playerSpeed*Time.deltaTime);
        //rb.velocity = newPos;
        rb.MovePosition(newPos);



    }

    void SetDirection ()
    {

        if (math.abs(input.x) >= 0.3 || math.abs(input.z) >= 0.3)
        {
            facingDirection = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.z), 1);


        }

        attackDirection = new Vector3(facingDirection.x, transform.position.y, facingDirection.z);




        attackTriger.transform.position = new Vector3(transform.position.x + facingDirection.x, attackTriger.transform.position.y, transform.position.z + facingDirection.z) + attackOffset;
        if (attackDirection.x > 0) { animator.SetFloat("MoveX", 1); }

        if (attackDirection.x < 0) { animator.SetFloat("MoveX", -1); }

        if (attackDirection.x == 0) { animator.SetFloat("MoveX", 0); }

        if (attackDirection.z > 0) { animator.SetFloat("MoveY", 1); }

        if (attackDirection.z < 0) { animator.SetFloat("MoveY", -1); }

        if (attackDirection.z == 0) { animator.SetFloat("MoveY", 0); }
        if (attackDirection.x == 0&&attackDirection.z == 0) { animator.SetBool("IsStay", true); }

    }



    void Dead ()
    {
        //animator.SetTrigger("PlayerDead"); 
    }

    #endregion



#if UNITY_EDITOR
    void Debugs ()
    {

        Debug.DrawRay(transform.position, facingDirection * 2f, Color.green); //not correctly draw size of attack direction ray
    }


#endif
    void UpdateStamina ()
    {

        if (_stamina < _maxStamina)
        {
            _stamina += Time.deltaTime * _staminaRegeneration;
        }

    }


    #region Enumerators
    IEnumerator AttackCd ()
    {

        _isAttackAvailable = false;
        attackTriger.Attack();
        yield return new WaitForSeconds(_attackSpeed);
        _isAttackAvailable = true;

    }
    IEnumerator Roll ()
    {

        if (_stamina >= _rollSpendStamina)
        {
            _rollIsPossible = false;
            _playerSpeed += _rollSpeed;
            _stamina -= _rollSpendStamina;
            yield return new WaitForSeconds(_rollCd);
            _playerSpeed -= _rollSpeed;
            _rollIsPossible = true;
        }
    }



    #endregion


}
