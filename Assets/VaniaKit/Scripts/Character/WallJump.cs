using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vaniakit.Player;

namespace Vaniakit.Items
{
    public class WallJump : MonoBehaviour
    {
        [SerializeField] private Transform rightSideOfPlayer;
        [SerializeField] private Transform leftSideOfPlayer;
        [SerializeField] private float wallDetectionRadius = 0.03f;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float slideDownSpeed;
        [SerializeField] private float pushBackForce; //amount back player goes pressing the jump button on a wall
        private bool slideDownSpeedSet = false;
        private bool jumpingFromLeftWall = false;
        private bool playerJumpedFromWall = false;
        private float normalGravityScale; //Current gravity Scale given when player is playing the game normally
        //Components in player
        private Transform playerParent;
        private PlayerController playerController;
        private PlayerJump playerJump;
        private InputAction m_jumpAction;
        
        public static bool PlayerHangingOnWall =  false;
        #region Events
        protected virtual void onPlayerOnRightWall()
        {
            Debug.Log("Player On Right Wall");
        }

        protected virtual void onPlayerOnLeftWall()
        {
            Debug.Log("Player On Left Wall");
        }

        protected virtual void onPlayerLeftRightWall()
        {
            Debug.Log("Player Left Right Wall");
        }

        protected virtual void onPlayerLeftLeftWall()
        {
            Debug.Log("Player Left Left Wall");
        }
        #endregion
        void Start()
        {
            if (playerParent == null)
                playerParent = transform.parent;
            playerController = playerParent.GetComponent<PlayerController>();
            playerJump = playerParent.GetComponent<PlayerJump>();
            m_jumpAction = InputSystem.actions.FindAction("Jump");
        }
        private void Update()
        {
            if (playerJump.isGrounded)
            {
                PlayerHangingOnWall = false;
                return; 
            }//Won't run if player is on ground
            //If player right side overlapping with ground layermask
            if (Physics2D.OverlapCircle(rightSideOfPlayer.position, wallDetectionRadius, groundLayerMask))
            {
                    jumpingFromLeftWall = false;
                    playerOnWall();
                    PlayerHangingOnWall = true;
                    onPlayerOnRightWall();
            }
            //If player left side overlapping with ground layermask
            else if (Physics2D.OverlapCircle(leftSideOfPlayer.position, wallDetectionRadius, groundLayerMask))
            {
                 jumpingFromLeftWall = true;
                 playerOnWall();
                 PlayerHangingOnWall = true;
                 onPlayerOnLeftWall();
            }

          
            if (m_jumpAction.WasReleasedThisFrame() && slideDownSpeedSet)
            {
                slideDownSpeedSet = false;
                playerController.getPlayerRigidbody().linearVelocity = new Vector2(playerController.getPlayerRigidbody().linearVelocity.x,0f);
            }

            if (playerJumpedFromWall) //If player has jumped from a wall
            {
                if (jumpingFromLeftWall && playerController.getPlayerRigidbody().linearVelocity.x <= 0f) //Going left now
                {
                    slideDownSpeedSet = false;
                    playerJumpedFromWall = false;
                    PlayerHangingOnWall = false;
                }
                else if (!jumpingFromLeftWall && playerController.getPlayerRigidbody().linearVelocity.x >= 0f) //Going back right now
                {
                    slideDownSpeedSet = false;
                    playerJumpedFromWall = false;
                    PlayerHangingOnWall = false;
                }
            }
           
        }

        private void playerOnWall()
        {
            if (playerJumpedFromWall)
                return;
            if (!slideDownSpeedSet)
            {
                playerController.getPlayerRigidbody().linearVelocity = new Vector2(playerController.getPlayerRigidbody().linearVelocity.x,0f); //Stops player going up
                normalGravityScale = playerController.getPlayerRigidbody().gravityScale; //Save current gravity scale when not jumping
                slideDownSpeedSet = true; 
                playerController.getPlayerRigidbody().gravityScale = slideDownSpeed; //Sets currents gravity to zero so player sticks onto wall
            }
            if (m_jumpAction.WasPressedThisFrame()) //When player exits wall
            {
                playerJumpedFromWall = true;
                playerController.getPlayerRigidbody().gravityScale = normalGravityScale; //Set gravity back to normal
                if (jumpingFromLeftWall)
                {
                    playerController.getPlayerRigidbody().linearVelocity =
                        new Vector2(-pushBackForce, playerJump.getMaxJumpHeightValue()); //Launch player
                }
                else
                {
                    playerController.getPlayerRigidbody().linearVelocity =
                        new Vector2(pushBackForce, playerJump.getMaxJumpHeightValue()); //Launch player
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (rightSideOfPlayer != null && leftSideOfPlayer != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(leftSideOfPlayer.position, wallDetectionRadius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(rightSideOfPlayer.position, wallDetectionRadius);
            }
          
        }
    }
}

