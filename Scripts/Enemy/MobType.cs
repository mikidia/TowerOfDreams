using UnityEngine;

public class MobType : MonoBehaviour
{
    [SerializeField] private bool _isRegularMob;
    [SerializeField] private bool _isEliteMob;
    [SerializeField] private bool _isBossMob;


    public bool IsRegularMob => _isRegularMob;
    public bool IsEliteMob => _isEliteMob;
    public bool IsBossMob => _isBossMob;
    public void SelectType(int _chanseToSpawnRegular, int _chanseToSpawnElite, int _chanseToSpawnBoss)
    {

        var type = Random.Range(0, 1000);
        if (type <= _chanseToSpawnRegular)
        {
            _isRegularMob = true;



        }
        else if (type > _chanseToSpawnRegular && type <= _chanseToSpawnElite)
        {
            _isEliteMob = true;

        }
        else if (type <= _chanseToSpawnBoss && type > _chanseToSpawnElite)
        {

            _isBossMob = true;

        }



    }

}