using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Map;

public class MapEvents : MapManagementEvents
{
   public override void onRoomFullyLoaded()
   {
      TeleportToNearestCheckpoint.findAllCheckPointsInScene();
   }
}
