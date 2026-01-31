using System;
using UnityEngine;

namespace Vaniakit.Player
{
    /// <summary>
    /// Default camera for player,
    /// Can be overridden by making this a subclass of the playercamera
    /// </summary>
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] protected GameObject player;
        protected static PlayerCamera instance;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                player = GameObject.FindGameObjectWithTag("Player");
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position, 2f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }

        /// <summary>
        /// Snaps the camera straight to the player for when they switch rooms
        /// </summary>
        protected virtual void snapCamToPlayer()
        {
            instance.transform.position = instance.player.transform.position;
            instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y, -10f);
        }
    
        /// <summary>
        /// The accessable version of snap camera to player for other scripts
        /// </summary>
        public static void snapCameraToPlayer()
        {
            instance.snapCamToPlayer();
        }
    }
}

