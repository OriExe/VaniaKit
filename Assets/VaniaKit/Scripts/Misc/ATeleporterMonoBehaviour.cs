using System;
using UnityEngine;

namespace Vaniakit.Misc
{
    public abstract class ATeleporterMonoBehaviour : MonoBehaviour
    {
        protected virtual void vkStart()
        {
            
        }

        protected virtual void vkUpdate()
        {
            
        }

        protected virtual void Start()
        {
            vkStart();
        }

        protected virtual void Update()
        {
            vkUpdate();
        }

        public abstract bool amITheRightObject(string gameObjectName); //Returns true if this spawn point is correct
    }
}