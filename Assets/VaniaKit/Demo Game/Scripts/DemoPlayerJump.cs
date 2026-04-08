using UnityEngine;



public class DemoPlayerJump : Vaniakit.Player.PlayerJump
{
        [SerializeField] private AudioSource jumpSound;
        protected override void onPlayerJump()
        {
                jumpSound.Play();
        }
}
