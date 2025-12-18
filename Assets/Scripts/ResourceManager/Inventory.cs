using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceManager
{
    public class Inventory : MonoBehaviour 
    {
        private static Inventory _instance;
        [SerializeField]private List<InventorySlot> items;
        void Awake()
        {
            //Keeps the inventory alive between levels
            DontDestroyOnLoad(gameObject);
            if (_instance == null)
            {
                _instance = this; //Spawns a static value of the inventory 
            }
            else
            {
                Debug.LogError("There is more than one instance of Inventory in the scene"); 
                Destroy(this);
            }
        }
        

        public static List<InventorySlot> GetAllItems() //Returns all items in the list
        {
            return _instance.items;
        }

        private void Start()
        {
            foreach (InventorySlot item in items)
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
        private IEquipable scriptInGame;
        [Tooltip("Do you want to equip the item at the start of a game")]
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
            if (scriptInGame != null)
            {
                scriptInGame = script;
            }
        }
        
    }
}

