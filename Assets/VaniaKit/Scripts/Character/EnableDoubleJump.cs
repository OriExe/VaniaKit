using UnityEngine;

namespace Vaniakit.Items
{
    public class EnableDoubleJump : MonoBehaviour, IEquipable
    {
        public void Equip()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Vaniakit.Player.PlayerJump>().EnableDoubleJump(true);
        }

        public void Unequip()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Vaniakit.Player.PlayerJump>().EnableDoubleJump(false);
        }
    }
}
/// <summary>
/// An item that can be stored in your inventory that enables and disables the double jump 
/// </summary>

