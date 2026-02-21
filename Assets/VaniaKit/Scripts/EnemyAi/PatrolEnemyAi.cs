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

        [SerializeField] private Transform rayStartingPoint;
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
                
            Debug.Log("Player in line of sight");
        }

        protected virtual void onPlayerNearby()
        {
            Debug.Log("Player Nearby");
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
            detectPlayer();
            applyGravity();
        }

        //Runs code that detects if the player is in either detection radius
        void detectPlayer()
        {
            if (lineOfSightDistance > 0f) //Only runs if the line of the sight is more than 0
            {
                RaycastHit2D hit = Physics2D.Raycast(rayStartingPoint.position, Vector2.right, lineOfSightDistance);
                if (hit)
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        onPlayerInLineOfSight();
                    }
                }
            }

            if (detectionRange > 0f) //Only runs if the range is bigger than 0
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,detectionRange,Vector2.zero);
                if (hits.Length > 0) //Goes through all items that were caught in the cast.
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            onPlayerNearby();
                        }
                    }
                }
            }
        }
        void applyGravity()
        {
            if (groundCheck == null)
            {
                Debug.LogWarning("Ground Check is null Ai won't fall");
                return;
            }
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedDetectionRange, groundMask);
            if (!isGrounded)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + -0.981f * Time.deltaTime, transform.position.z);
            }
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
            onTakenDamage();
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
            {
                Vector2 targetPoint = new Vector2(pointsToGoTo[pointToFollowIndex].position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed / 1 * Time.deltaTime);
                
            }
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
            Gizmos.DrawRay(rayStartingPoint.position, Vector2.right * lineOfSightDistance);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundedDetectionRange);
        }
    }
}
