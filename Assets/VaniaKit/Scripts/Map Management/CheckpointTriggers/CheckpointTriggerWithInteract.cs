using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Events;
using Vaniakit.Map;


/// <summary>
/// Uses show text tool to show details
/// </summary>
public class CheckpointTriggerWithInteract : Checkpoint,IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string checkpointActivatedAnimationTrigger;
    [SerializeField] private string AnimationTrigger;

    private void Start()
    {
        if (EventManager.hasEventBeenTriggeredBefore("CheckPointActivated"))
        {
            animator.SetTrigger(AnimationTrigger);
        }
    }

    protected override void onPlayerActivatedCheckpoint()
    {
        EventManager.saveEvent("CheckPointActivated");
        animator.SetTrigger(checkpointActivatedAnimationTrigger);
    }

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
