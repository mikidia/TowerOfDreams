using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;


    void Start()
    {

    }

    public void TakeDamage(int amount)
    {
        Player._instance.Hp -= amount;
        StartCoroutine(DamageEffect());

        if (Player._instance.Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
    IEnumerator DamageEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color = Color.white;



    }

}
