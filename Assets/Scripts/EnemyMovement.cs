using UnityEngine;

public enum EnemyState
{
    IDLE, ATTACK
}

public class EnemyMovement : CharacterScript
{
    [Header("Behaviour")]
    [SerializeField] private EnemyState state;

    [Header("Attack Behaviour")]
    [SerializeField] private bool chasePlayer;
    [SerializeField] private float safeDistance;

    [Header("Idle Behaviour")]
    [SerializeField] private Transform[] patrolPoints;

    private Patrol patrol;

    protected override void Awake()
    {
        base.Awake();
        if (patrolPoints.Length > 1)
            patrol = new Patrol(patrolPoints, 0.6f, 1f);
    }


    protected override void OnUpdate()
    {
        
    }

    protected override Vector2 GetBaseVelocity()
    {
        if (state == EnemyState.IDLE)
        {
            if (patrol != null) return patrol.GetDirection(transform.position);
        }

        return Vector2.zero;
    }
    protected override void OnWeaponPause() { }
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

