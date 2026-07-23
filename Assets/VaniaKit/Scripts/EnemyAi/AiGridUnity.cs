using UnityEngine;

namespace Vaniakit.Ai
{
    [RequireComponent(typeof(Grid))]
    public class AiGridUnity : MonoBehaviour
    {
        [SerializeField] private Grid aiGrid;
        [SerializeField] private LayerMask unwalkableLayerMask;
        [Tooltip("How many nodes it takes to reach the top right corner of the playable space")]
        [SerializeField]private Vector2Int topRightCorner;
        private static AiGridUnity instance;
        public static VkAiNode[,] allGridNodes;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            transform.position = Vector3.zero; //Fixes issue where grid outputs wrong position
            if (aiGrid == null)
                aiGrid = GetComponent<Grid>(); 
            if (FindObjectsByType<AiGridUnity>(FindObjectsSortMode.None).Length > 1)
            {
                Debug.LogError("More than one instance of the ai grid in your scene.");
            }
            instance = this;
            allGridNodes = new VkAiNode[topRightCorner.x, topRightCorner.y]; //Creates a list of grids
        }

        public static Grid getPathFindingGrid()
        {
            return instance.aiGrid;
        }

        public static Vector2Int getGridSize()
        {
            return instance.topRightCorner;
        }
        
        public static bool getIsWalkable(Vector3Int nodePosition)
        {
            Vector2 nodePositionV2 = new Vector2(nodePosition.x, nodePosition.y);
            return !Physics2D.OverlapBox(nodePositionV2, instance.aiGrid.cellSize, 0, instance.unwalkableLayerMask);
        }
    }
}

