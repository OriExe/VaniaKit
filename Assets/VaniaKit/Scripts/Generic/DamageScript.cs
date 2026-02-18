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

        private IDamageable damagedObj;

        private void Start()
        {
            elapsedTime = timeToDoDamageAgain;
        }
//
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageObj))
            {
                startTimer = true;
                damagedObj = damageObj;
                doDamage(damagedObj);

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
                    doDamage(damagedObj);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            other.gameObject.TryGetComponent<IDamageable>(out IDamageable otherCollision);
            if (otherCollision == damagedObj)
            {
                startTimer = false;
                elapsedTime = timeToDoDamageAgain;
            }
        }

        private void doDamage(IDamageable damageObj)
        {
            elapsedTime = timeToDoDamageAgain;
            damageObj.OnHit(damage, isDeadly);
        }
    }
}

