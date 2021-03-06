using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class AbstractEnemyController : MonoBehaviour
{
    protected bool isDead = false;

    protected const int IDLE = 0;
    protected const int PATROLLING = 1;
    protected const int CHASING = 2;
    protected const int ATTACKING = 3;
    protected const int DYING = 4;

    // NavMeshAgent
    protected UnityEngine.AI.NavMeshAgent agent;
    protected Animator animator;
    
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    protected bool alreadyAttacked;

    protected Transform player;

    protected AbstractEnemyState fsmState;

    [Header ("Layers")]
    public LayerMask groundLayer, playerLayer;

    [Header ("General Behaviour")]
    public float fov = 94f;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange, attackRange;

    [Header ("Stats")]
    public float health;
    public int damage;

    [Header ("Debug")]
    public bool shouldNotPatrol;
    public bool shouldNotFollow;
    private AudioSource audioSource;
    float timer = 5f;

    protected AbstractEnemyController() 
    {
        this.fsmState = new PatrolAreaEnemyState(this);
    }

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        audioSource = GetComponent<AudioSource>();

        if (player == null) {
            Debug.Log("There is no player for the enemy to track!");
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected void FixedUpdate()
    {
        if (alreadyAttacked || isDead) {
            return;
        }

        if (!audioSource.isPlaying && timer <= 0)
        {
            audioSource.Play();
            timer = 5f;
        }
        
        timer -= Time.deltaTime;

        fsmState.DoAction();
        fsmState.CheckStateChange();
    }

    public void ChangeState(AbstractEnemyState state)
    {
        this.fsmState = state;
    }

    public void PatrolArea()
    {
        SetAnimationState(PATROLLING);
        if (shouldNotPatrol) {
            return;
        }

        if (!walkPointSet) {
            SearchWalkPoint();
            agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);
        }

        // Walkpoint reached
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Walkpoint reached");
                    walkPointSet = false;
                }
            }
        }
    }

    protected void SearchWalkPoint()
    {
        GameObject[] patrollingPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
        int numberOfPoints = patrollingPoints.Length;
        int selectedPoint = Random.Range(0, numberOfPoints);

        walkPoint = new Vector3(
            patrollingPoints[selectedPoint].transform.position.x,
            patrollingPoints[selectedPoint].transform.position.y,
            patrollingPoints[selectedPoint].transform.position.z);
        walkPointSet=true;
    }

    public void ChasePlayer()
    {
        SetAnimationState(CHASING);
        if (shouldNotFollow) {
            return;
        }

        agent.SetDestination(player.position);
        transform.LookAt(player);
        walkPointSet = true;
        walkPoint = player.position;
    }

    protected void StopMotion() {
        agent.SetDestination(transform.position);
    }

    protected void ResetAttack()
    {
        SetAnimationState(IDLE);
        alreadyAttacked = false;
    }

    protected void SetAnimationState(int state) {
        animator.SetInteger("State", state);
    }

    protected void DestroyEnemy()
    {
        isDead = true;
        SetAnimationState(DYING);
        Debug.Log("Destroying enemy...");
        Destroy(gameObject, 2.5f);
    }

    protected void DrawPOV()
    {
        Gizmos.color = Color.yellow;
        float rayRange = sightRange;
        float halfFOV = fov / 2.0f;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward * rayRange;
        Vector3 rightRayDirection = rightRayRotation * transform.forward * rayRange;

        Gizmos.DrawRay(transform.position, leftRayDirection);
        Gizmos.DrawRay(transform.position, rightRayDirection);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        DrawPOV();
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;

        if (health <= 0 && !isDead) {
            var Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            DestroyEnemy();
            Player.IncrementKillCount();
        }
    }

    public void AttackPlayer() 
    {
        transform.LookAt(player);
        StopMotion();
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(50);

        if (!alreadyAttacked)
        {
            Debug.Log("Attacking player...");
            SetAnimationState(ATTACKING);
            AttackAction();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    public bool PlayerInAttackRange() 
    {
        return Physics.CheckSphere(transform.position, attackRange, playerLayer);
    }

    public bool PlayerInSightRange()
    {
        return Physics.CheckSphere(transform.position, sightRange, playerLayer);
    }
    
    public bool PlayerInFieldOfView()
    {
        bool visibility = false;
        float angle;
        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - transform.position;

        if (Physics.Raycast (transform.position, rayDirection, out hit, Mathf.Infinity)) {
            if (hit.transform.tag.Equals (player.tag)) {
                angle = Vector3.Angle (transform.forward, rayDirection);
                if (angle <= fov) {
                    visibility = true;
                }
            }
        }

        return visibility;
    }
    
    // Attack code here
    public abstract void AttackAction();
}
