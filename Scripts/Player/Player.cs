using System;
using UnityEngine;
using UnityEngine.XR;


public class Player : MonoBehaviour
{
    #region Declarations 
    [Header("Player movement settings")]
    [SerializeField] float playerSpeed,playerDamage,xInput,yInput;
    [NonSerialized] Vector2 inputVector;
    [NonSerialized] bool playerDamageable;
    [NonSerialized] Vector2 facingDirection;
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
    }
    #endregion
}
