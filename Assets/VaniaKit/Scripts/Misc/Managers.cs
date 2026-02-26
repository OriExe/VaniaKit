using System;
using System.Collections;
using System.Collections.Generic;
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
            yield return StartCoroutine(Vaniakit.SaveSystem.SaveSystem.LoadAllData());
            onGameReady();
    
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
    
    
}

