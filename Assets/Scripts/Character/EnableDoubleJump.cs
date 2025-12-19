using UnityEngine;

/// <summary>
/// An item that can be stored in your inventory that enables and disables the double jump 
/// </summary>
public class EnableDoubleJump : MonoBehaviour, IEquipable
{
    public void Equip()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player.PlayerJump>().EnableDoubleJump(true);
    }

    public void Unequip()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player.PlayerJump>().EnableDoubleJump(false);
    }
}
