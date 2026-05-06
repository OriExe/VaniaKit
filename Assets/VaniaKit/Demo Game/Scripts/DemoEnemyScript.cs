using System;
using UnityEngine;
using Vaniakit.Ai;

public class DemoEnemyScript : PatrolEnemyAi
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
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

    protected override void OnReachedPatrolPoint(int index)
    {
        animator.SetBool("IsMoving", false);
        base.OnReachedPatrolPoint(index);
    }

    protected override void switchPointToPatrol()
    {
        animator.SetBool("IsMoving", true);
        if (LookingDirection == Vector2.right)
        {

            sprite.flipX = true;
        }
        else if (LookingDirection == Vector2.left)
        {
            sprite.flipX = false;
        }
        base.switchPointToPatrol();
    }
}
