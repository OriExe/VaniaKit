using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Items;
using Vaniakit.Player;

public class PlayerSword : SwordAttack
{
   [SerializeField] private float pogoForce;
   protected override void onDamageableComponentHit()
   {
      
      switch (PlayerMovement.returnVerticalLookState())
      {
         case PlayerMovement.lookStatesVertical.up:
            break;
         case PlayerMovement.lookStatesVertical.down:
            Vaniakit.Player.PlayerController.getPlayerRigidbody().AddForce(new Vector2(0f, pogoForce),ForceMode2D.Impulse);
            break;
         case PlayerMovement.lookStatesVertical.none:
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
}
