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
    public static Player instance;
    [Header("sound settings")]
    [SerializeField] GameObject stepsAudio;


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
    SpriteRenderer sprite;
    GameManager gameManager;
    SoundManager audio;
    bool paused;
    bool isDeath =false;

    #endregion


    #region Input Buttons
    KeyCode interractButton = KeyCode.E;
    KeyCode attackButton = KeyCode.Mouse0;
    KeyCode rollButton = KeyCode.LeftShift;
    KeyCode inventoryButton = KeyCode.I;
    KeyCode pause = KeyCode.Escape;








    #endregion

    #region Getters
    
    public float MaxStamina { get => _maxStamina; }
    public bool RollIsPossible { get => _rollIsPossible; set => _rollIsPossible = value; }
    public int PlayerHp { get => _playerHp; set => _playerHp = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float RollSpendStamina { get => _rollSpendStamina; set => _rollSpendStamina = value; }
    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }

    #endregion



    #endregion


    #region MonoBehaviour

    private void Awake ()
    {
        if (instance == null)
        {

            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody>();
        _playerControlIsEnable = true;
        _rollIsPossible = true;
        attackTriger = GetComponentInChildren<PlayerAttackTriger>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Hp", PlayerHp);
        sprite = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GameObject.Find("Sound manager").GetComponent<SoundManager>();




        //uiManager = UiManager.instance;


    }
    private void Start ()
    {
        PlayerHp = _maxHp;
        healthBar.SetMaxHealth(_maxHp);
        
    }

    #endregion


    void Update ()
    {


        if (_playerControlIsEnable&&!isDeath)
        {
            GetInputs();
            SetDirection();
            Movement();
            healthBar.SetHealth(PlayerHp);


        }
        UpdateStamina();
        
        
    }

    void GetInputs ()
    {

        if (Input.GetKeyDown(rollButton) &&  (math.abs(input.x) >= 0.1 || math.abs(input.z) >= 0.1))
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
            animator.SetTrigger("Attack");
            audio.PlayerAttackSound();

            StartCoroutine("AttackCd");
        }
        if (Input.GetKeyDown(inventoryButton) )
        {
            
        }
        if (Input.GetKeyDown(pause)&& !paused) 
        {
             paused = true;
             gameManager.Pause();
        }else if (Input.GetKeyDown(pause) && paused) 
        {
            gameManager.UpPause();
            paused = false;

        }
    }

     public void TakeDamage (int damage)
    {
        if (!isDeath) 
        {
            
            audio.PlayerGetHitSound();
            PlayerHp -= damage;
            animator.SetFloat("Hp", PlayerHp);
            healthBar.SetHealth(PlayerHp);
            animator.SetTrigger("GetDamage");
            StartCoroutine("damageEffect");
            if (PlayerHp < 0)
            {
                isDeath = true;
                audio.PlayerDeathSound();
                Dead();
                gameManager.loose();

            }


        }


    }
    IEnumerator damageEffect ()
    {
        
        
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;


    }
    //bool  ColliderCheck () 
    //{



    //}
    #region Movement,animations
    void Movement ()
    {


        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.x == 0 && input.z == 0) 
        { 
            animator.SetBool("IsStay", true); 
            rb.velocity = Vector3.zero;
              
            
                stepsAudio.SetActive(false);

        }
        else
        { 

            animator.SetBool("IsStay", false);


            stepsAudio.SetActive(true);

            Vector3 newPos =rb.position+(input.normalized*_playerSpeed*Time.deltaTime);
            
            rb.MovePosition(newPos);
        }
        

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

    }

    

    void Dead ()
    {
        animator.SetTrigger("PlayerDead");
        
    }

    #endregion




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

        if (_stamina >= _rollSpendStamina&& _rollIsPossible)
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
