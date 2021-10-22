using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class AbstractEnemyController : MonoBehaviour
{
    // NavMeshAgent
    protected UnityEngine.AI.NavMeshAgent agent;

    // Player transform
    protected Transform player;

    // Layers on what is ground and what is player
    public LayerMask groundLayer, playerLayer;

    // Health of enemy
    public float health;

    // Patroling Walk Point
    public Vector3 walkPoint;
    protected bool walkPointSet;
    public float walkPointRange;

    // FOV of enemy
    public float fov = 94;
    public float angle;

    // Attacking
    public float timeBetweenAttacks;
    public GameObject projectile;
    protected bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange, playerHasBeenSeen;

    protected void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
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
                Debug.Log("Attack player");
                AttackPlayer();
            } else if (playerInSightRange) {
                Debug.Log("Chase player");
                ChasePlayer();
            }
        }
        
    }

    protected void Patroling()
    {
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
        agent.SetDestination(player.position);
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
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
