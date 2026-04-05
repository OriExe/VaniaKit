using System;
using TMPro;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.DialogueSystem;
using Vaniakit.Player;
using Vaniakit.ResourceManager;

public class MarkosDialogue : AiDialogue, IInteractable
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text whoIsTalkingText;
    [SerializeField] private InventorySlot walletItem;
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
        uiPanel.SetActive(true);
        PlayerController.playerControllerEnabled(false);
        if (Inventory.GetAllItems().Contains(walletItem))
        {
            dialogueListIndex = 2;
            Inventory.removeItemFromInventory(walletItem);
            
        }
    }

    protected override void onDialogueEnd()
    {
        uiPanel.SetActive(false);
        PlayerController.playerControllerEnabled(true);
        if (dialogueListIndex == 0)
        {
            dialogueListIndex = 1;
        }
    }
}
