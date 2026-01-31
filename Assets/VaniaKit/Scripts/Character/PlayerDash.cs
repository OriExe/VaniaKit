using UnityEngine;
using System.Collections.Generic;
using Vaniakit.Player;
using UnityEngine.InputSystem;

namespace Vaniakit.Player
{
    /// <summary>
    /// A dashing script for the player 
    /// </summary>
    public class PlayerDash : MonoBehaviour, IEquipable
    {
        #region Events

        protected virtual void onPlayerDash()
        {
            Debug.Log("Dash");
        }

        protected virtual void onPlayerDashFinish()
        {
            Debug.Log("Dash Finished");
        }

        protected virtual void onPlayerDashReady()
        {
            Debug.Log("Dash Ready");
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
            
            playerJump = GetComponentInParent<PlayerJump>();
            _playerController = GetComponentInParent<PlayerController>();
            currentGravityScale = _playerController.getPlayerRigidbody().gravityScale; //Stores the current gravity scale when the game starts NOTE this will mean any gravity scale changes made won't be saved and this script will revert back to the old instance when dashed is used.
            DashAction = InputSystem.actions.FindAction("Dash");
        }

        // Update is called once per frame https://www.youtube.com/watch?v=lckH-nJi2j8
        private void Update()
        {
            if (DashAction.WasPressedThisFrame() && !hasDashed && dashTimeLeft == dashingTime) //Triggers the dash
            {
                hasDashed = true;
                playerJump.isDashing = true;
                _playerController.getPlayerRigidbody().gravityScale = 0;
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
                transform.parent.position = Vector2.Lerp(transform.parent.position,transform.parent.position + Vector3.right * (playerDirection * dashDistance),dashingTime);
                
            }

            if (dashTimeLeft <= 0 && hasDashed) //End dash
            {
                playerJump.isDashing = false;
                hasDashed = false;
                _playerController.getPlayerRigidbody().gravityScale = currentGravityScale;
                onPlayerDashFinish();
            }

            if (dashTimeLeft <= 0 && playerJump.isGrounded)
            {
                hasDashed = false;
                dashTimeLeft = dashingTime;
                onPlayerDashReady();
            }
            
        }
        public void Equip()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Instantiate(gameObject, player.transform.position, transform.rotation,player.transform);
        }

        public void Unequip()
        {
            Destroy(gameObject);
        }
        
    }
}

