using System;
using UnityEngine;
using UnityEngine.InputSystem;



namespace Vaniakit.Player
{
    /// <summary>
    /// Player character controller
    /// Things to include
    /// Player Movement
    /// Variable Jump Height
    /// Apex Modifiers
    /// Jump buffering
    /// Coyote Time (Leaving the platform)
    /// Clamped fall speed (Make fallign fun)
    /// Edge detection 
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerController _playerController;
        private static PlayerMovement _instance; 
        private lookStatesHorizontal playerHorizontalLookState; //Where is the player currently looking
        private lookStatesVertical playerVerticalLookState = lookStatesVertical.none;
        private Rigidbody2D rb => _playerController.getPlayerRigidbody();
        private InputAction m_moveAction;
        [SerializeField]private float movementSpeed;
        private bool playerNotMoving;//Static Instance of player controller
        #region Events
        protected virtual void onPlayerMove(lookStatesHorizontal direction)
        {
            
        }
        protected virtual void onPlayerIdle()
        {
            Debug.Log("Player Idle");
        }
        
        #endregion
       
        public enum lookStatesHorizontal
        {
            left,
            right,
        }

        public enum lookStatesVertical
        {
            up,
            down,
            none,
        }
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
           _playerController = GetComponent<PlayerController>();
            m_moveAction = InputSystem.actions.FindAction("Move");
            
            if (_instance == null)
            {
                _instance = this;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            move();
        }

        void Update()
        {
            Vector2 moveValue = m_moveAction.ReadValue<Vector2>();

           
            #region MoveStates
            if (moveValue.y > 0.4f)
            {
                playerVerticalLookState = lookStatesVertical.up;
            }
            else if (moveValue.y < -0.4f)
            {
                playerVerticalLookState = lookStatesVertical.down;
            }
            else
            {
                playerVerticalLookState = lookStatesVertical.none;
            }
            if (moveValue.x > 0)
            {
                playerHorizontalLookState = lookStatesHorizontal.right;
                onPlayerMove(lookStatesHorizontal.right);
                playerNotMoving = false;
                
            }
            else if (moveValue.x < 0)
            {
                playerHorizontalLookState = lookStatesHorizontal.left;
                onPlayerMove(lookStatesHorizontal.left);
                playerNotMoving = false;
            }
            else
            {
                if (!playerNotMoving)
                {
                    onPlayerIdle();
                    playerNotMoving = true;
                }
                
            }
           
            #endregion
        }

        
        private void move()
        {
            rb.linearVelocity = new Vector2(movementSpeed * m_moveAction.ReadValue<Vector2>().x, rb.linearVelocity.y);
        }
        

        public static lookStatesHorizontal returnHorizontalLookState()
        {
            return _instance.playerHorizontalLookState;
        }

        public static lookStatesVertical returnVerticalLookState()
        {
            return _instance.playerVerticalLookState;
        }
    }
}

