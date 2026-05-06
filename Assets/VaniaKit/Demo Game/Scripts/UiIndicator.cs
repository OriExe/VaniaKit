using System;
using TMPro;
using UnityEngine;
using Vaniakit.Events;

public class UiIndicator : MonoBehaviour
{
  private static UiIndicator instance;
  [SerializeField] private Animator animator;
  [SerializeField] private TMP_Text text;
  [SerializeField] private int amountFound;
  private void Start()
  {
    instance = this;
    if (EventManager.hasEventBeenTriggeredBefore("1fasttravelpoints"))
    {
      amountFound = 1;
      text.text = "Find Fast Travel Points 1/2";
    }
    else if (EventManager.hasEventBeenTriggeredBefore("2fasttravelpoints"))
    {
      amountFound = 2;
      text.text = "Find Fast Travel Points 2/2";
    }
  }

  public static void increaseAmountFound()
  {
    instance.amountFound++;
    instance.text.text = "Find Fast Travel Points " + instance.amountFound.ToString() + "/2";
    instance.animator.SetTrigger("PointFound");
    if (instance.amountFound == 1)
    {
      EventManager.saveEvent("1fasttravelpoints");
    }
    else if (instance.amountFound == 2)
    {
      EventManager.saveEvent("2fasttravelpoints");
    }
  }
}
