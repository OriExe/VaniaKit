using UnityEngine;
using Vaniakit.Map;

public class CheckpointTriggerWithInteract : Checkpoint,IInteractable
{
    public void onInteract()
    {
        TriggerCheckPoint();
    }
}
