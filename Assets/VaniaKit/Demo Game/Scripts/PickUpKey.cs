using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Vaniakit.Collections;
using Vaniakit.Events;

public class PickUpItem : EquipItem
{
    [SerializeField] private string EventName; 
    private void Start()
    {
        if (EventManager.hasEventBeenTriggeredBefore(EventName))
        {
            Destroy(gameObject);
        }
    }

    protected override void onPlayerPickedUpItem()
    {
        EventManager.saveEvent(EventName);
        Destroy(gameObject);
    }
}
