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
