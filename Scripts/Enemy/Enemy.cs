using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{



    #region Declaration 
    [SerializeField] int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]Player _player;
    [SerializeField]Transform smallBearsParent;
    Rigidbody rb;
    [SerializeField]float maxTpRange;
    [SerializeField]float waitUntilTp;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip deathSoundClip;
    LevelGenerator levelGenerator;
    [SerializeField]GameObject BearPrefab;
    Vector3 _newPos;
    bool IsTeleporting;
    bool IsSpawning;

    [SerializeField]private AudioSource audioSource;

    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        levelGenerator = GameObject.Find("LevelGeneratorManager").GetComponent<LevelGenerator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    #endregion
    private void Start ()
    {

        spawnBear ();
        audioSource = GetComponent<AudioSource>();
        Player player = Player.instance;

    }
    private void Update ()
    {
        Move();
        

        
    }
    public void GetDamage (int damage)
    {
        _hp -= damage;
        if (_hp < 0)
        {
            audioSource.clip = deathSoundClip;
            audioSource.Play();
            Destroy(gameObject);
        }
    }


    void Move ()
    {

        if (Vector3.Distance(transform.position, _player.transform.position) < 2&& !IsTeleporting)
        {
            StartCoroutine("teleport");

        }

    }

    void GenerateNewPos ()
    {
        _newPos = new Vector3(_player.transform.position.x - Random.Range(2, maxTpRange), transform.position.y, _player.transform.position.z - Random.Range(2, maxTpRange));
        if (_newPos.x < levelGenerator.FloorSize[0].x && _newPos.x > levelGenerator.FloorSize[1].x || _newPos.z < levelGenerator.FloorSize[0].z&&_newPos.x > levelGenerator.FloorSize[1].z) 
        {
            GenerateNewPos();
        }
    }


    IEnumerator teleport()
    {
        


        IsTeleporting = true;
        yield return new WaitForSeconds(waitUntilTp);
        GenerateNewPos();
        transform.position = _newPos;
        IsTeleporting=false;
        if (BearPrefab.GetComponentsInChildren<SmallBear>().Length<1)
        {
            spawnBear();
        }



    }
    private void spawnBear () 
    {
        
        
        GameObject Bear =  Instantiate(BearPrefab,smallBearsParent);
        GenerateNewPos();
        Bear.transform.position =_newPos;
        Bear.SetActive(true);
        
    }
}

