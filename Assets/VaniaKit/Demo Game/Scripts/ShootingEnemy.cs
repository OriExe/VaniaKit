using UnityEngine;
using Vaniakit.Ai;

public class ShootingEnemy : PatrolEnemyAi
{
    [SerializeField] private SpriteRenderer spriteR;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform[] bulletSpawn;
    private int bulletIndex = 1;
    [SerializeField] private float timeTillNextShot;
    [SerializeField] private GameObject sprite;
    private float timeLeft;
    protected override void OnPlayerInLineOfSight()
    {
        patrolling();
        if (timeLeft <= 0)
        {
            timeLeft = timeTillNextShot;
            Instantiate(bullet, bulletSpawn[bulletIndex].position, bulletSpawn[bulletIndex].rotation);
        }
    }

    protected override void OnTakenDamage()
    {
        spriteR.color = Color.red;
        Invoke("changetoWhite", 1.2f);
    }

    void changetoWhite()
    {
        spriteR.color = Color.white;
    }

    void Update()
    {
        base.Update();
        timeLeft -= Time.deltaTime;
    }
    
    protected override void switchPointToPatrol()
    {
        base.switchPointToPatrol();
        if (LookingDirection == Vector2.right)
        {
                bulletIndex=1;
                sprite.transform.localScale = new  Vector3(-1f, sprite.transform.localScale.y, sprite.transform.localScale.z);
        }
        else if (LookingDirection == Vector2.left)
        {
               bulletIndex=0;
               sprite.transform.localScale = new  Vector3(1f, sprite.transform.localScale.y, sprite.transform.localScale.z);
        }
    }
}
