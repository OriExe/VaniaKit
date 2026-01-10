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
    private Scene scene;
    
    /// <summary>
    /// Makes sure the box colider is a trigger
    /// </summary>
    private void Start()
    {
        scene = SceneManager.GetSceneByName(sceneName);
        GetComponent<BoxCollider2D>().isTrigger = true;       
    }

    /// <summary>
    /// Teleports the player to anothe scene
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Fade to black
        //Load scene async 
        //If scene loaded teleport the player to the spawn point 
        //Unload this scene
    }

    private IEnumerator FadeToBlack()
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator unFadeFromBlack()
    {
        yield return new WaitForEndOfFrame(); 
    }
}


