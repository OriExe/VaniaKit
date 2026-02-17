using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vaniakit.Map.Management;

namespace Vaniakit.FastTravelSystem
{
    public class FastTravelSystem
    {
        public const string fileNameForFTSystem= "VaniaKitFastTravelPoints.json"; //Change this if you want a different name for your files 
        public static List<FastTravelData> allPoints;
        public static void teleportToPoint(FastTravelData point)
        {
            ;
            FadeInManager.instance.StartCoroutine(FadeInManager.instance.FadeToBlack(point.SceneName, point.GameObjectName)); //Need to update this to accept this game object
        }
    }
}

