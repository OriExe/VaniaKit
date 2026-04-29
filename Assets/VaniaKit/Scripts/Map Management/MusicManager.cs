using System;
using UnityEngine;

namespace Vaniakit.Map
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _instance;
        [SerializeField] private Music[] allSongs;
        private Music currentAreaType;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Debug.LogWarning("Another instance already exists");
                Destroy(gameObject);
            }
        }

        public static void sceneLoaded(string sceneName) //When new scene has loaded
        {
            if (_instance == null)
                return;
            bool songFound = false;
            foreach (Music areatype in _instance.allSongs) //Looks through all song types that can be played
            {
                if (songFound) //Won't loop again if song found
                    break;
                foreach (string sceneListed in areatype.sceneNames) //Loops through all scenes where a certain song should play
                {
                    if (sceneListed == sceneName) //Is the scene listed the current scene
                    {
                        Debug.Log("Scene type is of loaded scene is " + areatype.areaType);
                        songFound = true;
                        if (areatype == _instance.currentAreaType) //Music has already loaded
                        {
                            Debug.Log("This Area type is already being played, nothing will be done");
                        }
                        else //Play a new song
                        {
                            _instance.currentAreaType = areatype;
                            try //Tries to change the song and play it 
                            {
                                _instance.audioSource.clip = areatype.musicToPlay;
                                _instance.audioSource.Play();
                            }
                            catch (Exception e)
                            {
                                Debug.Log("Not a valid song, must be null");
                                pauseMusic();
                            }
                            
                        }
                        break;
                    }
                }

                if (!songFound) //If the scene is not listed play no song
                {
                    _instance.audioSource.Pause();
                }
            }
        }
        public static void pauseMusic()
        {
            if (_instance == null)
                return;
            _instance.audioSource.Pause();
        }
    }
    
   

    [System.Serializable]
    public class Music
    {
        [Tooltip("What part of the map should this song play")]
        public string areaType;
        public string[] sceneNames;
        public AudioClip musicToPlay;
    }
}

