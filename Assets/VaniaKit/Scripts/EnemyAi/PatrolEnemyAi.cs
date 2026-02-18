using UnityEngine;

namespace Vaniakit.Ai
{
    public class PatrolEnemyAi : MonoBehaviour, IDamageable
    {
        [SerializeField] private float speed = 2;
        [SerializeField] private Transform[] pointsToGoTo;
        private int pointToFollowIndex;
        [SerializeField] private float health;
        #region Events
        protected virtual void onDeath()
        {

        }
        protected virtual void onTakenDamage()
        {

        }
        #endregion

        private void Update()
        {
            onPatrol();
        }
        public void OnHit(int damage = 0, bool isCritical = false)
        {
            if (isCritical)
            {
                health = 0;
            }
            else
            {
                health-=damage;
            }

            if (health == 0)
            {
                onDeath();
                Destroy(gameObject);
            }
        }

        protected virtual void onPatrol()
        {
            if (pointsToGoTo.Length <=1)
            {
                Debug.LogError("This ai can't move as not enough points has been assigned");
                return;
            }

            if (Vector2.Distance(pointsToGoTo[pointToFollowIndex].position, transform.position) < 0.2f)
            {
                pointToFollowIndex++;
                pointToFollowIndex %= pointsToGoTo.Length;
            }
            transform.position = Vector2.Lerp(transform.position,pointsToGoTo[pointToFollowIndex].position, speed);
        }

      
    }
}
