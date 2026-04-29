using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using Vaniakit.Json;
using Vaniakit.Manager;

namespace Vaniakit.ResourceManager
{
    public class Inventory : MonoBehaviour 
    {
        private static Inventory _instance;
        [SerializeField]private List<InventorySlot> items;
        void Awake()
        {
            //Keeps the inventory alive between levels
            if (_instance == null)
            {
                _instance = this; //Spawns a static value of the inventory 
            }
        }

        public static void removeItemFromInventory(InventorySlot item)
        {
            _instance.items.Remove(item);
        }
        public static void addItemToInventory(InventorySlot itemToGive)
        {
            if (!_instance.items.Contains(itemToGive)) //Shoudln't give duplicates
            {
                _instance.items.Add(itemToGive);
            }
            else
            {
                Debug.Log(itemToGive.item.GetName() + " has already been added to the inventory");
            }

            InventoryItem newItem = new InventoryItem(itemToGive.GetAmountOfItem(),itemToGive.spawnAtStart,itemToGive.item.GetSaveReference());
            
        }
        public static List<InventorySlot> GetAllItems() //Returns all items in the list
        {
            return _instance.items;
        }
        
        /// <summary>
        /// Checks if there's an instance of the manager script 
        /// </summary>
        /// <returns></returns>
        IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            //Checks if the main manager exists 
            if (Managers.instance == null)
            {
                Debug.LogWarning("There is no Manager found in the scene. You should add one otherwise your inventory will be deleted");
            }
            else
            {
                if (Managers.instance.gameObject.transform != gameObject.transform.parent)
                {
                    Debug.LogWarning("Your Inventory is not the child of the main managers object. You should move your Inventory there otherwise your Inventory may be deleted when you load another scene");
                }
            }
        }

        /// <summary>
        /// Loads all item that should start at the start of the game another function so it works with the save method in the future
        /// </summary>
        public static void loadAllNecessaryItems()
        {
            foreach (InventorySlot item in _instance.items)
            {
                try
                {
                    if (item.item.actionScript.TryGetComponent(out IEquipable script))
                    {
                        if (item.spawnAtStart)
                            script.Equip();
                        item.SetScriptInGame(script);
                        Debug.Log("The script has an IEquipable script attached");
                    }
                    else
                    {
                        Debug.Log(item.item.GetName()  + " has no IEquipable script attached");
                    }
                }
                catch
                {
                    Debug.Log("Item doesn't have an attach script");
                }
                
            }
        }

        /// <summary>
        /// Loads the save file and adds all the items to the inventory 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerator LoadFromSave(InventoryItemList items)
        {
            List<InventorySlot>  newSlots = new List<InventorySlot>();
            if (items.items.Count <= 0)
            {
                Debug.Log("No items saved in inventory");
            }
            else
            {
                int count = 0;
                //List<AsyncOperationHandle<ItemObject>> loadedItems= new List<AsyncOperationHandle<ItemObject>>();
                foreach (InventoryItem item in items.items)
                {
                    Addressables.LoadAssetAsync<ItemObject>(item.path).Completed += //Loads item from pased on the path in the json if sucessful
                    (OperationHandle) =>
                    {
                        // loadedItems.Add(OperationHandle); //Not necessary
                        if (OperationHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            InventorySlot itemInInventory = new InventorySlot();
                            itemInInventory.item = OperationHandle.Result;
                            itemInInventory.spawnAtStart = item.inventorySlotSpawnAtStart;
                            itemInInventory.AddAmount(item.inventorySlotAmountOfItem);
                            try
                            {
                                if (OperationHandle.Result.actionScript.TryGetComponent(out IEquipable script))
                                    itemInInventory.SetScriptInGame(script);
                                else
                                    Debug.Log(OperationHandle.Result.GetName() + " has no IEquipable script attached");
                                newSlots.Add(itemInInventory);
                                count++;
                            }
                            catch
                            {
                                Debug.Log("This item doesn't have a actionScript attached");
                                newSlots.Add(itemInInventory);
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                            Debug.LogError("One item wasn't found");
                            JsonInstructions.deleteFile(SaveSystem.SaveSystem.dataFileName);
                            Application.Quit();
                        }
                        
                    };
                }
                while (count < items.items.Count) ///Runs till it goes through every item in the save
                {
                    yield return null;
                }
                _instance.items = newSlots;
                
            }
            loadAllNecessaryItems();
        }

        


       
        /// <summary>
        /// Returns a serializable list that can then be saved in a save file
        /// </summary>
        /// <returns></returns>
        public static InventoryItemList saveInventory()
        {
            InventoryItemList listToSave = new  InventoryItemList();
            listToSave.items = new List<InventoryItem>();
            foreach (InventorySlot item in _instance.items)
            {
                listToSave.items.Add(new InventoryItem(item.GetAmountOfItem(),item.spawnAtStart,item.item.GetSaveReference()));
            }
            return listToSave;
        }
    }

    [System.Serializable]
    public struct InventorySlot
    {
        public ItemObject item;
        [SerializeField]private int amountOfItem;
        [Tooltip("The script that allows for code to be executed when equipped")]
        private IEquipable itemCode;
        [Tooltip("Do you want to equip the item at the start of a game useful for ablities")]
        public bool spawnAtStart;
        /// <summary>
        /// Adds {X} amount of item to your inventory.
        /// </summary>
        /// <param name="amount"></param>
        /// 
        public void AddAmount(int amount)
        {
            if (item.IsStackable())
            {
                amountOfItem+= amount;
            }
        }
        
        public void SetScriptInGame(IEquipable script)
        {
            if (itemCode == null)
            {
                itemCode = script;
            }

        }
        public IEquipable GetItemScriptInGame()
        {
            if (itemCode != null)
            {
                return itemCode;
            }
            else
            {
                Debug.LogWarning("This item doesn't have an IEquipable interface attached so can't be equiped. \n It may be currency");
            }
            return null;
        }

        public int GetAmountOfItem()
        {
            return amountOfItem;
        }

       
        
    }

    [System.Serializable]
    public class InventoryItem
    {
        /// <summary>
        /// Path to adresssible
        /// </summary>
        public string path;
        
        //public string prefabPath;
        //Inventory slot class
        public int inventorySlotAmountOfItem;
        public bool inventorySlotSpawnAtStart;

        public InventoryItem(int amountOfItem, bool spawnAtStart, string pathOfItem)
        {
            path = pathOfItem;
            inventorySlotAmountOfItem = amountOfItem;
            inventorySlotSpawnAtStart = spawnAtStart;
        }
    }

    [System.Serializable]
    public class InventoryItemList
    {
        public List<InventoryItem> items;
    }
    
    /// <summary>
    /// Defines an inventory object that can be added in Unity
    /// </summary>
    [System.Serializable]
    public class AssetReferenceInventoryObject : AssetReferenceT<ItemObject>
    {
        public AssetReferenceInventoryObject(string guid) : base(guid)
        {
        }
    }
}

