using System;
using TMPro;
using UnityEngine;
using Vaniakit.FastTravelSystem;

namespace Vaniakit.Collections
{
    /// <summary>
    /// Script for creating buttons in the main fast travel ui
    /// </summary>
    public class SelectFastTravelPoint : MonoBehaviour
    {
        private string pointName;
       [SerializeField] private TMP_Text buttonText;
       
       protected virtual void vkStart()
       {
            
       }

       protected virtual void vkUpdate()
       {
            
       }

       private void Start()
       {
           vkStart();
       }

       private void Update()
       {
           vkUpdate();
       }
       
       public void setName(string name)
       {
           pointName = name;
           buttonText.text = name;
       }
       /// <summary>
       /// Teleports player to that point
       /// </summary>
       public void buttonPressed()
       {
           FastTravelSystem.FastTravelSystem.teleportToPoint(FastTravelSystem.FastTravelSystem.findPointInArray(pointName));
           FastTravelUi.instance.gameObject.SetActive(false);
       }
    }
}