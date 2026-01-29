using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vaniakit.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerJump : MonoBehaviour
    {
        private Rigidbody2D rb => _playerController.getPlayerRigidbody();
        [SerializeField]private float jumpSpeed;
        [Header("GroundCheck")]
        [SerializeField] private LayerMask groundLayers;
        public bool isGrounded {get; private set;}
        private bool isInAir = false;
        [SerializeField] private float groundCheckRadius;
        [Tooltip("What object will check below for ground")]
        [SerializeField] private Transform groundCheck;
        private InputAction m_jumpAction;
        
        [Header("Double Jump Values")]
        [SerializeField] private bool doubleJumpEnabled = false;
        [Range(0f, 2.99f)] [Tooltip("How much higher or lower you jump compared to normal")]
        [SerializeField] private float doubleJumpMultiplier = 1f;

        private bool playerHasJumped = false;
        private PlayerController _playerController;

        [HideInInspector]public bool isDashing;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        #region Events

        
        protected virtual void onPlayerJump()
        {
            Debug.Log("Played Jumped");
        }

        protected virtual void onPlayerDoubleJump()
        {
            Debug.Log("Player DoubleJump");
        }

        protected virtual void onPlayerLand()
        {
            Debug.Log("Player landed");
        }

        protected virtual void onPlayerReleaseJump()
        {
            Debug.Log("Player Released Jump");
        }
        #endregion
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            if (groundCheck == null)
            {
                groundCheck = gameObject.transform;
            }
        }

        void Start()
        {
            m_jumpAction = InputSystem.actions.FindAction("Jump");
            
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers); 
            
            if (isDashing) //don't run while dashing
                return;
            if (m_jumpAction.WasPressedThisFrame() && isGrounded) //When the player presses the jump button
            {
                jump();
                onPlayerJump();
            }

            if ( m_jumpAction.WasPressedThisFrame() && !isGrounded && !playerHasJumped && doubleJumpEnabled)//Let the player double jump
            {
                doubleJump();
                onPlayerDoubleJump();
            }
            if (m_jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0) //When the player releases the jump button
            {
                releaseJump();
                onPlayerReleaseJump();
            }

            if (isInAir && isGrounded && rb.linearVelocity.y <= 0) //When player lands
            {
                isInAir = false;
                onPlayerLand();
            }
        }
        
        /// <summary>
        /// Nakes Player Jump
        /// </summary>
        private void jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            playerHasJumped = false;
            isInAir = true;
        }
        
        /// <summary>
        /// Triggers the player's double jump
        /// </summary>
        private void doubleJump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpMultiplier * jumpSpeed);
            playerHasJumped = true;
        }
        
        /// <summary>
        /// Makes Player Fall
        /// </summary>
        private void releaseJump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        public void EnableDoubleJump(bool enable)
        {
            doubleJumpEnabled = enable;
        }
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null)
            {
                groundCheck = gameObject.transform;
            }

            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
    
}
