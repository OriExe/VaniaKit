using UnityEngine;
using System.Collections.Generic;
using Vaniakit.Player;
using UnityEngine.InputSystem;

namespace Vaniakit.Player
{
    public class PlayerDash : MonoBehaviour, IEquipable
    {
        #region Events

        protected virtual void onPlayerDash()
        {
            
        }

        protected virtual void onPlayerDashFinish()
        {
            
        }
        #endregion
        [SerializeField] private float dashDistance;
        [SerializeField] private float dashingTime;
        private float dashTimeLeft;
        private bool hasDashed = false;

        private Player.PlayerController _playerController;

        private InputAction DashAction;

        private float currentGravityScale;
        
        private PlayerJump playerJump;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            playerJump = GetComponent<PlayerJump>();
            _playerController = GetComponent<PlayerController>();
            currentGravityScale = _playerController.getPlayerRigidbody().gravityScale;
            DashAction = InputSystem.actions.FindAction("Dash");
        }

        // Update is called once per frame https://www.youtube.com/watch?v=lckH-nJi2j8
        private void Update()
        {
            if (DashAction.WasPressedThisFrame() && !hasDashed) //Triggers the dash
            {
                Debug.Log("Dash");
                hasDashed = true;
                playerJump.isDashing = true;
                _playerController.getPlayerRigidbody().gravityScale = 0;
                dashTimeLeft = dashingTime;
                onPlayerDash();
            }

            if (hasDashed) //When dashing
            {
                dashTimeLeft-= Time.deltaTime;
                int playerDirection;
                if (PlayerMovement.returnHorizontalLookState() == PlayerMovement.lookStatesHorizontal.left)
                {
                    playerDirection = -1;
                }
                else
                {
                    playerDirection = 1;
                }
                transform.position = Vector2.Lerp(transform.position,transform.position + Vector3.right * (playerDirection * dashDistance),dashingTime);
                
            }

            if (dashTimeLeft <= 0 && hasDashed) //End dash
            {
                playerJump.isDashing = false;
                hasDashed = false;
                _playerController.getPlayerRigidbody().gravityScale = currentGravityScale;
                onPlayerDashFinish();
            }
            
        }
        public void Equip()
        {
            Instantiate(gameObject, transform.position, transform.rotation,GameObject.FindGameObjectWithTag("Player").transform);
        }

        public void Unequip()
        {
            Destroy(gameObject);
        }
        
    }
}

