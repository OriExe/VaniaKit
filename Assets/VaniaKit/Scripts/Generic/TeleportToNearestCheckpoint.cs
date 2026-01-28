using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vaniakit.Collections
{
   public class TeleportToNearestCheckpoint : MonoBehaviour
   {
      [Tooltip("The name of the tag that defines all checkpoints in the game")]
      [SerializeField] private string tagName;
      private static string whatIsACheckpoint = "";
      private static List<Transform> allCheckPointsInScene;
      private void Start()
      {
         if (whatIsACheckpoint == "")
            whatIsACheckpoint = tagName;
         findAllCheckPointsInScene();
      }
      /// <summary>
      /// Teleport player to the nearest checkpoint when called
      /// </summary>
      /// <param name="player"></param>
      public static void TeleportPlayerToNearestCheckpoint(Transform player)
      {
         if (allCheckPointsInScene.Count == 0)
         {
            Debug.Log("TeleportPlayerToNearestCheckpoint - No checkpoints found");
            return;
         }
         Transform nearestCheckpoint = null;
         float closestDistance = float.MaxValue;
         foreach (Transform checkpoint in allCheckPointsInScene)
         {
            float distance = Vector3.Distance(player.position, checkpoint.position);
            if (distance < closestDistance)
            {
               closestDistance = distance;
               nearestCheckpoint = checkpoint;
            }
         }
         if (nearestCheckpoint != null)
         {
            player.position = nearestCheckpoint.position;  
         }
      }

      /// <summary>
      /// Add all checkpoints in the scene to the list when called
      /// Deletes the old list
      /// </summary>
      public static void findAllCheckPointsInScene()
      {
         GameObject[] list = GameObject.FindGameObjectsWithTag(whatIsACheckpoint);
         allCheckPointsInScene = new List<Transform>();
         foreach (GameObject go in list)
         {
            allCheckPointsInScene.Add(go.transform);
         }

         if (list.Length == 0)
         {
            Debug.LogWarning("No checkpoints found, did you spell the tag wrong?");
         }
      }
   }
   
}
