using UnityEngine;

namespace Vaniakit.Ai
{
    public class AiGridUnity : MonoBehaviour
    {
        [SerializeField] private Grid aiGrid;
        [SerializeField] private LayerMask unwalkableLayerMask;
        [SerializeField] private Transform player;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            transform.position = Vector3.zero;
        }

        /// <summary>
        /// True if the node is walkable
        /// </summary>
        /// <param name="nodePosition"></param>
        /// <returns></returns>
        private bool getIsWalkable(Vector3Int nodePosition)
        {
            Vector2 nodePositionV2 = new Vector2(nodePosition.x, nodePosition.y);
            return Physics2D.OverlapBox(nodePositionV2, aiGrid.cellSize, 0, unwalkableLayerMask);
        }
        // Update is called once per frame
        void Update()
        {
            bool walkable = getIsWalkable(aiGrid.WorldToCell(player.position));
            if (walkable)
                print("Node " + aiGrid.WorldToCell(player.position)  + " is walkable ");
            else
            {
                print("Node " + aiGrid.WorldToCell(player.position) + " is not walkable ");
            }

        }
    }
}

