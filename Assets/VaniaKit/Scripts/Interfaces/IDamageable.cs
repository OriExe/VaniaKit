using UnityEngine;

public interface IDamageable
{
   void OnHit(int  damage = 0, bool isCritical = false);
}
