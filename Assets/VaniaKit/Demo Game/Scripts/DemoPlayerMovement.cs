using UnityEngine;


public class DemoPlayerMovement : Vaniakit.Player.PlayerMovement
{
    [SerializeField] private AudioSource walkSound;

    protected override void onPlayerMove(lookStatesHorizontal direction)
    {
        walkSound.Play();
    }
}
