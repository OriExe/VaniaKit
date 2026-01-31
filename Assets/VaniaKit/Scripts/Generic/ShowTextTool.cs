using System;
using TMPro;
using UnityEngine;

namespace Vaniakit.Collections
{
    /// <summary>
    /// Used for making Ui elements such as press e to Interact
    /// </summary>
    public class ShowTextTool : MonoBehaviour
    {
        private static ShowTextTool instance;
        [SerializeField]private TMP_Text textComponent;
        [Tooltip("Does the text component have a parent such as the parent time \n Make true if you want to hide the button as well ")]
        private void Start()
        {
            if (instance == null)
                instance = this;
            else
            {
                Debug.Log("There are 2 instances of ShowTextTool, only one is needed");
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }
        
        public static void showText(string textToShow)
        {
            try
            {
                instance.textComponent.text = textToShow;
                instance.gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogWarning("There might not be a static instance of ShowTextTool, Did you enable it before starting the game?");
            }
            
        }
    
        public static void hideText()
        {
            try
            {
                instance.gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogWarning("There might not be a static instance of ShowTextTool, Did you enable it before starting the game?");
            }
        }
    }
    
   
    
}
