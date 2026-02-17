using UnityEngine;
using Vaniakit.FastTravelSystem;

public class FastInteract : MonoBehaviour,IInteractable
{
    public void onInteract()
    {
        Debug.Log("Interacted");
        FastTravelSystem.teleportToPoint(FastTravelSystem.allPoints[1]);
    }
}
