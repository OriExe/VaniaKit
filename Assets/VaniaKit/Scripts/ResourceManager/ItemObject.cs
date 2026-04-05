using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Vaniakit.ResourceManager
{
   [CreateAssetMenu(fileName = "New Item", menuName = "Vaniakit/Item")]
    public class ItemObject : ScriptableObject
    {
        public enum Categories
        {
            Item,
            Ablity,
        }
        
        [FormerlySerializedAs("name")] [SerializeField]private string itemName;
        [TextArea(15,15)]
        [SerializeField] private string description;
        [SerializeField]private Categories category;
        [SerializeField] private bool stackable;
        [Tooltip("The field where your prefab that holds the script gets stored")]
        public GameObject actionScript;
        [Tooltip("The Reference that allows this obejct to be loaded when needed. Used in the save system")]
        [SerializeField]private AssetReferenceInventoryObject saveReference;
        private string addressablePath;
        
        /// <summary>
        /// Code that gets the addressable path which allows the item to load when the player exists
        /// </summary>
        /// <returns></returns>
        private void OnEnable()
        {
            Debug.Log("Awake called in ScriptableObject");
            Addressables.LoadResourceLocationsAsync(saveReference).Completed += (loadedPath) =>
            {
                if (loadedPath.Status == AsyncOperationStatus.Succeeded && loadedPath.Result.Count > 0)
                {
                    Debug.Log("Address has been found");
                    Debug.Log(loadedPath.Result[0].PrimaryKey);
                    addressablePath = loadedPath.Result[0].PrimaryKey;
                }
                else
                {
                    Debug.LogWarning("There is no address found");
                }
            };
        }

        public bool IsStackable()
        {
            return stackable;
        }

        public string GetName()
        {
            return itemName;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetSaveReference()
        {
            return addressablePath;
        }
        
    }
}
