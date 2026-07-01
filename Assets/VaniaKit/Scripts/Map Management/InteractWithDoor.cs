using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Map;
using Vaniakit.Misc;
using Vaniakit.ResourceManager;

public class InteractWithDoor : ATeleporterMonoBehaviour,IInteractable
{
    [Tooltip("Put the key that opens the door here")]
    [SerializeField]private InventorySlot key;

    [SerializeField] private string Destination;
    [SerializeField] private string DestinationSceneName;

    [SerializeField] private string lockedMessage = "You'll need a key";
    [SerializeField] private string notLockedMessage = "Press E to enter the door";
    // Shows text on the player's screen

    #region Events
    protected virtual void onPlayerInteractsWithKey()
    {
        Debug.Log("Player Interacted with key");
    }

    protected virtual void onPlayerInteractsWithoutKey()
    {
        Debug.Log("Player Interacted without key");
    }
    protected virtual void onPlayerLoadedHere()
    {
        Debug.Log("Player Loaded Here");
    }
    #endregion
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && Inventory.GetAllItems().Contains(key))
        {
            ShowTextTool.showText(notLockedMessage);
        }
        else if (other.tag == "Player")
        {
            ShowTextTool.showText(lockedMessage);
        }
    }
    //Hides text from the player screen
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ShowTextTool.hideText();
        }
    }

    /// <summary>
    /// If the player has the correct key the door will open
    /// </summary>
    public void onInteract()
    {
        if (Inventory.GetAllItems().Contains(key))
        {
            onPlayerInteractsWithKey();
            FadeInManager.instance.StartCoroutine(FadeInManager.instance.FadeToBlack(DestinationSceneName, Destination));
        }
        else
        {
            onPlayerInteractsWithoutKey();
        }
    }

    /// <summary>
    /// Runs when someone teleports at the door 
    /// </summary>
    /// <param name="gameObjectName"></param>
    /// <returns></returns>
    public override bool amITheRightObject(string gameObjectName)
    {
        if (gameObjectName == gameObject.name)
        {
            onPlayerLoadedHere();
            //justTeleported = true;
            return true;
        }
        return false;
    }
}
