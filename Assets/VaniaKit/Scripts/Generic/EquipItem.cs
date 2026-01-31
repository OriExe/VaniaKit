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
    public class EquipItem : ItemHolder
    {
        [SerializeField] private string showText;
        private bool playerNearby;
        private InputAction m_InteractAction;
        private void Start()
        {
            m_InteractAction = InputSystem.actions.FindAction("Interact");
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

        private void Update()
        {
            if (playerNearby)
            {
                if (m_InteractAction.WasPressedThisFrame())
                {
                    givePlayerItem(true);
                    Destroy(gameObject);
                }
            }
        }
    }
}

