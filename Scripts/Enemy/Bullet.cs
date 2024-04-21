using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	#region Declaration
	[SerializeField] float bulletSpeed;
	[SerializeField] int bulletDamage;
	Vector3 playerLastPos;
	Player playerscript;
    [SerializeField]Vector3 offset;


    Rigidbody rb;



	#endregion


	#region MonoBehaviour
	private void Awake ()
	{

    }
	private void Start ()
	{
        playerscript     = Player.instance;
        rb = GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
        playerLastPos = playerscript.transform.position;
        rb.AddForce((playerLastPos - transform.position) * bulletSpeed, ForceMode.Impulse);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position);
        StartCoroutine("destroyBullet");



    }

    #endregion

    private void OnTriggerEnter (Collider collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {

            player.TakeDamage(bulletDamage);

        }
        
    }
   IEnumerator destroyBullet () 
    {
    
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }



}
