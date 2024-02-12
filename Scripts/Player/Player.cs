using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;


public class Player : MonoBehaviour
{
    #region Declarations 
    [Header("Player settings")]
    [SerializeField] float playerSpeed,playerDamage,xInput,yInput;
    [SerializeField] int playerHp;
    [SerializeField] Vector3 offset;
    [SerializeField] float attackDistance;





    [NonSerialized] Vector2 inputVector;
    [NonSerialized] bool playerDamageable;
    [NonSerialized] Vector2 facingDirection;
    [NonSerialized] Vector2 attackDirection;

    [NonSerialized] Animator animator;



    Rigidbody rb;


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
        SetDirection();
        Attack();
    }




    #region Movement and animations
    void Movement () 
    {

        xInput = Input.GetAxis("Horizontal"); 
        yInput = Input.GetAxis("Vertical");
        Debug.Log(yInput);
        Vector3 newPos = new Vector3 (rb.position.x+xInput*playerSpeed*Time.deltaTime ,0,rb.position.z+yInput*playerSpeed*Time.deltaTime);  
        rb.MovePosition(newPos); 


    }

    void SetDirection ()
    {
        facingDirection = Vector2.ClampMagnitude(new Vector2(xInput, yInput), 1); 

    }

    #endregion


    void Attack () 
    {

        RaycastHit2D hit  =  Physics2D.Raycast(transform.position +offset,transform.forward*facingDirection,attackDistance);
        
    }
    public void GetDamage (int damage) 
    {
        playerHp-= damage;
        if (playerHp <= 0)
        {

            PlayerDead();
        
        
        }
    
    }

    void PlayerDead () 
    {

        
        //animator.SetTrigger("playerDead"); 
    
    }

    void Debugs () 
    {
        Debug.DrawRay(transform.position, facingDirection * 2, Color.red);


    }

}
