using UnityEngine;



public class DemoPlayerJump : Vaniakit.Player.PlayerJump
{
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private Animator animator;
        protected override void onPlayerJump()
        {
                jumpSound.Play();
                animator.SetTrigger("isJumpStart");
        }

        protected override void onPlayerReleaseJump()
        {
                animator.SetBool("isFalling",true);
        }

        protected override void onPlayerLand()
        {
                animator.SetBool("isFalling",false);
                base.onPlayerLand();
        }

        protected override void onPlayerDoubleJump()
        {
                animator.SetTrigger("isDoubleJump");
        }
}
