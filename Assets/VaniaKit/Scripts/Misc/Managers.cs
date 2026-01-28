using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace VaniaKit.Manager
{
    public class Managers : MonoBehaviour
    {
        public static Managers instance;
        public static Image fadePanel;

        [SerializeField] private GameObject uiCanvas;
        [SerializeField] private Image fadePanelObj;

        public float timetakenToFade;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);      
            }
            else
            {
                Debug.Log("There is more than one Manager system, Destroying other managers!");
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

        private void createAFadePanel()
        {
            if (fadePanel != null) return; //Returns if not empty
            
            if (fadePanelObj == null)
            {
                fadePanelObj = Resources.Load<GameObject>("Vaniakit/FadePanel").GetComponent<Image>(); //Finds the default panel in the vaniakit resources folder
            }
            fadePanel = Instantiate(fadePanelObj, uiCanvas.transform); //
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
    
    
}

