using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        List<EnemyMain> enemy = new List<EnemyMain>();
        foreach (var i in other.GetComponents<EnemyMain>())
        {
            enemy.Add(i);

        }


        if (enemy.Count > 0)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                enemy[i].GetDamage(Player._instance.PlayerDamage);

            }

        }
    }
}


