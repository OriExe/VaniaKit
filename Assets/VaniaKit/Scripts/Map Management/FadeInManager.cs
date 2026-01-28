using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using VaniaKit.Manager;

namespace Vaniakit.Map.Management
{
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
            }
            //If scene loaded teleport the player to the spawn point 
            //Unload this scene
            SceneManager.UnloadSceneAsync(currentScene);
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
                        spawnPoints.JustTeleportedHere();
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