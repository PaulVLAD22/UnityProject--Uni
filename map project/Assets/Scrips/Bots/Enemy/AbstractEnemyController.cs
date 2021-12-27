﻿using System.Collections;
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
    protected bool goingtoPatrolPoint=false;

    protected Transform player;

    protected bool playerInSightRange, playerInAttackRange, playerHasBeenSeen;

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

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;

        if (player == null) {
            Debug.Log("There is no player for the enemy to track!");
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        if (alreadyAttacked || isDead) {
            return;
        }

        // Check for sight and attack range
        playerInSightRange = PlayerInSightRange();
        playerInAttackRange = PlayerInAttackRange();
        playerHasBeenSeen = PlayerInFieldOfView();

        if (playerInAttackRange) {
            AttackPlayer();
        } else {
            if (playerHasBeenSeen && playerInSightRange) {
                ChasePlayer();
            } else {
                Patrolling();
            }
        }
    }

    protected void Patrolling()
    {
        SetAnimationState(PATROLLING);
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
       
        
       
            
            GameObject[] patrollingPoints;
            patrollingPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
            int nbOfPoints = patrollingPoints.Length;

            int random_number = Random.Range(0,nbOfPoints);
            Debug.Log(random_number);



            walkPoint = new Vector3(patrollingPoints[random_number].transform.position.x,patrollingPoints[random_number].transform.position.y,patrollingPoints[random_number].transform.position.z);
            walkPointSet=true;
            
        
        

        // pt a merge la loc
        //agent.SetDestination(player.position);

        // float randomZ = Random.Range(-walkPointRange, walkPointRange);
        // float randomX = Random.Range(-walkPointRange, walkPointRange);

        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) {
        //     walkPointSet = true;
        // }
    }

    protected void ChasePlayer()
    {
        SetAnimationState(CHASING);
        if (shouldNotFollow) {
            return;
        }

        Debug.Log("Chasing player...");
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
        // float coneDirection = 0;

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

        if (health <= 0) {
            DestroyEnemy();
        }
    }

    protected void AttackPlayer() 
    {
        transform.LookAt(player);
        StopMotion();

        if (!alreadyAttacked)
        {
            Debug.Log("Attacking player...");
            SetAnimationState(ATTACKING);
            AttackAction();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Attack code here
    protected abstract void AttackAction();
}
