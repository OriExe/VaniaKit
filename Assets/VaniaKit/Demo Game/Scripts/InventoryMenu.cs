using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryMenu : MonoBehaviour
{
    private InputAction m_OpenInventoryButton;
    private GameObject inventoryMenu;
    private void Update()
    {
        if (m_OpenInventoryButton.WasPressedThisFrame())
        {
            inventoryMenu.SetActive(!inventoryMenu.activeInHierarchy);
            
        }
    }
}
