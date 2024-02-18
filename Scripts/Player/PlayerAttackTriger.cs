using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine;
using System.Collections;


public class PlayerAttackTriger : MonoBehaviour
{
    bool attack=false;

    public void Attack ()
    {
        print("Attack");
        attack =true;

    }
    void OnTriggerStay(Collider other)
    {
        

        if (other.TryGetComponent(out IDamageable hit) && attack)
        {
            hit.GetDamage(1);
            attack = false;



        }
    }

}
