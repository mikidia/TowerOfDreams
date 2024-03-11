using System.Collections;
using UnityEngine;

public class Flyingenemy : MonoBehaviour
{



    #region Declaration 
    [Header("Settings")]
    [SerializeField]int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]bool isGoToPlayer = true;
    

    [Header("Attack ")]
    [SerializeField]float _distanceToPlayer;
    [SerializeField]float _attackSpeed;
    [SerializeField]bool _isAttackAvailable = true;



    #region Nonserialized
    Player _player;
    Rigidbody rb;
    Animator animator;
    #endregion


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
    }
    private void Update ()
    {
        Move();
    }
    #endregion

    public void GetDamage (int damage)
    {
        _hp -= damage;
        if (_hp < 0)
        {
            Destroy(gameObject);
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
    IEnumerator AttackCd ()
    {

        _isAttackAvailable = false;
        
        yield return new WaitForSeconds(_attackSpeed);
        _isAttackAvailable = true;

    }

}
