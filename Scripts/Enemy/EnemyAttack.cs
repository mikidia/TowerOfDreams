using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
EnemyMain enemy ;
int damage ;
float attackCooldown ;

    private void Start() {
        enemy = GetComponentInParent<EnemyMain>();
        damage  = (int)enemy.Damage1 ;
        // attackCooldown = enemy.at
    }


    void OnTriggerEnter(Collider other)
    {

        var player = other.GetComponent<PlayerHealth>();
        if (player!=null)
        {
            player.TakeDamage(damage);
        }
    }


    IEnumerator AttackCd()
    {


        yield return new WaitForSeconds(0);
    }
}
