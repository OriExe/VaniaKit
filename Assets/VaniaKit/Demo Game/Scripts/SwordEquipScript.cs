using System;
using UnityEngine;
using Vaniakit.Events;
public class SwordEquipScript : Vaniakit.Collections.EquipItem
{
    private void Start() //Needs to be changed soon
    {
        if (EventManager.hasEventBeenTriggeredBefore("SwordCollected"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Add a void start that checks if a sword event has been triggered I need like VaStart 
    /// </summary>
    protected override void onPlayerPickedUpItem()
    {
        EventManager.saveEvent("SwordCollected");
        Destroy(transform.parent.gameObject);
    }
}
