using System.Collections;
using UnityEngine;

public class Flyingenemy : MonoBehaviour
{



    #region Declaration 
    [Header("Settings")]
    [SerializeField]int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]bool isGoToPlayer = true;
    [SerializeField]GameObject BulletPrefab;
    

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
    #endregion


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
        audioSource = GetComponent<AudioSource>();
    }
    private void Update ()
    {
        Move();
        Attack();
    }
    #endregion

    public void GetDamage (int damage)
    {
        _hp -= damage;
        if (_hp < 0)
        {
            audioSource.clip = deathSoundClip;
            audioSource.Play();
            Destroy(gameObject);
            animator.SetTrigger("Death");
        }
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
            audioSource.clip = damageSoundClip;
            audioSource.Play();
            GameObject bullet =  Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
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
