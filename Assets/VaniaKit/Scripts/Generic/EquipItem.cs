using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Vaniakit.ResourceManager;

namespace Vaniakit.Collections
{
    /// <summary>
    /// A scripts that lets players equip items.
    /// Uses the ShowTextTool in Vaniakit.Collections
    /// </summary>
    public class EquipItem : ItemHolder,IInteractable
    {
        [SerializeField] private string showText;
        private bool playerNearby;
        [SerializeField] private bool itemCanBeEquipped = true;
        private InputAction m_InteractAction;
        
        protected virtual void onPlayerPickedUpItem()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ShowTextTool.showText(showText);
                playerNearby = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ShowTextTool.hideText(); 
                playerNearby = false;
            }
        }

        public void onInteract()
        {
            givePlayerItem(itemCanBeEquipped);
            onPlayerPickedUpItem();
        }
    }
}

