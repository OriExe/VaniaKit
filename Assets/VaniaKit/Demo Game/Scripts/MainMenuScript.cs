using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vaniakit.Json;
using Vaniakit.SaveSystem;

public class MainMenuScript : MonoBehaviour
{
    private const string saveFileName = "DemoSave"; 
    [SerializeField] private GameObject buttonParent; //parent that holds all the buttons
    [Tooltip("Scene name load when no save data exists (so the first level)")]
    private bool[] sessionExists; //An array that stores all the saved sessions the player has
    [Tooltip("Enter the name of the scene where all the player data and manager data is stored")]
    [SerializeField]private string gameLoadingScene;
    
    private void Start() 
    {
        sessionExists = new bool[buttonParent.transform.childCount]; 
        int counter = 1;
        foreach (Transform child in buttonParent.transform) //Checks what saveData is available for each game
        {
            saveData loadedData;
            if (JsonInstructions.loadJsonArray(saveFileName + counter.ToString() + ".json", out loadedData))
            {
                print("DataExists");
                child.GetComponentInChildren<TMP_Text>().text = "SaveFile";
                sessionExists[counter-1] = true;
            }
            else
            {
                print("DataDoesntExist");
                sessionExists[counter-1] = false;
            }
            counter++;
        }
    }

    /// <summary>
    /// Triggered when button in menu pressed
    /// </summary>
    /// <param name="bNum"></param>
    public void startGameButtonPressed(int bNum)
    {
        //Changes name of the saveFile to load and save data so that data is stored
        switch (bNum) //Which button was pressed
        {
            case 0:
                SaveSystem.dataFileName = saveFileName + "1" + ".json"; 
                break;
            case 1:
                SaveSystem.dataFileName = saveFileName + "2" + ".json";
                break;
            case 2:
                SaveSystem.dataFileName = saveFileName + "3" + ".json";
                break;
            case 3:
                SaveSystem.dataFileName = saveFileName + "4" + ".json";
                break;
        }
        
        LoadedFromMenuMessage.gameLoadedFromMenu();
        if (sessionExists[bNum])
        {
            loadGame();
        }
        else
        {
            loadNewGame();
        }
    }
    /// <summary>
    /// Loads an already saved game
    /// </summary>
    private void loadGame()
    {
        SceneManager.LoadScene(gameLoadingScene);
    }

    /// <summary>
    /// Loads a completly new game
    /// </summary>
    public void loadNewGame()
    {
        SceneManager.LoadScene(gameLoadingScene);
    }
    
    
}
