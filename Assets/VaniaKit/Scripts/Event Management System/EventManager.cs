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
        private static List<string>  allTriggeredEvents =  new List<string>();
        public const string fileNameForEMSystem = "VaniakitEvents.json";
        
        public static void saveEvent(string eventName)
        {
            allTriggeredEvents.Add(eventName);
            Debug.Log(eventName + "has been saved");
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

        public static void loadEventSystem(EventList allEvents)
        {
            Debug.Log(allEvents.events.Count + "Events have been loaded");
            allTriggeredEvents = allEvents.events;
        }
        
        /// <summary>
        /// Saves the current events triggered in a save file
        /// </summary>
        /// <returns></returns>
        public static EventList saveEventSystem()
        {
            EventList allEvents = new EventList();
            allEvents.events = allTriggeredEvents;
            return allEvents;
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

