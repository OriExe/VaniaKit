using System;
using UnityEngine;
using Vaniakit.DialogueSystem;
public class interactDialogueScript : AiDialogue, IInteractable
{
    public void onInteract()
    {
        triggerDialogue();
    }
}
