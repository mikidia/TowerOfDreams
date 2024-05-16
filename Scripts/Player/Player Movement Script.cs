using System.Collections;
using UnityEngine;


public class PlayerMovementScript : MonoBehaviour
{
    Rigidbody rb;

    [Header("Player Movement Settings")]
    [SerializeField] private float _playerMoveSpeed;
    [Header("Dash Settings")]
    [SerializeField] private bool _isDashAnvaliable;
    [SerializeField] float _dashCd;
    [SerializeField] float _returnBasicSpeed;
    [SerializeField] Player player;
    [SerializeField] float _dashSpeed;
    [SerializeField] float dashUseStamina;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

    }


    public void Movement(Vector3 input)
    {
        Vector3 newPos = (input.normalized * _playerMoveSpeed * Time.deltaTime);
        newPos = new Vector3(rb.position.x + newPos.x, rb.position.y, rb.position.z + newPos.z);
        //Debug.Log(newPos);
        rb.MovePosition(newPos);


    }
    public void Dash()
    {
        if (!_isDashAnvaliable && player.Stamina - dashUseStamina < 0) { return; }
        else
        {
            player.Stamina -= dashUseStamina;
            StartCoroutine("DashAddSpeed");
        }



    }
    IEnumerator DashAddSpeed()
    {

        _isDashAnvaliable = false;
        _playerMoveSpeed += _dashSpeed;
        yield return new WaitForSeconds(_returnBasicSpeed);
        _playerMoveSpeed -= _dashSpeed;
        StartCoroutine("DashCd");



    }

    IEnumerator DashCd()
    {
        yield return new WaitForSeconds(_dashCd);
        _isDashAnvaliable = true;

    }






}
