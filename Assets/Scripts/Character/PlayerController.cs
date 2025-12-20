using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private InputActionAsset inputActions;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private int startingHealth = 100;
        private int currentHealth;
        private InputAction m_moveAction;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            m_moveAction = InputSystem.actions.FindAction("Move");
            currentHealth = startingHealth;
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>(); //Just get whatever ridigboy component is in the player
            }
        }

        

        private void onEnable()
        {
            inputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            inputActions.FindActionMap("player").Disable();
        }

        #region Getters
        public Rigidbody2D getPlayerRigidbody()
        {
            return rb;
        }
        
        #endregion
        
        public void OnHit(int damage = 0)
        {
            currentHealth -= damage;
            Debug.Log("Player has taken " + damage + " damage: Current Health: " + currentHealth );
        }
        
    }
    
}
