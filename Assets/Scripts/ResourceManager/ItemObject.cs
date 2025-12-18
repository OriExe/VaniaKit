using System;
using UnityEngine;

namespace ResourceManager
{
   [CreateAssetMenu(fileName = "New Item", menuName = "Vaniakit/Item")]
    public class ItemObject : ScriptableObject
    {
        public enum Categories
        {
            Item,
            Ablity,
        }
        
        [SerializeField]private string name;
        [TextArea(15,15)]
        [SerializeField] private string description;
        [SerializeField]private Categories category;
        [SerializeField] private bool stackable;
        [Tooltip("The field where your prefab that holds the script gets stored")]
        public GameObject actionScript;
        public bool IsStackable()
        {
            return stackable;
        }

        public string getName()
        {
            return name;
        }
    }
}
