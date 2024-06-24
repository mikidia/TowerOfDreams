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
    [SerializeField] private Transform player;
    public int numberOfPatrolPoints = 8;
    public float patrolRadius = 5f;
    private Vector3 patrolCenter;
    private Vector3[] patrolPoints;
    private int currentPatrolIndex;
    [SerializeField] public float baseDetectionRadius = 10f;
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
    private float detectionDelay = 1f;

    // Serialized stats with properties
    private float intellect;
    private float stamina;
    private float strength;
    private float _agility;
    private float vitality;
    [SerializeField] private float _energy = 0;
    [Header("Dash Settings")]
    [SerializeField] private float _maxEnergy = 50;
    [SerializeField] private float _energyRegen;

    // Aggression level
    public float aggressionLevel = 1f;
    private float detectionRadius;

    // Dash variables
    [SerializeField] private bool _isDashAvailable = true;
    [SerializeField] private float dashUseStamina = 50f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashCd = 10f; // Changed to 10 seconds
    [SerializeField] private float _returnBasicSpeed = 1f;
    private Vector3 dashDirection;

    // Raycast check variables
    public LayerMask validLayers; // Valid ground layers
    public float rayDistance = 1.0f; // Ray distance downwards
    public Vector3 rayOffset = Vector3.zero; // Ray offset
    private Vector3 lastValidPosition; // Last valid position

    private Rigidbody rb;

    // Facing direction
    private Vector3 facingDirection;

    // Attack trigger
    private Collider attackTrigger;

    // Property for agility with speed update
    public float agility
    {
        get => _agility;
        set
        {
            _agility = value;
        }
    }

    private void Update()
    {
        regenEnergy();
        // UpdateFacingDirection();

    }

    void regenEnergy()
    {
        if (_energy < _maxEnergy)
        {
            _energy += _energyRegen * Time.deltaTime;
        }
    }

    private void Awake()
    {
        Instance = this;
        lastValidPosition = transform.position;

    }

    void Start()
    {
        player = Player._instance.transform;
        currentPatrolIndex = 0;
        currentState = State.Patrol;

        SetStats();

        initialForward = transform.forward;

        lastValidPosition = transform.position; // Initialize last valid position
        rb = GetComponent<Rigidbody>();
        attackTrigger = transform.Find("AttackTrigger").GetComponent<Collider>();
        attackTrigger.enabled = false;
        StartCoroutine(FSM());
    }

    void FixedUpdate()
    {
        CheckGround();
        if (player == null)
        {
            player = Player._instance.transform;
        }
        // Debug.Log(Vector3.Distance(transform.position, player.position) +"  "+ detectionRadius);



    }

    // Method to check if the enemy is on valid ground
    void CheckGround()
    {
        Vector3 rayOrigin = transform.position + rayOffset;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & validLayers) != 0)
            {
                lastValidPosition = transform.position;
            }
            else
            {
                transform.position = lastValidPosition;
            }
        }
        else
        {
            transform.position = lastValidPosition;
        }
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
        // Debug.Log(IsPlayerInFOV() +"player in fov "   + IsPlayerInDetectionRadius()+"player in detection radius" );

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
        // Debug.Log(IsPlayerInFOV() +"player in fov "   + IsPlayerInDetectionRadius()+"player in detection radius" );
        if (!IsPlayerInFOV() && !IsPlayerInDetectionRadius())
        {
            currentState = State.Patrol;
            EnterPatrolState();
            return;
        }

        lastKnownPlayerPosition = player.position;
        // Debug.Log(lastKnownPlayerPosition);

        if (_isDashAvailable && _energy - dashUseStamina >= 0)
        {
            // UseDash();
        }

        MoveTowards(lastKnownPlayerPosition);

        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            currentState = State.Attack;
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
        attackTrigger.enabled = true; // Enable attack trigger
        // Call your attack method here
        // GetComponent<EnemyAttack>().PerformAttack(player);

        yield return new WaitForSeconds(attackCooldown);
        attackTrigger.enabled = false; // Disable attack trigger
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

        // Draw ray for ground check
        Vector3 rayOrigin = transform.position + rayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);
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
        Vector3 targetPosition = new Vector3(target.x, transform.position.y, target.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed
            * speedMultiplier * Time.deltaTime);
        facingDirection = direction; // Update facing direction
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
        _energy = enemyMain.Energy;

        moveSpeed = enemyMain.MoveSpeed;

        UpdateDetectionRadius();
    }

    // Method to use dash when the player is in FOV
    public void UseDash()
    {
        if (_isDashAvailable && _energy - dashUseStamina >= 0)
        {
            DetermineDashDirection();
            _energy -= dashUseStamina;
            StartCoroutine(DashAddSpeed());
        }
    }

    void DetermineDashDirection()
    {
        dashDirection = (player.position - transform.position).normalized;
    }

    IEnumerator DashAddSpeed()
    {
        _isDashAvailable = false;
        float originalMoveSpeed = moveSpeed;
        moveSpeed += _dashSpeed;
        rb.AddForce(dashDirection * _dashSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(_returnBasicSpeed);
        moveSpeed = originalMoveSpeed;
        rb.velocity = Vector3.zero;
        StartCoroutine(DashCd());
    }

    IEnumerator DashCd()
    {
        yield return new WaitForSeconds(_dashCd);
        _isDashAvailable = true;
    }
    public IEnumerator SlowDown(float time)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        var tempVariable = moveSpeed;
        moveSpeed = 0;

        yield return new WaitForSeconds(time);
        moveSpeed = tempVariable;
    }

    // Method to update facing direction
    // void UpdateFacingDirection()
    // {
    //     // Only update the rotation if the enemy needs to turn left or right
    //     if (facingDirection != Vector3.zero)
    //     {
    //         // Calculate the target rotation based on the facing direction
    //         // Quaternion targetRotation = Quaternion.LookRotation(facingDirection);

    //         // // Ensure the rotation is only around the Y-axis
    //         // targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

    //         // // Smoothly rotate the enemy towards the target direction
    //         // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    //     }
    // }
}
