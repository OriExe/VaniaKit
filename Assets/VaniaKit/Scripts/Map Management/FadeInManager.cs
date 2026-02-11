using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vaniakit.Manager;
using Vaniakit.Player;

namespace Vaniakit.Map.Management
{
    [RequireComponent(typeof(MapManagementEvents))]
    public class FadeInManager : MonoBehaviour
    {
        public static FadeInManager instance;
        public static Image fadePanel;
        [SerializeField] private GameObject uiCanvas;
        [SerializeField] private Image fadePanelObj;

        public float timetakenToFade;
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
            
            if (uiCanvas == null)
            {
                Debug.Log("There is no ui canvas for the manager to use");
            }
            else
            {
                if (fadePanelObj == null) //Checks if there is a fade panel to use already 
                {
                    createAFadePanel();
                }
                else
                {
                    if (fadePanelObj.gameObject.scene.rootCount == 0) //Checks if the object is a prefab
                    {
                        fadePanel = Instantiate(fadePanelObj, uiCanvas.transform); //If it is it creates it as a gameobject
                    }
                }
            }
        }
        
        /// <summary>
        /// Fade the current scene to black. Then load the next scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public IEnumerator FadeToBlack(string sceneName, string destination) 
        {
            MapManagementEvents.instance.onRoomStartsLoading(); //Triggers global room is loading event
            float elapsedTime = 0;
            while (elapsedTime < timetakenToFade)
            {
                elapsedTime += Time.deltaTime;
                Color Transparent = Color.black;
                Transparent.a = 0f;
                fadePanel.color = Color.Lerp(Transparent, Color.black, elapsedTime / timetakenToFade);
                yield return null;
            }

            StartCoroutine(loadNewScene(sceneName, destination));
        }
        
        public IEnumerator unFadeFromBlack() 
        {
            float elapsedTime = 0;
            while (elapsedTime < timetakenToFade)
            {
                elapsedTime += Time.deltaTime;
                Color Transparent = Color.black;
                Transparent.a = 0f;
                fadePanel.color =  Color.Lerp(Color.black, Transparent, elapsedTime / timetakenToFade);
                yield return null;
            }
        }
        
        /// <summary>
        /// If no fade panel exists in game then the game will create a basic one stored in the resources folder
        /// </summary>
        private void createAFadePanel()
        {
            if (fadePanel != null) return; //Returns if not empty
            
            if (fadePanelObj == null)
            {
                fadePanelObj = Resources.Load<GameObject>("Vaniakit/FadePanel").GetComponent<Image>(); //Finds the default panel in the vaniakit resources folder
            }
            fadePanel = Instantiate(fadePanelObj, uiCanvas.transform); //
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

            //If scene loaded teleport the player to the spawn point 
           
            if (sceneLoading.isDone)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                Debug.Log("Scene loading done");
                findSpawnPoint(sceneName, destination);
                MapManagementEvents.instance.onRoomLoaded();
            }
            //Unload this scene
            AsyncOperation sceneUnloading =  SceneManager.UnloadSceneAsync(currentScene);
            while (sceneUnloading.isDone == false)
            {
                yield return null;
            }

            if (sceneUnloading.isDone)
            {   
                MapManagementEvents.instance.onRoomFullyLoaded();
                PlayerCamera.snapCameraToPlayer();
                StartCoroutine(unFadeFromBlack());
            }
        }
        
        /// <summary>
        /// Finds the location to spawn the player in the new scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="destination"></param>
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