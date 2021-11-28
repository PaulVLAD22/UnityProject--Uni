using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class AbstractEnemyController : MonoBehaviour
{
    // NavMeshAgent
    protected UnityEngine.AI.NavMeshAgent agent;
    
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    protected bool alreadyAttacked;

    protected Transform player;

    protected bool playerInSightRange, playerInAttackRange, playerHasBeenSeen;

    [Header ("Layers")]
    public LayerMask groundLayer, playerLayer;

    [Header ("General Behaviour")]
    public float fov = 94f;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public GameObject projectile;
    public float sightRange, attackRange;

    [Header ("Stats")]
    public float health;
    public float damage;

    [Header ("Debug")]
    public bool shouldNotPatrol;
    public bool shouldNotFollow;

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    protected bool PlayerInAttackRange() 
    {
        return Physics.CheckSphere(transform.position, attackRange, playerLayer);
    }

    protected bool PlayerInSightRange()
    {
        return Physics.CheckSphere(transform.position, sightRange, playerLayer);
    }

    protected bool PlayerInFieldOfView()
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

    protected void FixedUpdate()
    {
        // Check for sight and attack range
        playerInSightRange = PlayerInSightRange();
        playerInAttackRange = PlayerInAttackRange();
        playerHasBeenSeen = PlayerInFieldOfView();

        if (!playerHasBeenSeen) {
            Patroling();
        } else {
            if (playerInAttackRange) {
                AttackPlayer();
            } else if (playerInSightRange) {
                ChasePlayer();
            }
        }
        
    }

    protected void Patroling()
    {
        if (shouldNotPatrol) {
            return;
        }

        Debug.Log("Patrolling...");
        if (!walkPointSet) {
            SearchWalkPoint();
            agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);
        }
            
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }

    protected void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) {
            walkPointSet = true;
        }
    }

    protected void ChasePlayer()
    {
        if (shouldNotFollow) {
            return;
        }

        Debug.Log("Chasing player...");
        agent.SetDestination(player.position);
        transform.LookAt(player);
        walkPointSet = true;
        walkPoint = player.position;
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    protected void DrawPOV()
    {
        Gizmos.color = Color.yellow;
        float rayRange = sightRange;
        float halfFOV = fov / 2.0f;
        float coneDirection = 0;

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
        health -= damage;

        if (health <= 0) 
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    protected abstract void AttackPlayer();
}
