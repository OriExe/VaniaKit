using UnityEngine;

public class AddItem : MonoBehaviour
{
    [SerializeField] private Vaniakit.ResourceManager.InventorySlot itemToGive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vaniakit.ResourceManager.Inventory.addItemToInventory(itemToGive);
    }

 
}
