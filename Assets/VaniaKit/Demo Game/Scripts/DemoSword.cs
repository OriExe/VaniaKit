using System;
using UnityEngine;
using Vaniakit.Items;

public class DemoSword : SwordItem
{
   [SerializeField] private Animator animator;

   private void Start()
   {
      animator = GetComponentInParent<PlayerController>().returnAnimator();
   }

   protected override void onPlayerAttack()
   {
      animator.SetTrigger("isAttack1");
   }
}
