using UnityEngine;
using System.Collections.Generic;
using Player;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour, IEquipable
{
    [SerializeField] private float dashDistance;
    private bool hasDashed = false;

    private Player.PlayerController _playerController;

    private InputAction DashAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        DashAction = InputSystem.actions.FindAction("Dash");
    }

    // Update is called once per frame https://www.youtube.com/watch?v=lckH-nJi2j8
    private void Update()
    {
        if (DashAction.WasPressedThisFrame())
        {
            Debug.Log("Dash");
            _playerController.getPlayerRigidbody().linearVelocity = Vector2.right * dashDistance;
        }
    }

    // private IEnumerator Dash()
    // {
    //
    //     yield return new WaitForSeconds(1f);
    // }
    public void Equip()
    {
        Instantiate(gameObject, transform.position, transform.rotation,GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void Unequip()
    {
        Destroy(gameObject);
    }
}
