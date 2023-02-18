using UnityEngine;

public enum EnemyState
{
    IDLE, ATTACK
}

public class EnemyMovement : CharacterScript
{
    [Header("Behaviour")]
    [SerializeField] private EnemyState state;
    [SerializeField] private GameObject spawnWeapon;

    [Header("Attack Behaviour")]
    [SerializeField] private bool chasePlayer;
    [SerializeField] private float safeDistance;
    [SerializeField] private float attackInterval;

    [Header("Idle Behaviour")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSlowdownFactor;
    [SerializeField] private float patrolWaitTime;

    private Patrol patrol;
    private float attackDelta;
    private HealthBarScript healthBar;

    protected override void Awake()
    {
        base.Awake();
        if (patrolPoints.Length > 1)
            patrol = new Patrol(patrolPoints, patrolSlowdownFactor, patrolWaitTime);

        healthBar = GetComponentInChildren<HealthBarScript>();

        attackDelta = 0;
    }

    private void Start()
    {
        GameObject gun = Instantiate(spawnWeapon, null);
        PickupObject(gun.GetComponent<PickupLogic>());
    }

    protected override void OnUpdate()
    {
        if (state == EnemyState.ATTACK)
        {
            if (GetWeapon())
            {
                GetWeapon().LookTowards(LevelManager.instance.player.position);
                attackDelta += Time.deltaTime;
                if (attackDelta >= attackInterval)
                {
                    attackDelta = 0;
                    GetWeapon().Attack();
                }
            }
        }
    }

    protected override Vector2 GetBaseVelocity()
    {
        if (state == EnemyState.IDLE && patrol != null)
        {
            return patrol.GetDirection(transform.position);
        }
        else if (state == EnemyState.ATTACK && chasePlayer)
        {
            Vector3 playerPos = LevelManager.instance.player.position;
            Vector2 direction = (playerPos - transform.position);
            float proceed = 1;

            if (GetWeapon().GetWeaponType() == WeaponType.RANGED)
            {
                float dist = Vector2.Distance(playerPos, transform.position);
                if (dist < safeDistance - 1) proceed = -1;
                else if (dist <= safeDistance) proceed = 0;
            }
            
            return direction.normalized * proceed;
        }

        return Vector2.zero;
    }
    protected override void OnWeaponPause() { }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        healthBar.UpdateValue(health / maxHealth);
    }
}

class Patrol
{
    private readonly Transform[] patrolPoints;
    private int target;
    private float slowdownFactor;
    private float waitTime;

    private float waited;

    public Patrol(Transform[] patrolPoints, float slowdownFactor, float waitTime)
    {
        this.patrolPoints = patrolPoints;
        this.slowdownFactor = slowdownFactor;
        this.waitTime = waitTime;

        this.waited = 0;
    }

    public Vector2 GetDirection(Vector2 pos)
    {
        if (waited >= 0)
        {
            waited -= Time.deltaTime;
            return Vector2.zero;
        }
        
        if (Vector2.Distance(patrolPoints[target].position, pos) <= 1)
        {
            target = (target + 1) % patrolPoints.Length;
            waited = waitTime * Random.Range(0.7f, 1.3f);
        }

        return ((Vector2)patrolPoints[target].position-pos).normalized * slowdownFactor;
    }
}

