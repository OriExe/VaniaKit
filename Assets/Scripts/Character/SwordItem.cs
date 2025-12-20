using System;
using ResourceManager;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

public class SwordItem : MonoBehaviour, IEquipable
{
    [SerializeField] private float damage;

    [SerializeField] private float range;

    [SerializeField] private float secondsToWaitTillNextAttack;
    private float attackingCooldown;
    private InputAction m_attack;
    
    [SerializeField] private GameObject attackingBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_attack = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        attackingCooldown -= Time.deltaTime;
        if (attackingCooldown <= 0)
        {
            attackingCooldown = 0;
            if (m_attack.IsPressed())
            {
                attackingBox.SetActive(true);
                attackingCooldown = secondsToWaitTillNextAttack;

                switch (PlayerMovement.returnVerticalLookState())
                {
                    case PlayerMovement.lookStatesVertical.up:
                        attackingBox.transform.localPosition = new Vector3(transform.localPosition.x,1f,transform.localPosition.z);
                        break;
                    case PlayerMovement.lookStatesVertical.down:
                        attackingBox.transform.localPosition = new Vector3(transform.localPosition.x,-1f,transform.localPosition.z);
                        break;
                    case PlayerMovement.lookStatesVertical.none:
                        switch (PlayerMovement.returnHorizontalLookState())
                        {
                            case PlayerMovement.lookStatesHorizontal.left:
                                attackingBox.transform.localPosition = new Vector3(-1f,transform.localPosition.y,transform.localPosition.z);
                                break;
                            case PlayerMovement.lookStatesHorizontal.right:
                                attackingBox.transform.localPosition = new Vector3(1f,transform.localPosition.y,transform.localPosition.z);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    /// <summary>
    /// This will add the weapon as a seperate object, making it able for the player to attack
    /// </summary>
    public void Equip()
    {
        Instantiate(gameObject, new Vector3(0,0,0), Quaternion.identity, GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void Unequip()
    {
        Destroy(gameObject);
    }
}
