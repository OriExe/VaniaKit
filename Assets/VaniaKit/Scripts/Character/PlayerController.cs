using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vaniakit.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private static PlayerController instance;
        private InputActionAsset inputActions;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private int startingHealth = 100;
        protected int currentHealth;
        delegate void OnPlayerDead();
        OnPlayerDead onPlayerDead;
        
        #region Events

        /// <summary>
        /// When player is hit normally, won't be called if critical
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void onPLayerHit(int damage = 0)
        {
            Debug.Log("Player has taken " + damage + " damage: Current Health: " + currentHealth );
        }

        /// <summary>
        /// Called if they hit a trap for example 
        /// </summary>
        protected virtual void onPlayerHitCritical(int damage = 0)
        {
            Debug.Log("Player has taken critical damage of  " + damage + " + : Current Health: " + currentHealth);
        }
        #endregion
        
       
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
            currentHealth = startingHealth;
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>(); //Just get whatever ridigboy component is in the player
            }
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Event that triggers when the player dies
        /// </summary>

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
        
        /// <summary>
        /// Can be activated by an enemy or trap and do damage to the player
        /// </summary>
        /// <param name="damage"> How much damage to do</param>
        /// <param name="isCritical">If the player needs to respawn to the nearest checkpoint </param>
        public void OnHit(int damage = 0, bool isCritical = false) 
        {
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                try
                {
                    if (onPlayerDead != null)
                        onPlayerDead.Invoke();
                    else 
                        Debug.Log("No methods trigger onPlayerDead");
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning("No Script triggers the player dead event");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                return;
            }
            if (!isCritical)
                onPLayerHit(damage);
            else
                onPlayerHitCritical(damage);
        }
        
        
        
        
    }
    
}
