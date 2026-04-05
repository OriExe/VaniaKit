using System;
using UnityEngine;
using Vaniakit.Collections;
using Vaniakit.Map;
using Vaniakit.Player;
using Vaniakit.SaveSystem;

public class PlayerController : Vaniakit.Player.PlayerController
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float force = 12f;
    private bool deadEventInvoked;

    protected override void onPLayerHit(int damage = 0, IDamageable.Direction direction = IDamageable.Direction.none)
    {
        spriteRenderer.color = Color.red;
        if (direction == IDamageable.Direction.left)
        {
            getPlayerRigidbody().AddForce(new Vector2(force, 3f), ForceMode2D.Impulse);
        }
        else if (direction == IDamageable.Direction.right)
        {
            getPlayerRigidbody().AddForce(new Vector2(-force, 3f), ForceMode2D.Impulse);
        }
    }
    
    protected override void onPlayerHitCritical(int damage = 0, IDamageable.Direction direction = IDamageable.Direction.none)
    {
        onPLayerHit(damage);
        TeleportToNearestCheckpoint.TeleportPlayerToNearestCheckpoint(gameObject.transform);
    }

    protected override void onPlayerCanTakeDamage()
    {
        spriteRenderer.color = Color.white; 
    }

    private void Start()
    {
        onPlayerDead += playerDead; //Subscribes to player dead event
    }

    /// <summary>
    /// Calls when player dies, it stops the player moving and makes them fly
    /// </summary>
    private void playerDead()
    {
        playerControllerEnabled(false);
        Camera.main.GetComponent<PlayerCamera>().enabled = false;
        getPlayerRigidbody().linearVelocity = new Vector3(0f, 25f, 0f);
        if (!deadEventInvoked)
            Invoke("respawn", 3f);
        deadEventInvoked = true;
    }

    private void respawn() //This could all be a single vaniakit script rather than a demo script
    {
        playerControllerEnabled(true);
        Camera.main.GetComponent<PlayerCamera>().enabled = true;
        getPlayerRigidbody().linearVelocity = new Vector3(0f, 0f, 0f);
        deadEventInvoked = false;
        restoreHealthToNormal();
        if (Vaniakit.Map.Checkpoint.activeCheckPointData == null)
        {
            FadeInManager.instance.FadeToBlack(LoadedFromMenuMessage.firstLevelRoomID.sceneName,LoadedFromMenuMessage.firstLevelRoomID.gameObjectName);
        }
        else
        {
            FadeInManager.instance.FadeToBlack(Checkpoint.activeCheckPointData.sceneName,Checkpoint.activeCheckPointData.gameObjectName);
        }
    }
}
