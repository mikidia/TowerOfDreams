using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable
{
    #region Declarations 
    [Header("Player settings")]
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerDamage;
    [SerializeField] int _playerHp;
    [SerializeField] bool _playerControlIsEnable;
    [Header("Player attack settings")]
    [SerializeField] float _attackDistance;
    [SerializeField] float _attackSpeed;
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
    [NonSerialized] Vector3 offset;
    [NonSerialized] Vector3 input;
    [NonSerialized] bool _isAttackAvailable = true;
    [NonSerialized] bool _playerDamageable;
    [NonSerialized] Vector3 facingDirection,attackDirection;
    [NonSerialized] Animator animator;
    [NonSerialized] Rigidbody rb;
    //[NonSerialized] KeyCode _inputBuffer; // dont forger add input buffer after roll (attack,another roll)
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

    }
    private void Start ()
    {

    }

    #endregion


    private void FixedUpdate ()
    {

    }

    void Update ()
    {


        if (_playerControlIsEnable)
        {
            GetInputs();
            SetDirection();
            Movement();


        }
        UpdateStamina();
        Debugs();
    }


    void GetInputs ()
    {

        if (Input.GetKeyDown(rollButton) && _rollIsPossible == true && (math.abs(input.x) >= 0.1 || math.abs(input.z) >= 0.1))
        {
            StartCoroutine("Roll");

        }
        if (Input.GetKeyDown(interractButton))
        {
            //Interract

        }
        if (Input.GetKeyDown(attackButton) && _isAttackAvailable == true)
        {
            StartCoroutine("Attack");
        }






    }

    #region Movement, attack and their animations
    void Movement ()
    {


        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 newPos =rb.position+(input.normalized*_playerSpeed*Time.deltaTime);
        //rb.velocity = newPos;
        rb.MovePosition(newPos);



    }

    void SetDirection ()
    {
        facingDirection = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.z), 1);
        if (math.abs(input.x) >= 0.1 || math.abs(input.z) >= 0.1)
        {
            attackDirection = new Vector2(input.x, input.z);
        }
    }


    public void GetDamage (int damage)
    {
        _playerHp -= damage;
        if (_playerHp <= 0)
        {
            Dead();
        }
    }



    void Dead ()
    {
        //animator.SetTrigger("playerDead"); 
    }

    #endregion


    void Debugs ()
    {
        //Debug.DrawRay(transform.position, facingDirection * 2, Color.red);// draw move direction ray
        //Debug.DrawRay(transform.position, attackDirection * 2f, Color.green); //not correctly draw size of attack direction ray
    }

    void UpdateStamina ()
    {

        if (_stamina < _maxStamina)
        {
            _stamina += Time.deltaTime * _staminaRegeneration;
        }

    }



    IEnumerator Attack ()
    {
        print("attack");
        //RaycastHit2D hit  =  Physics2D.Raycast(transform.position +offset,transform.forward*facingDirection,_attackDistance);
        _isAttackAvailable = false;
        _rollIsPossible = false;
        yield return new WaitForSeconds(_attackSpeed);
        _rollIsPossible = true;
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

    //IEnumerator Dash ()
    //{

    //if (_stamina >= _dashSpendStamina)
    //{


    //Vector3 rollPosition = transform.position +input.normalized* _rollDistance ;
    //transform.position = Vector3.MoveTowards(transform.position, rollPosition, _rollSpeed);

    //yield return new WaitForSeconds(_dashCd);
    //}
    //}



    
}
