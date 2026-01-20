using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            StartCoroutine((FadeToBlack()));
            
            StartCoroutine(loadNewScene());
            
            StartCoroutine((unFadeFromBlack()));
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                justTeleported = false;
            }
        }
    }

    private IEnumerator FadeToBlack() //Not implemented yet 
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator loadNewScene()
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
            findSpawnPoint();
        }
        //If scene loaded teleport the player to the spawn point 
        //Unload this scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
    private IEnumerator unFadeFromBlack() //Not implemented yet
    {
        yield return new WaitForEndOfFrame(); 
    }

    private void findSpawnPoint()
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
                    spawnPoints.justTeleported = true;
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
            Debug.Log("No spawn point with correct name found");
        }
    }
    
}


