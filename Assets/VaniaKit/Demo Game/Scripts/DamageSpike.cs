using System;
using UnityEngine;
using Vaniakit.Collections;

public class DamageSpike : MonoBehaviour,IDamageable
{
    private float timeOut;
    public void OnHit(int damage = 0, bool isCritical = false, float cooldownPeriod = 0,
        IDamageable.Direction directionOfAttack = IDamageable.Direction.none)
    {
        if (timeOut > 1)
        {
            timeOut = 0;
            Vaniakit.Player.PlayerController.getPlayerRigidbody().AddForce(new Vector2(0f, 13f), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        timeOut += Time.deltaTime;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ShowTextTool.showText("Tip: You can attack the spike downwards to fly up ");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ShowTextTool.hideText();
    }
}
