using System;
using UnityEngine;

namespace Vaniakit.ResourceManager
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] InventorySlot item;
        protected virtual void vkStart()
        {
            
        }

        protected virtual void vkUpdate()
        {
            
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            try
            {
                if (item.item.actionScript.TryGetComponent(out IEquipable script)) //Sees if this item has a IEquipable 
                {
                    item.SetScriptInGame(script); //Add the IEquipable to the itemSlot so it can be loaded
                    Debug.Log("The script has an IEquipable script attached");
                }
                else
                {
                    Debug.Log(item.item.GetName()  + " has no IEquipable script attached");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("This item doesn't have a script");
            }
           
        }

        private void Start()
        {
            vkStart();
        }

        private void Update()
        {
            vkUpdate();
        }

        protected void givePlayerItem(bool equipItem = false)
        {
            Inventory.addItemToInventory(item);

            if (equipItem)
            {
                item.GetItemScriptInGame().Equip();
            }
        }
        
        
        
    }
}