using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Help from https://www.youtube.com/watch?v=nhiFx28e7JY
namespace Vaniakit.Ai
{
    public class AiGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask unwalkableMask;
        [SerializeField]private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        private VkNode[,] grid;

        private float nodeDiameter;
        int gridsizeX, gridsizeY;
        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridsizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
            gridsizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter)*2; //2 was added as it fixes issue where the y grid was too small
            createGrid();
        }

        //Creates a 2d grid
        void createGrid()
        {
             grid = new VkNode[gridsizeX, gridsizeY];
             Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
             
             for (int x = 0; x < gridsizeX; x++)
             {
                 //Creates a node
                 for (int y = 0; y < gridsizeY; y++)
                 {
                     Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter) + Vector3.up * (y * nodeRadius); //Got rid of vector 3 foward
                     bool walkable = !Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0, unwalkableMask);
                     grid[x, y] = new VkNode(walkable, worldPoint);
                 }
             }
        }
        private void OnDrawGizmos()
        {
            //Need to make it x and y as it targets 2d space
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,0));

            if (grid != null)
            {
                foreach (VkNode node in grid)
                {
                    Gizmos.color = (node.walkable) ? new Color(0f,1f,0f,0.5f) : new Color(1f,0f,0f,0.5f);
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
        
        //Converts world position to node not finished yet
        public VkNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            
            return new VkNode(false, worldPosition);
        }
    }

    
    
}

