using System;
using UnityEngine;

namespace Vaniakit.Items
{
    public class SwordAttack : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float howLongAttackBoxAppears = 0.7f;
        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
        private void OnEnable()
        {
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
                damageable.OnHit();
                Debug.Log("Hit");
            }
            else
            {
                Debug.Log("No Damageable Component");
            }
        }
    }
}

