using System;
using UnityEngine;

namespace Vaniakit.Map
{
    /// <summary>
    /// Stores and renders the checkpoint
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        private CheckPointData checkPointData;
        public static CheckPointData activeCheckPointData;

        protected virtual void onPlayerActivatedCheckpoint()
        {
            Debug.Log("Checkpoint activated");
        }
        protected void Awake()
        {
            checkPointData = new CheckPointData();
            checkPointData.sceneName = gameObject.scene.name;
            checkPointData.gameObjectName = gameObject.name;
        }

        /// <summary>
        /// Runs when the player triggers the checkpoint (such as by pressing E while next to it)
        /// </summary>
        protected void TriggerCheckPoint(bool saveData = true)
        {
            onPlayerActivatedCheckpoint();
            if (saveData)
            {
                activeCheckPointData = checkPointData;
                SaveSystem.SaveSystem.SaveAllData();
            }
        }
        
    }

    [Serializable]
    public class CheckPointData
    {
        [HideInInspector]public string sceneName = "";
        [HideInInspector]public string gameObjectName;
    }
}