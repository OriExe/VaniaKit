using System.Collections;
using UnityEngine;
using Vaniakit.ResourceManager;
using Vaniakit.Json;

namespace Vaniakit.SaveSystem
{
    public static class SaveSystem 
    {
        public static string dataFileName = "SaveFile1.json"; //The name of the current saveFile being accessed
        /// <summary>
        /// Saves all the data by calling all the save methods in each category
        /// </summary>
        public static void SaveAllData()
        {
            saveData newData = new saveData();
            newData.eventList = Vaniakit.Events.EventManager.saveEventSystem();
            newData.accessibleFastTravelPoints = Vaniakit.FastTravelSystem.FastTravelSystem.saveActiveFastTravelPoints();
            newData.inventoryItemList = Vaniakit.ResourceManager.Inventory.saveInventory();
            newData.playerCheckPointData = Vaniakit.Map.Checkpoint.activeCheckPointData;
            JsonInstructions.saveAsJsonArray(newData, dataFileName);
        }
        
        
        /// <summary>
        /// Loads the save data
        /// </summary>
        /// <returns></returns>
        public static IEnumerator LoadAllData()
        {
            saveData newData = new saveData();
            if (JsonInstructions.loadJsonArray(dataFileName, out newData)) //Returns true if file exists
            {
                Vaniakit.Events.EventManager.loadEventSystem(newData.eventList); //Loads all events
                Vaniakit.FastTravelSystem.FastTravelSystem.allActivePoints = newData.accessibleFastTravelPoints.travelPoints; //Saves travel points to an array
                yield return Vaniakit.Manager.Managers.instance.StartCoroutine(Vaniakit.ResourceManager.Inventory.LoadFromSave(newData.inventoryItemList)); //Starts a courtine that loads all the inventory items
                Vaniakit.Map.Checkpoint.activeCheckPointData = newData.playerCheckPointData;
                Debug.Log("Save loaded");
            }
            else
            {
                Debug.Log("Load Failed. File doesn't exist");
                yield return null;
            }
        
        }

    }

    [System.Serializable]
    public class saveData
    {
        public Vaniakit.Events.EventList eventList; //Data holding all the triggered events
        public Vaniakit.FastTravelSystem.FastTravelPoints accessibleFastTravelPoints; //Data holding all the fast travel points found by the player
        public Vaniakit.ResourceManager.InventoryItemList inventoryItemList; //Data holding the player's inventory
        public Vaniakit.Map.CheckPointData playerCheckPointData; //Data holding the player's last checkpoint
    }
}
