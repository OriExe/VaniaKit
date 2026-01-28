using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vaniakit.Manager;

namespace Vaniakit.Map.Management
{
        [RequireComponent(typeof(BoxCollider2D))]
    public class SceneTeleporter : MonoBehaviour
    {
        //VARIABLES NEEDED
        //SceneToLoad
        //WhereToTeleportPlayer [determined by a list of game objects //I could use tags to get the obj 
        
        [Tooltip("The name of the gameobject you want to teleport to.")]
        [SerializeField] private String destination;
        [Tooltip("The name of the scene you want to load")]
        [SerializeField]private String sceneName;
        
        bool justTeleported = false;
        /// <summary>
        /// Makes sure the box colider is a trigger
        /// </summary>
        private void Start()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            
        }

        /// <summary>
        /// Teleports the player to anothe scene
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!justTeleported && other.CompareTag("Player"))
            {
                Debug.Log("Trigger entered");
                StartCoroutine(FadeInManager.instance.FadeToBlack(sceneName,destination)); //Loads a new scene and unloads the current scene
            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    justTeleported = false;
                }
            }
        }

        public void justTeleportedHere()
        {
            justTeleported = true;
        }
    }

}


