using UnityEngine;
using Vaniakit.Events;
public class SwordEquipScript : Vaniakit.Collections.EquipItem
{
    
    /// <summary>
    /// Add a void start that checks if a sword event has been triggered
    /// </summary>
    protected override void onPlayerPickedUpItem()
    {
        EventManager.saveEvent("SwordCollected");
        Destroy(transform.parent.gameObject);
    }
}
