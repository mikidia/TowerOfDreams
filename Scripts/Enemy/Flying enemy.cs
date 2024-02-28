using UnityEngine;

public class Flyingenemy : MonoBehaviour
{



    #region Declaration 
    [SerializeField] int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField] float _distanceToPlayer;
    Player _player;
    [SerializeField]Rigidbody rb;


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    #endregion
    private void Update ()
    {
        Move();
    }
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
            transform.position += target * _enemySpeed;

        }
    }

}
