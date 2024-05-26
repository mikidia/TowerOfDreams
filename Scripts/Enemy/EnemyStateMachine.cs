using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        Rest
    }

    public State currentState;
    private Transform player;
    public int numberOfPatrolPoints = 8;
    public float patrolRadius = 5f;
    private Vector3 patrolCenter;
    private Vector3[] patrolPoints;
    private int currentPatrolIndex;
    public float baseDetectionRadius = 10f;
    public float attackRadius = 2f;
    public float attackCooldown = 1.5f;
    private bool isAttacking;
    [SerializeField] private float moveSpeed;

    public static EnemyStateMachine Instance;

    // FOV variables
    public float fieldOfView = 120f;
    private Vector3 initialForward;
    private bool isTrackingPlayer;
    private Vector3 lastKnownPlayerPosition;
    private float detectionDelay = 2f;

    // Serialized stats with properties
    private float intellect;
    private float stamina;
    private float strength;
    private float _agility;
    private float vitality;
    private float _energy;

    // Aggression level
    public float aggressionLevel = 1f;
    private float detectionRadius;

    // Property for agility with speed update
    public float agility
    {
        get => _agility;
        set
        {
            _agility = value;
        }
    }

    // Property for energy
    public float energy
    {
        get => _energy;
        set
        {
            _energy = value;
            if (_energy < 0)
            {
                _energy = 0;
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = Player._instance.transform;
        currentPatrolIndex = 0;
        currentState = State.Patrol;

        SetStats();

        initialForward = transform.forward;

        StartCoroutine(FSM());
    }

    // Method to update detection radius
    void UpdateDetectionRadius()
    {
        detectionRadius = baseDetectionRadius * aggressionLevel;
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Patrol:
                    Patrol();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Rest:
                    Rest();
                    break;
            }
            yield return null;
        }
    }

    void EnterPatrolState()
    {
        patrolCenter = transform.position;
        patrolPoints = CalculatePatrolPoints(patrolCenter, patrolRadius, numberOfPatrolPoints);
        currentPatrolIndex = 0;
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            EnterPatrolState();
        }

        Vector3 targetPosition = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        if (IsPlayerInFOV() || IsPlayerInDetectionRadius())
        {
            currentState = State.Chase;
        }
    }

    void Chase()
    {
        if (!IsPlayerInFOV() && !IsPlayerInDetectionRadius())
        {
            currentState = State.Patrol;
            EnterPatrolState();
            return;
        }

        if (IsPlayerInFOV())
        {
            lastKnownPlayerPosition = player.position;
            isTrackingPlayer = false;
        }
        else if (IsPlayerInDetectionRadius() && !isTrackingPlayer)
        {
            StartCoroutine(DelayedPlayerDetection());
        }

        MoveTowards(lastKnownPlayerPosition);

        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            currentState = State.Attack;
        }
    }

    IEnumerator DelayedPlayerDetection()
    {
        isTrackingPlayer = true;
        yield return new WaitForSeconds(detectionDelay);

        if (IsPlayerInDetectionRadius() && !IsPlayerInFOV())
        {
            lastKnownPlayerPosition = player.position;
            initialForward = (lastKnownPlayerPosition - transform.position).normalized;
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        // Call your attack method here
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;

        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            currentState = State.Chase;
        }
    }

    void Rest()
    {
        currentState = State.Patrol;
        EnterPatrolState();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw FOV arc
        float halfFOV = fieldOfView / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * detectionRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * detectionRadius);

        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
    }

    // Method to check if the player is in the enemy's FOV
    bool IsPlayerInFOV()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(initialForward, directionToPlayer);

        if (angleToPlayer < fieldOfView / 2 && Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            return true;
        }
        return false;
    }

    bool IsPlayerInDetectionRadius()
    {
        return Vector3.Distance(transform.position, player.position) < detectionRadius;
    }

    // Method to calculate patrol points around a circle
    Vector3[] CalculatePatrolPoints(Vector3 center, float radius, int points)
    {
        Vector3[] result = new Vector3[points];
        float angleStep = 360f / points;

        for (int i = 0; i < points; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            result[i] = new Vector3(center.x + Mathf.Cos(angle) * radius, center.y, center.z + Mathf.Sin(angle) * radius);
        }

        return result;
    }

    // Method to move the enemy towards a target point
    void MoveTowards(Vector3 target, float speedMultiplier = 1f)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * speedMultiplier * Time.deltaTime;
    }

    // Method to change stats (called based on game events)
    public void SetStats()
    {
        var enemyMain = GetComponent<EnemyMain>();

        aggressionLevel = enemyMain.Agressive;
        intellect = enemyMain.Intelect;
        stamina = enemyMain.Stamina;
        strength = enemyMain.Strength;
        agility = enemyMain.Agility;
        vitality = enemyMain.Vitality;
        energy = enemyMain.Energy;

        moveSpeed = enemyMain.MoveSpeed;

        UpdateDetectionRadius();
    }
}
