using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine;
using System.Collections;


public class PlayerAttackTriger : MonoBehaviour
{
    bool attack=false;
    Player player;


    public void Awake ()
    {
        player = GetComponentInParent<Player>();
    }

    public void Attack ()
    {
        print("Attack");
        attack =true;

    }
    public void Update ()
    {

    }
    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Wall")
        {
            player.RollIsPossible = false;
        }


        if (other.TryGetComponent(out IDamageable hit) && attack)
        {
            hit.GetDamage(1);
            attack = false;



       
        }
    }


    public void OnTriggerExit (Collider other)
    {
               
        
            player.RollIsPossible = true;

        
    }


    //public void OnTriggerStay (Collision collision)
    //{
    //    print("Asdasdasdsd");


    //}


}
