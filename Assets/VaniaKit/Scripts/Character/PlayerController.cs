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
        [SerializeField] private int startingHealth = 100;
        protected int currentHealth;
        private InputAction m_InteractAction;
        delegate void OnPlayerDead();
        OnPlayerDead onPlayerDead;
        private List<Transform> triggersinRange = new List<Transform>();
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

        protected virtual void onPlayerInteractedWithAnObject(System.Object nameOfObject)
        {
            Debug.Log("Interacted with a " + nameOfObject.ToString());
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
