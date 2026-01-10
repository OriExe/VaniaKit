using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryData {
    private string itemPath; 
    private int amountOfItem;
    private bool isitemStackable;
    
    public static List<InventoryData> allInventoryData; 

    InventoryData (string path, int amount, bool itemStackable)
    {
        
        itemPath = path;
        amountOfItem = amount;
        isitemStackable = itemStackable;

        allInventoryData.Add(this);
    }
}