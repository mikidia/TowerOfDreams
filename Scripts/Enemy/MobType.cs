using UnityEngine;

public class MobType : MonoBehaviour
{
    [SerializeField] private bool _isRegularMob;
    [SerializeField] private bool _isEliteMob;
    [SerializeField] private bool _isBossMob;

    public bool IsRegularMob => _isRegularMob;
    public bool IsEliteMob => _isEliteMob;
    public bool IsBossMob => _isBossMob;

}