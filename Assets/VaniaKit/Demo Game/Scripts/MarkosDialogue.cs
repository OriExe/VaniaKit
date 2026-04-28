using System;
using TMPro;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.DialogueSystem;
using Vaniakit.Events;
using Vaniakit.Player;
using Vaniakit.ResourceManager;

public class MarkosDialogue : AiDialogue, IInteractable
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text whoIsTalkingText;
    [SerializeField] private InventorySlot walletItem;
    [SerializeField] private InventorySlot doubleJumpItem;

    private void Start()
    {
        if (EventManager.hasEventBeenTriggeredBefore("TalkedToMarkos"))
        {
            dialogueListIndex = 3;
        }
    }

    public void onInteract()
    {
        ShowTextTool.hideText();
        triggerDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ShowTextTool.showText("Press E to talk To Markos");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ShowTextTool.hideText();
    }

    protected override void onDialogueChangedTo(DialogueData dialogueData)
    {
        dialogueText.text = dialogueData.dialogue;
        if (dialogueData.speakingCharacterIndex == 0) //Markos
        {
            whoIsTalkingText.text = "Markos";
        }
        else if (dialogueData.speakingCharacterIndex == 1) //Player
        {
            whoIsTalkingText.text = "Player";
        }
    }

    protected override void onDialogueStart()
    {
        //Disable controller and enable ui panel
        uiPanel.SetActive(true);
        PlayerController.playerControllerEnabled(false); 
        if (Inventory.GetAllItems().Contains(walletItem)) //Check if the player has the correct item in their inventory and remove it
        {
            dialogueListIndex = 2;
            Inventory.removeItemFromInventory(walletItem);
            
        }
    }

    protected override void onDialogueEnd()
    {
        //Re enables player controller and disables the dialogue popup
        uiPanel.SetActive(false); 
        PlayerController.playerControllerEnabled(true);
        if (dialogueListIndex == 0) //First convo completed
        {
            dialogueListIndex = 1;
        }

        if (dialogueListIndex == 2) //Triggered when the player has the item. Gives them the double jump 
        {
            Inventory.addItemToInventory(doubleJumpItem);
            doubleJumpItem.item.actionScript.GetComponent<IEquipable>().Equip();
            EventManager.saveEvent("TalkedToMarkos");
            dialogueListIndex = 3;
        }
    }
}
