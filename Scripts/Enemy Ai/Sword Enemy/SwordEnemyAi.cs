using UnityEngine;
using UnityEngine.AI;

public class SwordEnemyAi : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Search,
        Attack
    }

    [Header("State")]
    [SerializeField] private State currentState;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    private AnimationContE1 animController;

    private EnemyHealth enemyHealth;

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    [Header("Memory")]
    [SerializeField] private float memoryDuration = 3f;

    private Vector3 lastKnownPosition;
    private float memoryTimer;

    private NavMeshAgent agent;
    private int patrolIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animController = GetComponent<AnimationContE1>();
        enemyHealth = GetComponent<EnemyHealth>();

        currentState = State.Patrol;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
            return;

        if (enemyHealth != null && enemyHealth.IsStunned)
        {
            agent.isStopped = true;
            animController.UpdateMovement(0);
            return;
        }

        animController.UpdateMovement(agent.velocity.magnitude);

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Search: Search(); break;
            case State.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        DetectPlayer();
    }

    void Chase()
    {
        lastKnownPosition = player.position;
        memoryTimer = memoryDuration;
        agent.isStopped = false;
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            currentState = State.Attack;
        }
    }

    void Search()
    {
        agent.SetDestination(lastKnownPosition);

        memoryTimer -= Time.deltaTime;

        if (memoryTimer <= 0f)
        {
            currentState = State.Patrol;
        }
    }

    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime;

    void Attack()
    {
        if (enemyHealth != null && (enemyHealth.IsDead || enemyHealth.IsStunned))
            return;

        agent.isStopped = true;

        LookAt(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            currentState = State.Chase;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animController.TriggerAttack();
            lastAttackTime = Time.time;
        }
    }

    void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            currentState = State.Chase;
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }
}