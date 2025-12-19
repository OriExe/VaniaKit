using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private InputActionAsset inputActions;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private int StartingHealth = 100;
        private int currentHealth;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            currentHealth = StartingHealth;
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
