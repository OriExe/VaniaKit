using System;
using System.Collections.Generic;
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
        [SerializeField] protected int startingHealth = 100;
        protected int currentHealth;
        private InputAction m_InteractAction;
        public delegate void OnPlayerDead();
        public OnPlayerDead onPlayerDead;
        private List<Transform> triggersinRange = new List<Transform>();
        private float playerCooldownPeriod; //If above 0 player can't take more damage
        private bool coolDownPeriodOver = true; 
        #region Events

        /// <summary>
        /// When player is hit normally, won't be called if critical
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void onPLayerHit(int damage = 0, IDamageable.Direction direction = IDamageable.Direction.none)
        {
            Debug.Log("Player has taken " + damage + " damage: Current Health: " + currentHealth );
        }

        /// <summary>
        /// Called if they hit a trap for example 
        /// </summary>
        protected virtual void onPlayerHitCritical(int damage = 0, IDamageable.Direction direction = IDamageable.Direction.none)
        {
            Debug.Log("Player has taken critical damage of  " + damage + " + : Current Health: " + currentHealth);
        }

        protected virtual void onPlayerInteractedWithAnObject(System.Object nameOfObject)
        {
            Debug.Log("Interacted with a " + nameOfObject.ToString());
        }
        protected virtual void onPlayerCanTakeDamage()
        {
            Debug.Log("Player can take more damage");
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
                return;
            }
            currentHealth = startingHealth;
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>(); //Just get whatever rigidbody component is in the player
            }
            DontDestroyOnLoad(gameObject);
            m_InteractAction = InputSystem.actions.FindAction("Interact");
        }

        /// <summary>
        /// Event that triggers when the player dies
        /// </summary>
        protected void Update()
        {
            if (playerCooldownPeriod > 0f) //Is there a cooldown period happening 
            {
                playerCooldownPeriod -= Time.deltaTime; //Reduce value overtime
            }
            else if (!coolDownPeriodOver) //If the value is marked is false but isn't running
            {
                onPlayerCanTakeDamage();
                coolDownPeriodOver = true; //Make it true 
            }
            
            if (m_InteractAction.WasPressedThisFrame())
            {
                foreach (Transform trigger in triggersinRange)
                {
                    try
                    {
                        if (trigger.TryGetComponent(out IInteractable interactable))
                        {
                            interactable.onInteract();
                            onPlayerInteractedWithAnObject(interactable.GetType());
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("A deleted object has been found");
                    }
                   
                }
            }
        }
        
        private void onEnable()
        {
            inputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            try
            {
                inputActions.FindActionMap("player").Disable();
            }
            catch 
            {
              Debug.Log("Game has ended or player has teleported to another scene");
            }
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.parent != transform) //Won't run if the trigger is a child of the player object 
            {
                triggersinRange.Add(other.transform);
                Debug.Log("A trigger has been saved");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (triggersinRange.Contains(other.transform))
            {
                Debug.Log("A trigger has been been deleted");
                triggersinRange.Remove(other.transform);
            }
        }

        #region Getters
        public static Rigidbody2D getPlayerRigidbody()
        {
            return instance.rb;
        }
        
        #endregion

        /// <summary>
        /// Can be activated by an enemy or trap and do damage to the player
        /// </summary>
        /// <param name="damage"> How much damage to do</param>
        /// <param name="isCritical">If the player needs to respawn to the nearest checkpoint </param>
        /// <param name="cooldownPeriod">How long till the player is no longer immune</param>
        public void OnHit(int damage = 0, bool isCritical = false, float cooldownPeriod = 0f, IDamageable.Direction direction = IDamageable.Direction.none) 
        {
            if (playerCooldownPeriod > 0f) //If the cooldown period still going
            {
                return;
            }
            currentHealth -= damage;
            playerCooldownPeriod = cooldownPeriod;
            coolDownPeriodOver = false;
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
                onPLayerHit(damage, direction);
            else
                onPlayerHitCritical(damage, direction);
        }
        
        /// <summary>
        /// Teleports the player to their spawn point in the game scene after loading the game
        /// </summary>
        /// <param name="position"></param>
        public static void teleportPlayer(Vector3 position)
        {
            instance.gameObject.transform.position = position;
        }
        
        /// <summary>
        /// Stop all movement from the player
        /// </summary>
        /// <param name="isEnabled"></param>
        public static void playerControllerEnabled(bool isEnabled)
        {
            instance.GetComponent<PlayerMovement>().enabled = isEnabled;
            instance.rb.linearVelocity = Vector3.zero;
            instance.GetComponent<PlayerJump>().enabled = isEnabled;
            
        }

        /// <summary>
        /// Restores health to their max. Useful for checkpoints
        /// </summary>
        public static void restoreHealthToNormal()
        {
            instance.currentHealth = instance.startingHealth;
        }

        /// <summary>
        /// Increases player starting health
        /// </summary>
        /// <param name="amount"></param>
        public static void increaseStartingHealth(int amount)
        {
            instance.startingHealth += amount;
        }

        /// <summary>
        /// Returns starting health
        /// </summary>
        /// <returns></returns>
        public static int getStartingHealth()
        {
            return instance.startingHealth;
        }
    }
    
}
