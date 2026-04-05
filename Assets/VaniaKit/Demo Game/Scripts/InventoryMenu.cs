using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Vaniakit.ResourceManager;

public class InventoryMenu : MonoBehaviour
{
    private InputAction m_OpenInventoryButton;
    [SerializeField]private GameObject inventoryMenu;
    [SerializeField]private TMP_Text text;

    private void Start()
    {
        m_OpenInventoryButton = InputSystem.actions.FindAction("OpenInventory");
    }

    private void Update()
    {
        if (m_OpenInventoryButton.WasPressedThisFrame())
        {
            inventoryMenu.SetActive(!inventoryMenu.activeInHierarchy);
            PlayerController.playerControllerEnabled(!inventoryMenu.activeInHierarchy);
            if (!inventoryMenu.activeInHierarchy)
                return;
            text.text = "";
            foreach (var item in Inventory.GetAllItems())
            {
                text.text += item.item.GetName();
                text.text += "\n";
                text.text += item.item.GetDescription();
                text.text += "\n";
            }
        }
    }
}
