using System;
using Vaniakit.ResourceManager;
using UnityEngine;
using UnityEngine.InputSystem;
using Vaniakit.Player;

namespace Vaniakit.Items
{
    public class SwordItem : MonoBehaviour, IEquipable
    {
        [SerializeField] private float damage;

        [SerializeField] private float range;

        [SerializeField] private float secondsToWaitTillNextAttack;
        private float attackingCooldown;
        private InputAction m_attack;
        
        [SerializeField] private GameObject attackingBox;

        protected enum playerLookingDirection
        {
            up,
            down,
            right,
            left
        }
        /// <summary>
        /// States the directin the player attacks at, can be used with the onPlayerAttack event.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected playerLookingDirection attackingDirecton
        {
            get
            {
                switch (PlayerMovement.returnVerticalLookState())
                {
                    case PlayerMovement.lookStatesVertical.up:
                        return playerLookingDirection.up;
                     
                    case PlayerMovement.lookStatesVertical.down:
                        return playerLookingDirection.down;
                    
                    case PlayerMovement.lookStatesVertical.none:
                        switch (PlayerMovement.returnHorizontalLookState())
                        {
                            case PlayerMovement.lookStatesHorizontal.left:
                                return playerLookingDirection.left;
                     
                            case PlayerMovement.lookStatesHorizontal.right:
                                return playerLookingDirection.right;
                           
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
        }
        protected virtual void onPlayerAttack()
        {
            Debug.Log("Player attacked in the " + attackingDirecton + " direction");
        }
        
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

                    onPlayerAttack();
                }
            }
            
        }

        /// <summary>
        /// This will add the weapon as a seperate object, making it able for the player to attack
        /// </summary>
        public void Equip()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject obj = Instantiate(gameObject, player.transform.position, Quaternion.identity, player.transform);
            obj.transform.position = new Vector3(0, 0, 0);
        }

        public void Unequip()
        {
            Destroy(gameObject);
        }
    }
    
}
