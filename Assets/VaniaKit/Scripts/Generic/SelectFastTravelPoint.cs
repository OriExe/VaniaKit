using TMPro;
using UnityEngine;
using Vaniakit.FastTravelSystem;

namespace Vaniakit.Collections
{
    public class SelectFastTravelPoint : MonoBehaviour
    {
        private string pointName;
       [SerializeField] private TMP_Text buttonText;
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