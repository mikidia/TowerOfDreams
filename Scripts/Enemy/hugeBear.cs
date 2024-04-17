using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hugeBear : MonoBehaviour, IDamageable
{
    [SerializeField]Player _player;
    [SerializeField]float speed;
    [SerializeField] float _hp;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip deathSoundClip;

    private AudioSource audioSource;

    Animator animator;
    bool isAttacking = false;
    bool isDeath = false;

    // Start is called before the first frame update
    void Start ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (!isDeath) 
        {
            if (!isAttacking)
            {
                Move();
            }
            else
            {
                animator.SetBool("IsMoving", false);


            }
        }

    }
    void Move ()
    {

        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
        animator.SetBool("IsMoving", true);

    }

    public void GetDamage (int damage)
    {
        _hp -= damage;

        if (_hp < 0)
        {
            audioSource.clip = deathSoundClip;
            audioSource.Play();
            animator.SetTrigger("Death");
            isDeath = true;
            StartCoroutine("Death");
        }
    }

    IEnumerator Death () 
    {
    
    yield return new WaitForSeconds(1);
        Destroy(gameObject);
    
    }





}
