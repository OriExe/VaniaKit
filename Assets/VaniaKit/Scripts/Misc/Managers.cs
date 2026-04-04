using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vaniakit.FastTravelSystem;
using Vaniakit.Map;
using Vaniakit.Player;
using Vaniakit.SaveSystem;

namespace Vaniakit.Manager
{
    public class Managers : MonoBehaviour
    {
        public static Managers instance;
        
        #region Events

        protected virtual void onGameStartedToLoad()
        {
            Debug.Log("Game Started");    
        }
        protected virtual void onGameFinishedLoading()
        {
            Debug.Log("onGameReady");
        }

        protected virtual void onNewGameLoading()
        {
            Debug.Log("onNewGameStarted");
        }

        protected virtual void onPreviousGameLoading()
        {
            Debug.Log("onPreviousGameStarted");
        }
        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (instance == null)
            {
                onGameStartedToLoad();
                instance = this;
                DontDestroyOnLoad(gameObject);
                StartCoroutine(loadOtherMainCode());
            }
            else
            {
                Debug.Log("There is more than one Manager system, Destroying other managers!");
                Destroy(gameObject);
            }
        }


        IEnumerator loadOtherMainCode()
        {
            yield return StartCoroutine(Vaniakit.SaveSystem.SaveSystem.LoadAllData()); //Loads all save Data
            
            if (LoadedFromMenuMessage.hasGameLoadedFromMenu()) //Checks if game was loaded from the main meny
            {
                string sceneName;
                string gameObjectName;
                if (Map.Checkpoint.activeCheckPointData == null) //Is this a new save
                {
                    Debug.Log("No scene name, This is a new save");
                    onNewGameLoading();
                    LoadedFromMenuMessage.createRoomID(); //Creates the struct holding the default scene name and game obj name
                    sceneName = LoadedFromMenuMessage.firstLevelRoomID.sceneName;
                    gameObjectName = LoadedFromMenuMessage.firstLevelRoomID.gameObjectName;
                } 
                else //Is this a previous save
                {
                    onPreviousGameLoading();
                    sceneName = Map.Checkpoint.activeCheckPointData.sceneName;
                    gameObjectName = Map.Checkpoint.activeCheckPointData.gameObjectName;
                }
                
                StartCoroutine(FadeInManager.instance.FadeToBlack(sceneName,gameObjectName)); //Uses the map manager to load the first scene 
                //This should probably be changed
            }
            onGameFinishedLoading();
            

        }
        
    }
    
}

