using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyMain enemy;
    int damage;
    float attackCooldown;
    bool canAttack = true;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyMain>();
        damage = (int)enemy.Damage1;
        attackCooldown = enemy.AttackCooldown; // Assuming there's an attackCooldown field in EnemyMain
    }

    private void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<PlayerHealth>();
        if (player != null && canAttack)
        {
            StartCoroutine(AttackCd());
            player.TakeDamage(damage);

        }
    }

    IEnumerator AttackCd()
    {
        if (enemy == null) { transform.GetComponentInParent<EnemyMain>().GetComponentInParent<EnemyMain>(); }
        var tempSpeed = enemy.MoveSpeed;
        enemy.MoveSpeed = 0;
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        enemy.MoveSpeed = tempSpeed;
        canAttack = true;
    }

}
