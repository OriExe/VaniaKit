using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Vaniakit.Json
{
    public static class JsonInstructions
    {
        
        /// <summary>
        /// Function for saving json arrays in persistentDataPath
        /// </summary>
        /// <param name="nameOfFile"></param>
        public static void saveAsJsonArray<T>(T ArrayToSave, string nameOfFile)
        {
            string json = "";
            string filePath = Application.persistentDataPath + nameOfFile;   //File path that can be updated if you want to update it to a differentPath
            
            json = JsonUtility.ToJson(ArrayToSave, true); //Saves the json in a
            File.WriteAllText(filePath, json);
            Debug.Log(json);
        }
        
        /// <summary>
        /// Tries to load a json file of type
        /// </summary>
        /// <param name="nameOfFile"></param>
        /// <param name="ArrayThatLoaded"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns false if it fails</returns>
        public static bool loadJsonArray<T>(string nameOfFile, out T ArrayThatLoaded)
        {
            string filePath = Application.persistentDataPath + nameOfFile;
            if (File.Exists(filePath))
            {
                Debug.Log("File Exists");
                string json = File.ReadAllText(filePath);
                ArrayThatLoaded = JsonUtility.FromJson<T>(json); //Loads the file and returns it as ArrayThatLoaded
                return true;
            }
            else
            {
                ArrayThatLoaded = default(T); //Returns false as there is no file to load
                return false;
            }
        }
    }
}