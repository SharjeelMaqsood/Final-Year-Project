using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    public Animator animator;
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

    [Header("Ranges")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    [Header("Vision")]
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Memory")]
    [SerializeField] private float memoryDuration = 3f;

    private Vector3 lastKnownPosition;
    private float memoryTimer;

    private NavMeshAgent agent;
    private int patrolIndex = 0;

    
    void Start()
    {
        animator = GetComponent<Animator>();
       agent = GetComponent<NavMeshAgent>();    
        currentState = State.Patrol;

        GoToNextPatrolPoint();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude, 0.1f, Time.deltaTime);


        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Search:
                Search();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }
   
    // ---------------- STATES ----------------

    void Patrol()
    {
        if (agent == null || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        DetectPlayer();
    }

    void Chase()
    {
        if (CanSeePlayer())
        {
            lastKnownPosition = player.position;
            memoryTimer = memoryDuration;

            agent.SetDestination(player.position);
        }
        else
        {
            currentState = State.Search;
            return;
        }

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
            GoToNextPatrolPoint();
        }

        // If we see player again → chase
        if (CanSeePlayer())
        {
            currentState = State.Chase;
        }
    }

    void Attack()
    {
        agent.SetDestination(transform.position);

        LookAt(player.position);

        

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            currentState = State.Chase;
        }
    }

    // ---------------- DETECTION ----------------

    void DetectPlayer()
    {
        if (CanSeePlayer())
        {
            currentState = State.Chase;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position);
        float distance = dirToPlayer.magnitude;

        if (distance > detectionRange) return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);

        if (angle > viewAngle / 2f) return false;

        // Line of sight check
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, distance, obstacleMask))
        {
            return false; // blocked by wall
        }

        return true;
    }

    // ---------------- HELPERS ----------------

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[patrolIndex].position;

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        dir.y = 0f;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}