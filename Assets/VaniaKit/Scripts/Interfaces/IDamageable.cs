using UnityEngine;

public interface IDamageable
{
   public enum Direction
   {
      up,
      down,
      left,
      right,
      none
   };
   void OnHit(int  damage = 0, bool isCritical = false, float cooldownPeriod = 0f, Direction directionOfAttack = Direction.none);
}

namespace Vaniakit.Collections
{
   public static class DamageFunctions
   {
      public static IDamageable.Direction WhereAttackTookPlace(Vector3 Attacker, Vector3 Victim) //Returns the direction the victim was attacked
      {
         if (Attacker.x < Victim.x)
         {
            return IDamageable.Direction.left;
         }
         else if (Attacker.x > Victim.x)
         {
            return IDamageable.Direction.right;
         }

         return IDamageable.Direction.none;
      }
   }
   
}
