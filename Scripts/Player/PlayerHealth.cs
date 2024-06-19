using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        StartCoroutine(DamageEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
    }
    IEnumerator DamageEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color = Color.black;



    }

}
