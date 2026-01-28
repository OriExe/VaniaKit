using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Vaniakit.Manager;

namespace Vaniakit.Map.Management
{
    [RequireComponent(typeof(MapManagementEvents))]
    public class FadeInManager : MonoBehaviour
    {
        public static FadeInManager instance;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("Another FadeInManager is already created");
                Destroy(gameObject);
            }
        }
        public IEnumerator FadeToBlack(string sceneName, string destination) 
        {
            MapManagementEvents.instance.onRoomStartsLoading();
            float elapsedTime = 0;
            while (elapsedTime < Managers.instance.timetakenToFade)
            {
                elapsedTime += Time.deltaTime;
                Color Transparent = Color.black;
                Transparent.a = 0f;
                Managers.fadePanel.color = Color.Lerp(Transparent, Color.black, elapsedTime / Managers.instance.timetakenToFade);
                yield return null;
            }

            StartCoroutine(loadNewScene(sceneName, destination)) ;
            StartCoroutine(unFadeFromBlack());
        }
        
        public IEnumerator unFadeFromBlack() 
        {
            float elapsedTime = 0;
            while (elapsedTime < Managers.instance.timetakenToFade)
            {
                elapsedTime += Time.deltaTime;
                Color Transparent = Color.black;
                Transparent.a = 0f;
                Managers.fadePanel.color =  Color.Lerp(Color.black, Transparent, elapsedTime / Managers.instance.timetakenToFade);
                yield return null;
            }
        }
        
        /// <summary>
        /// Loads the new scene and then unloads the old scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private IEnumerator loadNewScene(string sceneName, string destination)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);//Load scene async
            while (sceneLoading.isDone == false)
            {
                yield return null;
            }

            if (sceneLoading.isDone)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                Debug.Log("Scene loading done");
                findSpawnPoint(sceneName, destination);
                MapManagementEvents.instance.onRoomLoaded();
            }
            //If scene loaded teleport the player to the spawn point 
            //Unload this scene
            AsyncOperation sceneUnloading =  SceneManager.UnloadSceneAsync(currentScene);
            while (sceneUnloading.isDone == false)
            {
                yield return null;
            }

            if (sceneUnloading.isDone)
            {   
                MapManagementEvents.instance.onRoomFullyLoaded();
            }
        }
        
        private void findSpawnPoint(string sceneName, string destination)
        {
            SceneTeleporter[] sceneTeleporter = FindObjectsByType<SceneTeleporter>(FindObjectsSortMode.None);
            Transform spawnPoint = null;

            foreach (SceneTeleporter spawnPoints in sceneTeleporter)
            {
                if (spawnPoints.gameObject.scene.name == sceneName)
                {
                    Debug.Log("In right scene");
                    if (spawnPoints.gameObject.name == destination)
                    {
                        Debug.Log("Found destination");
                        spawnPoints.justTeleportedHere();
                        spawnPoint = spawnPoints.gameObject.transform;
                        break;
                    }
                }
            }
            if (spawnPoint != null)
            {
                GameObject.FindGameObjectWithTag("Player").transform.position = spawnPoint.position;
            }
            else
            { 
                Debug.LogError("No point called {" + destination + "} found");
            }
        }

    }
}