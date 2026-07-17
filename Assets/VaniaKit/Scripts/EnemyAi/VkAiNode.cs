using UnityEngine;

namespace Vaniakit.Ai
{
    public class VkAiNode
    {
        public Vector3Int position;

        public int fCost => gCost + hCost;

        public int gCost;
        public int hCost;
        private bool nodeWalkableTestCompleted = false;
        private bool walkable;
        public VkAiNode parent;

        public bool nodeWalkable
        {
            get
            {
                if (nodeWalkableTestCompleted)
                {
                    return walkable;
                }
                else
                {
                    walkable = AiGridUnity.getIsWalkable(position);
                    nodeWalkableTestCompleted = true;
                    return walkable;
                }
            }
        }
        
        /// <summary>
        /// True if the node is walkable
        /// </summary>
        /// <param name="nodePosition"></param>
        /// <returns></returns>
        

        public VkAiNode(Vector3Int pos)
        {
            position = pos;
        }
    }
}