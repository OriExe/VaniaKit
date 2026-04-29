using System;
using UnityEngine;
using Vaniakit.Ai;

public class DemoEnemyScript : PatrolEnemyAi
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private SpriteRenderer sprite;
    protected override void OnDeath()
    {
        deathSound.Play();
    }
    protected override void OnTakenDamage()
    {
        sprite.color = Color.red;
        Invoke("changetoWhite", 1.2f);
    }

    void changetoWhite()
    {
        sprite.color = Color.white;
    }
}
