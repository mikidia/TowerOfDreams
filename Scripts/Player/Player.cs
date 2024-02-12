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
    [SerializeField] float _attackDistance;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _playerHp;


    #region Nonserialized
    [NonSerialized] Vector3 offset;
    [NonSerialized] Vector3 input;
    [NonSerialized] bool _isAttackAvailable = true;
    [NonSerialized] bool _playerDamageable;
    [NonSerialized] Vector2 facingDirection,attackDirection,inputVector;
    [NonSerialized] Animator animator;
    [NonSerialized] Rigidbody rb;
    #endregion


    #region Input Buttons
    KeyCode interractButton = KeyCode.E;
    KeyCode attackbutton = KeyCode.Mouse0;

    #endregion



    #endregion


    #region MonoBehaviour

    private void Awake ()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Start ()
    {

    }

    #endregion




    private void FixedUpdate ()
    {
        Movement();
        Debugs();
        GetInputs();
    }


    void GetInputs ()
    {
        if (Input.GetKeyDown(interractButton))
        {
            //Interract

        }
        if (Input.GetKeyDown(attackbutton) && _isAttackAvailable == true)
        {
            StartCoroutine("AttackCd");
        }
        


    }

    #region Movement, attack and their animations
    void Movement ()
    {


        input = new Vector3( Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")).normalized;
        
        
        Vector3 newPos =rb.position+input*_playerSpeed*Time.deltaTime;
        rb.MovePosition( newPos);

        SetDirection();

    }

    void SetDirection ()
    {
        facingDirection = Vector2.ClampMagnitude(new Vector2(input.x, input.y), 1);
        if (math.abs(input.x) >= 0.1 || math.abs(input.y) >= 0.1)
        {
            attackDirection = new Vector2(input.x, input.y);
        }
    }


    public void GetDamage (int damage)
    {
        _playerHp -= damage;
        if (_playerHp <= 0)
        {
            PlayerDead();
        }
    }



    void PlayerDead ()
    {
        //animator.SetTrigger("playerDead"); 
    }

    #endregion


    void Debugs ()
    {
        //Debug.DrawRay(transform.position, facingDirection * 2, Color.red);// draw move direction ray
        //Debug.DrawRay(transform.position, attackDirection * 2f, Color.green); //not correctly draw size of attack direction ray
    }


    IEnumerator AttackCd ()
    {
        print("attack");
        //RaycastHit2D hit  =  Physics2D.Raycast(transform.position +offset,transform.forward*facingDirection,_attackDistance);
        _isAttackAvailable = false;
        yield return new WaitForSeconds(_attackSpeed);
        _isAttackAvailable = true;

    }


}
