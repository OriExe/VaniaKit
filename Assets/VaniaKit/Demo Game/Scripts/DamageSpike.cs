using UnityEngine;

public class DamageSpike : MonoBehaviour,IDamageable
{
    public void OnHit(int damage = 0, bool isCritical = false, float cooldownPeriod = 0,
        IDamageable.Direction directionOfAttack = IDamageable.Direction.none)
    {
        Vaniakit.Player.PlayerController.getPlayerRigidbody().AddForce(new Vector2(0f, 13f), ForceMode2D.Impulse);
    }
}
