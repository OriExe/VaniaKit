using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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

        public static void addItemToInventory(InventorySlot itemToGive)
        {
            _instance.items.Add(itemToGive);
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
            loadAllNecessaryItems();
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
                if (item.item.actionScript.TryGetComponent(out IEquipable script))
                {
                    if (item.spawnAtStart)
                        script.Equip();
                    item.SetScriptInGame(script);
                    Debug.Log("The script has an IEquipable script attached");
                }
                else
                {
                    Debug.Log(item.item.getName()  + " has no IEquipable script attached");
                }
                    
            }
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
        
    }
}

