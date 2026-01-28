using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Vaniakit.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        #region Events

        protected virtual void onPLayerHit()
        {
            
        }
        #endregion
        
        private static PlayerController instance;
        private InputActionAsset inputActions;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private int startingHealth = 100;
        protected int currentHealth;
        private InputAction m_moveAction;
        delegate void OnPlayerDead();
        OnPlayerDead onPlayerDead;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            m_moveAction = InputSystem.actions.FindAction("Move");
            currentHealth = startingHealth;
            onPlayerDead += playerDead;
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>(); //Just get whatever ridigboy component is in the player
            }
            DontDestroyOnLoad(gameObject);
        }

        void playerDead()
        {
            
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
            if (currentHealth <= 0)
            {
                onPlayerDead.Invoke();
            }
        }
        
    }
    
}
