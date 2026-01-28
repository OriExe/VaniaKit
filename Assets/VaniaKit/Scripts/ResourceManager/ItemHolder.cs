using UnityEngine;

namespace Vaniakit.ResourceManager
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] InventorySlot item;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
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