using UnityEngine;

public class InventoryData {
    private string itemPath; 
    private int amountOfItem;
    private bool isitemStackable;

    public static list<InventoryData> allInventoryData; 

    InventoryData (string path, int amount, bool itemStackable)
    {
        
        itemPath = path;
        amountOfItem = amount;
        isitemStackable = itemStackable;

        allInventoryData.push(this);
    }
}