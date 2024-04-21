using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;


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
            
           if ( other.gameObject.TryGetComponent(out SpriteRenderer sprite)&& other.gameObject.name != "Bear Small") 
            {

                StartCoroutine("damageEffect",sprite);
               
                
            }
            hit.GetDamage(1);
            
            attack = false;



       
        }
    }

    IEnumerator damageEffect (SpriteRenderer sprite) 
    {
        

        sprite.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        try
        {
            sprite.color = Color.white;


        }catch (MissingReferenceException ex) { }



}
    public void OnTriggerExit (Collider other)
    {
               
        
            player.RollIsPossible = true;

        
    }


}
