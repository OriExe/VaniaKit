using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Vaniakit.FastTravelSystem
{
    /// <summary>
    /// Object that is a fast travel point
    /// </summary>
    public class FastTravelPoint : Vaniakit.Misc.ATeleporterMonoBehaviour
    {
        private FastTravelData TravelData;
        [SerializeField]private string pointName = "example";

        #region Events

        protected virtual void onPlayerLoadedHere()
        {
            Debug.Log("Player loaded here");
        }

        #endregion
        
        /// <summary>
        /// Method for Saving the data in a json file
        /// </summary>
        public void jsonSaveFunction()
        {
            //Adds all the Travel points for this value
            TravelData = new FastTravelData();
            TravelData.PointName = pointName;
            TravelData.SceneName = gameObject.scene.name;
            TravelData.GameObjectName = gameObject.name;
            TravelData.x = transform.position.x;
            TravelData.y = transform.position.y;
            TravelData.z = transform.position.z;
            FastTravelPoints allPoints = null;
            //Loads the json file 
            bool duplicate = false; //Goes yes if it's a duplicate 
            if (!Vaniakit.Json.JsonInstructions.loadJsonArray(FastTravelSystem.fileNameForFTSystem, out allPoints))
            {
                allPoints = new FastTravelPoints(); //Creates the list if it doesn't exist
                allPoints.travelPoints = new List<FastTravelData>();
            }
            else //Checks if this data already exists
            {
                
                foreach (FastTravelData point in allPoints.travelPoints)
                {
                    if (TravelData.SceneName == point.SceneName)
                    {
                        if (TravelData.GameObjectName == point.GameObjectName || TravelData.PointName == point.PointName) //Did the user change the pointName of the gameobject name
                        {
                            Debug.Log("This is the same point saved in another json file and will be updated");
                            allPoints.travelPoints.IndexOf(point);
                            allPoints.travelPoints[allPoints.travelPoints.IndexOf(point)] = TravelData;
                            duplicate = true;
                            break;
                        }
                    }
                   
                }
            }

            if (!duplicate) //Only replaces if a duplicate isn't found
            {
                allPoints.travelPoints.Add(TravelData);
            }
            Vaniakit.Json.JsonInstructions.saveAsJsonArray(allPoints, FastTravelSystem.fileNameForFTSystem); //Saves the json file 

        }

        
        public override bool amITheRightObject(string gameObjectName)
        {
            if (gameObjectName == gameObject.name)
            {
                onPlayerLoadedHere();
                return true;
            }
            return false;
        }
        
    }
    
    /// <summary>
    /// List of arrays that holds all the fast travel points in the game
    /// </summary>
    [Serializable]
    public class FastTravelPoints
    {
        public List<FastTravelData> travelPoints;
    }
    
    /// <summary>
    /// Data about a fast travel point
    /// </summary>
    [Serializable]
    public class FastTravelData
    {
        public string PointName = "example";
        public string SceneName;
        public string GameObjectName;
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class FastTravelPointInfo
    {
        public bool isUnlocked;
        public FastTravelData travelPointData;
    }

    public class accessibleFastTravelPoints
    {
        public List<FastTravelPointInfo> allTravelPoints;
    }
    /// <summary>
    /// The editor that shows the make a json file button
    /// </summary>
    [CustomEditor(typeof(FastTravelPoint))]
    public class FastTravelPointEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Save this point in Json")) //Saves this point in a json file when this button is pressed
            {
                FastTravelPoint point = (FastTravelPoint)target;
                point.jsonSaveFunction();
            }
        }
    }
}

