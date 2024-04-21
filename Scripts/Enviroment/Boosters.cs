using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosters : MonoBehaviour
{
    [SerializeField]bool HpPoision;
    [SerializeField]bool StaminaPoision;
    [SerializeField]bool StaminaInfinityPoision;
    [SerializeField]bool speedPoision;

    float stamina;
    float movespeed;


    private void OnTriggerEnter (Collider other)
    {
        print("sadasd");
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null && HpPoision) 
        {
            if (player.PlayerHp+25 < player.MaxHp) 
            {

                player.PlayerHp += 25;
                Destroy(gameObject);

            }

        }
        if (player != null && StaminaPoision)
        {
            if (player.Stamina + 25 < player.MaxStamina)
            {
                player.Stamina += 25;
                Destroy(gameObject);
            }
        }
        if (player != null && speedPoision)
        {

            StartCoroutine("MoveSpeed",player);
            transform.position = new Vector3(transform.position.x,transform.position.y+100,transform.position.z) ;
        }
        if (player != null && StaminaInfinityPoision)
        {
            StartCoroutine(InfinityStamina(player));
            transform.position = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z);
        }

    }
    IEnumerator InfinityStamina (Player player) 
    {
        stamina = player.RollSpendStamina;
        player.Stamina = player.MaxStamina;
        player.RollSpendStamina = 0;

        yield return new WaitForSeconds(5);
        player.RollSpendStamina = stamina;
        Destroy(gameObject);
    }
    IEnumerator MoveSpeed (Player player)
    {
        movespeed = player.PlayerSpeed;
        player.PlayerSpeed = player.PlayerSpeed*1.5f;

        yield return new WaitForSeconds(5);
        player.PlayerSpeed = movespeed;
        Destroy(gameObject);
    }
}
