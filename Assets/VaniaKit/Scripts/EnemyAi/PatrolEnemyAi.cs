using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Vaniakit.Player;
using Debug = UnityEngine.Debug;

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

        //Pathfinding Values
        private List<VkAiNode> path;
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
            Invoke(nameof(switchPointToPatrol), timeToWait);
        }
        #endregion

        
        protected virtual void vkStart()
        {
            
        }

        protected virtual void vkUpdate()
        {
            
        }

        private void Start()
        {
            vkStart();
        }

        private Transform target;
        /// <summary>
        /// Change this back to private when possible 
        /// </summary>
        protected void Update()
        {
            // if (!detectPlayer())
            //     patrolling();
            // applyGravity();
            //vkUpdate();
            FindPath(transform.position,target.position);
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
            
            //Ai pathfinding gizmos
            
        }
        
        
        /// <summary>
        /// This needs to be redone
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<VkAiNode> GetNeighbours(VkAiNode node) // returns a list of all the nearest neighbours of a given node
        {
            List<VkAiNode> neighbours = new List<VkAiNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.position.x + x;
                    int checkY = node.position.y + y;

                    if (checkX >= 0 && checkX < AiGridUnity.getGridSize().x && checkY >= 0 && checkY < AiGridUnity.getGridSize().y)
                    {
                        neighbours.Add(AiGridUnity.allGridNodes[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }
        
        /// <summary>
        /// Finds and returns the node with the lowest fcost in the list
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        VkAiNode lowestFCost(List<VkAiNode> nodes)
        {
            VkAiNode lowestFcostNode = nodes[0];
            foreach (VkAiNode node in nodes)
            {
                if (lowestFcostNode.fCost > node.fCost)
                {
                    lowestFcostNode = node;
                }
            }

            return lowestFcostNode;
        }
        void RetracePath(VkAiNode startNode, VkAiNode endNode) // retraces the path by using the parent property stored in each node, saves this path in a list and passes it to the grid class to be handled
        {
            path = new List<VkAiNode>();
            VkAiNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            //grid.path = path;
		
        }
        
        int GetDistance(VkAiNode nodeA, VkAiNode nodeB) // returns the distance between two given nodes
        {
            int dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
            int dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
        
        void FindPath(Vector3 startPos, Vector3 targetPos)
	    {

		    var timer = Stopwatch.StartNew(); // start a new stopwatch timer (this is just for diagnostics and nothing to do with the A* algorithm)

		    Vector3Int startPosition = AiGridUnity.getPathFindingGrid().WorldToCell(startPos);; // starting point
		    Vector3Int endPosition = AiGridUnity.getPathFindingGrid().WorldToCell(targetPos);

		    List<VkAiNode> openSet = new List<VkAiNode>(); // open set is the set of nodes to be evaluated
		    HashSet<VkAiNode> ClosedSet = new HashSet<VkAiNode>(); // closed set is the set of nodes already evaluated

            VkAiNode startNode = new VkAiNode(startPosition);
            VkAiNode targetNode = new VkAiNode(endPosition);
            
		    openSet.Add(startNode); // add the start node to the open set

		    // loop once for every node in open set
		    while (openSet.Count > 0)    // while the number of nodes in the open set is greater than 0
		    {
                VkAiNode current = openSet[0]; // declare the current node variable and set it equal to the first node in the open set
			    current = lowestFCost(openSet); // if fCost of the selected node in the open set is <= the fCost of current node, then selected node is now the current node
			    openSet.Remove(current); // remove the current node from the open set
			    ClosedSet.Add(current); // add the current node to the closed set

			    if (current == targetNode)// if current node is the target node then the path has been found
			    {
				    RetracePath(startNode, targetNode); // retrace the path from the start node to the target node using the provided RetracePath method
				    break; // [return] to exit the while loop
			    }

			    foreach (VkAiNode neighbour in GetNeighbours(current)) // for each neighbour of the current node (you can get the list of neighbours using the provided GetNeighbours method)
			    {
				    //We don't need to scan this
				    if (!neighbour.nodeWalkable|| ClosedSet.Contains(neighbour)) // if the neighbour is not traversable or the neighbour is in the closed set then [continue] to the next neighbour in the list
				    {
					    continue; //Oh continue skips this
				    }
				    //int newPath = current.gCost + (neighbour.gCost); // define an int: new gCost to neighbour. set equal to the current node's gCost + the distance between current node and the neighbour 

				    if (neighbour.hCost < current.hCost || openSet.Contains(neighbour) == false)
				    {
					    neighbour.gCost = GetDistance(neighbour, startNode);
					    neighbour.hCost = GetDistance(neighbour, targetNode);
					    
					    neighbour.parent = current;
					    if (openSet.Contains(neighbour) == false)
					    {
						    openSet.Add(neighbour);
					    }
				    }

			    }
		    }
            
            
		    ////////////////////////////////////////
		    // Your code above.

		    timer.Stop();
		    long nanosecondsPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
		    long numberOfTicks = timer.ElapsedTicks;
		    long nanoseconds = numberOfTicks * nanosecondsPerTick;
		    Debug.Log(string.Format("The A* Search from {0} to {1} took {2} nanoseconds to complete.", startPos.ToString(), targetPos.ToString(), nanoseconds.ToString()));
	    }

      
    }
}
