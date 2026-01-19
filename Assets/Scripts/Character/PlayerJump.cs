using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerJump : MonoBehaviour
    {
        private Rigidbody2D rb => _playerController.getPlayerRigidbody();
        [SerializeField]private float jumpSpeed;
        [Header("GroundCheck")]
        [SerializeField] private LayerMask groundLayers;
        public bool isGrounded {get; private set;}
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
            
        }

        protected virtual void onPlayerDoubleJump()
        {
            
        }

        protected virtual void onPlayerLand()
        {
            
        }

        protected virtual void onPlayerReleaseJump()
        {
            
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
            
            if (isDashing)
                return;
            if (m_jumpAction.WasPressedThisFrame() && isGrounded)
            {
                jump();
                onPlayerJump();
            }

            if ( m_jumpAction.WasPressedThisFrame() && !isGrounded && !playerHasJumped && doubleJumpEnabled)//Let the player double jump
            {
                doubleJump();
                onPlayerDoubleJump();
            }
            if (m_jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0)
            {
                releaseJump();
                onPlayerReleaseJump();
            }
        }
        
        /// <summary>
        /// Nakes Player Jump
        /// </summary>
        private void jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            playerHasJumped = false;
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
