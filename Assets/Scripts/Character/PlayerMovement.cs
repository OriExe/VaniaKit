using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour, IDamageable
{
    private static PlayerMovement _instance; //Static Instance of player controller
    public enum lookStates
    {
        up,
        down,
        left,
        right
    }
    private lookStates playerLookState; //Where is the player currently looking
    protected event EventHandler onPlayerLand;
    private Rigidbody2D rb;
    
    private InputActionAsset inputActions;
    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    [Header("Movement Speed")] 
    [SerializeField]private float movementSpeed;
    [SerializeField]private float jumpSpeed;
    
    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayers;
    private bool isGrounded;
    [SerializeField] private float groundCheckRadius;
    [Tooltip("What object will check below for ground")]
    [SerializeField] private Transform groundCheck;
    
    [Header("Health Variables")]
    [SerializeField] private int startHealth;
    [SerializeField] private int currentHealth;
    
    private void onEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("player").Disable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (groundCheck == null)
        {
            groundCheck = gameObject.transform;
        }
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();

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

        if (moveValue.y > 0)
        {
            playerLookState = lookStates.up;
        }
        else if (moveValue.y < 0)
        {
            playerLookState = lookStates.down;
        }
        else if (moveValue.x > 0)
        {
            playerLookState = lookStates.right;
        }
        else if (moveValue.x < 0)
        {
            playerLookState = lookStates.left;
        }
        
        if (m_jumpAction.WasPressedThisFrame())
        {
            jump();
        }

        if (m_jumpAction.WasReleasedThisFrame())
        {
            releaseJump();
        }
    }

    /// <summary>
    ///Makes Player jump
    /// </summary>
    private void jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
    }

    /// <summary>
    /// Makes player fall
    /// </summary>
    private void releaseJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
    }

    private void move()
    {
        rb.linearVelocity = new Vector2(movementSpeed * m_moveAction.ReadValue<Vector2>().x, rb.linearVelocity.y);
    }

    public void OnHit(int damage = 0)
    {
        currentHealth -= damage;
    }

    public static lookStates returnCurrentState()
    {
        return _instance.playerLookState;
    }
}
