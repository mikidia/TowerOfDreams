using System.Collections;
using UnityEngine;

public class Flyingenemy : MonoBehaviour
{



    #region Declaration 
    [Header("Settings")]
    [SerializeField]int _hp;
    [SerializeField]float _enemySpeed;
    [Header("Attack ")]
    [SerializeField]float _distanceToPlayer;
    [SerializeField]float _attackSpeed;
    [SerializeField]bool _isAttackAvailable = true;



    #region Nonserialized
    Player _player;
    Rigidbody rb;
    #endregion


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
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

        }
        else if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToPlayer) 
        {
            Vector3 target =new Vector3( _player.transform.position.x - transform.position.x ,0,_player.transform.position.z - transform.position.z);
            transform.position -= target * (_enemySpeed*Time.deltaTime);


        }
    }
    IEnumerator AttackCd ()
    {

        _isAttackAvailable = false;
        
        yield return new WaitForSeconds(_attackSpeed);
        _isAttackAvailable = true;

    }

}
