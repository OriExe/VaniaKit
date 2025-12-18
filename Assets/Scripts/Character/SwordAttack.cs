using System;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float howLongAttackBoxAppears = 0.7f;
    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void OnEnable()
    {
        Invoke(nameof(disableSword), howLongAttackBoxAppears);
    }

    void disableSword()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.OnHit();
        }
    }
}
