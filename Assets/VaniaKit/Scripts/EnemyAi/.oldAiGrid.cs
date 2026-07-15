using UnityEngine;

///
/// Script is not in use as I don't know how to get the node get position to work
///
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
        Vector3 worldBottomLeft;
        Vector3 worldTopRight;
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
             worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
             
             Debug.Log(new Vector3(worldBottomLeft.x * gridWorldSize.x, worldBottomLeft.y * gridWorldSize.y, 0) * 0.5f);
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

        private VkNode playerNode;

        void Update()
        {
            tempMethod();
        }
        void tempMethod()
        {
             playerNode = NodeFromWorldPoint(player.position);
        }
        public Transform player;
        private void OnDrawGizmos()
        {
            //Need to make it x and y as it targets 2d space
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,0));

            if (grid != null)
            {
                foreach (VkNode node in grid)
                {
                    
                    if (playerNode == node)
                    {
                        Gizmos.color = Color.cyan;
                    }
                    else
                    {
                        Gizmos.color = (node.walkable) ? new Color(0f,1f,0f,0.5f) : new Color(1f,0f,0f,0.5f);
                    }
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                   
                }
            }
        }
        
        //Converts world position to node not finished yet
        public VkNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            //Make the distant a percentage (a value of 0 and 1)
            float percentX = (worldPosition.x - worldBottomLeft.x);
            float percentY = (worldPosition.y - worldBottomLeft.y); 
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridsizeX-1) * percentX);
            int y = Mathf.RoundToInt((gridsizeY-1) * percentY);
            Debug.Log("X is " + x + " and Y is " + y + "\n Meanwhile percentX is " + percentX + " and percent y is" + percentY);
            return grid[x,y];
        }
    }

    
    
}

