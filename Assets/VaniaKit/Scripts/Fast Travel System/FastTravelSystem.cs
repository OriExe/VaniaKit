using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vaniakit.Map;

namespace Vaniakit.FastTravelSystem
{
    public class FastTravelSystem
    {
        public const string fileNameForFTSystem= "VaniaKitFastTravelPoints.json"; //Change this if you want a different name for your files 
        public static List<FastTravelData> allActivePoints =  new List<FastTravelData>();
        public static void teleportToPoint(FastTravelData point)
        {
            FadeInManager.instance.StartCoroutine(FadeInManager.instance.FadeToBlack(point.SceneName, point.GameObjectName)); //Need to update this to accept this game object
        }
        
        /// <summary>
        /// Returns all points and puts them in the fast travel points class which is serializable 
        /// </summary>
        /// <returns></returns>
        public static FastTravelPoints saveActiveFastTravelPoints()
        {
            FastTravelPoints allPoints = new  FastTravelPoints();
            allPoints.travelPoints = allActivePoints;
            return allPoints;
        }

        public static void savePointToArray(FastTravelPoint point, GameObject gameObjectOfPoints)
        {
            //Adds all the Fast travel data to the array for this value
            FastTravelData TravelData = new FastTravelData();
            TravelData.PointName = point.pointName;
            TravelData.SceneName = gameObjectOfPoints.scene.name;
            TravelData.GameObjectName = gameObjectOfPoints.name;
            TravelData.x = gameObjectOfPoints.transform.position.x;
            TravelData.y = gameObjectOfPoints.transform.position.y;
            TravelData.z = gameObjectOfPoints.transform.position.z;
            allActivePoints.Add(TravelData);
        }

        public static bool doesPointInArrayExst(string pointName)
        {
            foreach (FastTravelData point in allActivePoints)
            {
                if (point.PointName == pointName)
                {
                    return true;
                }
            }
            return false;
        }
        public static FastTravelData findPointInArray(string pointName)
        {
            foreach (FastTravelData point in allActivePoints)
            {
                if (point.PointName == pointName)
                {
                    return point;
                }
            }
            return null;
        }
        
        
        /// <summary>
        /// Loads all the points when the game starts in an array in memory
        /// </summary>
        /// <param name="points"></param>
        public static void loadPointsToArray(FastTravelPoints points)
        {
            allActivePoints = new List<FastTravelData>();
            allActivePoints = points.travelPoints;
        }

    }
}

