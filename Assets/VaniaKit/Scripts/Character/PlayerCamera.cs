using System;
using UnityEngine;

namespace Vaniakit.Player
{
    
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private static PlayerCamera instance;
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
    }
}

