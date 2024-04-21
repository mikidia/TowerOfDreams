using System.Collections;
using UnityEngine;

public class hugeBear : MonoBehaviour, IDamageable
{
    [SerializeField]Player _player;
    [SerializeField]float speed;
    [SerializeField] float _hp;

    [SerializeField] float attackCdTime;
    [SerializeField]bool _isAttackAvailable;

    [SerializeField] GameObject attackPrefab;

    SoundManager audio;

    Animator animator;
    GameManager gameManager;
    bool isAttacking = false;
    bool isDeath = false;

    // Start is called before the first frame update
    void Start ()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        audio = GameObject.Find("Sound manager").GetComponent<SoundManager>();
        audio.HugeBearReliseSound();
        audio.StartBossFight();
    }

    void Update ()
    {
        if (!isDeath)
        {
            if (!isAttacking)
            {
                Move();
                Attack();
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
        if (!isDeath)
        {


            _hp -= damage;

            if (_hp < 0)
            {

                animator.SetTrigger("Death");
                audio.HugeBearDeathSound();
                isDeath = true;
                StartCoroutine("Death");
            }
        }
    }

    IEnumerator Death ()
    {

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        gameManager.addDeathForEnemy();




    }

    void Attack ()
    {
        if (_isAttackAvailable)
        {
            //audioSource.Play();
            GameObject bullet =  Instantiate(attackPrefab);
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            StartCoroutine("AttackCd");
        }


    }
    IEnumerator AttackCd ()
    {

        _isAttackAvailable = false;
        yield return new WaitForSeconds(attackCdTime);
        _isAttackAvailable = true;

    }





}
