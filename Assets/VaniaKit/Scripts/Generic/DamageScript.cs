using System;
using UnityEngine;


namespace Vaniakit.Collections
{
    /// <summary>
    /// Small Script that lets players or enemies do damage to the player
    /// </summary>
    public class VaniaKitDamageScript : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private bool isDeadly = false;

        [Tooltip("Make the player take damage again if they stay there for too long")]
        [SerializeField] private float timeToDoDamageAgain;
        private float elapsedTime;
        bool startTimer = false;

        private Transform player;

        private void Start()
        {
            elapsedTime = timeToDoDamageAgain;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                startTimer = true;
                player = other.transform;
                doDamage(other.transform);

                //Code like this should be included in your player controller script
                // if (isDeadly)
                // {
                //     TeleportToNearestCheckpoint.TeleportPlayerToNearestCheckpoint(other.transform); //Can be commented out if you want your own solution
                // }
            }
        }

        private void Update()
        {
            if (startTimer)
            {
                elapsedTime -= Time.deltaTime;
                if (elapsedTime <= 0)
                {
                    doDamage(player);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                startTimer = false;
                elapsedTime = timeToDoDamageAgain;
            }
        }

        private void doDamage(Transform player)
        {
            elapsedTime = timeToDoDamageAgain;
            player.gameObject.GetComponent<IDamageable>().OnHit(damage, isDeadly);
        }
    }
}

