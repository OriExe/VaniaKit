using System;
using UnityEngine;
using VaniaKit.DialogueSystem;
public class interactDialogueScript : AiDialogue, IInteractable
{
    public void onInteract()
    {
        triggerDialogue();
    }
}
