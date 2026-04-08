using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.FastTravelSystem;

public class FastTravelPoints : FastTravelPoint
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!FastTravelSystem.doesPointInArrayExst(pointName))
            {
                ShowTextTool.showText("Press E to unlock point");
            }
            else
            {
                ShowTextTool.showText("Press E to open Fast Travel Menu");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            ShowTextTool.hideText();
    }

    protected override void onPlayerUnLockedTravelPoint()
    {
        ShowTextTool.showText("Press E to open Fast Travel Menu ");
    }
    
    
}
