using System;
using UnityEngine;

namespace Vaniakit.Map.Management
{
    public class MapManagementEvents : MonoBehaviour
    {
        public static MapManagementEvents instance;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
            {
                Debug.Log("There are multiple Map managers loaded, Deleting one!");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Triggers when the other room has fully unloaded
        /// </summary>
        public virtual void onRoomFullyLoaded()
        {
            Debug.Log("Room fully loaded!");
        }
        public virtual void onRoomLoaded()
        {
            Debug.Log("Room loaded!");
        }

        public virtual void onRoomStartsLoading()
        {
            Debug.Log("Room starts loading!");
        }
    }
}