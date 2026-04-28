using System;
using UnityEngine;
using Vaniakit.Events;

public class DestroyWallScript : MonoBehaviour,IDamageable
{
    
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private float health = 10;

    private void Start()
    {
        if (EventManager.hasEventBeenTriggeredBefore("WallDestroyed"))
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Makes the wall take damamage, if at 0 it breaks
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isCritical"></param>
    /// <param name="cooldownPeriod"></param>
    /// <param name="directionOfAttack"></param>
    public void OnHit(int damage = 0, bool isCritical = false, float cooldownPeriod = 0,
        IDamageable.Direction directionOfAttack = IDamageable.Direction.none)
    {
        foreach (ParticleSystem p in particles)
        {
            p.Play();
            health -= damage;
        }

        if (health <= 0)
        {
            EventManager.saveEvent("WallDestroyed");
            Destroy(gameObject);
        }
    }
}
