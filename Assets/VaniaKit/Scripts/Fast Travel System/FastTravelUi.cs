using System;
using UnityEngine;
using Vaniakit.Collections;

namespace Vaniakit.FastTravelSystem
{
    public class FastTravelUi : MonoBehaviour
    {
        public static FastTravelUi instance;
        [SerializeField] private GameObject fastTravelButtonPrefab;
        [SerializeField]private GameObject buttonHolderParent;
        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("More than one instance of FastTravelUi exists");
                Destroy(gameObject);
            }

            if (buttonHolderParent == null)
            {
                buttonHolderParent = gameObject;
            }
        }

        private void OnEnable()
        {
            //Removes all previous children
            foreach (Transform child in buttonHolderParent.transform)
            {
                Destroy(child.gameObject);
            }
            
            //Creates buttons and sets them as a point
            foreach (FastTravelData data in FastTravelSystem.allActivePoints)
            {
                var button =  Instantiate(fastTravelButtonPrefab, buttonHolderParent.transform);
                button.GetComponent<SelectFastTravelPoint>().setName(data.PointName);
            }
            
        }
    }
}