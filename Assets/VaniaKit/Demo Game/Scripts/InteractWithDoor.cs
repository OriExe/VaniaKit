using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Map;
using Vaniakit.Misc;
using Vaniakit.ResourceManager;

public class InteractWithDoor : ATeleporterMonoBehaviour,IInteractable
{
    [SerializeField]private InventorySlot key;

   [SerializeField] private string Destination;
    [SerializeField] private string DestinationSceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && Inventory.GetAllItems().Contains(key))
        {
            ShowTextTool.showText("Press E to enter the door");
        }
        else if (other.tag == "Player")
        {
            ShowTextTool.showText("You'll need a key");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ShowTextTool.hideText();
        }
    }

    public void onInteract()
    {
        if (Inventory.GetAllItems().Contains(key))
        {
            Debug.Log("Key found");
            FadeInManager.instance.StartCoroutine(FadeInManager.instance.FadeToBlack(DestinationSceneName, Destination));
        }
    }

    public override bool amITheRightObject(string gameObjectName)
    {
        if (gameObjectName == gameObject.name)
        {
            //onPlayerLoadedHere();
            //justTeleported = true;
            return true;
        }
        return false;
    }
}
