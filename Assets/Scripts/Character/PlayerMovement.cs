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
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private InputActionAsset inputActions;
    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    [Header("Movement Variables")] 
    [SerializeField]private float movementSpeed;
    [SerializeField]private float jumpSpeed;

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
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        move();
    }

    void Update()
    {
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
}
