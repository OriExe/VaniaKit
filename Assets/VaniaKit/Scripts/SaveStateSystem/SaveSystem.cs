using UnityEngine;

public static class SaveSystem 
{
    

}

[System.Serializable]
public class saveData
{
    public Vaniakit.Events.EventList eventList;
    public Vaniakit.FastTravelSystem.accessibleFastTravelPoints accessibleFastTravelPoints;
    public Vaniakit.ResourceManager.InventoryItemList inventoryItemList;
}