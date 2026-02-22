using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vaniakit.FastTravelSystem;

namespace Vaniakit.Manager
{
    public class Managers : MonoBehaviour
    {
        public static Managers instance;

        #region Events

        protected virtual void onGameStarted()
        {
            Debug.Log("Game Started");    
        }
        protected virtual void onFastTravelSystemLoaded()
        {
            Debug.Log("onFastTravelSystemLoaded");
        }

        protected virtual void onGameReady()
        {
            Debug.Log("onGameReady");
        }
        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (instance == null)
            {
                onGameStarted();
                instance = this;
                DontDestroyOnLoad(gameObject);
                loadOtherMainCode();
            }
            else
            {
                Debug.Log("There is more than one Manager system, Destroying other managers!");
                Destroy(gameObject);
            }
            
            
            
        }


        void loadOtherMainCode()
        {
            //Loads the Fast Travel System
            try
            {
                Json.JsonInstructions.loadJsonArray(FastTravelSystem.FastTravelSystem.fileNameForFTSystem, out FastTravelPoints data);
                FastTravelSystem.FastTravelSystem.allPoints = data.travelPoints;
                onFastTravelSystemLoaded();
            }
            catch (Exception e)
            {
                Debug.LogWarning("FastTravelSystemLoadError, There may not be a json file for FastTravel. That's usually Fine if you don't have a Fast Travel System.");
                Debug.LogError(e);
            }
            //Load Event System
            Vaniakit.Events.EventManager.loadEventSystem();
            onGameReady();
    
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
    
    
}

