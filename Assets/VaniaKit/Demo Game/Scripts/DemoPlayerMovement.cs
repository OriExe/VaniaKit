using System;
using UnityEngine;


public class DemoPlayerMovement : Vaniakit.Player.PlayerMovement
{
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    protected override void onPlayerMove(lookStatesHorizontal direction)
    {
        walkSound.Play();
        animator.SetBool("isWalking", true);
        switch (direction)
        {
            case lookStatesHorizontal.left:
                sprite.flipX = true;
                break;
            case lookStatesHorizontal.right:
                sprite.flipX = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    protected override void onPlayerIdle()
    {
        animator.SetBool("isWalking", false);
    }
}
