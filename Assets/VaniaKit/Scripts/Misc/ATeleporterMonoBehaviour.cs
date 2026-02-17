using UnityEngine;

namespace Vaniakit.Misc
{
    public abstract class ATeleporterMonoBehaviour : MonoBehaviour
    {
        public abstract bool amITheRightObject(string gameObjectName); //Returns true if this spawn point is correct
    }
}