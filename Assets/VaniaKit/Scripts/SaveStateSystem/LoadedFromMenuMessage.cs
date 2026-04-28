using UnityEngine;

namespace Vaniakit.SaveSystem
{
    public class LoadedFromMenuMessage
    {
        private static bool loadedFromMenu = false;
        public static firstLevelRoom firstLevelRoomID {get; private set;}
        /// <summary>
        /// Tells the game that the player loaded the game from the main menu
        /// </summary>
        public static void gameLoadedFromMenu()
        {
            Debug.Log("Game Loaded From Menu message recieved");
            loadedFromMenu = true;
        }

        public static void createRoomID()
        {
            if (firstLevelRoomID == null)
                firstLevelRoomID = new firstLevelRoom();
        }
        /// <summary>
        /// Has the game been loaded from the menu
        /// If true the game should load the scene the player last was
        /// This is so the player doesn't load to another scene if there're debugging the game within the editor
        /// </summary>
        /// <returns></returns>
        public static bool hasGameLoadedFromMenu()
        {
            if (loadedFromMenu)
            {
                loadedFromMenu = false;
                return true;
            }
            return false;
        }
    }

    public class firstLevelRoom
    {
        /*
        READ THIS
        Edit these values with the names of the first scene you want to spawn to 
        And the name of the game object you want to spawn to 
        */
        public string sceneName = "DemoLevel1";
        public string gameObjectName = "SpawnPoint";
        
    }
}