using System.Collections.Generic;
using UnityEngine;
using Vaniakit.Json;

namespace Vaniakit.Events
{
    public static class EventManager
    {
        /// <summary>
        /// Saves the event to the list
        /// </summary>
        /// <param name="eventToSave"></param>
        ///
        private static List<string>  allTriggeredEvents;
        public const string fileNameForEMSystem = "VaniakitEvents.json";
        
        public static void saveEvent(string eventName)
        {
            allTriggeredEvents.Add(eventName);
            saveEventSystem();
        }

        public static bool hasEventBeenTriggeredBefore(string eventName)
        {
            if (allTriggeredEvents.Contains(eventName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void loadEventSystem()
        {
            if (!JsonInstructions.loadJsonArray(fileNameForEMSystem, out EventList allEvents))
            {
                 Debug.Log("No events saved in Event Manager!");
                 allTriggeredEvents = new List<string>();
            }
            else
            {
                Debug.Log(allEvents.events.Count + "Events have been loaded");
                allTriggeredEvents = allEvents.events;
            }
        }

        private static void saveEventSystem()
        {
            EventList allEvents = new EventList();
            allEvents.events = allTriggeredEvents;
            JsonInstructions.saveAsJsonArray(allEvents, fileNameForEMSystem);
        }
    }


    /// <summary>
    /// Class that stores all the events that have been triggered
    /// </summary>
    [System.Serializable]
    public class EventList
    {
        public List<string> events;
    }
}

