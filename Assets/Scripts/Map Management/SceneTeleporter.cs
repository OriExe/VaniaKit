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

    public string NAMESCENEOBJECTISIN { get; private set; } //The scene the object is in
    
    /// <summary>
    /// Makes sure the box colider is a trigger
    /// </summary>
    private void Start()
    {
        NAMESCENEOBJECTISIN = SceneManager.GetActiveScene().name;
        GetComponent<BoxCollider2D>().isTrigger = true;       
    }

    /// <summary>
    /// Teleports the player to anothe scene
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine((FadeToBlack()));
        
        StartCoroutine(loadNewScene());
        
        StartCoroutine((unFadeFromBlack()));
    }

    private IEnumerator FadeToBlack()
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
            
        }
        //If scene loaded teleport the player to the spawn point 
        //Unload this scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
    private IEnumerator unFadeFromBlack()
    {
        yield return new WaitForEndOfFrame(); 
    }

    private void findSpawnPoint()
    {
        SceneTeleporter[] sceneTeleporter = GameObject.FindObjectsByType<SceneTeleporter>(FindObjectsSortMode.None);
        Transform spawnPoint;

        foreach (SceneTeleporter spawnPoints in sceneTeleporter)
        {
            if (spawnPoints.NAMESCENEOBJECTISIN == sceneName)
            {
                Debug.Log("In right scene");
                if (spawnPoints.gameObject.name == destination)
                {
                    Debug.unityLogger.Log("Found destination");
                    spawnPoint = spawnPoints.gameObject.transform;;
                }
            }
        }
    }
}


