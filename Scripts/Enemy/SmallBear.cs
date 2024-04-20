using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBear : MonoBehaviour,IDamageable
{
    [SerializeField]Player _player;
    [SerializeField]float speed;
    [SerializeField] float _hp;
    GameManager gameManager;
    Animator animator;

    // Start is called before the first frame update
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move () 
    {

      transform.position =  Vector3.MoveTowards(transform.position, _player.transform.position,speed*Time.deltaTime);
    
    
    }

    public void GetDamage (int damage)
    {
        _hp -= damage;

        if (_hp < 0)
        {
            animator.SetTrigger("Death");
            gameManager.addDeathForEnemy();
            Destroy(gameObject);


        }
    }



    private void OnCollisionEnter (Collision collision)
    {





        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {

            player.TakeDamage(10);

            Destroy(gameObject);


        }
    }
    
}
