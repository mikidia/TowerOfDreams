using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampirism : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.VampirismIsActive = true;
    }
    private void OnDisable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.VampirismIsActive = false;
    }

}

