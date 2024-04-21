using System.Collections;
using UnityEngine;

public class Flyingenemy : MonoBehaviour,IDamageable
{



    #region Declaration 
    [Header("Settings")]
    [SerializeField]int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]bool isGoToPlayer = true;
    [SerializeField]GameObject BulletPrefab;
    bool isDeath = false;
    

    [Header("Attack ")]
    [SerializeField]float _distanceToPlayer;
    [SerializeField]float _attackSpeed;
    [SerializeField]bool _isAttackAvailable = true;
    [SerializeField]float bulletCd;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip deathSoundClip;



    #region Nonserialized
    Player _player;
    Rigidbody rb;
    Animator animator;
    private AudioSource audioSource;
    GameManager gameManager;
    SoundManager audio;
    #endregion


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GameObject.Find("Sound manager").GetComponent<SoundManager>();
    }
    private void Update ()
    {
        if ( !isDeath)
        {
            Move();
            Attack();  
        }

    }
    #endregion

    public void GetDamage (int damage)
    {
        if (!isDeath) 
        {
            _hp -= damage;
            if (_hp < 0)
            {
                isDeath = true;
                audio.BatDeathSound();
                gameManager.addDeathForEnemy();
                animator.SetTrigger("Death");
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


    void Move ()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToPlayer)
        {
            Vector3 target =new Vector3( _player.transform.position.x - transform.position.x ,0,_player.transform.position.z - transform.position.z);
            transform.position += target * (_enemySpeed * Time.deltaTime);
            isGoToPlayer = false;
           

        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToPlayer) 
        {
            Vector3 target =new Vector3( _player.transform.position.x - transform.position.x ,0,_player.transform.position.z - transform.position.z);
            transform.position -= target * (_enemySpeed*Time.deltaTime);
            isGoToPlayer = true;
            

        }
        if (Vector3.Distance(transform.position, _player.transform.position) == _distanceToPlayer) 
        {
            animator.SetTrigger("IsStaying");
        

        }
        animator.SetBool("IsGoToPlayer", isGoToPlayer); 
        

    }

    void Attack () 
    {
        if (_isAttackAvailable) 
        {
            GameObject bullet =  Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            audio.BatAttackSound();
            StartCoroutine("AttackCd");
        }
    
    
    }


    IEnumerator AttackCd ()
    {
        
        _isAttackAvailable = false;
        yield return new WaitForSeconds(bulletCd);
        _isAttackAvailable = true;

    }

}
