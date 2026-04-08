using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Map;


/// <summary>
/// Uses show text tool to show details
/// </summary>
public class CheckpointTriggerWithInteract : Checkpoint,IInteractable
{
    public void onInteract()
    {
        TriggerCheckPoint();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ShowTextTool.showText("Press E to interact with Checkpoint");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ShowTextTool.hideText();
        }
    }
}
