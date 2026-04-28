using System;
using UnityEngine;
using Vaniakit.Player;

namespace Vaniakit.Ai
{
    public class PatrolEnemyAi : MonoBehaviour, IDamageable
    {
        private int pointToFollowIndex;
        private bool aiIdle;
        [SerializeField] private float speed = 2;
        [SerializeField] protected int health;
        [SerializeField] private Transform[] pointsToGoTo;
        
        [Header("Player Detection ranges")]
        [Tooltip("Line of Sight Distance in Yellow")]
        [SerializeField] private float lineOfSightDistance;
        private static Transform _player; //Reference to the player
        [SerializeField] private Transform rayStartingPoint;
        
        [Tooltip("Detection Range in Red")]
        [SerializeField] private float detectionRange;
        
        [Header("Grounded Values")]
        [SerializeField] private float groundedDetectionRange;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        protected bool IsGrounded;
        protected Vector2 LookingDirection = Vector2.right;
        [Tooltip("Time to wait to switch to the next point after going to a point ")]
        [SerializeField] protected float timeToWait; //Time to wait 
        #region Events
        protected virtual void OnDeath()
        {
            Debug.Log("Enemy has died");
        }
        protected virtual void OnTakenDamage()
        {
            Debug.Log("Enemy has taken damage" + health);
        }
        
        protected virtual void OnPlayerInLineOfSight()
        {
                //Finish this later
            Debug.Log("Player is in line of sight");
        }

        /// <summary>
        /// By default this will face the direction of the player and attack them
        /// </summary>
        protected virtual void OnPlayerNearby()
        {
            Debug.Log("Player Nearby");
            if (_player.transform.position.x < transform.position.x)
            {
                LookingDirection = Vector2.left;
            }
            else if (_player.transform.position.x > transform.position.x)
            {
                LookingDirection = Vector2.right;
            }
            
            Vector2 targetPoint = new Vector2(_player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed / 1 * Time.deltaTime);
        }

        protected virtual void OnIdle()
        {
            Debug.unityLogger.Log("idle");
        }

        protected virtual void OnReachedPatrolPoint(int index)
        {
            Invoke("switchPointToPatrol", timeToWait);
        }
        #endregion

        /// <summary>
        /// Change this back to private when possible 
        /// </summary>
        protected void Update()
        {
            if (!detectPlayer())
                patrolling();
            applyGravity();
        }

        //Runs code that detects if the player is in either detection radius
        private bool detectPlayer()
        {
            bool playerDetected = false; //If player is detected in any method the function returns true
            //Line of sight Detection code
            if (lineOfSightDistance > 0f) //Only runs if the line of the sight is more than 0
            { 
                //Sees all things in the way of the raycast
                RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartingPoint.position, LookingDirection, lineOfSightDistance); 
                foreach (RaycastHit2D thingHit in hits)
                {
                    if (thingHit)
                    {
                        if (thingHit.transform.CompareTag("Player")) //If player then does action
                        {
                            playerDetected = true;
                            if (_player == null) //Makes sure the player isn't null when executing
                            {
                                _player = thingHit.transform;
                            }
                            OnPlayerInLineOfSight();
                        }
                    } 
                }
                
            }
            //Radius Detection code
            if (detectionRange > 0f) //Only runs if the range is bigger than 0
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,detectionRange,Vector2.zero);
                if (hits.Length > 0) //Goes through all items that were caught in the cast.
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            playerDetected = true;
                            if (_player == null) //Makes sure the player isn't null when executing
                            {
                                _player = hit.transform;
                            }
                            OnPlayerNearby();
                        }
                    }
                }
            }
            return playerDetected;
        }
        /// <summary>
        /// Applies gravity so the Ai starts falling
        /// </summary>
        private void applyGravity()
        {
            if (groundCheck == null)
            {
                Debug.LogWarning("Ground Check is null Ai won't fall");
                return;
            }
            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedDetectionRange, groundMask);
            if (!IsGrounded)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + -9.81f * Time.deltaTime, transform.position.z);
            }
        }
        public void OnHit(int damage = 0, bool isCritical = false, float cooldownPeriod = 0f, IDamageable.Direction direction = IDamageable.Direction.none)
        {
            if (isCritical)
            {
                health = 0;
            }
            else
            {
                health-=damage;
            }
            OnTakenDamage();
            if (health == 0)
            {
                OnDeath();
                Destroy(gameObject);
            }
        }

        protected void patrolling()
        {
            if (pointsToGoTo.Length <=1)
            {
                Debug.LogError("This ai can't move as not enough points has been assigned");
                return;
            }

            Vector3 destination = new Vector3(pointsToGoTo[pointToFollowIndex].position.x, transform.position.y, pointsToGoTo[pointToFollowIndex].position.z);
            if (Vector2.Distance(destination, transform.position) < 0.1f && !aiIdle)
            {
                OnReachedPatrolPoint(pointToFollowIndex);
                aiIdle = true;
            }

            if (!aiIdle)
            {
                Vector2 targetPoint = new Vector2(pointsToGoTo[pointToFollowIndex].position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed / 1 * Time.deltaTime);
                
            }
        }

        /// <summary>
        /// Goes to a different patrol point
        /// It's virtual so the developer if they want this function to be used or not
        /// </summary>
        protected virtual void switchPointToPatrol()
        {
            pointToFollowIndex++;
            pointToFollowIndex %= pointsToGoTo.Length;
            aiIdle = false;
            
            if (pointsToGoTo[pointToFollowIndex].position.x < transform.position.x)
            {
                LookingDirection = Vector2.left;
            }
            else if (pointsToGoTo[pointToFollowIndex].position.x > transform.position.x)
            {
                LookingDirection = Vector2.right;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (rayStartingPoint == null)
                rayStartingPoint = transform;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayStartingPoint.position, LookingDirection * lineOfSightDistance);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundedDetectionRange);
        }
    }
}
