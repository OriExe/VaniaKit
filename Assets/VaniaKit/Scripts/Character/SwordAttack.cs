using System;
using UnityEngine;

namespace Vaniakit.Items
{
    public class SwordAttack : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float howLongAttackBoxAppears = 0.7f;
        [SerializeField] private int damage;


        #region Events
        //When the sword hits an object
        protected virtual void onDamageableComponentHit()
        {
            Debug.Log("Hit a damageable component");
        }

        //When the player presses the attack button
        protected virtual void onSwordActivated()
        {
            Debug.Log("Sword Activated");
        }
        

        #endregion
        
        protected virtual void vkStart()
        {
            
        }

        protected virtual void vkUpdate()
        {
            
        }
        
        
        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        private void Start()
        {
            vkStart();
        }

        private void Update()
        {
            vkUpdate();
        }

        private void OnEnable()
        {
            onSwordActivated();
            Invoke(nameof(disableSword), howLongAttackBoxAppears);
        }

        void disableSword()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Sword Hit" + other.name);
            if (!other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.OnHit(damage);
                onDamageableComponentHit(); //Do damage to whatever has entered the swords trigger expect for the player
            }
            else
            {
                Debug.Log("No Damageable Component");
            }
        }
    }
}

