using UnityEngine;

namespace Vaniakit.Ai
{
    public class VkNode
    {
        public bool walkable;
        public Vector3 worldPosition;
        
        public VkNode(bool isWalkable, Vector3 worldpos) 
        {
            walkable = isWalkable;
            worldPosition = worldpos;
        }
    }
    
    
    
}