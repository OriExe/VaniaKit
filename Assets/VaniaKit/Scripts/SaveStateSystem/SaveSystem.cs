using System.Collections;
using UnityEngine;
using Vaniakit.ResourceManager;
using Vaniakit.Json;

namespace Vaniakit.SaveSystem
{
    public static class SaveSystem 
    {
        private const string dataFileName = "SaveFile1.json";
        /// <summary>
        /// Saves all the data by calling all the save methods in each category
        /// </summary>
        public static void SaveAllData()
        {
            saveData newData = new saveData();
            newData.eventList = Vaniakit.Events.EventManager.saveEventSystem();
            newData.accessibleFastTravelPoints = Vaniakit.FastTravelSystem.FastTravelSystem.saveActiveFastTravelPoints();
            newData.inventoryItemList = Vaniakit.ResourceManager.Inventory.saveInventory();
            JsonInstructions.saveAsJsonArray(newData, dataFileName);
        }
        
        
        /// <summary>
        /// Loads
        /// </summary>
        /// <returns></returns>
        public static IEnumerator LoadAllData()
        {
            saveData newData = new saveData();
            if (JsonInstructions.loadJsonArray(dataFileName, out newData))
            {
                Vaniakit.Events.EventManager.loadEventSystem(newData.eventList); //Loads all events
                Vaniakit.FastTravelSystem.FastTravelSystem.allActivePoints = newData.accessibleFastTravelPoints.travelPoints;
                yield return Vaniakit.Manager.Managers.instance.StartCoroutine(Vaniakit.ResourceManager.Inventory.LoadFromSave(newData.inventoryItemList)); //Starts a courtine that loads all the inventory items
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
        public Vaniakit.Events.EventList eventList;
        public Vaniakit.FastTravelSystem.FastTravelPoints accessibleFastTravelPoints;
        public Vaniakit.ResourceManager.InventoryItemList inventoryItemList;
    }
}
