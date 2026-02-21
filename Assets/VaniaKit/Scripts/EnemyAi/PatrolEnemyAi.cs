using System;
using UnityEngine;

namespace Vaniakit.Ai
{
    public class PatrolEnemyAi : MonoBehaviour, IDamageable
    {
        private int pointToFollowIndex;
        private bool aiIdle;
        [SerializeField] private float speed = 2;
        [SerializeField] private Transform[] pointsToGoTo;
        [SerializeField] private float health;
        [Header("PLayer Detection ranges")]
        [Tooltip("Line of Sight Distance in Yellow")]
        [SerializeField] private float lineOfSightDistance;
        [Tooltip("Detection Range in Red")]
        [SerializeField] private float detectionRange;
        [Header("Grounded Values")]
        [SerializeField] private float groundedDetectionRange;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        protected bool isGrounded;
        #region Events
        protected virtual void onDeath()
        {

        }
        protected virtual void onTakenDamage()
        {

        }
        
        protected virtual void onPlayerInLineOfSight()
        {
            
        }

        protected virtual void onPlayerNearby()
        {
            
        }

        protected virtual void onIdle()
        {
            
        }

        protected virtual void onReachedPatrolPoint(int index)
        {
            Invoke("switchPointToPatrol", 4f);
        }
        #endregion

        private void Update()
        {
            patrolling();
        }

        void applyGravity()
        {
            if (groundCheck == null)
            {
                Debug.LogWarning("Ground Check is null Ai won't fall");
                return;
            }
            transform.position = transform.up * -0.0981f * Time.deltaTime;
        }
        public void OnHit(int damage = 0, bool isCritical = false)
        {
            if (isCritical)
            {
                health = 0;
            }
            else
            {
                health-=damage;
            }

            if (health == 0)
            {
                onDeath();
                Destroy(gameObject);
            }
        }

        private void patrolling()
        {
            if (pointsToGoTo.Length <=1)
            {
                Debug.LogError("This ai can't move as not enough points has been assigned");
                return;
            }

            if (Vector2.Distance(pointsToGoTo[pointToFollowIndex].position, transform.position) < 0.1f && !aiIdle)
            {
                onReachedPatrolPoint(pointToFollowIndex);
                aiIdle = true;
            }
            if (!aiIdle)
                transform.position = Vector2.MoveTowards(transform.position, pointsToGoTo[pointToFollowIndex].position, speed / 1 * Time.deltaTime);
        }

        protected void switchPointToPatrol()
        {
            pointToFollowIndex++;
            pointToFollowIndex %= pointsToGoTo.Length;
            aiIdle = false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * lineOfSightDistance);
        }
    }
}
