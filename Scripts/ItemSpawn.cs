using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField]GameObject[] items;
    [SerializeField]Transform boosterParent;


    public void getRandomBuster ( Vector3 enemyPos) 
    {

        GameObject booster = Instantiate(items[Random.Range(0,items.Length)],boosterParent);
        booster.transform.position = enemyPos;
        booster.SetActive(true);
        Destroy(booster,7);

    
    }
}
