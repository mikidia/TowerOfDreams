using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        Rest,
        Sprint
    }

    public State currentState;
    private Transform player;
    public int numberOfPatrolPoints = 8;  // Number of points in the patrol circle
    public float patrolRadius = 5f;       // Radius of the patrol circle
    private Vector3 patrolCenter;
    private Vector3[] patrolPoints;
    private int currentPatrolIndex;
    public float baseDetectionRadius = 10f;
    public float attackRadius = 2f;
    public float attackCooldown = 1.5f;
    private bool isAttacking;
    [SerializeField] private float moveSpeed;

    public float sprintEnergyCost = 10f;
    public static EnemyStateMachine _instance;

    // FOV variables
    public float fieldOfView = 120f;  // FOV in degrees
    private Vector3 initialForward;

    // Serialized stats with properties
    float intellect;
    float stamina;
    float strength;
    private float _agility;
    float vitality;
    private float _energy;

    // Aggression level
    public float aggressionLevel = 1f;
    private float detectionRadius;

    // Property for agility with spee
    // d update
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
        _instance = this;
    }



    void Start()
    {
        player = Player._instance.transform;
        currentPatrolIndex = 0;
        currentState = State.Patrol;

        SetStats();

        initialForward = transform.forward; // Store the initial forward direction

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
                case State.Sprint:
                    Sprint();
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
        // Save the current position as the center of the patrol circle
        patrolCenter = transform.position;
        // Calculate patrol points around the circle
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

        if (IsPlayerInFOV())
        {
            currentState = State.Chase;
        }
    }

    void Chase()
    {
        if (!IsPlayerInFOV())
        {
            currentState = State.Patrol;
            EnterPatrolState(); // Reset patrol points when returning to patrol
            return;
        }

        MoveTowards(player.position);

        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            currentState = State.Attack;
        }
        else if (Vector3.Distance(transform.position, player.position) < detectionRadius / 2 && energy >= sprintEnergyCost)
        {
            currentState = State.Sprint;
        }
    }

    void Sprint()
    {
        if (energy < sprintEnergyCost || !IsPlayerInFOV())
        {
            currentState = State.Chase;
            return;
        }

        Vector3 targetPosition = player.position;
        MoveTowards(targetPosition);
        energy -= sprintEnergyCost * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            currentState = State.Attack;
        }
        else if (energy < sprintEnergyCost)
        {
            currentState = State.Chase;
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
        EnterPatrolState(); // Reset patrol points when resting
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);


        // Draw detection and attack spheres
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);




        // Draw FOV arc
        float halfFOV = fieldOfView / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * detectionRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * detectionRadius);

        // Draw a sphere to represent the maximum detection radius
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f); // Orange color with transparency
        Gizmos.DrawSphere(transform.position, detectionRadius);

        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }

        // Draw FOV
        Gizmos.color = Color.yellow;
        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView / 2, transform.up) * transform.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView / 2, transform.up) * transform.forward * detectionRadius;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
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

    // Method to move the enemy towards a target point, avoiding other enemies
    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        // Commented out the repulsion from other enemies
        // Vector3 repulsion = Vector3.zero;
        // foreach (var otherEnemy in FindObjectsOfType<EnemyStateMachine>())
        // {
        //     if (otherEnemy != this)
        //     {
        //         float distance = Vector3.Distance(transform.position, otherEnemy.transform.position);
        //         if (distance < 2.0f) // Threshold distance to start repulsion
        //         {
        //             Vector3 awayFromOtherEnemy = transform.position - otherEnemy.transform.position;
        //             repulsion += awayFromOtherEnemy.normalized / distance;
        //         }
        //     }
        // }

        // Vector3 moveDirection = direction + repulsion;dire
        Vector3 moveDirection = direction; // Use only the direction towards the target
        transform.position += moveDirection.normalized * moveSpeed / 5 * Time.deltaTime;
    }


    // Method to change stats (called based on game events)
    public void SetStats()
    {
        // Assume EnemyMain has these properties defined

        var enemyMain = GetComponent<EnemyMain>();

        aggressionLevel = enemyMain.Agressive;
        intellect = enemyMain.Intelect;
        stamina = enemyMain.Stamina;
        strength = enemyMain.Strength;
        agility = enemyMain.Agility;
        vitality = enemyMain.Vitality;
        energy = enemyMain.Energy;  // Assuming EnemyMain has an energy property

        moveSpeed = enemyMain.MoveSpeed;

        // Update detection radius and move speed based on new stats
        UpdateDetectionRadius();

    }


}
