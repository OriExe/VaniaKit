using System;
using UnityEngine;
using Vaniakit.Ai;

public class DemoEnemyScript : PatrolEnemyAi
{
    [SerializeField] private AudioSource deathSound;
    protected override void OnDeath()
    {
        deathSound.Play();
    }
}
