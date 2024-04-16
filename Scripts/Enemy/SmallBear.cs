using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBear : MonoBehaviour,IDamageable
{
    [SerializeField]Player _player;
    [SerializeField]float speed;
    [SerializeField] float _hp;
    Animator animator;

    // Start is called before the first frame update
    void Start ()
    {
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
            Destroy(gameObject);
        }

    }


    private void OnCollisionEnter (Collider collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null) 
        {

            player.TakeDamage(10);
            Destroy(gameObject);


        }
    }
}
